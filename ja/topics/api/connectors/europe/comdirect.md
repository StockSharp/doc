# comdirect

**comdirect コネクター**は、StockSharp を金融市場のブローカーまたは電子取引所に接続します。 プロバイダー固有のデータと操作を統一された StockSharp メッセージモデルへ変換するため、異なる取引先でも同じ購読とワークフローを利用できます。

## 主な機能

- 主な対象：株式。
- 銘柄検索とプロバイダーの参照データ。
- プロバイダーが対応する注文送信と約定処理。
- ポートフォリオ、残高、ポジション、約定状態の更新。
- プロバイダー固有の通信、セッション、データ形式は標準 StockSharp API の背後に隠蔽されます。

## 主な用途

プロバイダーへ直接接続するライブ戦略、取引端末、注文管理サービス、監視ツールに利用できます。

利用可能な銘柄、データ深度、取引権限、制限、サービス状況は comdirect、API プラン、接続口座の権限によって決まります。

## 関連項目

[コネクタ設定](comdirect/configuration_comdirect.md)

[グラフィカル設定](comdirect/graphical_configuration_comdirect.md)

[アダプターの初期化](comdirect/adapter_initialization_comdirect.md)
