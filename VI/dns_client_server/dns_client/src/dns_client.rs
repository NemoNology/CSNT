use std::{
    error::Error,
    net::{Ipv4Addr, UdpSocket},
};

pub struct DnsClient {
    udp_socket: UdpSocket,
    hosts_path: String,
    server_port: u16,
}

impl DnsClient {
    pub fn build(hosts_path: String, server_port: u16) -> Result<DnsClient, Box<dyn Error>> {
        let udp_socket = UdpSocket::bind("127.0.0.1:0")?;

        Ok(DnsClient {
            udp_socket,
            hosts_path,
            server_port,
        })
    }
}
