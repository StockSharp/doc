# Mutually exclusive rules

Mutually exclusive rules are rules which are removed after the activation of one of these rules. To do this, the [MarketRuleHelper.Exclusive](xref:StockSharp.Algo.MarketRuleHelper.Exclusive(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule)) method is called, and the rule is passed to it. This rule will be removed after its activation.

For example, two rules are registered: one on the successful order registration and another on the unsuccessful order registration. One of them must be removed in case of activation of another:

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

Also mutually exclusive rules can be created through the adding to [ExclusiveRules](xref:StockSharp.Algo.IMarketRule.ExclusiveRules):

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
