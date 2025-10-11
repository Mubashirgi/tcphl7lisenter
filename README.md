# HL7 Endpoint TCP Listener

A Windows service that listens for HL7 messages on a TCP port and forwards them to an HTTPS endpoint. This service acts as a bridge between legacy HL7 systems that communicate over TCP and modern web-based healthcare systems that use HTTPS.

## Description

This C# Windows service provides a reliable way to:
- Listen for incoming HL7 messages on a configurable TCP port
- Forward received messages to a specified HTTPS endpoint
- Run as a Windows service for continuous operation
- Handle authentication for the target HTTPS endpoint
- Provide logging and error handling

## Features

- **TCP to HTTPS Bridge**: Seamlessly forwards HL7 messages from TCP to HTTPS
- **Windows Service**: Runs continuously in the background
- **Configurable**: Easy configuration through App.config file
- **Authentication Support**: Supports username/password authentication for HTTPS endpoints
- **Error Handling**: Comprehensive error logging to Windows Event Viewer
- **Thread-Safe**: Multi-threaded design for handling concurrent connections

## Prerequisites

- Windows operating system
- .NET Framework 4.0 or higher
- Administrator privileges for service installation

## Configuration

1. Copy \`App.config.template\` to \`App.config\`:
   ```cmd
   copy HL7forwardTCPtoHTTPS\App.config.template HL7forwardTCPtoHTTPS\App.config
   ```

2. Edit the \`App.config\` file to configure the service:

\`\`\`xml
<appSettings>
    <add key="ReceiveToIP" value="127.0.0.1" />
    <add key="ReceiveToPort" value="7777" />
    <add key="ForwardToHost" value="https://your-endpoint.example.com/api/hl7" />
    <add key="ForwardToUser" value="your-username" />
    <add key="ForwardToPassword" value="your-password" />
</appSettings>
\`\`\`

### Configuration Parameters

- **ReceiveToIP**: IP address to listen on (default: 127.0.0.1)
- **ReceiveToPort**: TCP port to listen on
- **ForwardToHost**: HTTPS endpoint URL to forward messages to
- **ForwardToUser**: Username for HTTPS authentication
- **ForwardToPassword**: Password for HTTPS authentication

## Installation

### Building the Project

1. Open the solution in Visual Studio:
   \`\`\`
   HL7forwardTCPtoHTTPS/HL7forwardTCPtoHTTPS.sln
   \`\`\`

2. Build the solution (Build → Build Solution)

### Installing as Windows Service

1. Open Command Prompt as Administrator

2. Navigate to the .NET Framework installation directory:
   \`\`\`cmd
   cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319
   \`\`\`

3. Install the service:
   \`\`\`cmd
   installutil "path\to\HL7endpointTCPlistener.exe"
   \`\`\`

### Uninstalling the Service

To remove the service:
\`\`\`cmd
installutil /u "path\to\HL7endpointTCPlistener.exe"
\`\`\`

## Usage

1. **Configure** the service by editing \`App.config\`
2. **Install** the service using the installation steps above
3. **Start** the service through Windows Services Manager or command line:
   \`\`\`cmd
   net start "HL7 TCP to HTTPS Service"
   \`\`\`
4. **Monitor** the service through Windows Event Viewer for logs and errors

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

## Troubleshooting

### Common Issues

1. **Service won't start**: Check Windows Event Viewer for error details
2. **Connection refused**: Verify TCP port is not in use by another application
3. **HTTPS errors**: Verify the target endpoint URL and credentials
4. **Permission errors**: Ensure the service runs with appropriate privileges

### Logging

The service logs events to the Windows Event Viewer under:
- **Source**: HL7 TCP to HTTPS Service
- **Log**: Application

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For support and questions, please open an issue on GitHub.

## Version History

- **1.0**: Initial release with basic TCP to HTTPS forwarding functionality
