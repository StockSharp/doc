# コネクタ設定: Choice FinX

Choice FinX に接続する前に、次のアダプタープロパティを設定します。この一覧は [ChoiceFinXMessageAdapter](xref:StockSharp.ChoiceFinX.ChoiceFinXMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)
- `AuthorizationHeader` (`string`)
- `AuthorizationScheme` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `VendorId` (`string`)
- `VendorKey` (`SecureString`)
- `WebSocketToken` (`SecureString`)
- `DefaultProduct` (`ChoiceFinXProducts`)
- `PortfolioName` (`string`)
- `ModeType` (`string`)
- `Mode` (`int?`)
- `DeviceId` (`string`)
- `PriceDivisor` (`decimal`)
- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## 関連項目

[グラフィカル設定](graphical_configuration_choice_finx.md)

[アダプターの初期化](adapter_initialization_choice_finx.md)
