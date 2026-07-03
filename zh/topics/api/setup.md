# 设置环境

## 要求

要与 StockSharp 一起工作，您需要：

- **.NET 10**（SDK 和运行时）— [下载](https://dotnet.microsoft.com/download/dotnet/10.0)
- **IDE** — Visual Studio 2022+, JetBrains Rider, 或 VS Code
- **NuGet** — 软件包管理器（内置于 Visual Studio 和 Rider 中）

验证 SDK 是否已安装：

```bash
dotnet --version
```

## 创建项目

### Visual Studio 2022+

1. **文件 → 新建 → 项目**
2. 选择 **控制台应用程序** 或 **WPF 应用程序** 模板
3. 将目标框架设置为 **.NET 10**

### JetBrains Rider

1. **文件 → 新建解决方案**
2. 选择 **.NET / .NET Core → 控制台应用程序**
3. 设置 **目标框架：net10.0**

### 命令行（CLI）

```bash
# Console application
dotnet new console -n MyTradingApp --framework net10.0
cd MyTradingApp

# WPF application (Windows only)
dotnet new wpf -n MyTradingGui --framework net10.0-windows
```

## NuGet 软件包目录

StockSharp 通过 NuGet 分发。以下是按类别整理的完整软件包目录。

### 核心

| 软件包 | 描述 |
|---------|-------------|
| [StockSharp.Messages](https://www.nuget.org/packages/StockSharp.Messages/) | 基础消息和合约。整个框架的基础 |
| [StockSharp.BusinessEntities](https://www.nuget.org/packages/StockSharp.BusinessEntities/) | 交易实体：交易品种、订单、交易、投资组合等 |
| [StockSharp.Algo](https://www.nuget.org/packages/StockSharp.Algo/) | 核心算法交易，连接器，订阅，K线 |
| [StockSharp.Configuration](https://www.nuget.org/packages/StockSharp.Configuration/) | 配置管理，连接设置 |
| [StockSharp.Localization](https://www.nuget.org/packages/StockSharp.Localization/) | 本地化系统（默认英文） |

### 策略与指标

| 软件包 | 描述 |
|---------|-------------|
| [StockSharp.Algo.Strategies](https://www.nuget.org/packages/StockSharp.Algo.Strategies/) | 策略框架 — 基础策略类、持仓、盈亏 |
| [StockSharp.Algo.Indicators](https://www.nuget.org/packages/StockSharp.Algo.Indicators/) | 100+ 技术指标（SMA, EMA, RSI, MACD, 布林带等） |

### 测试

| 软件包 | 描述 |
|---------|-------------|
| [StockSharp.Algo.Testing](https://www.nuget.org/packages/StockSharp.Algo.Testing/) | 历史数据回测，交易模拟 |

### 存储与数据

| 软件包 | 描述 |
|---------|-------------|
| [StockSharp.Algo.Export](https://www.nuget.org/packages/StockSharp.Algo.Export/) | 市场数据导出（CSV、Excel、JSON 等） |
| [StockSharp.Algo.Import](https://www.nuget.org/packages/StockSharp.Algo.Import/) | 从外部格式导入数据 |

### 分析与计算

| 软件包 | 描述 |
|---------|-------------|
| [StockSharp.Algo.Analytics](https://www.nuget.org/packages/StockSharp.Algo.Analytics/) | 分析脚本接口 |
| [StockSharp.Algo.Compilation](https://www.nuget.org/packages/StockSharp.Algo.Compilation/) | 运行时代码编译 |
| [StockSharp.Algo.Gpu](https://www.nuget.org/packages/StockSharp.Algo.Gpu/) | GPU加速指标计算（通过ILGPU的CUDA） |

### GUI 组件（仅限 Windows）

| 软件包 | 描述 |
|---------|-------------|
| [StockSharp.Xaml](https://www.nuget.org/packages/StockSharp.Xaml/) | WPF 控件：工具、投资组合和订单表格 |
| [StockSharp.Xaml.Charting](https://www.nuget.org/packages/StockSharp.Xaml.Charting/) | 烛线图，指标，股票曲线 |
| [StockSharp.Charting.Interfaces](https://www.nuget.org/packages/StockSharp.Charting.Interfaces/) | 图表组件接口 |
| [StockSharp.Alerts.Interfaces](https://www.nuget.org/packages/StockSharp.Alerts.Interfaces/) | 警报系统接口 |
| [StockSharp.Diagram.Core](https://www.nuget.org/packages/StockSharp.Diagram.Core/) | 可视化策略设计器核心 |

### 连接器（交易所和经纪商）

每个连接器都是一个独立的 NuGet 封装。主要连接器：

| 套餐 | 交易所/经纪商 |
|---------|----------------|
| `StockSharp.Binance` | 币安 |
| `StockSharp.InteractiveBrokers` | 互动经纪商 |
| `StockSharp.Fix` | FIX 协议（通用） |
| `StockSharp.Connectors.Coinbase` | Coinbase |
| `StockSharp.Connectors.BitStamp` | 比特斯坦普 |
| `StockSharp.Connectors.Bittrex` | Bittrex |

> [!NOTE]
> 有关连接器的完整列表，请参见 [连接器](connectors.md) 部分。某些连接器仅通过 [私有 NuGet 服务器](#private-nuget-server) 提供。

### 本地化

基础 `StockSharp.Localization` 软件包包含英语。其他语言作为单独的软件包安装：

| 包 | 语言 |
|---------|----------|
| `StockSharp.Localization.ru` | 俄语 |
| `StockSharp.Localization.zh` | 中文 |
| `StockSharp.Localization.de` | 德语 |
| `StockSharp.Localization.es` | 西班牙语 |
| `StockSharp.Localization.ja` | 日语 |
| `StockSharp.Localization.ko` | 韩语 |
| `StockSharp.Localization.All` | 所有语言（元包） |

也可提供：`ar`、`bn`、`ca`、`cs`、`da`、`el`、`fa`、`fi`、`fr`、`he`、`hi`、`hu`、`it`、`jv`、`ms`、`my`、`nl`、`no`、`pa`、`pl`、`pt`、`ro`、`sk`、`sr`、`sv`、`ta`、`th`、`tl`、`tr`、`uk`、`uz`、`vi`。

## 安装软件包

### 通过命令行界面（推荐）

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

### 通过 Visual Studio

1. 右键点击项目 → **管理 NuGet 包...**
2. 搜索 `StockSharp`
3. 选择所需的软件包 → **安装**

所有依赖项都会自动安装。

### 通过 JetBrains Rider

1. 右键点击项目 → **管理 NuGet 包**
2. 搜索 `StockSharp`
3. 选择软件包 → **安装**

### 通过包管理器控制台 (Visual Studio)

```powershell
Install-Package StockSharp.Algo
Install-Package StockSharp.Binance
Install-Package StockSharp.Algo.Strategies
```

## 私人 NuGet 服务器

某些组件（加密连接器等）仅通过面向注册用户的私人 NuGet 服务器提供。

### 方法1：通过URL中的令牌进行身份验证

1. 在 StockSharp 网站上注册。
2. 从你的[个人账户](https://stocksharp.ru/profile/)复制令牌。
3. 添加软件包源：

**命令行界面:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/{YOUR_TOKEN}/v3/index.json" --name StockSharpPrivate
```

**Visual Studio:** 打开 **工具 → 选项 → NuGet 包管理器 → 包源** 并添加一个 URL 为 `https://nuget.stocksharp.com/{YOUR_TOKEN}/v3/index.json` 的新源。

**Rider:** 打开 **设置 → 构建、执行、部署 → NuGet → 源** 并添加该源。

### 方法二：通过用户名和密码进行身份验证

1. 添加一个 URL 为 `https://nuget.stocksharp.com/x/v3/index.json` 的软件包源
2. 当你尝试使用此资源时，会出现登录提示
3. 输入您的 StockSharp 帐户凭据（或使用 `x` 作为用户名，您的令牌作为密码）

**命令行界面:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/x/v3/index.json" --name StockSharpPrivate --username YOUR_LOGIN --password YOUR_PASSWORD --store-password-in-clear-text
```

> [!TIP]
> 要在 Windows 上重置已保存的凭据，请打开 **控制面板 → 用户账户 → 凭据管理器**，然后删除与 `nuget.stocksharp.com` 相关的条目。

## 更新软件包

### 命令行界面

```bash
# Check for available updates
dotnet list package --outdated

# Update a specific package
dotnet add package StockSharp.Algo
```

### Visual Studio

1. **管理 NuGet 软件包...** → **更新** 标签
2. 选择包 → **更新**

### 骑手

1. **管理 NuGet 软件包** → **升级** 选项卡
2. 选择软件包 → **升级**

## 故障排除

### 版本不匹配

所有 StockSharp 包必须是相同版本。如果遇到类型或方法错误，请确保所有 `StockSharp.*` 包都更新到相同的版本。

### 在公共 NuGet 上未找到包

有些连接器只能通过[私人服务器](#private-nuget-server)使用。请确保已添加正确的来源。

### 非 Windows 系统上的 GUI 问题

GUI 组件（`StockSharp.Xaml`, `StockSharp.Xaml.Charting`）仅在 Windows 上运行。在 Linux/macOS 上，请使用不带 GUI 包的控制台应用程序。

### 包还原错误

```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Retry restore
dotnet restore
```

## 项目文件结构 (.csproj)

### 极简（控制台交易机器人）

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

### 扩展（含指标和测试的策略）

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

### 带图表的WPF应用程序

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

## 验证安装

创建一个最小化应用程序：

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

## 例子

StockSharp 的现成用法示例可在仓库的 [Samples/](https://github.com/stocksharp/stocksharp/tree/master/Samples) 目录中找到。它们涵盖了连接交易所、订阅数据、构建K线、指标、策略以及测试。
