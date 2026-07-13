# Servidor Hydra

O **Hydra Server** é um serviço que transmite dados pela rede para que programas externos, como o [Designer](designer.md), possam ligar-se a ele.

Ao contrário do [modo de servidor](hydra/server_mode/settings.md), o **Hydra Server** é um programa multiplataforma separado, criado como uma aplicação de consola, e pode ser executado em servidores Windows ou Linux.

> [!TIP]
> No Windows, o Hydra Server pode ser registado como um serviço do Windows e iniciado automaticamente no arranque do sistema. Para mais informações, consulte [serviço do Windows](https://en.wikipedia.org/wiki/Windows_service).

O **Hydra Server** usa as mesmas definições que o [Hydra](hydra.md). Para a configuração inicial, execute primeiro o [Hydra](hydra.md) e depois use as definições criadas pelo Hydra Server.

![servidor Hydra](../images/hydraserver_console.png)

O programa tem um ficheiro de configuração `appsettings.json`:

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

- **WebApiAddress** - o endereço da StockSharp WebAPI. Usado para gestão através do [Telegram](telegram_services.md).
- **LogLevel** - o nível de registo.
- **AutoDownload** - se deve ativar o descarregamento automático das origens no arranque.
- **CompanyPath** - se usar o programa como serviço do Windows, é necessário definir o caminho como "C:\\Users\\%user_name%\\Documents\\StockSharp".
- **AppDataPath** - no caso de mover o diretório de definições do [Hydra](hydra.md), deve ser especificado um novo caminho para as definições.
