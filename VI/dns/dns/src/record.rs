use std::{net::Ipv4Addr, str::FromStr};

use errors::DnsRecordParseError;

use crate::name::DomainName;

#[derive(Debug, PartialEq)]
pub struct DnsRecord {
    /// Resolving domain name
    pub domain_name: DomainName,
    /// Resolved address
    pub address: Ipv4Addr,
}

impl DnsRecord {
    pub fn new(domain_name: DomainName, address: Ipv4Addr) -> Self {
        DnsRecord {
            domain_name,
            address,
        }
    }
}

impl FromStr for DnsRecord {
    type Err = DnsRecordParseError;

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        // Ignore case
        let s = s.to_lowercase();
        // Split DNS record to domain name and address
        // Handle error
        let mut parts = s.split_ascii_whitespace();
        let (domain_name, address_str) = match (parts.next(), parts.next()) {
            (Some(name), Some(address_str)) => (name, address_str),
            _ => return Err(DnsRecordParseError::NotEnoughArguments),
        };

        // Build domain name
        let domain_name = match DomainName::build(domain_name.to_string()) {
            Err(e) => {
                return Err(DnsRecordParseError::InvalidDomainName(e));
            }
            Ok(name) => name,
        };

        // Unwrap address and try to parse it in IPv4
        match Ipv4Addr::from_str(address_str) {
            Err(e) => Err(DnsRecordParseError::InvalidAddress(e)),
            Ok(address) => Ok(DnsRecord {
                domain_name,
                address,
            }),
        }
    }
}

pub mod errors {
    use std::{self, error, fmt};

    use crate::name::errors::BadCharError;

    #[derive(Debug, Clone)]
    pub enum DnsRecordParseError {
        NotEnoughArguments,
        InvalidDomainName(BadCharError),
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
                DnsRecordParseError::InvalidDomainName(bad_char_error) => {
                    write!(f, "{}", bad_char_error)
                }
                DnsRecordParseError::InvalidAddress(address) => {
                    write!(f, "Unexpected IPv4 address: {}", address)
                }
            }
        }
    }

    impl error::Error for DnsRecordParseError {}
}
