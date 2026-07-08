# ルールのサスペンド

複数のルールをサスペンドモードに設定したい場合があります（コードの初期化が完了するまで、それらがトリガーされるのを防ぎます）。これを行うには、[MarketRuleHelper.SuspendRules](xref:StockSharp.Algo.MarketRuleHelper.SuspendRules(System.Action))**(**[System.Action](xref:System.Action) action **)** メソッドを使用します。

## ルールサスペンドの使用

- ストラテジー外:

  ```cs
  MarketRuleHelper.SuspendRules(() =>	
  {
  	order
  		.WhenRegistered(Connector)
  		.Do(() => Connector.AddInfoLog("The order was successfully registered."))
  		.Once()
  		.Apply(this);
  	
  	order
  		.WhenCanceled(Connector)
  		.Do(() => Connector.AddInfoLog("The order was successfully cancelled."))
  		.Once()
  		.Apply(this);
  });
  							
  ```
- ストラテジー内:

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

