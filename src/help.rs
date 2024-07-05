use man::prelude::*;
use std::fs::File;
use std::io::Write;
use std::process::Command;

fn generate_man_page() -> String {
    let page = Manual::new("watch-file")
        .about("watch-file - watch a file for changes")
        .author(Author::new("Josh Brunton").email("josh.brunton@proton.me"))
        .option(
            Opt::new("head")
                .short("-h")
                .long("--head")
                .help("Output only the first n lines. n must be a 32-bit integer. Defaults to 10 if no value is given.")
        )
        .option(
            Opt::new("tail")
                .short("-t")
                .long("--tail")
                .help("Output only the last n lines. n must be a 32-bit integer. Defaults to 10 if no value is given.")
        )
        .option(
            Opt::new("interval")
                .short("-i")
                .long("--interval")
                .help("How often to check the file for changes, in milliseconds. Must be an unsigned 64-bit integer. Defaults to 1000 if no value is given.")
        )
        .option(
            Opt::new("output")
                .short("-o")
                .long("--output")
                .help("A file to output to. See also --output-mode.")
        )
        .option(
            Opt::new("output-mode")
                .long("--output-mode")
                .help("When passing the --output flag, should we append to the output file, or overwrite it? Default behaviour is append. Only has effect when passed with --output.")
        )
        .flag(
            Flag::new()
                .long("--no-header")
                .help("Skip outputting the header (which shows the filename and time of read)")
        )
        .flag(
            Flag::new()
                .long("--no-footer")
                .help("Skip outputting the footer (which shows the filename and time of read)")
        )
        .flag(
            Flag::new()
                .short("-r")
                .long("--raw")
                .help("Equivalent to passing --no-header and --no-footer together.")
        )
        .flag(
            Flag::new()
                .short("-d")
                .long("--diff")
                .help("Instead of displaying the pure contents ot the file, generate a patch from the previous version each time it changes.")
        )
        .flag(
            Flag::new()
                .short("-q")
                .long("--quiet")
                .help("Don't output anything to the console. Only useful when used with --output.")
        )
        .render();

    return format!("{}", page);
}

pub fn install_man_page() {
    let content = generate_man_page();
    let mut file = File::create("/usr/share/man/man1/watch-file.1").expect("Could not create the file at /usr/share/man/man1/watch-file.1");
    file.write_all(content.as_bytes()).expect("Could not write to the file");
}

pub fn print_man_page() {
    let output = Command::new("man")
        .args(&["-P", "cat", "/usr/share/man/man1/watch-file.1"])
        .output()
        .expect("Could not apply man to the manfile");
    let output_str = String::from_utf8_lossy(&output.stdout);
    println!("{}", output_str);
}
