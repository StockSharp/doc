# Взаимоисключающие правила

Взаимоисключающие правила, это правила которые удаляются по активации одного из этих правил. Для этого вызвается метод [MarketRuleHelper.Exclusive](xref:StockSharp.Algo.MarketRuleHelper.Exclusive(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule1, [StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule2**)** в который передается правило, которое будет удалено при активации данного правила.

Например, регистрируется два правила, на успешную и неуспешную регистрацию заявки, одно из них нужно удалить в случае активации другого:

```cs
var order = this.CreateOrder(direction, (decimal) Security.GetCurrentPrice(direction), Volume);
var ruleReg = order.WhenRegistered(Connector);
var ruleRegFailed = order.WhenRegisterFailed(Connector);
ruleReg
    .Do(() => this.AddInfoLog("Заявка успешно зарегистрирована"))
    .Once()
    .Apply(this)
    .Exclusive(ruleRegFailed);
ruleRegFailed
    .Do(() => this.AddInfoLog("Заявка не принята биржей"))
    .Once()
    .Apply(this)
    .Exclusive(ruleReg);
// регистрирация заявки
RegisterOrder(order);
		
```

Также взаимоисключающие правила можно создать через добавление в [IMarketRule.ExclusiveRules](xref:StockSharp.Algo.IMarketRule.ExclusiveRules)

```cs
var order = this.CreateOrder(direction, (decimal) Security.GetCurrentPrice(direction), Volume);
var ruleReg = order.WhenRegistered(Connector);
var ruleRegFailed = order.WhenRegisterFailed(Connector);
ruleReg.ExclusiveRules.Add(ruleRegFailed);
ruleRegFailed.ExclusiveRules.Add(ruleReg);
ruleReg
    .Do(() => this.AddInfoLog("Заявка успешно зарегистрирована"))
    .Once()
    .Apply(this);
ruleRegFailed
    .Do(() => this.AddInfoLog("Заявка не принята биржей"))
    .Once()
    .Apply(this);
// регистрирация заявки
RegisterOrder(order);
		
```
