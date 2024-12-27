use std::{
    collections::HashMap,
    error::Error,
    net::{Ipv4Addr, SocketAddrV4, UdpSocket},
};

use crate::{packet::DnsPacket, records_table_controller::DnsRecordsTableController};

pub struct DnsServer {
    udp_socket: UdpSocket,
    records_table: HashMap<String, Ipv4Addr>,
}

impl DnsServer {
    pub const UDP_LISTENING_PORT: u16 = 60_053;

    pub fn new<'a>(records_table_path: &'a str) -> Result<Self, Box<dyn Error>> {
        let controller = DnsRecordsTableController::new(records_table_path);
        let records = controller.get_records_list()?;
        let mut records_table: HashMap<String, Ipv4Addr> = HashMap::with_capacity(records.len());

        for record in records {
            records_table.insert(record.domain_name.to_string(), record.address);
        }

        let udp_socket = UdpSocket::bind(SocketAddrV4::new(
            Ipv4Addr::LOCALHOST,
            Self::UDP_LISTENING_PORT,
        ))?;
        udp_socket.connect("127.0.0.1:0")?;

        Ok(DnsServer {
            udp_socket,
            records_table,
        })
    }

    pub fn resolve<'a>(&self, domain_name: &'a str) -> Option<Ipv4Addr> {
        self.records_table.get(domain_name).copied()
    }

    pub fn wait_packet_to_resolve_and_resolve_it(&self) -> Result<(), Box<dyn Error>> {
        let mut bytes_buffer = [0; DnsPacket::MAX_PACKET_LENGTH];
        let (packet_length, client_address) = self.udp_socket.recv_from(&mut bytes_buffer)?;

        let mut packet = DnsPacket::try_from(&bytes_buffer[..packet_length])?;
        if let Some(address) = self.resolve(&packet.domain_name) {
            packet.resolve_name(address);
        }

        self.udp_socket
            .send_to(&packet.into_bytes(), client_address)?;

        Ok(())
    }
}
