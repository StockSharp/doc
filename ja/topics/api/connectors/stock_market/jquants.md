# J-Quants

**J-Quants** は StockSharp を、東京証券取引所のデータを提供する J-Quants API V2 に接続します。

## 主な機能

- ソースコードで確認した機能: TSE の株式・先物・オプション検索、Level 1 値、約定、履歴ローソク足。市場データ専用で、取引操作は提供しません。
- プロバイダー固有の通信、セッション、データ形式は標準 StockSharp API の背後に隠蔽されます。

## 主な用途

日本の銘柄を検索し、研究・分析用にクォート、約定、ローソク足履歴を取得できます。

データ範囲、履歴の深さ、要求制限、利用可否は J-Quants の契約プランによって決まります。

## 関連項目

[コネクタ設定](jquants/configuration_jquants.md)

[グラフィカル設定](jquants/graphical_configuration_jquants.md)

[アダプターの初期化](jquants/adapter_initialization_jquants.md)
