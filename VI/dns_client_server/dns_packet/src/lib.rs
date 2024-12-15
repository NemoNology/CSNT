use serde::{Deserialize, Serialize};
use std::net::Ipv4Addr;

#[derive(Debug, PartialEq, Clone, Serialize, Deserialize)]
pub enum DnsRecordType {
    /// Resolved address of domain name
    Address(Ipv4Addr),
    /// Domain name of name server, which contains address of resolving domain name
    NameServer(String),
}

#[derive(Debug, PartialEq, Serialize, Deserialize)]
pub struct DnsRecord {
    /// Resolving domain name
    pub domain_name: String,
    /// Resolved record for domain name
    pub data: DnsRecordType,
}

#[derive(Debug, Clone, PartialEq, Serialize, Deserialize)]
pub enum DnsQueryResponse {
    DnsRecord(DnsRecordType),
    /// Address or name server of resolving domain name not found
    NotFound,
}

#[derive(Serialize, Deserialize)]
pub enum DnsData {
    /// Request to resolve domain name
    Query(String),
    /// Answer to resolving domain name request
    Response(DnsQueryResponse),
}

impl DnsData {
    pub fn is_query(&self) -> bool {
        match self {
            Self::Query(_) => true,
            _ => false,
        }
    }
}

#[derive(Serialize, Deserialize)]
pub struct DnsPacket {
    /// Transaction ID
    pub id: u16,
    /// Name server, which contains address of resolving domain name
    pub name_server: String,
    /// Transaction data (query or answer)
    pub data: DnsData,
}

impl DnsPacket {
    pub fn new(id: u16, name_server: String, data: DnsData) -> Self {
        DnsPacket {
            id,
            name_server,
            data,
        }
    }
}
