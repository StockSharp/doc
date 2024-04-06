# Rules suspension

Sometimes you want to set multiple rules in suspension mode (prevent to trigger any of them until code initialization will be finished). To do this, the [MarketRuleHelper.SuspendRules](xref:StockSharp.Algo.MarketRuleHelper.SuspendRules(System.Action))**(**[System.Action](xref:System.Action) action **)** method is used. 

## Rules suspension using

- Out of the strategy:

  ```cs
  MarketRuleHelper.SuspendRules(() =>	
  {
  	order
  		.WhenRegistered(Connector)
  		.Do(() => Connector.AddInfoLog("The order was successfully registered."))
  		.Once()
  		.Apply(this);
  	
  	order
  		.WhenCancelled(Connector)
  		.Do(() => Connector.AddInfoLog("The order was successfully cancelled."))
  		.Once()
  		.Apply(this);
  });
  							
  ```
- Inside the strategy:

  ```cs
  class FirstStrategy : Strategy
  {
  	...
  	
         this.SuspendRules(() =>
         {
  		_connector
                 .WhenCandlesFinished(_series)
                 .Do(FinishCandle)
                 .Apply(this);
             Security
                 .WhenNewTrade(Connector)
                 .Do(NewTrade)
                 .Apply(this);
         });
     }
      ...
  }
  							
  ```
