use std::error::Error;
use std::fs::{File, OpenOptions};
use std::io::{BufRead, BufReader, Write};
use std::str::FromStr;

use errors::DnsRecordsTableParseError;

use crate::record::DnsRecord;

pub struct DnsRecordsTableController<'a>(&'a str);

impl<'a> DnsRecordsTableController<'a> {
    pub fn new(records_table_path: &'a str) -> Self {
        DnsRecordsTableController(records_table_path)
    }

    pub fn get_records_list(&self) -> Result<Vec<DnsRecord>, Box<dyn Error>> {
        let file = File::open(&self.0)?;
        let mut records: Vec<DnsRecord> = Vec::new();

        // Read lines with their line number
        for (line_number, line) in BufReader::new(file).lines().enumerate() {
            let line = line?;
            // Handle errors/Parse record and push it
            records.push(match DnsRecord::from_str(&line) {
                Ok(record) => record,
                Err(inner_error) => {
                    return Err(Box::new(DnsRecordsTableParseError {
                        line_number,
                        inner_error,
                    }))
                }
            });
        }

        Ok(records)
    }

    pub fn add_record_to_table(&self, record: &DnsRecord) -> Result<(), Box<dyn Error>> {
        let mut file = OpenOptions::new().write(true).append(true).open(&self.0)?;
        writeln!(file, "{} {}", record.domain_name, record.address)?;
        Ok(())
    }
}

pub mod errors {
    use std::{error::Error, fmt};

    use crate::record::errors::DnsRecordParseError;

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
}
