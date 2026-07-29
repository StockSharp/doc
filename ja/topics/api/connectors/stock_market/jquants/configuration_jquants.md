# コネクタ設定: J-Quants

J-Quants に接続する前に、次のアダプタープロパティを設定します。この一覧は [JQuantsMessageAdapter](xref:StockSharp.JQuants.JQuantsMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Key` (`SecureString`)

## 詳細設定

これらのプロパティは、接続先、要求間隔、フィルター、データオプション、結果上限を制御します。

- `RestEndpoint` (`string`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_jquants.md)

[アダプターの初期化](adapter_initialization_jquants.md)
