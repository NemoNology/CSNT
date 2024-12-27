use std::{
    error::Error,
    fs::File,
    io::{BufRead, BufReader},
    net::{Ipv4Addr, SocketAddrV4, UdpSocket},
    time::Duration,
};

use crate::packet::DnsPacket;

pub struct DnsClient<'a> {
    udp_socket: UdpSocket,
    hosts_path: &'a str,
}

impl<'a> DnsClient<'a> {
    /// set `server_wait_timeout_secs` to `0` for endless server answer wait.
    pub fn new(
        hosts_path: &'a str,
        server_port: u16,
        server_wait_timeout_secs: u64,
    ) -> Result<Self, Box<dyn Error>> {
        let udp_socket = UdpSocket::bind("127.0.0.1:0")?;
        udp_socket.connect(SocketAddrV4::new(Ipv4Addr::LOCALHOST, server_port))?;
        udp_socket.set_read_timeout(Some(Duration::new(server_wait_timeout_secs, 0)))?;

        Ok(DnsClient {
            udp_socket,
            hosts_path,
        })
    }

    pub fn resolve_in_hosts<'b>(
        &self,
        domain_name: &'b str,
    ) -> Result<Option<Ipv4Addr>, Box<dyn Error>> {
        let file = File::open(&self.hosts_path)?;
        let reader = BufReader::new(file);

        for line_result in reader.lines() {
            let line = line_result?;

            if line.starts_with("#") || line.trim().is_empty() {
                continue;
            }

            // split line to address & domain name
            let mut parts = line.split_ascii_whitespace();
            // get address, when line contains both parts
            let (address, name) = match (parts.next(), parts.next()) {
                (Some(address), Some(name)) => (address, name),
                _ => continue,
            };

            if domain_name == name {
                return Ok(Some(address.parse()?));
            }
        }

        Ok(None)
    }

    pub fn send_dns_query(&self, domain_name: String) -> Result<(), Box<dyn Error>> {
        let packet = DnsPacket::init(domain_name.len() as u8, domain_name);
        let bytes: Vec<u8> = packet.into_bytes();
        self.udp_socket.send(&bytes)?;
        Ok(())
    }

    pub fn wait_server_answer_packet(&self) -> Result<DnsPacket, Box<dyn Error>> {
        let mut bytes_buffer = [0; DnsPacket::MAX_PACKET_LENGTH];
        let answer_size = self.udp_socket.recv(&mut bytes_buffer)?;
        let answer_packet = DnsPacket::try_from(&bytes_buffer[..answer_size])?;

        Ok(answer_packet)
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn hosts_resolving() {
        let client = DnsClient::new(r"C:\Users\NemoNology\hosts.txt", 60053, 2).unwrap();

        let domain_names: [&str; 4] = ["test1.com", "oizi.sud.net", "yo.ru", "wiki.local"];
        let addresses: [Ipv4Addr; 4] = [
            Ipv4Addr::new(55, 44, 66, 55),
            Ipv4Addr::new(10, 11, 3, 139),
            Ipv4Addr::new(44, 55, 66, 77),
            Ipv4Addr::new(127, 0, 1, 1),
        ];

        for (name, address) in domain_names.iter().zip(addresses) {
            let resolved_address = client.resolve_in_hosts(name).unwrap().unwrap();
            assert_eq!(resolved_address, address);
        }
    }

    /// Requires started server
    #[test]
    fn server_resolving() {
        let domain_names: [&str; 6] = [
            "ya.ru",
            "oizi.dms.zab",
            "paint.local",
            "rutube.ru",
            "работа.рф",
            "wiki.net",
        ];
        let addresses: [Ipv4Addr; 6] = [
            Ipv4Addr::new(44, 55, 66, 55),
            Ipv4Addr::new(10, 11, 3, 139),
            Ipv4Addr::new(127, 0, 1, 25),
            Ipv4Addr::new(44, 55, 44, 55),
            Ipv4Addr::new(55, 55, 55, 55),
            Ipv4Addr::new(11, 12, 13, 14),
        ];

        let client = DnsClient::new("", 60053, 2).unwrap();
        for (name, addr) in domain_names.iter().zip(addresses) {
            let _ = client.send_dns_query(name.to_string());
            let answer = client.wait_server_answer_packet().unwrap();
            assert_eq!(answer.resolved_address.unwrap(), addr);
        }
    }
}
