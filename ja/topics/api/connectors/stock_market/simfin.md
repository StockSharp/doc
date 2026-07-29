# SimFin

**SimFin** は StockSharp を、企業、価格、ファンダメンタルデータを提供する SimFin Web API v3 に接続します。

## 主な機能

- ソースコードで確認した機能: 企業検索、日次 Level 1 価格、日足、`SimFinDataTypes.Fundamentals` による構造化ファンダメンタルデータ。履歴専用で取引は提供しません。
- プロバイダー固有の通信、セッション、データ形式は標準 StockSharp API の背後に隠蔽されます。

## 主な用途

対応企業の日次価格、財務諸表、派生値、任意の財務比率を取得できます。

企業範囲、履歴、項目、要求制限、利用可否は SimFin の契約プランによって決まります。

## 関連項目

[コネクタ設定](simfin/configuration_simfin.md)

[グラフィカル設定](simfin/graphical_configuration_simfin.md)

[アダプターの初期化](simfin/adapter_initialization_simfin.md)
