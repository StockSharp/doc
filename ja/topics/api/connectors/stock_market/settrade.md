# Settrade

**Settrade** は StockSharp を、タイ証券取引所の株式と TFEX デリバティブに対応する Settrade Open API v2 に接続します。アダプターは、対応する市場データとトランザクション操作を標準の StockSharp メッセージモデルを通じて提供します。

## 主な機能

- ソースコードで確認したアダプター機能: リアルタイムデータ更新、Level 1 クォート、板情報、ローソク足データ、履歴データ要求、株式口座とデリバティブ口座のポートフォリオおよび注文操作。
- プロバイダー固有の通信、セッション、データ形式は標準 StockSharp API の背後に隠蔽されます。

## 主な用途

SET と TFEX の銘柄監視、クォート、板情報、ローソク足の分析、履歴データの取得、および対応する注文処理に利用できます。

利用可能な銘柄、データの深さ、取引権限、要求制限、サービス提供状況は、Settrade、ブローカー、接続した口座によって決まります。

## 関連項目

[コネクタ設定](settrade/configuration_settrade.md)

[グラフィカル設定](settrade/graphical_configuration_settrade.md)

[アダプターの初期化](settrade/adapter_initialization_settrade.md)
