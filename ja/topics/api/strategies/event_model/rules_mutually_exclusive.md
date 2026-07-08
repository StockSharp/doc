# 相互排他的なルール

相互排他的なルールとは、それらのうち 1 つがアクティブ化された後に削除されるルールです。これを行うには、[MarketRuleHelper.Exclusive](xref:StockSharp.Algo.MarketRuleHelper.Exclusive(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule1, [StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule2 **)** メソッドを呼び出し、対象のルールを渡します。このルールは、アクティブ化後に削除されます。

たとえば、2 つのルールが登録されています。1 つは注文登録の成功に対するルール、もう 1 つは注文登録の失敗に対するルールです。片方がアクティブ化された場合、もう片方は削除される必要があります。

```cs
var order = this.CreateOrder(direction, (decimal) Security.GetCurrentPrice(direction), Volume);
var ruleReg = order.WhenRegistered();
var ruleRegFailed = order.WhenRegisterFailed();
ruleReg
	.Do(() => this.AddInfoLog("The order was successfully registered."))
	.Once()
	.Apply(this)
	.Exclusive(ruleRegFailed);
ruleRegFailed
	.Do(() => this.AddInfoLog("The order was not accepted by broker."))
	.Once()
	.Apply(this)
	.Exclusive(ruleReg);
// 注文の登録
RegisterOrder(order);
		
```

また、相互排他的なルールは [IMarketRule.ExclusiveRules](xref:StockSharp.Algo.IMarketRule.ExclusiveRules) に追加することでも作成できます。

```cs
var order = this.CreateOrder(direction, (decimal) Security.GetCurrentPrice(direction), Volume);
var ruleReg = order.WhenRegistered(Connector);
var ruleRegFailed = order.WhenRegisterFailed(Connector);
ruleReg.ExclusiveRules.Add(ruleRegFailed);
ruleRegFailed.ExclusiveRules.Add(ruleReg);
ruleReg
	.Do(() => this.AddInfoLog("The order was successfully registered."))
	.Once()
	.Apply(this);
ruleRegFailed
	.Do(() => this.AddInfoLog("The order was not accepted by broker."))
	.Once()
	.Apply(this);
// 注文の登録
RegisterOrder(order);
		
```
