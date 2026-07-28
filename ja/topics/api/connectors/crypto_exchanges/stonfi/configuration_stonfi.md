# コネクタ設定: STON.fi

STON.fi に接続する前に、次のアダプタープロパティを設定します。この一覧は [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `ApiEndpoint` (`string`)
- `TonCenterEndpoint` (`string`)
- `TonCenterApiKey` (`SecureString`)
- `WalletAddress` (`string`)
- `Mnemonic` (`SecureString`)

## 詳細設定

これらのプロパティは、ウォレット情報、プール選択、制限、定期照会、履歴、トランザクション動作を制御します。

- `WalletSubwalletId` (`uint`)
- `WalletRevision` (`int`)
- `Pools` (`string`)
- `PoolLimit` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `HistoryBlockLimit` (`int`)
- `PrivatePollingInterval` (`TimeSpan`)
- `TransactionTimeout` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_stonfi.md)

[アダプターの初期化](adapter_initialization_stonfi.md)
