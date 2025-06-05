# NodePacker

A powerful C# tool that packages Node.js applications into standalone executables. Convert your Node.js projects into self-contained `.exe` files that can run on any Windows machine without requiring Node.js to be installed.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-4.0+-blue.svg)](https://dotnet.microsoft.com/)
[![Node.js](https://img.shields.io/badge/Node.js-LTS-green.svg)](https://nodejs.org/)

## Features

- **Zero Dependencies**: Creates completely self-contained executables
- **Automatic Node.js Download**: Fetches the latest LTS version automatically
- **Smart Packaging**: Excludes existing node_modules and installs fresh dependencies
- **Cross-Platform Support**: Works on Windows, Linux, and macOS
- **Professional Architecture**: Clean, maintainable code with proper separation of concerns
- **Easy to Use**: Simple command-line interface

## Quick Start

### Prerequisites

- .NET Framework 4.0 or higher
- Visual Studio or .NET SDK (for compilation)
- Internet connection (for downloading Node.js)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/imtaqin/NODEPACKER.git
cd NODEPACKER
```

2. Build the project:
```bash
dotnet build
```

### Usage

```bash
NodeJsPacker.exe <source-directory> <output-executable>
```

**Example:**
```bash
NodeJsPacker.exe "C:\MyNodeApp" "C:\Output\MyApp.exe"
```

This will:
1. Download the latest Node.js LTS version
2. Package your application with Node.js runtime
3. Create a standalone executable that runs your app

## Project Structure

```
NodeJsPacker/
├── Program.cs                          # Entry point
├── Models/
│   └── NodeJsRelease.cs               # Node.js release data model
├── Services/
│   ├── NodeJsPackerService.cs         # Main orchestration service
│   ├── NodeJsDownloader.cs            # Handles Node.js downloads
│   ├── ApplicationPackager.cs         # Packages the application
│   ├── ExecutableGenerator.cs         # Creates the final executable
│   └── Interfaces/                    # Service interfaces
├── Utils/
│   ├── PlatformHelper.cs              # Platform detection utilities
│   ├── FileHelper.cs                  # File operations
│   ├── DirectoryHelper.cs             # Directory operations
│   ├── ProcessHelper.cs               # Process execution utilities
│   └── CompilerHelper.cs              # C# compiler utilities
└── Templates/
    └── LauncherCodeTemplate.cs        # Generated launcher code
```

## How It Works

1. **Download**: Automatically downloads the latest Node.js LTS version
2. **Package**: Copies your application files (excluding node_modules)
3. **Dependencies**: Installs production dependencies using npm
4. **Bundle**: Creates a launcher application that extracts and runs your Node.js app
5. **Compile**: Generates a standalone executable with embedded resources

## Requirements

### Your Node.js Application

- Must have a `package.json` file (recommended)
- Should specify a main entry point in package.json
- If no main entry is specified, NodePacker will look for:
  - `index.js`
  - `app.js`
  - `main.js`
  - `server.js`

### System Requirements

- **Windows**: .NET Framework 4.0+, Visual Studio or Build Tools
- **Linux/macOS**: Mono or .NET Core

## Command Line Options

```bash
NodeJsPacker <source-directory> <output-executable>
```

- `<source-directory>`: Path to your Node.js application folder
- `<output-executable>`: Path where the executable will be created

## Advanced Usage

### Custom Entry Point

When running the generated executable, you can specify a custom entry point:

```bash
MyApp.exe server.js
```

### Passing Arguments

You can pass arguments to your Node.js application:

```bash
MyApp.exe server.js --port 3000 --env production
```

## Architecture

NodePacker follows clean architecture principles:

- **Services**: Core business logic with dependency injection ready
- **Models**: Data structures and DTOs
- **Utils**: Helper classes and utilities
- **Templates**: Code generation templates
- **Interfaces**: Contracts for better testability

## Contributing

We welcome contributions! Here's how you can help:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Setup

1. Clone the repository
2. Open in Visual Studio or your preferred IDE
3. Restore NuGet packages
4. Build and run tests

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Known Issues

- Large applications may take some time to package
- Antivirus software might flag the executable (false positive)
- Cross-compilation between different platforms not fully supported yet

## Roadmap

- [ ] Support for custom Node.js versions
- [ ] GUI interface
- [ ] Docker support
- [ ] Plugin system for custom packaging logic
- [ ] Improved cross-platform compilation
- [ ] Size optimization features

## Support

If you encounter any issues or have questions:

1. Check the [Issues](https://github.com/imtaqin/NODEPACKER/issues) page
2. Create a new issue with detailed information
3. Join our discussions in the [Discussions](https://github.com/imtaqin/NODEPACKER/discussions) tab

## Acknowledgments

- Node.js team for the amazing runtime
- Microsoft for .NET Framework
- All contributors who help improve this project

---

**Made with love by [imtaqin](https://github.com/imtaqin)**

If this project helped you, please consider giving it a star!