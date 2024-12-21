use std::{
    error::Error,
    fs::File,
    io::{BufRead, BufReader},
    net::{Ipv4Addr, UdpSocket},
};

use crate::{packet::DnsPacket, server::DnsServer};

pub struct DnsClient<'a> {
    pub udp_socket: UdpSocket,
    hosts_path: &'a str,
}

impl<'a> DnsClient<'a> {
    pub fn new(hosts_path: &'a str) -> Result<Self, Box<dyn Error>> {
        let udp_socket = UdpSocket::bind("127.0.0.1:0")?;
        let server_address = format!("127.0.0.1:{}", DnsServer::UDP_LISTENING_PORT);
        udp_socket.connect(&server_address)?;

        Ok(DnsClient {
            udp_socket,
            hosts_path,
        })
    }

    pub fn resolve<'b>(&self, domain_name: &'b str) -> Result<Option<Ipv4Addr>, Box<dyn Error>> {
        let mut answer = self.resolve_in_hosts(domain_name);
        if answer.is_err() {
            answer = self.resolve_in_server(domain_name.to_string());
        }

        answer
    }

    fn resolve_in_hosts<'b>(
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

            let mut parts = line.split_ascii_whitespace();
            let found_address = parts.next().unwrap();
            let found_domain_name = parts.next();

            if found_domain_name.is_none() || found_domain_name.unwrap() != domain_name {
                continue;
            }

            let address: Ipv4Addr = found_address.parse()?;
            return Ok(Some(address));
        }

        Ok(None)
    }

    fn resolve_in_server<'b>(
        &self,
        domain_name: String,
    ) -> Result<Option<Ipv4Addr>, Box<dyn Error>> {
        let packet = DnsPacket::init(domain_name.len() as u8, domain_name);
        let bytes: Vec<u8> = packet.into_bytes();
        self.udp_socket.send(&bytes)?;
        let mut bytes_buffer = [42; 264];

        let answer_size = self.udp_socket.recv(&mut bytes_buffer)?;
        let answer_packet = DnsPacket::try_from(&bytes_buffer[..answer_size])?;

        Ok(answer_packet.resolved_address)
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn hosts_resolving() {
        let path = r"C:\Users\NemoNology\hosts.txt";
        let client = DnsClient::new(path).unwrap();

        let domain_names: [&str; 4] = ["test1.com", "oizi.dms.zab", "ya.ru", "wiki.local"];
        let addresses: [Ipv4Addr; 4] = [
            Ipv4Addr::new(55, 44, 66, 55),
            Ipv4Addr::new(10, 11, 3, 139),
            Ipv4Addr::new(44, 55, 66, 77),
            Ipv4Addr::new(127, 0, 1, 1),
        ];

        for (name, address) in domain_names.iter().zip(addresses) {
            let resolved_address = client.resolve(name).unwrap().unwrap();
            assert_eq!(resolved_address, address);
        }
    }
}
