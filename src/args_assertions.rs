use regex::Regex;

pub fn has_short_flag(args: Vec<String>, c: char) -> bool {
    let re = Regex::new("^-[A-Za-z0-9]+$").unwrap();

    for arg in args {
        if re.is_match(&arg) && arg.contains(c) {
            return true;
        }
    }

    return false;
}

pub fn ensure_args_recognised(args: Vec<String>) {
    let valid_args = [
        "-h",
        "--head",
        "-t",
        "--tail",
        "--no-header",
        "--no-footer",
        "-r",
        "--raw",
        "-i",
        "--interval",
        "-d",
        "--diff",
        "-o",
        "--output",
        "--output-mode",
        "-q",
        "--quiet",
        "-h",
        "--help",
        "-g",
        "--grep",
    ];

    for arg in args
        .into_iter()
        .filter(|s| s.chars().nth(0).unwrap() == '-')
    {
        if !valid_args.contains(&arg.as_str()) {
            panic!("Unrecognised argument: {}", arg);
        }
    }
}

pub fn ensure_none_conflicting(args: Vec<String>) {
    let exclusives = [
        ["--head", "-h"],
        ["--tail", "-t"],
        ["--interval", "-i"],
        ["--output", "-o"],
    ];

    for exclusive in exclusives {
        if exclusive
            .iter()
            .filter(|&item| args.contains(&item.to_string()))
            .count()
            >= 2
        {
            panic!(
                "Argument conflict: Only 1 of {} can be used",
                exclusive.join(", ")
            );
        }
    }
}

pub fn validate_value_types(args: Vec<String>) {
    ensure_i32_values(args.clone());
    ensure_u64_values(args.clone());
}

fn ensure_i32_values(args: Vec<String>) {
    let i32_keys = ["--head", "-h", "--tail", "-t"];

    for arg in &args {
        if !i32_keys.contains(&&arg.as_str()) {
            continue;
        }

        match args.iter().position(|s| *s == *arg) {
            Some(i) => match args[i + 1].parse::<i32>() {
                Ok(_) => {}
                Err(_) => {
                    panic!(
                        "Invalid value \"{}\" for {} - could not convert to 32-bit int",
                        args[i + 1],
                        args[i]
                    );
                }
            },
            None => {
                panic!("List item disappeared randomly??");
            }
        }
    }
}

fn ensure_u64_values(args: Vec<String>) {
    let u64_keys = ["--interval", "-i"];

    for arg in &args {
        if !u64_keys.contains(&&arg.as_str()) {
            continue;
        }

        match args.iter().position(|s| *s == *arg) {
            Some(i) => {
                match args[i + 1].parse::<u64>() {
                    Ok(_) => {}
                    Err(_) => {
                        panic!("Invalid value \"{}\" for {} - could not convert to unsigned 64-bit int", args[i+1], args[i]);
                    }
                }
            }
            None => {
                panic!("List item disappeared randomly??");
            }
        }
    }
}
