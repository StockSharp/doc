# Velodrome

**Velodrome** は StockSharp を分散型プロトコル Velodrome に接続します。アダプターは対応する市場データとトランザクション操作を標準 StockSharp メッセージモデルを通じて公開します。

## 主な機能

- ソースコードで確認したアダプター機能：リアルタイム更新、Level 1 クォート、約定データ、ローソク足データ、履歴データ要求、ポートフォリオと注文操作。
- プロバイダー固有の通信、セッション、データ形式は標準 StockSharp API の背後に隠蔽されます。

## 主な用途

分散型市場の監視、価格分析、プロトコルアダプターが実装するトランザクション処理に利用できます。

利用可能な銘柄、データ深度、トランザクション権限、要求制限、サービス状況は Velodrome、API プラン、接続口座の権限によって決まります。

## 関連項目

[コネクタ設定](velodrome/configuration_velodrome.md)

[グラフィカル設定](velodrome/graphical_configuration_velodrome.md)

[アダプターの初期化](velodrome/adapter_initialization_velodrome.md)
