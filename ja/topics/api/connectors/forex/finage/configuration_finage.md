# コネクタ設定: Finage

Finage に接続する前に、次のアダプタープロパティを設定します。この一覧は [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `ApiKey` (`SecureString`)

## 詳細設定

これらのプロパティは、REST と WebSocket のアクセス、接続先、市場データオプション、シンボルフィルター、結果上限を制御します。

- `StreamingToken` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_finage.md)

[アダプターの初期化](adapter_initialization_finage.md)
