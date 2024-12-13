use std::net::Ipv4Addr;

pub enum DnsRecordResponse {
    /// Resolved address of domain name
    Address(Ipv4Addr),
    /// Domain name of name server, which contains address of resolving domain name
    NameServer(String),
    /// Address or name server of resolving domain name not found
    NotFound,
}

pub enum DnsData {
    /// Request to resolve domain name
    Query(String),
    /// Answer to resolving domain name request
    Response(DnsRecordResponse),
}

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
