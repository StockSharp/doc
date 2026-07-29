# OpenFIGI

**OpenFIGI** は StockSharp を、世界の金融商品識別子を検索する OpenFIGI API v3 に接続します。

## 主な機能

- ソースコードで確認した機能: 取引所、MIC、通貨、市場セクター、証券種別による銘柄検索と絞り込み。メタデータのみを返し、クォートや取引操作は提供しません。
- プロバイダー固有の通信、セッション、データ形式は標準 StockSharp API の背後に隠蔽されます。

## 主な用途

外部識別子を FIGI レコードに対応付け、正規化した銘柄カタログを作成できます。

結果範囲、要求制限、匿名または API キーによるアクセスは OpenFIGI によって決まります。

## 関連項目

[コネクタ設定](openfigi/configuration_openfigi.md)

[グラフィカル設定](openfigi/graphical_configuration_openfigi.md)

[アダプターの初期化](openfigi/adapter_initialization_openfigi.md)
