# CoinGlass

**CoinGlass** は StockSharp を暗号資産市場データサービス CoinGlass に接続します。アダプターは標準 StockSharp メッセージモデルを通じてプロバイダーのデータを公開します。

## 主な機能

- ソースコードで確認したアダプター機能：リアルタイム更新、Level 1 クォート、ローソク足データ、履歴データ要求、先物市場、オプション市場。
- プロバイダー固有の通信、セッション、データ形式は標準 StockSharp API の背後に隠蔽されます。
- このアダプターはデータアクセス用であり、注文ルーティングは行いません。

## 主な用途

プロバイダーのデータをチャート、市場データ保存、分析、調査、戦略テストに利用できます。

利用可能な銘柄、データ深度、トランザクション権限、要求制限、サービス状況は CoinGlass、API プラン、接続口座の権限によって決まります。

## 関連項目

[コネクタ設定](coinglass/configuration_coinglass.md)

[グラフィカル設定](coinglass/graphical_configuration_coinglass.md)

[アダプターの初期化](coinglass/adapter_initialization_coinglass.md)
