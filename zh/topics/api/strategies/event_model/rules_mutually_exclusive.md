# 互斥规则

互斥规则是指在其中一个规则被激活后会被移除的规则。为此，调用 [MarketRuleHelper.Exclusive](xref:StockSharp.Algo.MarketRuleHelper.Exclusive(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule1, [StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule2 **)** 方法，并将规则传递给它。该规则在激活后将被移除。

例如，注册了两个规则：一个用于成功的订单注册，另一个用于失败的订单注册。在另一条规则被激活的情况下，必须移除其中一条规则：

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
// 注册订单
RegisterOrder(order);
		
```

还可以通过添加到 [IMarketRule.ExclusiveRules](xref:StockSharp.Algo.IMarketRule.ExclusiveRules) 来创建互斥规则：

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
// 注册订单
RegisterOrder(order);
		
```
