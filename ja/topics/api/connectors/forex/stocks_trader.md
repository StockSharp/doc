# StocksTrader

**StocksTrader** は StockSharp を公式 StocksTrader REST API に接続します。

このコネクタはデモ口座と実口座の検出、口座状態のポーリング、銘柄検索、最新の買気配、売気配、最終価格のスナップショットをサポートします。取引では成行注文、指値注文、ストップ注文、待機注文の変更とキャンセル、保有ポジションのストップロスとテイクプロフィットの変更、ポジションのクローズ、注文と約定の履歴に対応します。

StocksTrader にはストリーミングの市場データ API がないため、Level 1 のリクエストは利用可能な最新のスナップショットを返して直ちに終了し、注文、約定、口座状態はポーリングで取得されます。

接続する前に、StocksTrader の Web ターミナルでベアラートークンを作成してください。

## 関連項目

[コネクタ設定](stocks_trader/configuration_stocks_trader.md)

[グラフィカル設定](stocks_trader/graphical_configuration_stocks_trader.md)

[アダプターの初期化](stocks_trader/adapter_initialization_stocks_trader.md)

[StocksTrader 公式 API ドキュメント](https://api-doc.stockstrader.com/)
