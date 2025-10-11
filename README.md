# HL7 TCP to HTTPS Bridge

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-blue.svg)](https://dotnet.microsoft.com/download/dotnet-framework)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)](https://www.microsoft.com/windows)

A robust Windows service that bridges legacy HL7 systems with modern HTTPS endpoints. This service listens for HL7 messages on a TCP port and securely forwards them to any HTTPS endpoint, enabling seamless integration between healthcare systems.

## 🚀 Quick Start

1. **Download** the latest release or clone this repository
2. **Configure** your endpoints in `App.config`
3. **Install** as a Windows service
4. **Start** forwarding HL7 messages!

## 📋 Overview

This lightweight C# Windows service acts as a reliable bridge between legacy HL7 systems and modern web APIs. Perfect for healthcare organizations needing to integrate older systems with cloud-based or web-based healthcare platforms.

### Key Capabilities
- 🔄 **Real-time Message Forwarding**: Instantly forwards HL7 messages from TCP to HTTPS
- 🛡️ **Secure Authentication**: Built-in support for HTTP Basic Authentication
- 📊 **Comprehensive Logging**: Detailed event logging to Windows Event Viewer
- ⚙️ **Easy Configuration**: Simple XML configuration file
- 🔧 **Production Ready**: Runs as a Windows service with automatic startup

## ✨ Features

| Feature | Description |
|---------|-------------|
| **TCP Listener** | Configurable IP and port for incoming HL7 messages |
| **HTTPS Forwarding** | Secure message forwarding to any HTTPS endpoint |
| **Authentication** | Username/password authentication for target endpoints |
| **Error Handling** | Robust error handling with detailed logging |
| **Multi-threading** | Concurrent connection handling for high throughput |
| **Windows Service** | Runs continuously in background with system startup |

## 📋 Prerequisites

- **Operating System**: Windows 7/Server 2008 R2 or later
- **Framework**: .NET Framework 4.7.2 or higher
- **Privileges**: Administrator rights for service installation
- **Development** (optional): Visual Studio 2017 or later for building from source

## ⚙️ Configuration

### Step 1: Create Configuration File
```cmd
copy HL7forwardTCPtoHTTPS\App.config.template HL7forwardTCPtoHTTPS\App.config
```

### Step 2: Edit Configuration
Open `App.config` and configure your endpoints:

```xml
<appSettings>
    <!-- TCP Listener Settings -->
    <add key="ReceiveToIP" value="127.0.0.1" />
    <add key="ReceiveToPort" value="7777" />

    <!-- HTTPS Endpoint Settings -->
    <add key="ForwardToHost" value="https://your-api.example.com/hl7" />
    <add key="ForwardToUser" value="your-username" />
    <add key="ForwardToPassword" value="your-password" />
</appSettings>
```

### Configuration Reference

| Parameter | Description | Example |
|-----------|-------------|---------|
| `ReceiveToIP` | IP address to listen on | `127.0.0.1` (localhost) or `0.0.0.0` (all interfaces) |
| `ReceiveToPort` | TCP port for incoming HL7 messages | `7777`, `2575`, `6661` |
| `ForwardToHost` | Target HTTPS endpoint URL | `https://api.example.com/hl7/messages` |
| `ForwardToUser` | Username for HTTP Basic Auth | `hl7user` |
| `ForwardToPassword` | Password for HTTP Basic Auth | `secure-password` |

> **Security Note**: Store credentials securely and consider using Windows credential storage for production deployments.

## 🔧 Installation

### Option 1: Using Pre-built MSI Installer (Recommended)

1. Download `SetupHL7endpointTCPlistener.msi` from the `SetupHL7forwardTCPtoHTTPS` folder
2. Run the installer as Administrator
3. Follow the installation wizard
4. Configure the service using the steps above

### Option 2: Manual Installation

#### Build from Source
```bash
# Clone the repository
git clone https://github.com/vsaturnino/tcphl7lisenter.git
cd tcphl7lisenter

# Open in Visual Studio
start HL7forwardTCPtoHTTPS/HL7forwardTCPtoHTTPS.sln

# Build the solution (Ctrl+Shift+B)
```

#### Install as Windows Service
```cmd
# Open Command Prompt as Administrator
# Navigate to .NET Framework directory
cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319

# Install the service
installutil "C:\path\to\your\build\HL7endpointTCPlistener.exe"

# Start the service
net start "HL7endpointTCPlistener"
```

#### Uninstall Service
```cmd
# Stop the service
net stop "HL7endpointTCPlistener"

# Uninstall the service
installutil /u "C:\path\to\your\build\HL7endpointTCPlistener.exe"
```

## 🚀 Usage

### Basic Workflow
1. **Configure** your endpoints in `App.config`
2. **Install** the service using one of the methods above
3. **Start** the service:
   ```cmd
   net start "HL7endpointTCPlistener"
   ```
4. **Send HL7 messages** to the configured TCP port
5. **Monitor** logs in Windows Event Viewer

### Testing the Service
```bash
# Test TCP connection (using telnet or similar)
telnet localhost 7777

# Send a sample HL7 message
# The service will forward it to your configured HTTPS endpoint
```

### Monitoring
- **Windows Services**: Check service status in `services.msc`
- **Event Viewer**: View logs under `Windows Logs > Application`
- **Log Source**: Look for events from "HL7endpointTCPlistener"

## Project Structure

\`\`\`
HL7endpointTCPlistener/
├── HL7forwardTCPtoHTTPS/
│   ├── Program.cs              # Service entry point
│   ├── TCPtoHTTPS.cs          # Main service logic
│   ├── Subscriber.cs          # TCP listener implementation
│   ├── ProjectInstaller.cs    # Service installer
│   ├── App.config.template    # Configuration template (copy to App.config)
│   └── HL7forwardTCPtoHTTPS.csproj
└── SetupHL7forwardTCPtoHTTPS/  # Setup project
\`\`\`

## 🔍 Troubleshooting

### Common Issues

| Issue | Cause | Solution |
|-------|-------|----------|
| Service won't start | Configuration error | Check Event Viewer for details |
| Connection refused | Port already in use | Use `netstat -an` to check port usage |
| HTTPS errors | Invalid endpoint/credentials | Verify URL and authentication |
| Permission denied | Insufficient privileges | Run as Administrator |
| Messages not forwarding | Network/firewall issues | Check connectivity to target endpoint |

### Debugging Steps

1. **Check Event Viewer**:
   ```
   Windows Logs > Application > Filter by Source: "HL7endpointTCPlistener"
   ```

2. **Verify Port Availability**:
   ```cmd
   netstat -an | findstr :7777
   ```

3. **Test HTTPS Endpoint**:
   ```cmd
   curl -X POST -u username:password https://your-endpoint.com/api/hl7
   ```

4. **Check Service Status**:
   ```cmd
   sc query "HL7endpointTCPlistener"
   ```

## 🤝 Contributing

We welcome contributions! Here's how to get started:

1. **Fork** the repository
2. **Create** a feature branch: `git checkout -b feature/amazing-feature`
3. **Make** your changes and test thoroughly
4. **Commit** your changes: `git commit -m 'Add amazing feature'`
5. **Push** to the branch: `git push origin feature/amazing-feature`
6. **Open** a Pull Request

### Development Guidelines
- Follow existing code style and conventions
- Add appropriate error handling and logging
- Test with various HL7 message formats
- Update documentation for new features

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- **Issues**: Report bugs or request features via [GitHub Issues](https://github.com/vsaturnino/tcphl7lisenter/issues)
- **Discussions**: Ask questions in [GitHub Discussions](https://github.com/vsaturnino/tcphl7lisenter/discussions)
- **Documentation**: Check this README and inline code comments

## 📈 Version History

| Version | Date | Changes |
|---------|------|---------|
| **1.0.0** | 2024 | Initial release with TCP to HTTPS forwarding |

## 🏥 Use Cases

This service is perfect for:
- **Healthcare Integration**: Connect legacy HL7 systems to modern cloud APIs
- **System Modernization**: Bridge old hospital systems with new web applications
- **Data Pipeline**: Forward HL7 messages to analytics or storage systems
- **Protocol Translation**: Convert TCP-based HL7 to HTTPS REST APIs

---

**Made with ❤️ for the healthcare community**
