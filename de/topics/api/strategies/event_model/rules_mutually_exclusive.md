# Gegenseitig ausschließende Regeln

Gegenseitig ausschließende Regeln sind Regeln, die nach der Aktivierung einer dieser Regeln entfernt werden. Dazu wird die Methode [MarketRuleHelper.Exclusive](xref:StockSharp.Algo.MarketRuleHelper.Exclusive(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule1, [StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule2 **)** aufgerufen und die Regel an sie übergeben. Diese Regel wird nach ihrer Aktivierung entfernt.

Beispielsweise werden zwei Regeln registriert: eine für erfolgreiche Orderregistrierung und eine für fehlgeschlagene Orderregistrierung. Eine von ihnen muss entfernt werden, wenn die andere aktiviert wird:

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
// Order registrieren
RegisterOrder(order);
		
```

Gegenseitig ausschließende Regeln können auch durch Hinzufügen zu [IMarketRule.ExclusiveRules](xref:StockSharp.Algo.IMarketRule.ExclusiveRules) erstellt werden:

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
// Order registrieren
RegisterOrder(order);
		
```
