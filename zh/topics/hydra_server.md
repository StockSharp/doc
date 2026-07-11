# Hydra 服务器

**Hydra Server** 是一项专用服务，可通过网络广播数据，供外部程序连接使用，例如 [Designer](designer.md)。

与 Hydra 的[服务器模式](hydra/server_mode/settings.md)不同，**Hydra Server** 是一个独立的跨平台控制台程序，可以在 Windows 或 Linux 服务器上运行。

> [!TIP]
> 在 Windows 中，可以将该程序注册为 Windows 服务，使其在计算机启动时自动运行。有关详细信息，请参阅 [Windows 服务](https://en.wikipedia.org/wiki/Windows_service)。

**Hydra Server** 与 [Hydra](hydra.md) 使用相同的设置。因此，首次配置时需要先运行 [Hydra](hydra.md)，完成设置后再由 **Hydra Server** 使用这些设置。

![Hydra 服务器](../images/hydraserver_console.png)

程序包含配置文件 `appsettings.json`：

```json
{
	"Logging": {
		"LogLevel": {
			"Default": "Information",
			"Microsoft.Hosting.Lifetime": "Information"
		}
	},
	"Server": {
		"WebApiAddress": "api.stocksharp.com/v1/",
		"LogLevel": "Inherit",
		"AutoDownload": false,
		"CompanyPath": "",
		"AppDataPath": ""
	}
}

```

- **WebApiAddress** — StockSharp WebAPI 的地址，用于通过 [Telegram](telegram_services.md) 进行管理。
- **LogLevel** — 日志记录级别。
- **AutoDownload** — 是否在程序启动时自动下载数据源。
- **CompanyPath** — 如果将程序作为 Windows 服务运行，需要设置类似 `"C:\\Users\\%user_name%\\Documents\\StockSharp"` 的路径。
- **AppDataPath** — 如果移动了 [Hydra](hydra.md) 的设置目录，需要在此指定新的设置路径。
