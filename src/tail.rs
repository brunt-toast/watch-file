pub fn has_tail_flag(args: Vec<String>) -> bool {
    if args.contains(&String::from("--tail")) && args.contains(&String::from("-h")) {
        panic!("The --tail flag was passed with the -h flag");
    }

    return args.contains(&String::from("--tail")) || args.contains(&String::from("-h"));
}

pub fn get_tail_lines(args: Vec<String>) -> i32 {
    match args.iter().position(|s| s == "--tail") {
        Some(i) => {
            match args[i+1].parse::<i32>() {
                Ok(n) => { return n; }
                Err(_) => { panic!("Invalid value \"{}\" for --tail - could not convert to 32-bit int", args[i+1]); }
            }
        }
        None => {
            match args.iter().position(|s| s == "-t") {
                Some(i) => {
                    match args[i+1].parse::<i32>() {
                        Ok(n) => { return n; }
                        Err(_) => { panic!("Invalid value \"{}\" for -t - could not convert to 32-bit int", args[i+1]); }
                    }
                }
                None => { return 10; }
            }
        }
    }
}
