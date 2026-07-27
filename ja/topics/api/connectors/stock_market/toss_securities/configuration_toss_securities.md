# コネクタ設定: Toss Securities

Toss Securities に接続する前に、次のアダプタープロパティを設定します。この一覧は [TossSecuritiesMessageAdapter](xref:StockSharp.TossSecurities.TossSecuritiesMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AccountSequence` (`long`)
- `PollingInterval` (`TimeSpan`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `PortfolioName` (`string`)
- `AccountPollingInterval` (`TimeSpan`)
- `AdjustedCandles` (`bool`)
- `RestAddress` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_toss_securities.md)

[アダプターの初期化](adapter_initialization_toss_securities.md)
