use std::{env, error::Error};

use clap::Parser;

#[derive(Parser, Debug)]
#[command(about="", long_about = None)]
pub struct DnsClientArgs {
    /// Is DNS-client running as service
    #[arg(short = 'm', long = "service")]
    pub is_service_mode: bool,
    /// Port of DNS-server
    #[arg(short = 's', long = "server-port", default_value = "53")]
    pub server_port: Option<u16>,
    /// Domain name, which need to be resolved
    #[arg(short = 'n', long = "domain-name")]
    pub domain_name: Option<String>,
    /// Path to file contains hosts list
    #[arg(short = 'p', long = "hosts-path")]
    pub hosts_path: Option<String>,
}

impl DnsClientArgs {
    /// Create new DnsClientArgs instance.  
    /// ### Notes
    /// - If `hosts_path` is None, then it will be default OS hosts file path
    pub fn new() -> DnsClientArgs {
        let args = DnsClientArgs::parse();

        let hosts_path = args.hosts_path.or_else(|| {
            let hosts_path = match env::consts::OS {
                "windows" => "c:\\windows\\system32\\drivers\\etc\\hosts",
                "linux" => "/etc/hosts",
                "macos" => "/private/etc/hosts",
                _ => return None,
            };
            Some(hosts_path.to_string())
        });

        Self {
            is_service_mode: args.is_service_mode,
            server_port: args.server_port,
            domain_name: args.domain_name,
            hosts_path,
        }
    }
}
