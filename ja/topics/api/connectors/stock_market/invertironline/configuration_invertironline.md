# コネクタ設定: InvertirOnline

InvertirOnline に接続する前に、次のアダプタープロパティを設定します。この一覧は [InvertirOnlineMessageAdapter](xref:StockSharp.InvertirOnline.InvertirOnlineMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `PortfolioName` (`string`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Token` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `DefaultCountry` (`InvertirOnlineCountries`)
- `DefaultMarket` (`string`)
- `DefaultInstrumentType` (`string`)
- `DefaultSettlement` (`InvertirOnlineSettlements`)
- `AdjustedHistory` (`bool`)
- `MarketDataPollingInterval` (`TimeSpan`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)

## 関連項目

[グラフィカル設定](graphical_configuration_invertironline.md)

[アダプターの初期化](adapter_initialization_invertironline.md)
