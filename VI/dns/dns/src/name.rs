use errors::BadCharError;

#[derive(Debug, PartialEq)]
pub struct DomainName(String);

impl DomainName {
    pub const INVALID_SYMBOLS: [char; 18] = [
        ',', '~', ':', '!', '@', '#', '$', '%', '^', '&', '\'', '(', ')', '[', ']', '{', '}', ' ',
    ];

    pub fn build(name: String) -> Result<Self, BadCharError> {
        for bad_char in Self::INVALID_SYMBOLS {
            if let Some(index) = name.find(bad_char) {
                return Err(BadCharError(bad_char, index));
            }
        }
        Ok(DomainName(name))
    }
}

pub mod errors {
    #[derive(Debug, Clone)]
    pub struct BadCharError(pub char, pub usize);

    impl std::fmt::Display for BadCharError {
        fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
            write!(f, "Bad char '{}' at {} column", self.0, self.1)
        }
    }

    impl std::error::Error for BadCharError {}
}
