# コネクタ設定: Dexalot

Dexalot に接続する前に、次のアダプタープロパティを設定します。この一覧は [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `RpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## 詳細設定

これらのプロパティは、コントラクトアドレス、通貨ペアフィルター、板の深さ、定期照会、トランザクション動作を制御します。

- `TradePairsAddress` (`string`)
- `PortfolioAddress` (`string`)
- `Pairs` (`string`)
- `OrderBookDepth` (`int`)
- `PrivatePollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `SelfTradePrevention` (`DexalotSelfTradePrevention`)

## 関連項目

[グラフィカル設定](graphical_configuration_dexalot.md)

[アダプターの初期化](adapter_initialization_dexalot.md)
