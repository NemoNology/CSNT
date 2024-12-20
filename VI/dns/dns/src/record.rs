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
        ',', '~', ':', '!', '@', '#', '$', '%', '^', '&', '\'', '(', ')', '[', ']', '{', '}', ' ',
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
