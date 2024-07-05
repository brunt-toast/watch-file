pub fn has_head_flag(args: Vec<String>) -> bool {
    if args.contains(&String::from("--head")) && args.contains(&String::from("-h")) {
        panic!("The --head flag was passed with the -h flag");
    }

    return args.contains(&String::from("--head")) || args.contains(&String::from("-h"));
}

pub fn get_head_lines(args: Vec<String>) -> i32 {
    match args.iter().position(|s| s == "--head") {
        Some(i) => {
            match args[i+1].parse::<i32>() {
                Ok(n) => { return n; }
                Err(_) => { panic!("Invalid value \"{}\" for --head - could not convert to 32-bit int", args[i+1]); }
            }
        }
        None => {
            match args.iter().position(|s| s == "-h") {
                Some(i) => {
                    match args[i+1].parse::<i32>() {
                        Ok(n) => { return n; }
                        Err(_) => { panic!("Invalid value \"{}\" for -h - could not convert to 32-bit int", args[i+1]); }
                    }
                }
                None => { return 10; }
            }
        }
    }
}
