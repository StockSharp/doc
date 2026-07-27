# コネクタ設定: Definedge

Definedge に接続する前に、次のアダプタープロパティを設定します。この一覧は [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `WebSocketToken` (`SecureString`)
- `UserId` (`string`)
- `AccountId` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `OneTimePassword` (`SecureString`)
- `DefaultProduct` (`DefinedgeProducts`)
- `AlgoId` (`string`)
- `Address` (`Uri`)
- `LoginAddress` (`Uri`)
- `HistoryAddress` (`Uri`)
- `InstrumentMasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_definedge.md)

[アダプターの初期化](adapter_initialization_definedge.md)
