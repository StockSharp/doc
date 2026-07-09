# Reglas mutuamente excluyentes

Las reglas mutuamente excluyentes son reglas que se eliminan después de la activación de una de ellas. Para ello, se llama al método [MarketRuleHelper.Exclusive](xref:StockSharp.Algo.MarketRuleHelper.Exclusive(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule1, [StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule2 **)** y se le pasa la regla. Esta regla se eliminará después de su activación.

Por ejemplo, se registran dos reglas: una para el registro correcto de la orden y otra para el registro incorrecto de la orden. Una de ellas debe eliminarse si se activa la otra:

```cs
var order = this.CreateOrder(direction, (decimal) Security.GetCurrentPrice(direction), Volume);
var ruleReg = order.WhenRegistered();
var ruleRegFailed = order.WhenRegisterFailed();
ruleReg
	.Do(() => this.AddInfoLog("La orden se registró correctamente."))
	.Once()
	.Apply(this)
	.Exclusive(ruleRegFailed);
ruleRegFailed
	.Do(() => this.AddInfoLog("El broker no aceptó la orden."))
	.Once()
	.Apply(this)
	.Exclusive(ruleReg);
// registro de la orden
RegisterOrder(order);
		
```

También se pueden crear reglas mutuamente excluyentes agregándolas a [IMarketRule.ExclusiveRules](xref:StockSharp.Algo.IMarketRule.ExclusiveRules):

```cs
var order = this.CreateOrder(direction, (decimal) Security.GetCurrentPrice(direction), Volume);
var ruleReg = order.WhenRegistered(Connector);
var ruleRegFailed = order.WhenRegisterFailed(Connector);
ruleReg.ExclusiveRules.Add(ruleRegFailed);
ruleRegFailed.ExclusiveRules.Add(ruleReg);
ruleReg
	.Do(() => this.AddInfoLog("La orden se registró correctamente."))
	.Once()
	.Apply(this);
ruleRegFailed
	.Do(() => this.AddInfoLog("El broker no aceptó la orden."))
	.Once()
	.Apply(this);
// registro de la orden
RegisterOrder(order);
		
```
