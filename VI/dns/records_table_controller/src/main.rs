use clap::Parser;
use dns::{
    name::DomainName, record::DnsRecord, records_table_controller::DnsRecordsTableController,
};
use std::net::Ipv4Addr;

#[derive(Parser, Debug)]
#[command(version, about, long_about = None)]
struct Args {
    /// Path to table file.  
    /// Will be created if not exists
    #[arg(short, long)]
    path: String,
    /// Display list of records.
    #[arg(short, long, default_value = "false")]
    list: bool,
    /// Adding domain name.  
    /// Require `address` provided argument
    #[arg(short = 'n', long)]
    domain_name: Option<String>,
    /// Adding address record.
    /// Require `domain_name` provided argument
    #[arg(short, long)]
    address: Option<String>,
}

fn main() {
    let args = Args::parse();
    let controller = DnsRecordsTableController::new(&args.path);

    if args.list {
        println!(
            "DNS records table controller getting records list from file {}",
            &args.path
        );
        // Handle errors
        // Print every record
        match controller.get_records_list() {
            Err(e) => eprintln!("Can't get records list: {}", e),
            Ok(records) => records
                .iter()
                .map(|record| println!("{};", record))
                .collect(),
        }

        return;
    }

    let (domain_name, address) = match (args.domain_name, args.address) {
        (Some(name), Some(addr)) => (name, addr),
        _ => {
            eprintln!("Expected provided domain name and address for adding");
            return;
        }
    };

    let domain_name = match DomainName::build(domain_name.clone()) {
        Err(e) => {
            eprintln!("Can't parse domain name from '{}': {}", &domain_name, e);
            return;
        }
        Ok(name) => name,
    };

    let address = match address.parse::<Ipv4Addr>() {
        Err(e) => {
            eprintln!("Can't parse IPv4 address from '{}': {}", address, e);
            return;
        }
        Ok(addr) => addr,
    };

    match controller.add_record_to_table(&DnsRecord::new(domain_name, address)) {
        Err(e) => eprintln!("Can't add record to table: {}", e),
        _ => (),
    }
}

#[cfg(test)]
mod tests {
    use std::{net::Ipv4Addr, str::FromStr};

    use dns::{
        name::DomainName, record::DnsRecord, records_table_controller::DnsRecordsTableController,
    };

    #[test]
    fn list() {
        let path = r"C:\Users\NemoNology\records.txt".to_string();
        let records = vec![
            DnsRecord::from_str("ya.ru 44.55.66.55"),
            DnsRecord::from_str("oizi.dms.zab 10.11.3.139"),
            DnsRecord::from_str("paint.local 127.0.1.25"),
            DnsRecord::from_str("rutube.ru 44.55.44.55"),
            DnsRecord::from_str("работа.рф 55.55.55.55"),
            DnsRecord::from_str("wiki.net 11.12.13.14"),
        ];
        let controller = DnsRecordsTableController::new(&path);
        for (provided_record, expected_record) in
            controller.get_records_list().unwrap().iter().zip(records)
        {
            assert_eq!(provided_record, &expected_record.unwrap());
        }
    }

    #[test]
    fn add() {
        let path = r"C:\Users\NemoNology\records.txt".to_string();
        let controller = DnsRecordsTableController::new(&path);
        let domain_name = DomainName::build("wiki.net".to_string()).unwrap();
        let address = Ipv4Addr::new(11, 12, 13, 14);
        let _ = controller.add_record_to_table(&DnsRecord {
            domain_name,
            address,
        });
    }
}
