# 控制台安装程序

`Installer.Console` 是 StockSharp Installer 的跨平台版本。无需使用图形界面，即可通过它下载、更新和卸载产品。只要操作系统支持 [.NET 6](https://dotnet.microsoft.com/) 运行时，就可以运行该工具。

## 运行

1. 安装适用于当前平台的 .NET 6 SDK 或运行时。
2. 从[下载页面](https://stocksharp.com/products/download/)下载 `StockSharp.Installer.Console.zip`。
3. 解压文件，然后从命令行运行该工具：
   
   ```bash
   dotnet StockSharp.Installer.Console.dll <Command> [product] [dir] [options]
   ```

`<Command>` 可以是以下命令之一：

- `Install` — 安装产品。
- `Update` — 更新已安装的产品。
- `Repair` — 修复现有安装。
- `Remove` — 卸载产品。
- `License` — 显示产品的许可证。
- `Licenses` — 列出可用许可证。
- `HddId` — 输出硬盘标识符。
- `Products` — 列出可用产品。
- `Updates` — 显示可用更新。
- `Installed` — 列出已安装的程序。
- `Sign` — 为 DLL 文件签名。

可选参数 `[product]` 是[商店](https://stocksharp.com/store/)中的产品 ID。可以在产品页面中找到该 ID，例如 [Hydra Server 页面](https://stocksharp.com/store/hydra-server/)；也可以运行 `StockSharp.Installer.Console.exe Products -s hydra` 进行查询。`[dir]` 用于指定安装目录。

## 选项

该工具支持以下选项：

- `-s`、`--search` — 按名称筛选产品。
- `-r`、`--run` — 安装完成后自动运行应用程序，例如 `StockSharp.Hydra.Server.exe`。
- `-c`、`--cache` — 使用 NuGet 缓存。
- `-f`、`--force` — 忽略已配置的检查间隔，强制检查更新。
- `-p`、`--pre` — 允许安装预发布版本。
- `-e`、`--noerror` — 隐藏所有错误。
- `-b`、`--backup` — 在修复或更新前备份原有设置。
- `-l`、`--clear` — 安装前清空目标目录。
- `-t`、`--fw` — 指定目标 .NET 框架。
- `-d`、`--data` — 删除应用程序数据目录。
- `-i`、`--in` — 要签名的 DLL。
- `-o`、`--out` — 签名后生成的 DLL。

命令示例：

```bash
dotnet StockSharp.Installer.Console.dll Install 1269 /home/user/stocksharp -p -r StockSharp.Hydra.Server.exe
```

该命令将产品 **1269**（此处仅作为示例）安装到指定目录，允许使用预发布版本，并在安装完成后启动 `StockSharp.Hydra.Server.exe`。

## 签名

`Sign` 命令用于为交易机器人的 DLL 添加数字签名。分发自行开发的、基于 API 的交易机器人时，可以使用此命令，使所有接收者（包括使用[免费方案](https://stocksharp.com/pricing/)的用户）都能运行该机器人。

请先编译交易机器人，并添加 `StockSharp.Configuration` 命名空间中的 `[assembly: ProductId(9)]` 特性。需要签名的是 DLL 文件（例如 `MyRobot.dll`），而不是 `.exe` 文件。

示例：

```bash
StockSharp.Installer.Console.exe Sign -i "MyRobot.dll"
```

重新生成项目会移除签名，因此应在开发结束且不再需要重新编译时，最后为程序集签名。
