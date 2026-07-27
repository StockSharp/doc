# コネクタ設定: PPI

PPI に接続する前に、次のアダプタープロパティを設定します。この一覧は [PpiMessageAdapter](xref:StockSharp.Ppi.PpiMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AuthorizedClient` (`string`)
- `ClientKey` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Token` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `DefaultMarket` (`string`)
- `DefaultInstrumentType` (`string`)
- `DefaultSettlement` (`string`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `RealtimeAddress` (`Uri`)
- `SandboxRealtimeAddress` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_ppi.md)

[アダプターの初期化](adapter_initialization_ppi.md)
