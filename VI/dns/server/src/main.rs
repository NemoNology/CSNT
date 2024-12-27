use clap::Parser;
use dns::server::DnsServer;

#[derive(Parser, Debug)]
struct Args {
    #[arg(short, long, default_value = "60053")]
    server_port: u16,
    #[arg(short, long, default_value = r"C:\Users\NemoNology\records.txt")]
    records_table_path: String,
}

fn main() {
    let args = Args::parse();
    let server = match DnsServer::new(&args.records_table_path, args.server_port) {
        Ok(server) => {
            println!(
                "Server starts listening on {} port, with '{}' records table file...",
                &args.server_port, &args.records_table_path
            );
            server
        }
        Err(e) => {
            eprintln!(
                "Can't initialize DNS server with '{}' records file: {}",
                &args.records_table_path, e
            );
            return;
        }
    };
    loop {
        let (mut packet, client_address) = match server.wait_dns_query() {
            Ok((packet, client_addr)) => (packet, client_addr),
            Err(e) => {
                eprintln!("Error, when getting DNS query packet: {}", e);
                continue;
            }
        };

        println!(
            "Get DNS query ({:x}) for '{}' from '{}' address",
            packet.id, packet.domain_name, client_address
        );

        match server.resolve(&mut packet, &client_address) {
            Ok(address) => match address {
                Some(address) => println!(
                    "Successfully resolve DNS query ({:x}) '{}' by '{}' address for '{}' address",
                    packet.id, packet.domain_name, address, client_address
                ),
                None => println!(
                    "Not found address for resoling DNS query ({:x}) '{}' address [client address: '{}']",
                    packet.id, packet.domain_name, client_address
                ),
            },
            Err(e) => eprintln!(
                "Get error when resolving and sending answer for DNS query ({:x}) for '{}' for '{}' client address: {}",
                packet.id, packet.domain_name, client_address, e),
        }
    }
}
