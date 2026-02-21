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

         var candleSub = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
         var tickSub = new Subscription(DataType.Ticks, Security);

         this.SuspendRules(() =>
         {
             Connector
                 .WhenCandlesFinished(candleSub)
                 .Do(FinishCandle)
                 .Apply(this);
             tickSub
                 .WhenTickTradeReceived(this)
                 .Do(NewTrade)
                 .Apply(this);
         });

         Subscribe(candleSub);
         Subscribe(tickSub);
     }
      ...
  }

  ```
