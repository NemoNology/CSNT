use std::{
    error::Error,
    fs::File,
    io::{BufRead, BufReader},
    net::{Ipv4Addr, UdpSocket},
};

pub struct DnsClient<'a> {
    udp_socket: UdpSocket,
    /// Path to File contains hosts info (pairs of domain name & resolved address)
    hosts_path: &'a str,
    server_port: u16,
}

impl DnsClient<'_> {
    pub fn build(hosts_path: &str, server_port: u16) -> Result<DnsClient, Box<dyn Error>> {
        let udp_socket = UdpSocket::bind("127.0.0.1:0")?;

        Ok(DnsClient {
            udp_socket,
            hosts_path,
            server_port,
        })
    }

    /// Return IPv4 address from hosts file, if it exists in it
    fn resolve_in_hosts_file<'a>(
        &'a self,
        resolving_name: &str,
    ) -> Result<Option<Ipv4Addr>, String> {
        let hosts = File::open(&self.hosts_path);
        if let Err(e) = hosts {
            return Err(format!(
                "Can't open hosts file ({}): {}",
                self.hosts_path, e
            ));
        }
        let reader = BufReader::new(hosts.unwrap());

        for line_result in reader.lines() {
            if let Err(e) = line_result {
                return Err(format!(
                    "Can't read line of hosts file ({}): {}",
                    self.hosts_path, e
                ));
            }
            let line = line_result.unwrap();

            // skip comments & empty lines
            if line.starts_with('#') || line.trim().is_empty() {
                continue;
            }

            let mut parts = line.split_ascii_whitespace();

            // get first part of pair (domain name) and compare it
            if let Some(domain_name) = parts.next() {
                // skip not searching names
                if domain_name != resolving_name {
                    continue;
                }

                // get second part of pair (address) and parse it with return
                if let Some(address) = parts.next() {
                    let resolved_address = address.parse::<Ipv4Addr>();
                    return match resolved_address {
                        Err(e) => Err(format!(
                            "Can't resolve address of line ({}) of hosts file ({}): {}",
                            line, self.hosts_path, e
                        )),
                        Ok(address) => Ok(Some(address)),
                    };
                }
            }
        }

        // return none if address not found
        Ok(None)
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    /// Testing Ok results
    #[test]
    fn resolve_in_hosts_file_ok_test() {
        let hosts_path = "C:\\Users\\NemoNology\\hosts.txt";
        let client = DnsClient::build(hosts_path, 53).unwrap();
        let mut resolved_address = client.resolve_in_hosts_file("localhost").unwrap();
        assert_eq!(resolved_address, Some(Ipv4Addr::LOCALHOST));
        resolved_address = client.resolve_in_hosts_file("oizi.dms.zab").unwrap();
        assert_eq!(resolved_address, Some(Ipv4Addr::new(10, 11, 3, 139)));
    }

    /// Testing Err results
    #[test]
    fn resolve_in_hosts_file_err_test() {
        let hosts_path = "C:\\Users\\NemoNology\\hosts.txt";
        let client = DnsClient::build(hosts_path, 53).unwrap();
        let mut resolved_address = client.resolve_in_hosts_file("its.wrong");
        assert!(resolved_address.is_err());
        resolved_address = client.resolve_in_hosts_file("its.butterfly");
        assert!(resolved_address.is_err());
        resolved_address = client.resolve_in_hosts_file("its.not.ip-address");
        assert!(resolved_address.is_err());
    }

    /// Testing None result
    #[test]
    fn resolve_in_hosts_file_none_test() {
        let hosts_path = "C:\\Users\\NemoNology\\hosts.txt";
        let client = DnsClient::build(hosts_path, 53).unwrap();
        let mut resolved_address = client.resolve_in_hosts_file("localhost1").unwrap();
        assert_eq!(resolved_address, None);
        resolved_address = client.resolve_in_hosts_file("cake.is.a.lie").unwrap();
        assert_eq!(resolved_address, None);
        resolved_address = client.resolve_in_hosts_file("").unwrap();
        assert_eq!(resolved_address, None);
    }
}
