use std::net::Ipv4Addr;

pub enum DnsRecordResponse {
    Address(Ipv4Addr),
    NameServer(String),
    NotFound,
}

pub enum DnsRecord {
    Query(String),
    Response(DnsRecordResponse),
}

pub struct DnsPacket {
    pub id: u16,
    pub name_server: String,
    pub record: DnsRecord,
}

impl DnsPacket {
    pub fn new(id: u16, name_server: String, record: DnsRecord) -> Self {
        DnsPacket {
            id,
            name_server,
            record,
        }
    }
}
