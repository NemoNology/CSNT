use std::{
    collections::HashMap,
    error::Error,
    net::{Ipv4Addr, SocketAddr, SocketAddrV4, UdpSocket},
};

use crate::{packet::DnsPacket, records_table_controller::DnsRecordsTableController};

pub struct DnsServer {
    udp_socket: UdpSocket,
    records_table: HashMap<String, Ipv4Addr>,
}

impl DnsServer {
    pub fn new<'a>(records_table_path: &'a str, listen_port: u16) -> Result<Self, Box<dyn Error>> {
        let controller = DnsRecordsTableController::new(records_table_path);
        let records = controller.get_records_list()?;
        let mut records_table: HashMap<String, Ipv4Addr> = HashMap::with_capacity(records.len());

        for record in records {
            records_table.insert(record.domain_name.to_string(), record.address);
        }

        let udp_socket = UdpSocket::bind(SocketAddrV4::new(Ipv4Addr::LOCALHOST, listen_port))?;

        Ok(DnsServer {
            udp_socket,
            records_table,
        })
    }

    /// Return DNS query packet and client address
    pub fn wait_dns_query(&self) -> Result<(DnsPacket, SocketAddr), Box<dyn Error>> {
        let mut bytes_buffer = [0; DnsPacket::MAX_PACKET_LENGTH];
        let (packet_length, client_address) = self.udp_socket.recv_from(&mut bytes_buffer)?;

        Ok((
            DnsPacket::try_from(&bytes_buffer[..packet_length])?,
            client_address,
        ))
    }

    pub fn resolve(
        &self,
        dns_query_packet: &mut DnsPacket,
        dns_client_address: &SocketAddr,
    ) -> Result<Option<Ipv4Addr>, Box<dyn Error>> {
        let resolved_address = self
            .records_table
            .get(&dns_query_packet.domain_name)
            .copied();

        if let Some(address) = resolved_address {
            dns_query_packet.resolve_name(address);
        };

        self.udp_socket
            .send_to(&dns_query_packet.into_bytes(), dns_client_address)?;

        Ok(resolved_address)
    }
}
