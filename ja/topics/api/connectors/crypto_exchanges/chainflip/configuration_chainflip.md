# コネクタ設定: Chainflip

Chainflip に接続する前に、次のアダプタープロパティを設定します。この一覧は [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `StateRpcEndpoint` (`string`)
- `BackendEndpoint` (`string`)
- `EthereumRpcEndpoint` (`string`)
- `ArbitrumRpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## 詳細設定

これらのプロパティは、宛先アドレス、プールフィルター、定期照会、板の深さ、トランザクション動作を制御します。

- `BitcoinAddress` (`string`)
- `SolanaAddress` (`string`)
- `AssethubAddress` (`string`)
- `PolkadotAddress` (`string`)
- `TronAddress` (`string`)
- `Pools` (`string`)
- `ProbeVolume` (`decimal`)
- `OrderBookDepth` (`int`)
- `PollingInterval` (`TimeSpan`)
- `MaxBlocksPerPoll` (`int`)
- `InitialTickBlocks` (`int`)
- `SlippageTolerance` (`decimal`)
- `RetryDurationBlocks` (`int`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## 関連項目

[グラフィカル設定](graphical_configuration_chainflip.md)

[アダプターの初期化](adapter_initialization_chainflip.md)
