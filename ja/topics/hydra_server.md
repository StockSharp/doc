# Hydra サーバー

**Hydra Server** は、[Designer](designer.md) などの外部プログラムが接続できるように、ネットワーク経由でデータを配信するサービスです。

[サーバーモード](hydra/server_mode/settings.md)とは異なり、**Hydra Server** はコンソールアプリケーションとして作成された独立したクロスプラットフォームプログラムであり、Windows または Linux サーバー上で実行できます。

> [!TIP]
> Windows では、Hydra Server を Windows Service として登録し、システム起動時に自動的に起動できます。詳細については、[Windows service](https://en.wikipedia.org/wiki/Windows_service) を参照してください。

**Hydra Server** は [Hydra](hydra.md) と同じ設定を使用します。初期設定では、まず [Hydra](hydra.md) を実行し、その後 Hydra Server によって作成された設定を使用します。

![Hydra server](../images/hydraserver_console.png)

プログラムには設定ファイル `appsettings.json` があります。

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

- **WebApiAddress** - StockSharp WebAPI のアドレスです。[Telegram](telegram_services.md) 経由の管理に使用されます。
- **LogLevel** - ロギングレベルです。
- **AutoDownload** - 起動時にソースの自動ダウンロードを有効にするかどうかです。
- **CompanyPath** - プログラムを Windows Service として使用する場合は、"C:\\Users\\%user_name%\\Documents\\StockSharp" のようなパスを設定する必要があります。
- **AppDataPath** - [Hydra](hydra.md) の設定ディレクトリを移動した場合は、設定への新しいパスを指定する必要があります。
