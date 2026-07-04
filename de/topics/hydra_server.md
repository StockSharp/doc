# Hydra Server

**Hydra Server** ist ein Dienst, der Daten über das Netzwerk bereitstellt, sodass externe Programme wie [Designer](designer.md) eine Verbindung herstellen können.

Anders als der [Servermodus](hydra/server_mode/settings.md) ist **Hydra Server** ein separates, plattformübergreifendes Programm, das als Konsolenanwendung ausgeführt wird und auf Windows- oder Linux-Servern laufen kann.

> [!TIP]
> Unter Windows kann Hydra Server als Windows Service registriert und beim Systemstart automatisch gestartet werden. Weitere Informationen finden Sie unter [Windows service](https://en.wikipedia.org/wiki/Windows_service).

**Hydra Server** verwendet dieselben Einstellungen wie [Hydra](hydra.md). Führen Sie für die Ersteinrichtung zuerst [Hydra](hydra.md) aus und verwenden Sie anschließend die von Hydra Server erstellten Einstellungen.

![Hydra server](../images/hydraserver_console.png)

Das Programm besitzt die Konfigurationsdatei `appsettings.json`:

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

- **WebApiAddress** - die Adresse der StockSharp WebAPI. Wird für die Verwaltung über [Telegram](telegram_services.md) verwendet.
- **LogLevel** - die Protokollierungsstufe.
- **AutoDownload** - legt fest, ob das automatische Herunterladen von Quellen beim Start aktiviert wird.
- **CompanyPath** - wenn das Programm als Windows Service verwendet wird, muss ein Pfad wie "C:\\Users\\%user_name%\\Documents\\StockSharp" angegeben werden.
- **AppDataPath** - falls das Einstellungsverzeichnis von [Hydra](hydra.md) verschoben wurde, muss hier der neue Pfad zu den Einstellungen angegeben werden.
