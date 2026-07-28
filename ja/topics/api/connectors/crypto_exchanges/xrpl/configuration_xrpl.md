# コネクタ設定: XRPL DEX

XRPL DEX に接続する前に、次のアダプタープロパティを設定します。この一覧は [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `RpcEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `Account` (`string`)
- `Seed` (`SecureString`)

## 詳細設定

これらのプロパティは、市場の選択、板の深さ、履歴、手数料、定期照会、トランザクション保護を制御します。

- `Markets` (`string`)
- `DomainId` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLedgerLimit` (`int`)
- `FeeMultiplier` (`decimal`)
- `LastLedgerOffset` (`int`)
- `MarketOrderProtection` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_xrpl.md)

[アダプターの初期化](adapter_initialization_xrpl.md)
