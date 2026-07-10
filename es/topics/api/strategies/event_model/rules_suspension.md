# Suspensión de reglas

A veces es necesario poner varias reglas en modo de suspensión (evitar que se activen hasta que finalice la inicialización del código). Para ello se usa el método [MarketRuleHelper.SuspendRules](xref:StockSharp.Algo.MarketRuleHelper.SuspendRules(System.Action))**(**[System.Action](xref:System.Action) action **)**. 

## Uso de la suspensión de reglas

- Fuera de la estrategia:

  ```cs
  MarketRuleHelper.SuspendRules(() =>	
  {
  	order
  		.WhenRegistered(Connector)
		.Do(() => Connector.AddInfoLog("La orden se registró correctamente."))
  		.Once()
  		.Apply(this);
  	
  	order
  		.WhenCanceled(Connector)
		.Do(() => Connector.AddInfoLog("La orden se canceló correctamente."))
  		.Once()
  		.Apply(this);
  });
  							
  ```
- Dentro de la estrategia:

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
