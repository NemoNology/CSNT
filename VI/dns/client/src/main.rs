use clap::{arg, Parser};
use dns::client::DnsClient;

#[derive(Parser, Debug)]
struct Args {
    /// Path to hosts file
    #[arg(short, default_value = r"C:\Users\NemoNology\hosts.txt")]
    path: String,
    /// Resolving domain name
    #[arg(short)]
    domain_name: String,
}

fn main() {
    let args = Args::parse();
    let client = DnsClient::new(&args.path).expect("Can't init DNS client");

    match client.resolve(&args.domain_name) {
        Ok(address_option) => match address_option {
            Some(address) => println!("Resolved address: {}", address),
            None => println!("Address not resolved"),
        },
        Err(e) => eprint!("Error: {}", e),
    }
}
