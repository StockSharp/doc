# コネクタ設定: Primary

Primary に接続する前に、次のアダプタープロパティを設定します。この一覧は [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Token` (`SecureString`)
- `Proprietary` (`string`)
- `DefaultMarket` (`string`)
- `MarketDataLevel` (`int`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SandboxWebSocketAddress` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_primary.md)

[アダプターの初期化](adapter_initialization_primary.md)
