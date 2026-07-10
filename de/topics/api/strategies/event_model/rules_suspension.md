# Regeln aussetzen

Manchmal sollen mehrere Regeln in einen Aussetzungsmodus versetzt werden, damit sie erst auslösen, wenn die Codeinitialisierung abgeschlossen ist. Dazu wird die Methode [MarketRuleHelper.SuspendRules](xref:StockSharp.Algo.MarketRuleHelper.SuspendRules(System.Action))**(**[System.Action](xref:System.Action) action **)** verwendet.

## Verwenden der Regelaussetzung

- Außerhalb der Strategie:

  ```cs
  MarketRuleHelper.SuspendRules(() =>
  {
  	order
  		.WhenRegistered(Connector)
		.Do(() => Connector.AddInfoLog("Order erfolgreich registriert."))
  		.Once()
  		.Apply(this);

  	order
  		.WhenCanceled(Connector)
		.Do(() => Connector.AddInfoLog("Order erfolgreich storniert."))
  		.Once()
  		.Apply(this);
  });

  ```
- Innerhalb der Strategie:

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
