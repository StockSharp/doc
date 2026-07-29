# コネクタ設定: SEC EDGAR

SEC EDGAR に接続する前に、次のアダプタープロパティを設定します。この一覧は [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `DataEndpoint` (`Uri`)
- `UserAgent` (`string`)

## 詳細設定

これらのプロパティは、接続先、要求間隔、フィルター、データオプション、結果上限を制御します。

- `WebsiteEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Forms` (`string`)
- `MaximumHistoricalFiles` (`int`)
- `MaximumFacts` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_sec_edgar.md)

[アダプターの初期化](adapter_initialization_sec_edgar.md)
