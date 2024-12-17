use std::{
    error::{self},
    fmt,
    net::Ipv4Addr,
};

#[derive(Debug, PartialEq)]
pub struct DnsRecord {
    /// Resolving domain name
    pub domain_name: String,
    /// Resolved address
    pub address: Ipv4Addr,
}

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

#[derive(Debug, Clone)]
pub enum DnsPacketParseError {
    InvalidMinimumPacketLength {
        received_length: usize,
    },
    InvalidDomainNameLength {
        expected_length: usize,
        received_length: usize,
    },
    InvalidAddressLength {
        expected_length: usize,
        received_length: usize,
    },
}

impl fmt::Display for DnsPacketParseError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        // : {error_name: &str, expected_size: usize, received_size: usize }
        let info = match &self {
            DnsPacketParseError::InvalidMinimumPacketLength {
                received_length: received_size,
            } => (
                "Minimum packet length",
                &DnsPacket::MIN_PACKET_SIZE,
                received_size,
            ),
            DnsPacketParseError::InvalidDomainNameLength {
                expected_length: expected_size,
                received_length: received_size,
            } => ("Domain name length", expected_size, received_size),
            DnsPacketParseError::InvalidAddressLength {
                expected_length: expected_size,
                received_length: received_size,
            } => ("Resolved address length", expected_size, received_size),
        };

        write!(
            f,
            "{} error: expected '{}' packet length, but received '{}'",
            info.0, info.1, info.2
        )
    }
}

impl error::Error for DnsPacketParseError {}

impl DnsPacket {
    const MIN_PACKET_SIZE: usize = 4;

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
    type Error = DnsPacketParseError;

    fn try_from(bytes: &[u8]) -> Result<Self, Self::Error> {
        // id (2) + length (1) + flag of resolving (1)
        let len = bytes.len();
        if len < Self::MIN_PACKET_SIZE {
            return Err(DnsPacketParseError::InvalidMinimumPacketLength {
                received_length: len,
            });
        }

        let id = u16::from_be_bytes([bytes[0], bytes[1]]);
        let length = bytes[2];
        // check if provided length is valid
        let expected_size = Self::MIN_PACKET_SIZE + length as usize;
        if len < expected_size {
            return Err(DnsPacketParseError::InvalidDomainNameLength {
                expected_length: expected_size,
                received_length: len,
            });
        }
        // 3 = id (2) + length (1)
        let name_end_index: usize = (3 + length).into();
        let mut name = String::new();
        // if name domain provided, then read it
        if length > 0 {
            name = String::from_utf8_lossy(&bytes[3..name_end_index]).to_string();
        }

        // returning parsed packet if address not resolved
        // not resolved is bytes[3 + name length] == 0
        if bytes[name_end_index] == 0 {
            return Ok(Self::new(id, length, name, false, None));
        }

        // return error, if is resolved flag is true, but address not provided
        // 5 is flag (1) + IPv4 (4)
        if len < name_end_index + 5 {
            return Err(DnsPacketParseError::InvalidAddressLength {
                expected_length: expected_size,
                received_length: len,
            });
        }

        Ok(Self::new(
            id,
            length,
            name,
            true,
            Some(Ipv4Addr::new(
                bytes[name_end_index + 1],
                bytes[name_end_index + 2],
                bytes[name_end_index + 3],
                bytes[name_end_index + 4],
            )),
        ))
    }
}

#[cfg(test)]
mod tests {

    use crate::DnsPacket;

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
