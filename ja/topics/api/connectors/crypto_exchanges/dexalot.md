# Dexalot

**Dexalot** は StockSharp を、ブロックチェーン上で動作する Dexalot の中央指値注文板に接続します。アダプターは、対応する市場データとトランザクション操作を標準の StockSharp メッセージモデルを通じて提供します。

## 主な機能

- ソースコードで確認したアダプター機能: リアルタイムデータ更新、Level 1 クォート、約定データ、板情報、ローソク足データ、履歴データ要求、ポートフォリオと注文の操作。
- プロバイダー固有の通信、セッション、データ形式は標準 StockSharp API の背後に隠蔽されます。

## 主な用途

Dexalot 市場の監視、クォート、約定、板情報、ローソク足の分析、履歴データの取得、および対応する注文の送信に利用できます。

利用可能な通貨ペア、データの深さ、トランザクション権限、要求制限、サービス提供状況は、Dexalot と接続したウォレットによって決まります。

## 関連項目

[コネクタ設定](dexalot/configuration_dexalot.md)

[グラフィカル設定](dexalot/graphical_configuration_dexalot.md)

[アダプターの初期化](dexalot/adapter_initialization_dexalot.md)
