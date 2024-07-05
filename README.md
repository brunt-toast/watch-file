# watch-file

A simple CLI tool to watch files for changes. 

Sure, there are probably a million other tools that do this, including `tail -f` being built into most, if not all, linux distros. 

This package has some opinions that I thought to make it a bit more user-friendly: 
* It works just about anywhere - particularly, where more common solutions aren't as readily available. I didn't want to have to open git bash on windows just to use `tail -f`. 
* Its defaults assume you're a human looking at a screen, not a piece of code reading data. Unless you specify otherwise, you'll see other important, such as the last time a change was noticed. 
* It recognises that change and growth are different. Many solutions assume you're watching a log file and that its only changes will be new lines at the bottom of the file. This tool recognises that that's not everyone's use case. 

# Installation 
Installation is pretty simple. No setup, minimal prerequisites - just make sure you've got [cargo](https://doc.rust-lang.org/cargo/getting-started/installation.html). <small>*Maybe one day I'll put it on [crates.io](https://crates.io) and/or make some pre-compiled binaries!*</small>

```bash
cargo install --git https://github.com/brunt-toast/watch-file.git
```

If you want to edit the code, it's still just a simple rust project. 
```bash
git clone https://github.com/brunt-toast/watch-file.git
cd watch-file
cargo build
```

# Features
* **Diff mode** (`[--diff|-d]`) - Generate a patch between the current version of the file, and how it was the last time we read it. 

# Contribute
Got an idea on how to make the project better? Feel free to send in a PR! 

# Bugs, suggestions, etc 
If you find a bug, have an idea you can't implement yourself, or have anything else fitting, please open an issue on GitHub. 

# License
This project is under the GNU public license. See the [LICENSE](./LICENSE) file. 
