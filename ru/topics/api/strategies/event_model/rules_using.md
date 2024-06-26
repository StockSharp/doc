# Использование правил

- **Создание правила на условие регистрации заявки:**

  ```cs
  private void btnBuy_Click(object sender, RoutedEventArgs e)
  {
     var order = new Order
     { 
         Portfolio = Portfolio.SelectedPortfolio,
         Price = _instr1.BestAsk.Price,
         Security = _instr1,
         Volume = 1,
         Direction = Sides.Buy,
     };
     order
         .WhenRegistered(Connector)
         .Do(() => Connector.AddInfoLog("Заявка успешно зарегистрирована"))
         .Once()
         .Apply(this);
      
  	// регистрация заявки
     Connector.RegisterOrder(order);
  }
  	  	  		
  ```

  Теперь, когда сработает событие (заявка будет зарегистрирована на бирже), указанное через метод [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** действие будет вызвано. 

  В конце формирования правила вызывается метод [MarketRuleHelper.Apply](xref:StockSharp.Algo.MarketRuleHelper.Apply(StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule **)**. До тех пор, пока метод не будет вызван для правила \- оно неактивно (обработчик в [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** не будет вызываться). 
- **Создание правил внутри стратегии:**

  ```cs
  class FirstStrategy : Strategy
  {
  	...
  	
  	protected override void OnStarting()
  	{
  		_connector
  			.WhenCandlesFinished(_series)
  			.Do(FinishCandle)
  			.Apply(this);
  		Security
  			.WhenNewTrade(_connector)
  			.Do(NewTrade)
  			.Apply(this);
  		base.OnStarting();
  	}
      
      ...
  }    
  	  	  		
  ```
- **Удаление ненужных правил.**

  У [IMarketRule](xref:StockSharp.Algo.IMarketRule) есть [IMarketRule.Token](xref:StockSharp.Algo.IMarketRule.Token) \- токен правила, с которым он ассоциирован. Например, для правила [MarketRuleHelper.WhenCanceled](xref:StockSharp.Algo.MarketRuleHelper.WhenCanceled(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ITransactionProvider))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order, [StockSharp.BusinessEntities.ITransactionProvider](xref:StockSharp.BusinessEntities.ITransactionProvider) provider **)** токеном будет являться заявка.

  Когда сработало правило успешной отмены заявки, то лучше удалить все остальные правила, связанные с этой заявкой:

  ```cs
  var order = this.CreateOrder(direction, (decimal) Security.GetCurrentPrice(direction), Volume);
  var ruleCanceled = order.WhenCanceled(Connector);
  ruleCanceled
      .Do(() =>
      {
          this.AddInfoLog("Заявка успешно отменена");
          // удаление всех правил связанных с order
          Rules.RemoveRulesByToken(ruleCanceled, (IMarketRule) ruleCanceled.Token);
      })
      .Once()
      .Apply(this);
  order
      .WhenRegistered(Connector)
      .Do(() => this.AddInfoLog("Заявка успешно зарегистрирована"))
      .Once()
      .Apply(this);
  order
      .WhenRegisterFailed(Connector)
      .Do(() => this.AddInfoLog("Заявка не принята биржей"))
      .Once()
      .Apply(this);
  order
      .WhenMatched(Connector)
      .Do(() => this.AddInfoLog("Заявка полностью исполнена"))
      .Once()
      .Apply(this);
  // регистрирация заявки
  RegisterOrder(order);
  	  	  		
  ```
- **Oбъединение правил по условию [MarketRuleHelper.Or](xref:StockSharp.Algo.MarketRuleHelper.Or(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** \/ [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)**.**

  Когда выйдет время **ИЛИ** закроется свеча:

  ```cs
  CandleSeries _series;
  TimeSpan _holdTimeToOpen = TimeSpan.FromMilliseconds(5000);
  ...
  _connector
  	.WhenIntervalElapsed(_holdTimeToOpen)
  	.Or(_connector.WhenCandlesStarted(_series))
  	.Do(() => this.AddInfoLog("Свеча закрыта или вышло время"))
  	.Once()
  	.Apply(this);
  	  	  		
  ```

  Или такой формат записи:

  ```cs
  MarketRuleHelper
  	.Or(new IMarketRule[] {_connector.WhenIntervalElapsed(_holdTimeToOpen), _connector.WhenCandlesStarted(_series)})
  	.Do(() => this.AddInfoLog("Свеча закрыта или вышло время"))
  	.Once()
  	.Apply(this);
  	  	  		
  ```

  Когда цена последней сделки будет выше 135000 **И** ниже 140000:

  ```cs
  var priceMore = new Unit(135000m, UnitTypes.Limit);
  var priceLess = new Unit(140000m, UnitTypes.Limit);
  				
  MarketRuleHelper
  	.And(new IMarketRule[] {Security.WhenLastTradePriceMore(Connector, Connector, priceMore), Security.WhenLastTradePriceLess(Connector, Connector, priceLess)})
  	.Do(() => this.AddInfoLog(string.Format("Цена последней сделки находится в диапазоне от {0} до {1}", priceMore, priceLess)))
  	.Apply(this);
  	  	  		
  ```

  > [!TIP]
  > Обработчик в [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** вызовется после того, как сработает последнее правило добавленное через [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)**.
- **Периодичность работы правила \- [IMarketRule.Until](xref:StockSharp.Algo.IMarketRule.Until(System.Func{System.Boolean}))**(**[System.Func\<System.Boolean\>](xref:System.Func`1) canFinish **)**:**

  ```cs
  bool flag = false;
  ...
  				
  Security
  	.WhenNewTrade(Connector)
  	.Do(() =>
  			{
  				if(условие) flag = true;
  			})
  	.Until(() => flag)			
  	.Apply(this);
  	  	  		
  ```
