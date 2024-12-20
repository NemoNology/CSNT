use clap::Parser;
use dns::{record::DnsRecord, records_table_controller::DnsRecordsTableController};
use std::{error::Error, net::Ipv4Addr};

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

fn main() -> Result<(), Box<dyn Error>> {
    let args = Args::parse();
    let controller = DnsRecordsTableController::new(&args.path);
    let domain_name = args.domain_name;
    let address = args.address;

    if args.list {
        for record in controller.get_records_list()? {
            println!("{:?}", record);
        }
        return Ok(());
    }

    if domain_name.is_none() || address.is_none() {
        return Err("Expected provided domain name and/or address for adding".into());
    }

    let domain_name = domain_name.unwrap();

    let address: Ipv4Addr = address.unwrap().parse()?;
    controller.add_record_to_table(
        &(DnsRecord {
            domain_name,
            address,
        }),
    )?;

    Ok(())
}

#[cfg(test)]
mod tests {
    use std::net::Ipv4Addr;

    use dns::{record::DnsRecord, records_table_controller::DnsRecordsTableController};

    #[test]
    fn list() {
        let path = r"C:\Users\NemoNology\records.txt";
        let controller = DnsRecordsTableController::new(&path);
        for record in controller.get_records_list().unwrap() {
            println!("{:?}", record);
        }
    }

    #[test]
    fn add() {
        let path = r"C:\Users\NemoNology\records.txt";
        let controller = DnsRecordsTableController::new(&path);
        let domain_name = "wiki.net".to_string();
        let address = Ipv4Addr::new(11, 12, 13, 14);
        let _ = controller.add_record_to_table(&DnsRecord {
            domain_name,
            address,
        });
    }
}
