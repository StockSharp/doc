# コネクタ設定: Nuvama

Nuvama に接続する前に、次のアダプタープロパティを設定します。この一覧は [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestId` (`SecureString`)
- `AppIdKey` (`SecureString`)
- `PublicIpAddress` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `VendorToken` (`SecureString`)
- `Token` (`SecureString`)
- `AccountId` (`string`)
- `UserId` (`string`)
- `AccountType` (`string`)
- `EmployeeOrDependent` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`NuvamaProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`Uri`)
- `IpAddressService` (`Uri`)
- `StreamHost` (`string`)
- `StreamPort` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_nuvama.md)

[アダプターの初期化](adapter_initialization_nuvama.md)
