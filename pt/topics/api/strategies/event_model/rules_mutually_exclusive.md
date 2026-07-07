# Regras mutuamente exclusivas

Regras mutuamente exclusivas são regras que são removidas depois da activação de uma dessas regras. Para isso, é chamado o método [MarketRuleHelper.Exclusive](xref:StockSharp.Algo.MarketRuleHelper.Exclusive(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule1, [StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule2 **)**, passando-lhe a regra. Esta regra será removida após a sua activação.

Por exemplo, são registadas duas regras: uma para o registo bem-sucedido da ordem e outra para o registo mal-sucedido da ordem. Uma delas deve ser removida em caso de activação da outra:

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
// registering the order
RegisterOrder(order);
		
```

Também é possível criar regras mutuamente exclusivas através da adição a [IMarketRule.ExclusiveRules](xref:StockSharp.Algo.IMarketRule.ExclusiveRules):

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
// registering the order
RegisterOrder(order);
		
```
