# コネクタ設定: OpenFIGI

OpenFIGI に接続する前に、次のアダプタープロパティを設定します。この一覧は [OpenFigiMessageAdapter](xref:StockSharp.OpenFigi.OpenFigiMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)

## 詳細設定

これらのプロパティは、接続先、要求間隔、フィルター、データオプション、結果上限を制御します。

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)
- `MaximumResults` (`int`)
- `ExchangeCode` (`string`)
- `MicCode` (`string`)
- `Currency` (`string`)
- `MarketSector` (`string`)
- `SecurityType2` (`string`)
- `IncludeUnlistedEquities` (`bool`)

## 関連項目

[グラフィカル設定](graphical_configuration_openfigi.md)

[アダプターの初期化](adapter_initialization_openfigi.md)
