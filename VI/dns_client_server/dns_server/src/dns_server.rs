use std::{
    collections::HashMap,
    fs::File,
    net::{SocketAddr, UdpSocket},
};

use dns_packet::DnsRecordResponse;

struct DnsServer<'a> {
    /// Domain name zone, like `"."`, `"ru."` or `"zab.ru."`
    pub domain_name_zone: &'a str,
    /// Socket for communicate with clients
    udp_socket: UdpSocket,
    /// Path to file to storage DNS records
    pub records_file_path: &'a str,
    /// DNS records table
    records_table: HashMap<&'a str, DnsRecordResponse>,
}

impl<'a> DnsServer<'a> {
    pub const PORT: u16 = 60_053;

    pub fn build(
        domain_name_zone: &'a str,
        records_file_path: &'a str,
        records_table_capacity: usize,
    ) -> Result<Self, String> {
        // bind udp socket and check binding
        let udp_socket = UdpSocket::bind(SocketAddr::from(([127, 0, 0, 1], DnsServer::PORT)));
        if let Err(e) = udp_socket {
            return Err(format!("Can't bind UDP socket: {}", e));
        }

        // check possibility of DNS records file open to write
        if let Err(e) = File::create(&records_file_path) {
            return Err(format!(
                "Can't create or open to write DNS records file by path '{}': {}",
                records_file_path, e
            ));
        }

        // unwrap var-s
        let udp_socket = udp_socket.unwrap();
        // create table
        let records_table = HashMap::with_capacity(records_table_capacity);

        Ok(DnsServer {
            domain_name_zone,
            udp_socket,
            records_file_path,
            records_table,
        })
    }
}
