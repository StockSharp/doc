# コネクタ設定: DNSE

DNSE に接続する前に、次のアダプタープロパティを設定します。この一覧は [DnseMessageAdapter](xref:StockSharp.Dnse.DnseMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `TradingToken` (`SecureString`)
- `Account` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `OtpType` (`DnseOtpTypes`)
- `OneTimePassword` (`SecureString`)
- `RequestEmailOtpOnConnect` (`bool`)
- `DefaultLoanPackageId` (`int`)
- `DefaultBoardId` (`string`)
- `MarketDataPriceMultiplier` (`decimal`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `ApiVersion` (`string`)
- `DateHeaderName` (`string`)
- `RestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_dnse.md)

[アダプターの初期化](adapter_initialization_dnse.md)
