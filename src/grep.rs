use regex::Regex;

pub fn has_grep_flag(args: Vec<String>) -> bool {
    return args.contains(&String::from("--grep")) || args.contains(&String::from("-g"));
}

pub fn get_grep_regex(args: Vec<String>) -> Regex {
    match args.iter().position(|s| s == "--grep") {
        Some(i) => Regex::new(&args[i + 1]).unwrap(),
        None => match args.iter().position(|s| s == "-t") {
            Some(i) => Regex::new(&args[i + 1]).unwrap(),
            None => Regex::new(".*").unwrap(),
        },
    }
}
