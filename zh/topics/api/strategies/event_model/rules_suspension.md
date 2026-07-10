# 规则暂停

有时你可能想将多个规则设置为挂起模式（防止它们在代码初始化完成之前触发）。为此，可以使用 [MarketRuleHelper.SuspendRules](xref:StockSharp.Algo.MarketRuleHelper.SuspendRules(System.Action))**(**[System.Action](xref:System.Action) 操作 **)** 方法。

## 使用规则暂停

- 出于策略：

  ```cs
  MarketRuleHelper.SuspendRules(() =>	
  {
  	order
  		.WhenRegistered(Connector)
		.Do(() => Connector.AddInfoLog("订单已成功注册。"))
  		.Once()
  		.Apply(this);
  	
  	order
  		.WhenCanceled(Connector)
		.Do(() => Connector.AddInfoLog("订单已成功撤销。"))
  		.Once()
  		.Apply(this);
  });
  							
  ```
- 策略内部：

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
