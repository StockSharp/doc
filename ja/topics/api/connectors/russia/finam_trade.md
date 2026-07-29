# Finam Trade API

**Finam Trade API コネクター**は、StockSharp アプリケーションを Finam の証券口座と市場データに接続します。銘柄、気配値、注文、約定、ポートフォリオの状態を統一された StockSharp メッセージモデルへ変換します。

## 主な機能

- Finam で利用可能な株式、債券、通貨、ファンド、先物、オプションの検索。
- Level 1 気配値、板情報、市場約定、時間枠ローソク足。
- 過去のローソク足取得とリアルタイム市場データ購読。
- 成行、指値、ストップ、ストップリミット注文の発注と取消。
- 注文状態、自己約定、現金残高、ポジションの更新。
- API シークレットから短時間有効なセッショントークンへの自動交換。
- 互換ゲートウェイやテスト環境向けに設定可能な REST および WebSocket アドレス。

## 主な用途

Finam の市場データと取引を統一された StockSharp インターフェースで扱う売買ロボット、取引端末、ポートフォリオ監視、注文管理サービスに利用できます。

接続には Finam Trade API のシークレットが必要です。口座を明示的に選択するか、口座識別子を空にしてトークンで利用できる最初の口座を使用できます。銘柄は Finam の `ticker@MIC` 形式で識別されます。利用可能な市場、履歴範囲、リアルタイムデータ、取引権限、リクエスト制限は、接続口座と Finam のサービス条件によって異なります。

## 関連項目

[コネクタ設定](finam_trade/configuration_finam_trade.md)

[グラフィカル設定](finam_trade/graphical_configuration_finam_trade.md)

[アダプターの初期化](finam_trade/adapter_initialization_finam_trade.md)
