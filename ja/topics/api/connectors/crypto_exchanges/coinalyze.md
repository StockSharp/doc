# Coinalyze

**Coinalyze** は StockSharp を暗号資産市場データサービス Coinalyze に接続します。アダプターは標準 StockSharp メッセージモデルを通じてプロバイダーのデータを公開します。

## 主な機能

- ソースコードで確認したアダプター機能：ローソク足データ、履歴データ要求、先物市場。
- プロバイダー固有の通信、セッション、データ形式は標準 StockSharp API の背後に隠蔽されます。
- このアダプターはデータアクセス用であり、注文ルーティングは行いません。

## 主な用途

プロバイダーのデータをチャート、市場データ保存、分析、調査、戦略テストに利用できます。

利用可能な銘柄、データ深度、トランザクション権限、要求制限、サービス状況は Coinalyze、API プラン、接続口座の権限によって決まります。

## 関連項目

[コネクタ設定](coinalyze/configuration_coinalyze.md)

[グラフィカル設定](coinalyze/graphical_configuration_coinalyze.md)

[アダプターの初期化](coinalyze/adapter_initialization_coinalyze.md)
