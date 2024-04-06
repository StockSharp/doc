# Приостановка правил

Иногда требуется установить одновременно несколько правил, чтобы каждое из них было активно только тогда, когда завершено формирование всех этих правил. Для этого используется метод [MarketRuleHelper.SuspendRules](xref:StockSharp.Algo.MarketRuleHelper.SuspendRules(System.Action))**(**[System.Action](xref:System.Action) action **)**. 

## Использование приостановки правил

- Вне стратегии:

  ```cs
  MarketRuleHelper.SuspendRules(() =>
  {
      order
          .WhenRegistered(Connector)
          .Do(() => Connector.AddInfoLog("Заявка успешно зарегистрирована"))
          .Once()
          .Apply(this);
      order
          .WhenCanceled(Connector)
          .Do(() => Connector.AddInfoLog("Заявка успешно отменена"))
          .Once()
          .Apply(this);
  });
  							
  ```
- Внутри стратегии:

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
