# コネクタ設定: comdirect

comdirect に接続する前に、次のアダプタープロパティを設定します。この一覧は [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Login` (`string`)
- `Password` (`SecureString`)
- `TanType` (`ComdirectTanTypes`)
- `PollingInterval` (`TimeSpan`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `DefaultCurrency` (`string`)
- `Address` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_comdirect.md)

[アダプターの初期化](adapter_initialization_comdirect.md)
