# イベントモデル

イベントモデルは [IMarketRule](xref:StockSharp.Algo.IMarketRule) を使用して構築されます。[IMarketRule](xref:StockSharp.Algo.IMarketRule) は、[ストラテジー](../strategies.md) の内部でも外部でも使用できます。

[API](../../api.md) には、一般的なシナリオ向けに調整された [IMarketRule](xref:StockSharp.Algo.IMarketRule) 用の定義済み条件とアクションがいくつかあります。これらは [MarketRuleHelper](xref:StockSharp.Algo.MarketRuleHelper) の拡張メソッドとして追加されています。

- [ルールの使用](event_model/rules_using.md)
- [ルールの同期とサスペンド](event_model/rules_suspension.md)
- [相互排他的なルール](event_model/rules_mutually_exclusive.md)
- [カスタムルールの作成](event_model/rules_create.md)
