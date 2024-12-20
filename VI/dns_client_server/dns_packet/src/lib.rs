pub mod record {
    use std::{net::Ipv4Addr, str::FromStr};

    use errors::DnsRecordParseError;

    #[derive(Debug, PartialEq)]
    pub struct DnsRecord {
        /// Resolving domain name
        pub domain_name: String,
        /// Resolved address
        pub address: Ipv4Addr,
    }

    impl DnsRecord {
        const INVALID_DOMAIN_NAME_SYMBOLS: [char; 18] = [
            ',', '~', ':', '!', '@', '#', '$', '%', '^', '&', '\'', '(', ')', '[', ']', '{', '}',
            ' ',
        ];
    }

    impl TryFrom<&str> for DnsRecord {
        type Error = DnsRecordParseError;

        fn try_from(str: &str) -> Result<Self, Self::Error> {
            let str = str.to_lowercase();
            let mut parts = str.split_ascii_whitespace();
            let domain_name = parts.next();
            let address_str = parts.next();
            if domain_name.is_none() || address_str.is_none() {
                return Err(DnsRecordParseError::NotEnoughArguments);
            }

            let domain_name = domain_name.unwrap().to_string();
            for bad_char in Self::INVALID_DOMAIN_NAME_SYMBOLS {
                if domain_name.contains(bad_char) {
                    return Err(DnsRecordParseError::InvalidDomainName(
                        bad_char,
                        domain_name,
                    ));
                }
            }

            let address_str = address_str.unwrap();
            let address = Ipv4Addr::from_str(address_str);

            if let Err(e) = address {
                return Err(DnsRecordParseError::InvalidAddress(e));
            }

            let address = address.unwrap();

            Ok(DnsRecord {
                domain_name,
                address,
            })
        }
    }

    pub mod errors {
        use std::{self, error, fmt};

        #[derive(Debug, Clone)]
        pub enum DnsRecordParseError {
            NotEnoughArguments,
            InvalidDomainName(char, String),
            InvalidAddress(std::net::AddrParseError),
        }

        impl fmt::Display for DnsRecordParseError {
            fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
                match self {
                    DnsRecordParseError::NotEnoughArguments => {
                        write!(
                            f,
                            "Not enough arguments: DNS record require domain name and address"
                        )
                    }
                    DnsRecordParseError::InvalidDomainName(bad_char, name) => {
                        write!(f, "Unexpected char '{}' in domain name {}", bad_char, name)
                    }
                    DnsRecordParseError::InvalidAddress(address) => {
                        write!(f, "Unexpected IPv4 address: {}", address)
                    }
                }
            }
        }

        impl error::Error for DnsRecordParseError {}
    }
}

pub mod packet {
    use std::{
        error::{self},
        fmt,
        net::Ipv4Addr,
    };

    use errors::DnsPacketLengthError;

    #[derive(Debug)]
    pub struct DnsPacket {
        /// Transaction ID
        pub id: u16,
        /// Length of resolving domain name
        pub domain_name_length: u8,
        /// Resolving domain name in UTF-8 encoding
        pub domain_name: String,
        /// Flag, which mean that domain name was resolved
        pub is_domain_name_resolved: bool,
        /// Resolved address
        pub resolved_address: Option<Ipv4Addr>,
    }

    impl DnsPacket {
        const MIN_PACKET_LENGTH: usize = 4;

        pub fn new(
            id: u16,
            domain_name_length: u8,
            domain_name: String,
            is_domain_name_resolved: bool,
            resolved_address: Option<Ipv4Addr>,
        ) -> Self {
            DnsPacket {
                id,
                domain_name_length,
                domain_name,
                is_domain_name_resolved,
                resolved_address,
            }
        }
    }

    impl TryFrom<&[u8]> for DnsPacket {
        type Error = DnsPacketLengthError;

        fn try_from(bytes: &[u8]) -> Result<Self, Self::Error> {
            // id (2) + length (1) + flag of resolving (1)
            let packet_length = bytes.len();
            if packet_length < Self::MIN_PACKET_LENGTH {
                return Err(DnsPacketLengthError::MinimumPacketLength {
                    found_length: packet_length,
                });
            }

            let id = u16::from_be_bytes([bytes[0], bytes[1]]);
            let domain_name_length = bytes[2];
            // check if provided length is valid
            let found_name_length = packet_length - Self::MIN_PACKET_LENGTH;
            if found_name_length < domain_name_length as usize {
                return Err(DnsPacketLengthError::DomainNameLength {
                    expected_length: domain_name_length as usize,
                    found_length: found_name_length,
                });
            }
            // 3 = id (2) + length (1)
            let domain_name_end_index: usize = (3 + domain_name_length).into();
            let mut domain_name = String::new();
            // if name domain provided, then read it
            if domain_name_length > 0 {
                domain_name = String::from_utf8_lossy(&bytes[3..domain_name_end_index]).to_string();
            }

            // returning parsed packet if address not resolved
            // not resolved is bytes[3 + name length] == 0
            if bytes[domain_name_end_index] == 0 {
                return Ok(Self::new(id, domain_name_length, domain_name, false, None));
            }

            let found_address_length = packet_length - (domain_name_end_index + 1);
            // return error, if is resolved flag is true, but address not provided
            // 5 is flag (1) + IPv4 (4)
            if found_address_length < 4 {
                return Err(DnsPacketLengthError::AddressLength {
                    found_length: found_address_length,
                });
            }

            Ok(Self::new(
                id,
                domain_name_length,
                domain_name,
                true,
                Some(Ipv4Addr::new(
                    bytes[domain_name_end_index + 1],
                    bytes[domain_name_end_index + 2],
                    bytes[domain_name_end_index + 3],
                    bytes[domain_name_end_index + 4],
                )),
            ))
        }
    }

    pub mod errors {
        use std::{error, fmt};

        use crate::packet::DnsPacket;

        #[derive(Debug, Clone)]
        pub enum DnsPacketLengthError {
            MinimumPacketLength {
                found_length: usize,
            },
            DomainNameLength {
                expected_length: usize,
                found_length: usize,
            },
            AddressLength {
                found_length: usize,
            },
        }

        impl fmt::Display for DnsPacketLengthError {
            fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
                // : {error_name: &str, expected_size: usize, received_size: usize }
                let info = match &self {
                    DnsPacketLengthError::MinimumPacketLength { found_length } => (
                        "minimum packet",
                        &DnsPacket::MIN_PACKET_LENGTH,
                        found_length,
                    ),
                    DnsPacketLengthError::DomainNameLength {
                        expected_length,
                        found_length,
                    } => ("domain name", expected_length, found_length),
                    DnsPacketLengthError::AddressLength { found_length } => {
                        ("resolved address", &(4 as usize), found_length)
                    }
                };

                write!(
                    f,
                    "Unexpected {} length: expected '{}', found '{}'",
                    info.0, info.1, info.2
                )
            }
        }

        impl error::Error for DnsPacketLengthError {}
    }

    #[cfg(test)]
    mod tests {
        use crate::packet::DnsPacket;

        #[test]
        fn dns_packet_parsing_errors() {
            let bytes_packs: &[&[u8]] = &[
                // less then min length
                &[],
                &[0, 0],
                // not provided resolving domain name
                &[0, 0, 1, 0],
                &[0, 0, 99, 0],
                // not provided resolved address
                &[0, 0, 0, 1],
                &[0, 0, 0, 1, 0, 1, 1],
            ];
            for bytes in bytes_packs {
                let packet = DnsPacket::try_from(&bytes[..]);
                assert!(packet.is_err());
            }
        }

        #[test]
        fn dns_packet_parsing_ok() {
            let bytes_packs: &[&[u8]] = &[
                &[
                    0, 0, // id
                    0, // length of resolving name
                    0, // resolving flag
                ],
                &[
                    0, 0, // id
                    5, // length of resolving name
                    121, 97, 46, 114, 117, // name
                    0,   // resolving flag
                ],
                &[
                    0, 0,  // id
                    12, // length of resolving name
                    111, 105, 122, 105, 46, 100, 109, 115, 46, 122, 97, 98, // name
                    1,  // resolving flag
                    10, 11, 3, 139, // resolved address
                ],
            ];
            let mut packets: Vec<DnsPacket> = Vec::with_capacity(bytes_packs.len());

            for bytes in bytes_packs {
                let packet = DnsPacket::try_from(&bytes[..]);
                assert!(packet.is_ok(), "Packet is error");
                packets.push(packet.unwrap());
            }

            let packet = &packets[0];
            assert!(packet.domain_name.is_empty());
            assert_eq!(packet.resolved_address, None);

            let packet = &packets[1];
            assert_eq!(&packet.domain_name, "ya.ru");
            assert_eq!(packet.resolved_address, None);

            let packet = &packets[2];
            assert_eq!(&packet.domain_name, "oizi.dms.zab");
            assert_eq!(
                packet.resolved_address,
                Some(std::net::Ipv4Addr::new(10, 11, 3, 139))
            );
        }
    }
}
