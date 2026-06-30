# Hydra Server

**Hydra Server** is a service that broadcasts data over the network so external programs, such as [Designer](designer.md), can connect to it.

Unlike [server mode](hydra/server_mode/settings.md), **Hydra Server** is a separate cross-platform program made as a console application and can be run on Windows or Linux servers.

> [!TIP]
> On Windows, Hydra Server can be registered as a Windows Service and launched automatically at system startup. For more information, see [Windows service](https://en.wikipedia.org/wiki/Windows_service).

**Hydra Server** uses the same settings as [Hydra](hydra.md). For initial setup, run [Hydra](hydra.md) first and then use the settings created by Hydra Server.

![Hydra server](../images/hydraserver_console.png)

The program has a configuration file `appsettings.json`:

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

- **WebApiAddress** - the address of StockSharp WebAPI. Used for management via [Telegram](telegram_services.md).
- **LogLevel** - the logging level.
- **AutoDownload** - whether to enable automatic downloading of sources at startup.
- **CompanyPath** - if using the program as a Windows Service, you need to set the path like "C:\\Users\\%user_name%\\Documents\\StockSharp".
- **AppDataPath** - in case of moving the settings directory of [Hydra](hydra.md), a new path to the settings must be specified.
