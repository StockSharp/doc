# Suspensão de regras

Por vezes pretende colocar várias regras em modo de suspensão (impedir que sejam accionadas até a inicialização do código estar concluída). Para isso, é usado o método [MarketRuleHelper.SuspendRules](xref:StockSharp.Algo.MarketRuleHelper.SuspendRules(System.Action))**(**[System.Action](xref:System.Action) action **)**.

## Utilizar a suspensão de regras

- Fora da estratégia:

  ```cs
  MarketRuleHelper.SuspendRules(() =>	
  {
  	order
  		.WhenRegistered(Connector)
		.Do(() => Connector.AddInfoLog("A ordem foi registrada com sucesso."))
  		.Once()
  		.Apply(this);
  	
  	order
  		.WhenCanceled(Connector)
		.Do(() => Connector.AddInfoLog("A ordem foi cancelada com sucesso."))
  		.Once()
  		.Apply(this);
  });
  							
  ```
- Dentro da estratégia:

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
