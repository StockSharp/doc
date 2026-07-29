# コネクタ設定: m.Stock

m.Stock に接続する前に、次のアダプタープロパティを設定します。この一覧は [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `ClientCode` (`string`)

## 詳細設定

これらのプロパティは、認証とセッション状態、接続先、ストリーミング、定期照会を制御します。

- `Password` (`SecureString`)
- `Otp` (`SecureString`)
- `UseTotp` (`bool`)
- `RefreshToken` (`SecureString`)
- `AccessToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_mstock.md)

[アダプターの初期化](adapter_initialization_mstock.md)
