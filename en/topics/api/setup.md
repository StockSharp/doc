# Setting Up the Environment

## Requirements

To work with StockSharp you need:

- **.NET 10** (SDK and Runtime) — [download](https://dotnet.microsoft.com/download/dotnet/10.0)
- **IDE** — Visual Studio 2022+, JetBrains Rider, or VS Code
- **NuGet** — package manager (built into Visual Studio and Rider)

Verify the SDK is installed:

```bash
dotnet --version
```

## Creating a Project

### Visual Studio 2022+

1. **File → New → Project**
2. Select **Console App** or **WPF Application** template
3. Set the target framework to **.NET 10**

### JetBrains Rider

1. **File → New Solution**
2. Select **.NET / .NET Core → Console Application**
3. Set **Target Framework: net10.0**

### Command Line (CLI)

```bash
# Console application
dotnet new console -n MyTradingApp --framework net10.0
cd MyTradingApp

# WPF application (Windows only)
dotnet new wpf -n MyTradingGui --framework net10.0-windows
```

## NuGet Package Catalog

StockSharp is distributed via NuGet. Below is the complete package catalog organized by category.

### Core

| Package | Description |
|---------|-------------|
| [StockSharp.Messages](https://www.nuget.org/packages/StockSharp.Messages/) | Base messages and contracts. Foundation of the entire framework |
| [StockSharp.BusinessEntities](https://www.nuget.org/packages/StockSharp.BusinessEntities/) | Trading entities: Security, Order, Trade, Portfolio, etc. |
| [StockSharp.Algo](https://www.nuget.org/packages/StockSharp.Algo/) | Core algorithmic trading, Connector, subscriptions, candles |
| [StockSharp.Configuration](https://www.nuget.org/packages/StockSharp.Configuration/) | Configuration management, connection settings |
| [StockSharp.Localization](https://www.nuget.org/packages/StockSharp.Localization/) | Localization system (English by default) |

### Strategies and Indicators

| Package | Description |
|---------|-------------|
| [StockSharp.Algo.Strategies](https://www.nuget.org/packages/StockSharp.Algo.Strategies/) | Strategy framework — base Strategy class, positions, PnL |
| [StockSharp.Algo.Indicators](https://www.nuget.org/packages/StockSharp.Algo.Indicators/) | 100+ technical indicators (SMA, EMA, RSI, MACD, Bollinger, etc.) |

### Testing

| Package | Description |
|---------|-------------|
| [StockSharp.Algo.Testing](https://www.nuget.org/packages/StockSharp.Algo.Testing/) | Backtesting on historical data, trade emulation |

### Storage and Data

| Package | Description |
|---------|-------------|
| [StockSharp.Algo.Export](https://www.nuget.org/packages/StockSharp.Algo.Export/) | Market data export (CSV, Excel, JSON, etc.) |
| [StockSharp.Algo.Import](https://www.nuget.org/packages/StockSharp.Algo.Import/) | Data import from external formats |

### Analytics and Computation

| Package | Description |
|---------|-------------|
| [StockSharp.Algo.Analytics](https://www.nuget.org/packages/StockSharp.Algo.Analytics/) | Analytics scripts interfaces |
| [StockSharp.Algo.Compilation](https://www.nuget.org/packages/StockSharp.Algo.Compilation/) | Runtime code compilation |
| [StockSharp.Algo.Gpu](https://www.nuget.org/packages/StockSharp.Algo.Gpu/) | GPU-accelerated indicator calculations (CUDA via ILGPU) |

### GUI Components (Windows Only)

| Package | Description |
|---------|-------------|
| [StockSharp.Xaml](https://www.nuget.org/packages/StockSharp.Xaml/) | WPF controls: instrument, portfolio, and order tables |
| [StockSharp.Xaml.Charting](https://www.nuget.org/packages/StockSharp.Xaml.Charting/) | Candlestick charts, indicators, equity curves |
| [StockSharp.Charting.Interfaces](https://www.nuget.org/packages/StockSharp.Charting.Interfaces/) | Chart component interfaces |
| [StockSharp.Alerts.Interfaces](https://www.nuget.org/packages/StockSharp.Alerts.Interfaces/) | Alert system interfaces |
| [StockSharp.Diagram.Core](https://www.nuget.org/packages/StockSharp.Diagram.Core/) | Visual strategy designer core |

### Connectors (Exchanges and Brokers)

Each connector is a separate NuGet package. Main connectors:

| Package | Exchange/Broker |
|---------|----------------|
| `StockSharp.Binance` | Binance |
| `StockSharp.InteractiveBrokers` | Interactive Brokers |
| `StockSharp.Fix` | FIX protocol (universal) |
| `StockSharp.Connectors.Tinkoff` | Tinkoff Invest |
| `StockSharp.Connectors.Coinbase` | Coinbase |
| `StockSharp.Connectors.BitStamp` | Bitstamp |
| `StockSharp.Connectors.Bittrex` | Bittrex |
| `StockSharp.Finam` | Finam |

> [!NOTE]
> For the full list of connectors, see the [Connectors](connectors.md) section. Some connectors are only available through the [private NuGet server](#private-nuget-server).

### Localization

The base `StockSharp.Localization` package includes English. Additional languages are installed as separate packages:

| Package | Language |
|---------|----------|
| `StockSharp.Localization.ru` | Russian |
| `StockSharp.Localization.zh` | Chinese |
| `StockSharp.Localization.de` | German |
| `StockSharp.Localization.es` | Spanish |
| `StockSharp.Localization.ja` | Japanese |
| `StockSharp.Localization.ko` | Korean |
| `StockSharp.Localization.All` | All languages (meta-package) |

Also available: `ar`, `bn`, `ca`, `cs`, `da`, `el`, `fa`, `fi`, `fr`, `he`, `hi`, `hu`, `it`, `jv`, `ms`, `my`, `nl`, `no`, `pa`, `pl`, `pt`, `ro`, `sk`, `sr`, `sv`, `ta`, `th`, `tl`, `tr`, `uk`, `uz`, `vi`.

## Installing Packages

### Via CLI (Recommended)

```bash
# Core packages
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies

# Connector (example — Binance)
dotnet add package StockSharp.Binance

# Indicators
dotnet add package StockSharp.Algo.Indicators

# Backtesting
dotnet add package StockSharp.Algo.Testing

# Localization (Russian)
dotnet add package StockSharp.Localization.ru
```

### Via Visual Studio

1. Right-click on the project → **Manage NuGet Packages...**
2. Search for `StockSharp`
3. Select the desired package → **Install**

All dependencies are installed automatically.

### Via JetBrains Rider

1. Right-click on the project → **Manage NuGet Packages**
2. Search for `StockSharp`
3. Select the package → **Install**

### Via Package Manager Console (Visual Studio)

```powershell
Install-Package StockSharp.Algo
Install-Package StockSharp.Binance
Install-Package StockSharp.Algo.Strategies
```

## Private NuGet Server

Some components (crypto connectors, etc.) are only available through the private NuGet server for registered users.

### Method 1: Authentication via Token in URL

1. Register on the StockSharp website.
2. Copy the token from your [personal account](https://stocksharp.ru/profile/).
3. Add the package source:

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/{YOUR_TOKEN}/v3/index.json" --name StockSharpPrivate
```

**Visual Studio:** open **Tools → Options → NuGet Package Manager → Package Sources** and add a new source with the URL `https://nuget.stocksharp.com/{YOUR_TOKEN}/v3/index.json`.

**Rider:** open **Settings → Build, Execution, Deployment → NuGet → Sources** and add the source.

### Method 2: Authentication via Username and Password

1. Add a package source with the URL `https://nuget.stocksharp.com/x/v3/index.json`
2. When you try to use this source, a login prompt will appear
3. Enter your StockSharp account credentials (or `x` as the username and your token as the password)

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/x/v3/index.json" --name StockSharpPrivate --username YOUR_LOGIN --password YOUR_PASSWORD --store-password-in-clear-text
```

> [!TIP]
> To reset saved credentials on Windows, open **Control Panel → User Accounts → Credential Manager** and delete entries related to `nuget.stocksharp.com`.

## Updating Packages

### CLI

```bash
# Check for available updates
dotnet list package --outdated

# Update a specific package
dotnet add package StockSharp.Algo
```

### Visual Studio

1. **Manage NuGet Packages...** → **Updates** tab
2. Select packages → **Update**

### Rider

1. **Manage NuGet Packages** → **Upgrades** tab
2. Select packages → **Upgrade**

## Troubleshooting

### Version Mismatch

All StockSharp packages must be the same version. If you encounter type or method errors, ensure all `StockSharp.*` packages are updated to the same version.

### Package Not Found on Public NuGet

Some connectors are only available through the [private server](#private-nuget-server). Make sure the correct source is added.

### GUI Issues on Non-Windows

GUI components (`StockSharp.Xaml`, `StockSharp.Xaml.Charting`) work only on Windows. On Linux/macOS, use console applications without GUI packages.

### Package Restore Errors

```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Retry restore
dotnet restore
```

## Project File Structure (.csproj)

### Minimal (Console Trading Bot)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Binance" Version="*" />
  </ItemGroup>
</Project>
```

### Extended (Strategy with Indicators and Testing)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- Core -->
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Configuration" Version="*" />

    <!-- Strategies and indicators -->
    <PackageReference Include="StockSharp.Algo.Strategies" Version="*" />
    <PackageReference Include="StockSharp.Algo.Indicators" Version="*" />

    <!-- Connector -->
    <PackageReference Include="StockSharp.Binance" Version="*" />

    <!-- Backtesting -->
    <PackageReference Include="StockSharp.Algo.Testing" Version="*" />

    <!-- Localization -->
    <PackageReference Include="StockSharp.Localization.ru" Version="*" />
  </ItemGroup>
</Project>
```

### WPF Application with Charts

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Algo.Strategies" Version="*" />
    <PackageReference Include="StockSharp.Xaml.Charting" Version="*" />
    <PackageReference Include="StockSharp.Binance" Version="*" />
  </ItemGroup>
</Project>
```

## Verifying the Installation

Create a minimal application:

```csharp
using StockSharp.Algo;
using StockSharp.BusinessEntities;

Console.WriteLine("StockSharp successfully configured!");

var connector = new Connector();
Console.WriteLine($"Connector created: {connector}");
```

```bash
dotnet run
```

## Examples

Ready-made examples of StockSharp usage are available in the [Samples/](https://github.com/stocksharp/stocksharp/tree/master/Samples) directory of the repository. They cover connecting to exchanges, subscribing to data, building candles, indicators, strategies, and testing.
