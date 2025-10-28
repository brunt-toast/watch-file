<div>
<span style="text-align: center;">
<h1>watch-file</h1>
<p>
    A command-line utility for observing change (not growth!) in files. 
</p>

[![Git](https://img.shields.io/badge/Git-F05032?logo=git&logoColor=fff)](#) [![GitHub](https://img.shields.io/badge/GitHub-%23121011.svg?logo=github&logoColor=white)](#) [![GitHub Actions](https://img.shields.io/badge/GitHub_Actions-2088FF?logo=github-actions&logoColor=white)](#) [![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff)](#) [![C#](https://custom-icon-badges.demolab.com/badge/C%23-%23239120.svg?logo=cshrp&logoColor=white)](#) [![Linux](https://img.shields.io/badge/Linux-FCC624?logo=linux&logoColor=black)](#) [![Windows](https://custom-icon-badges.demolab.com/badge/Windows-0078D6?logo=windows11&logoColor=white)](#) 
</span>
</div>

<hr />

This tool acknowledges that change and growth are different.  Unlike `tail -f` and similar utilities, we reconsider the whole file when it changes. 

## Installation

This tool isn't currently available on any package managers. 

## Compile From Source

As a pre-requisite, you'll need the .NET 9 SDK. The shortest robust way to compile and install the tool is: 
```bash
dotnet tool uninstall -g WatchFile.Cli
dotnet tool restore
dotnet cake --target Pack
dotnet tool install -g --add-source ./src/WatchFile.Cli/bin/nupkg WatchFile.Cli
```

As a one-liner: 
```bash
dotnet tool uninstall -g WatchFile.Cli; dotnet tool restore && dotnet cake --target Pack && dotnet tool install -g --add-source ./src/WatchFile.Cli/bin/nupkg WatchFile.Cli;
```

## License

The MIT License (MIT)

Copyright 2025 Josh Brunton

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

## Credits &amp; Notices

Thanks to [Inter](https://github.com/inttter/md-badges) for the badges displayed in this README, which are released under the MIT license. 

Thanks to [the .NET Foundation and Contributors](https://github.com/dotnet/dotnet), who publish the .NET SDK under the MIT license. 

Thanks to mmanela, who publishes [DiffPlex](https://github.com/mmanela/diffplex/) under the Apache-2.0 license.