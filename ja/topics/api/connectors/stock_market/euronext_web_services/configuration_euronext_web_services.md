# コネクタ設定: Euronext Web Services

Euronext Web Services に接続する前に、次のアダプタープロパティを設定します。この一覧は [EuronextWebServicesMessageAdapter](xref:StockSharp.EuronextWebServices.EuronextWebServicesMessageAdapter) の実装で確認されています。

## 基本設定

接続エディターでは、これらの設定が最初に表示されます。

- `Token` (`SecureString`)

## 詳細設定

これらのプロパティは、エンドポイント、フィルター、制限値、およびプロバイダー固有のその他の動作を制御します。

- `Address` (`Uri`)
- `SessionQuality` (`EuronextSessionQualities`)
- `IntradayDepth` (`int`)

## 関連項目

[グラフィカル設定](graphical_configuration_euronext_web_services.md)

[アダプターの初期化](adapter_initialization_euronext_web_services.md)
