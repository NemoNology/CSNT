use clap::{arg, Parser};
use dns::client::DnsClient;

#[derive(Parser, Debug)]
struct Args {
    /// Path to hosts file
    #[arg(short, default_value = r"C:\Users\NemoNology\hosts.txt")]
    path: String,
    /// Server port
    #[arg(short, default_value = "60053")]
    server_port: u16,
    /// Resolving domain name
    #[arg()]
    domain_name: String,
    /// Server answer wait timeout (secs)
    #[arg(short = 't', default_value = "2")]
    server_answer_wait_timeout_secs: u64,
}

fn main() {
    let args = Args::parse();
    let client = match DnsClient::new(
        &args.path,
        args.server_port,
        args.server_answer_wait_timeout_secs,
    ) {
        Err(e) => {
            eprintln!(
                "Can't init DNS client with '{}' hosts path, '{}' server port and '{}' server answer wait timeout: {}",
                &args.path, &args.server_port, &args.server_answer_wait_timeout_secs, e);
            return;
        }
        Ok(client) => client,
    };

    println!(
        "Initialized DNS client with '{}' hosts path, '{}' server port and '{}' server answer wait timeout",
        &args.path, &args.server_port, &args.server_answer_wait_timeout_secs);
    let name = &args.domain_name;
    println!("Resolving '{}' domain name...", name);
    match client.resolve_in_hosts(name) {
        Ok(result) => match result {
            Some(addr) => {
                println!("Resolved at hosts to '{}' address", addr);
                return;
            }
            None => println!("Not found resolving address at hosts file"),
        },
        Err(e) => eprintln!("Can't resolve '{}' domain name in hosts: {}", name, e),
    };
    println!("Sending DNS query to server...");
    match client.send_dns_query(name.to_string()) {
        Ok(_) => println!("Successfully! Waiting answer from server..."),
        Err(e) => {
            eprintln!("Can't send query to server: {e}");
            return;
        }
    }
    match client.wait_server_answer_packet() {
        Ok(result) => match result.resolved_address {
            Some(addr) => {
                println!("Resolved at server to '{}' address", addr);
                return;
            }
            None => println!("Not found resolving address at server"),
        },
        Err(e) => eprintln!("Can't resolve domain name at server: {}", e),
    }
}
