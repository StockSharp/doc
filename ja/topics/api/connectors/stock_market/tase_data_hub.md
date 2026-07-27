# TASE Data Hub

**TASE Data Hub コネクター**は、StockSharp をプロ向け市場データ・分析サービスに接続します。 プロバイダー固有のデータと操作を統一された StockSharp メッセージモデルへ変換するため、異なる取引先でも同じ購読とワークフローを利用できます。

## 主な機能

- 主な対象：株式。
- 銘柄検索とプロバイダーの参照データ。
- プロバイダーが対応する市場、企業、提出書類、開示、参照データ。
- アダプターが対応する市場データ：Level 1 クォート、ローソク足。
- チャート、分析、バックテスト向けの履歴データ要求。
- このアダプターはデータアクセス用であり、注文ルーティングは行いません。
- プロバイダー固有の通信、セッション、データ形式は標準 StockSharp API の背後に隠蔽されます。

## 主な用途

プロバイダーのデータをチャート、市場データ保存、分析、調査、戦略テストに利用できます。

利用可能な銘柄、データ深度、取引権限、制限、サービス状況は TASE Data Hub、API プラン、接続口座の権限によって決まります。

## 関連項目

[コネクタ設定](tase_data_hub/configuration_tase_data_hub.md)

[グラフィカル設定](tase_data_hub/graphical_configuration_tase_data_hub.md)

[アダプターの初期化](tase_data_hub/adapter_initialization_tase_data_hub.md)
