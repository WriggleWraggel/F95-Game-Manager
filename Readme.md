# F95 Game Manager

A local game manager for your F95Zone library with a Blazor-based UI.

## Running Locally

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- Windows 10 (build 1803 or later) with [WebView2 Runtime](https://developer.microsoft.com/microsoft-edge/webview2/) installed

### WinForms Application (Recommended)

The WinForms application is the easiest way to run the game manager locally. It requires no additional installation beyond the .NET 8 SDK and the WebView2 runtime.

```bash
# Clone the repository
git clone https://github.com/WriggleWraggel/F95-Game-Manager.git
cd F95-Game-Manager

# Run the WinForms application
dotnet run --project F95GameManager.WinForms/F95GameManager.WinForms.csproj
```

Or open `F95GameManager.sln` in Visual Studio 2022 and set `F95GameManager.WinForms` as the startup project, then press F5.

### MAUI Application

The MAUI application requires the .NET MAUI workload to be installed:

```bash
# Install the MAUI workload (one-time setup)
dotnet workload install maui

# Run the MAUI application
dotnet run --project F95GameManager/F95GameManager.csproj --framework net8.0-windows10.0.19041.0
```

Or open `F95GameManager.sln` in Visual Studio 2022 and set `F95GameManager` as the startup project, then press F5.

---

# Install (MSIX Package)
1. Double click the .cer file

    ![Select File](./InstallGuide/SelectFile.png)
    
2. Click "Install Certificate"

    ![Install Cert](./InstallGuide/InstallCert.png)
    
3. Select Local Machine and click Next

    ![Install Cert](./InstallGuide/SelectLocalMachine.png)

4. Select "Place all certifcates in the following store" browse to "Trusted Root Certification Authorities" and click next

    ![Install Cert](./InstallGuide/SelectLocalMachine.png)

5. Finish

    ![Install Cert](./InstallGuide/Finish.png)