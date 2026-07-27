# コネクタ設定: Tradernet

Tradernet に接続する前に、次のアダプタープロパティを設定します。この一覧は [TradernetMessageAdapter](xref:StockSharp.Tradernet.TradernetMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `PollingInterval` (`TimeSpan`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `MaxMarketDepth` (`int`)
- `SecuritiesPageSize` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_tradernet.md)

[アダプターの初期化](adapter_initialization_tradernet.md)
