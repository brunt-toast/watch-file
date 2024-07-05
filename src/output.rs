use std::fs::File;
use std::fs::OpenOptions;
use std::io::Write;

pub fn has_output_flag(args: Vec<String>) -> bool {
    return args.contains(&String::from("--output")) || args.contains(&String::from("-o"));
}

pub fn get_output_file(args: Vec<String>) -> String {
    match args.iter().position(|s| s == "--output") {
        Some(i) => { return format!("{}", args[i+1]); }
        None => {
            match args.iter().position(|s| s == "-o") {
                Some(i) => { return format!("{}", args[i+1]); }
                None => { panic!("No value passed for output") }
            }
        }
    }
}

pub fn should_overwrite_output_file(args: Vec<String>) -> bool {
    match args.iter().position(|s| s == "--output-mode") {
        Some(i) => { 
            match &args[i+1].as_str() {
                &"append" => false,
                &"overwrite" => true,
                _ => { panic!("Unrecognised value for --output-mode"); }
            } 
        }
        None => { return false; }
    }
}

fn ensure_file_exists(path: &str) {
    if !std::path::Path::new(path).exists() {
        File::create(path).expect("Could not create file");
    }
}

pub fn write_to_file(path: &str, content: &str, overwrite: bool) {
    ensure_file_exists(path);

    if overwrite {
        let mut file = File::create(path).expect("Could not create file");
        file.write_all(content.as_bytes()).expect("Could not write to file");
    } else {
        let mut file = OpenOptions::new().append(true).open(path).expect("Could not open file");
        file.write_all(content.as_bytes()).expect("Could not write to file");
    }
}
