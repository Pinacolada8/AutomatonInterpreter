# Automaton Interpreter

**Description**:  Base code for creating automaton interpreters.


  - Developed in C# .Net Core 3.1
  - Cross Platform (Windows, Linux, MacOS)
  - **Status**:  Currently the project only contains code for a deterministic stack automaton.

## Dependencies

  - .Net Core 3.1 [Download1](https://dotnet.microsoft.com/download) [Download2](https://github.com/dotnet/core/blob/master/release-notes/3.1/3.1.8/3.1.402-download.md)

## Installation

Download and install .Net Core 3.1 [.Net Core Install instructions](https://github.com/dotnet/core/blob/master/release-notes/3.1/3.1.8/3.1.8-install-instructions.md).
You can use any IDE you feel confortable editing, but Visual Studio is preferred.

## Configuration

The logging output can be configured setting the environment variable "AUTOMATON_LOG_PRESET" according to the following enum:
````csharp
    [Flags]
    public enum LogConfigEnum
    {
        CONSOLE_DEFAULT = 0b0000_0001, // 1
        CONSOLE_COLORED = 0b0000_0010, // 2
        CONSOLE_ERRORS  = 0b0000_0100, // 4
        CONSOLE_DEBUG   = 0b0000_1000, // 8
        FILE            = 0b0001_0000, // 16

        // ---
        DEFAULT        = CONSOLE_DEFAULT,                                         // 5
        IMPROVED       = CONSOLE_COLORED | CONSOLE_ERRORS                 | FILE, // 22
        DEBUG_IMPROVED = CONSOLE_COLORED | CONSOLE_ERRORS | CONSOLE_DEBUG | FILE  // 30
    }
````
Example: 

- Linux:
````bash
export AUTOMATON_LOG_PRESET=6
````

- Windows:
````batch
setX AUTOMATON_LOG_PRESET 22
````

## Usage
Execution:
````bash
$ ./apd exemplo.json
````
Input file example:
````json
{
  "ap": [
    [ "i", "d" ],
    [ "0", "1" ],
    [ "Z", "U", "F" ],
    [
      [ "i", "0", "#", "d", "ZF" ],
      [ "i", "1", "#", "d", "UF" ],
      [ "d", "0", "Z", "d", "ZZ" ],
      [ "d", "0", "U", "d", "#" ],
      [ "d", "1", "U", "d", "UU" ],
      [ "d", "1", "Z", "d", "#" ],
      [ "d", "#", "F", "i", "#" ]
    ],
    "i",
    [ "i" ]
  ]
}
````

## How to test the software

For compiling the code use the following commands:
- Linux:
  - option 1:
    ````bash
    $ dotnet publish App/App.csproj /p:PublishProfile=App/Properties/PublishProfiles/LinuxX64.pubxml
    ````
  - option 2:
    ````bash
    $ dotnet publish App/App.csproj -c Release -r linux-x64 /p:PublishSingleFile=true --output ./bin/LinuxX64
    ````

- Windows:
  - option 1:
    ````batch
    > dotnet publish App/App.csproj /p:PublishProfile=App/Properties/PublishProfiles/WinX64.pubxml
    ````
  - option 2:
    ````batch
    > dotnet publish App/App.csproj -c Release -r win-x64 /p:PublishSingleFile=true --output ./bin/WindowsX64
    ````

- Windows X86:
  - option 1:
    ````batch
    > dotnet publish App/App.csproj /p:PublishProfile=App/Properties/PublishProfiles/WinX86.pubxml
    ````

For custom compilation settings search [.Net Build Instructions](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-build)

----

## Contributors

- Matheus Dutra Cerbino

## ISSUES

If you have questions, concerns, bug reports, etc, please file an issue in this repository's Issue Tracker.

----

## Credits and references

1. Project made as a homework for "LINGUAGENS FORMAIS E AUTÔMATOS" class of CEFET-MG college.
2. Project GitHub [GitHub](https://github.com/Pinacolada8/AutomatonInterpreter)
