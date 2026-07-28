# Pendle

**Pendle** は StockSharp を、対応する EVM ネットワーク上の Pendle 利回り市場に接続します。アダプターは、対応する市場データとトランザクション操作を標準の StockSharp メッセージモデルを通じて提供します。

## 主な機能

- ソースコードで確認したアダプター機能: リアルタイムデータ更新、Level 1 クォート、ローソク足データ、履歴データ要求、ポートフォリオと注文の操作。
- プロバイダー固有の通信、セッション、データ形式は標準 StockSharp API の背後に隠蔽されます。

## 主な用途

Pendle 利回り市場の監視、クォートとローソク足の分析、履歴データの取得、およびプロトコルアダプターが実装するトランザクション処理に利用できます。

利用可能なネットワークと市場、トランザクション権限、要求制限、サービス提供状況は、Pendle と接続したウォレットによって決まります。

## 関連項目

[コネクタ設定](pendle/configuration_pendle.md)

[グラフィカル設定](pendle/graphical_configuration_pendle.md)

[アダプターの初期化](pendle/adapter_initialization_pendle.md)
