use std::error::Error;
use std::fmt;
use std::fs::{File, OpenOptions};
use std::io::{prelude::Write, BufRead, BufReader};

use crate::record::errors::DnsRecordParseError;
use crate::record::DnsRecord;

#[derive(Debug, Clone)]
pub struct DnsRecordsTableParseError {
    pub line_number: usize,
    pub inner_error: DnsRecordParseError,
}

impl fmt::Display for DnsRecordsTableParseError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(
            f,
            "Unexpected DNS record at line {}. {}",
            self.line_number, self.inner_error
        )
    }
}

impl Error for DnsRecordsTableParseError {}

pub struct DnsRecordsTableController<'a> {
    pub records_table_path: &'a str,
}

impl<'a> DnsRecordsTableController<'a> {
    pub fn new(records_table_path: &'a str) -> Self {
        DnsRecordsTableController { records_table_path }
    }

    pub fn get_records_list(&self) -> Result<Vec<DnsRecord>, Box<dyn Error>> {
        let file = File::open(&self.records_table_path)?;
        let reader = BufReader::new(file);
        let mut records: Vec<DnsRecord> = Vec::new();
        let mut line_number: usize = 0;

        for line in reader.lines() {
            let line = line?;
            line_number += 1;
            let dns_record = DnsRecord::try_from(line.as_str());
            if let Err(inner_error) = dns_record {
                return Err(Box::new(DnsRecordsTableParseError {
                    line_number,
                    inner_error,
                }));
            }

            records.push(dns_record.unwrap());
        }

        Ok(records)
    }

    pub fn add_record_to_table(&self, record: &DnsRecord) -> Result<(), Box<dyn Error>> {
        let mut file = OpenOptions::new()
            .write(true)
            .append(true)
            .open(&self.records_table_path)?;
        let s = format!("{:?}", record);
        writeln!(file, "{}", s)?;
        Ok(())
    }
}
