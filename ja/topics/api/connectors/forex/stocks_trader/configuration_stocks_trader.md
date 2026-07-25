# コネクタ設定: StocksTrader

StocksTrader の Web ターミナルでトークンを生成し、接続パラメーターを指定します。

- `Token` - Web ターミナルで発行されるベアラートークン。
- `AccountId` - 口座識別子。選択したモードに一致する口座がちょうど 1 つの場合は任意です。
- `IsDemo` - デモ口座を選択します。既定値は `true` です。
- `Address` - REST エンドポイント。既定値は `https://api.stockstrader.com/`。
- `PollingInterval` - 注文、約定、口座状態を要求する間隔。既定値は 5 秒で、これより短い値は 2 秒に引き上げられます。

プロバイダーがストリーミングを提供していないため、注文とポジションの変更が戦略に届く速さは `PollingInterval` で決まります。活発に取引する場合は短くし、プロバイダーのリクエスト制限内に収める場合は長くしてください。

保護価格は [StocksTraderOrderCondition](xref:StockSharp.StocksTrader.StocksTraderOrderCondition) を通じて渡されます。注文または保有ポジションのストップロス価格とテイクプロフィット価格です。

## 関連項目

[StocksTrader 公式 API ドキュメント](https://api-doc.stockstrader.com/)
