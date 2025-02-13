use chrono;
use diffy::{create_patch, PatchFormatter};
use std::fs::metadata;
use std::path::Path;
use std::{env, thread, time};
use std::{fs, process};

mod args_assertions;
mod grep;
mod head;
mod help;
mod output;
mod tail;
mod wait;

fn main() {
    let args: Vec<_> = env::args().collect();
    args_assertions::ensure_args_recognised(args.clone());
    args_assertions::ensure_none_conflicting(args.clone());
    args_assertions::validate_value_types(args.clone());

    let help = args.contains(&String::from("--help"))
        || args_assertions::has_short_flag(args.clone(), 'h');
    if help {
        help::print_man_page();
        process::exit(0);
    }
    help::install_man_page();

    let head = head::has_head_flag(args.clone());
    let n_head = head::get_head_lines(args.clone());

    let tail = tail::has_tail_flag(args.clone());
    let n_tail = tail::get_tail_lines(args.clone());

    let grep = grep::has_grep_flag(args.clone());
    let grep_filter = grep::get_grep_regex(args.clone());

    let raw =
        args.contains(&String::from("--raw")) || args_assertions::has_short_flag(args.clone(), 'r');
    let no_header = args.contains(&String::from("--no-header")) || raw;
    let no_footer = args.contains(&String::from("--no-footer"));

    let use_diff = args.contains(&String::from("--diff"))
        || args_assertions::has_short_flag(args.clone(), 'r');

    let wait_interval = wait::get_wait_interval(args.clone());

    let quiet = args.contains(&String::from("--quiet"))
        || args_assertions::has_short_flag(args.clone(), 'q');

    let has_output_flag = output::has_output_flag(args.clone());
    let output_file = if has_output_flag {
        output::get_output_file(args.clone())
    } else {
        String::new()
    };
    let overwrite_output = output::should_overwrite_output_file(args.clone());

    let path = Path::new(&args[1]);
    if !path.exists() {
        panic!("Path \"{:?}\" does not exist", path);
    }
    match metadata(&path) {
        Ok(m) => {
            if !m.is_file() {
                panic!("\"{:?}\": not a file", path);
            }
        }
        Err(_) => {
            panic!("Failed to get metadata for {:?}", path);
        }
    }

    let mut last_content = String::new();
    loop {
        match fs::read_to_string(path) {
            Ok(content) => {
                if content != last_content {
                    let mut presentable_content = content.clone();

                    if use_diff {
                        let patch = create_patch(&last_content, &content);
                        presentable_content =
                            format!("{}", PatchFormatter::new().with_color().fmt_patch(&patch));
                    }

                    if head {
                        presentable_content = presentable_content
                            .split("\n")
                            .take(n_head as usize)
                            .collect::<Vec<&str>>()
                            .join("\n");
                    } else if tail {
                        let split_content: Vec<_> = presentable_content.split("\n").collect();
                        presentable_content = split_content
                            [(split_content.len() as i32 - n_tail) as usize..split_content.len()]
                            .join("\n");
                    }

                    if grep {
                        presentable_content = presentable_content
                            .split("\n")
                            .filter(|x| grep_filter.is_match(x))
                            .collect::<Vec<&str>>()
                            .join("\n");
                    }

                    if !quiet {
                        if !no_header {
                            println!(
                                "=== START {:?} at {} ===",
                                path,
                                chrono::offset::Local::now()
                            );
                        }
                        println!("{}", presentable_content);
                        if !no_footer {
                            println!("=== END {:?} at {} ===", path, chrono::offset::Local::now());
                        }
                    }

                    if has_output_flag {
                        output::write_to_file(&output_file, &presentable_content, overwrite_output);
                    }

                    last_content = content;
                }
                thread::sleep(time::Duration::from_millis(wait_interval));
            }
            Err(_) => {}
        }
    }
}
