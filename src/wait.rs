pub fn get_wait_interval(args: Vec<String>) -> u64 {
    match args.iter().position(|s| s == "--interval") {
        Some(i) => {
            match args[i+1].parse::<u64>() {
                Ok(n) => { return n; }
                Err(_) => { panic!("Invalid value \"{}\" for --interval - could not convert to 64-bit unsigned int", args[i+1]); }
            }
        }
        None => {
            match args.iter().position(|s| s == "-i") {
                Some(i) => {
                    match args[i+1].parse::<u64>() {
                        Ok(n) => { return n; }
                        Err(_) => { panic!("Invalid value \"{}\" for -i - could not convert to 64-bit unsigned int", args[i+1]); }
                    }
                }
                None => { return 1000; }
            }
        }
    }
}
