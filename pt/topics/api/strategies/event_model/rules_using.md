# Utilizar Regras

## Criar Regras

- **Criar uma regra para a condição de registo de ordem:**

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
         .Do(() => Connector.AddInfoLog("Order successfully registered"))
         .Once()
         .Apply(this);
      
     // order registration
     Connector.RegisterOrder(order);
  }
  ```

  Agora, quando o evento ocorrer (a ordem for registada na bolsa), será chamada a acção especificada através do método [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)**.

  No fim da formação da regra, é chamado o método [MarketRuleHelper.Apply](xref:StockSharp.Algo.MarketRuleHelper.Apply(StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule **)**. Enquanto este método não for chamado para a regra, ela fica inactiva (o processador em [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** não será chamado).
  
- **Criar regras dentro de uma estratégia:**

  ```cs
  class FirstStrategy : Strategy
  {
      protected override void OnStarted2(DateTime time)
      {
          // Subscription to candles
          var candleSubscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
          this
              .WhenCandlesStarted(candleSubscription)
              .Do(ProcessCandle)
              .Apply(this);
              
          // Subscription to tick trades
          var tickSubscription = new Subscription(DataType.Ticks, Security);
          tickSubscription
              .WhenTickTradeReceived(this)
              .Do(ProcessTick)
              .Apply(this);
              
          // Send subscription requests
          Subscribe(candleSubscription);
          Subscribe(tickSubscription);
              
          base.OnStarted2(time);
      }
      
      // Methods for event processing
      private void ProcessCandle(ICandleMessage candle) { /* ... */ }
      private void ProcessTick(ITickTradeMessage tick) { /* ... */ }
  }    
  ```
  
- **Remover regras desnecessárias.**

  [IMarketRule](xref:StockSharp.Algo.IMarketRule) tem [IMarketRule.Token](xref:StockSharp.Algo.IMarketRule.Token), um token da regra à qual está associado. Por exemplo, para a regra [WhenCanceled](xref:StockSharp.Algo.MarketRuleHelper.WhenCanceled(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ISubscriptionProvider)), o token será a ordem.

  Quando uma regra para cancelamento bem-sucedido de uma ordem for accionada, é melhor remover todas as outras regras relacionadas com esta ordem:

  ```cs
  var order = this.CreateOrder(direction, (decimal)Security.GetCurrentPrice(direction), Volume);
  var ruleCanceled = order.WhenCanceled(Connector);
  ruleCanceled
      .Do(() =>
      {
          this.AddInfoLog("Order successfully canceled");
          // removing all rules associated with order
          Rules.RemoveRulesByToken(ruleCanceled, (IMarketRule)ruleCanceled.Token);
      })
      .Once()
      .Apply(this);
  order
      .WhenRegistered(Connector)
      .Do(() => this.AddInfoLog("Order successfully registered"))
      .Once()
      .Apply(this);
  order
      .WhenRegisterFailed(Connector)
      .Do(() => this.AddInfoLog("Order not accepted by the exchange"))
      .Once()
      .Apply(this);
  order
      .WhenMatched(Connector)
      .Do(() => this.AddInfoLog("Order fully executed"))
      .Once()
      .Apply(this);
  // order registration
  RegisterOrder(order);
  ```
  
- **Combinar regras com a condição [MarketRuleHelper.Or](xref:StockSharp.Algo.MarketRuleHelper.Or(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** / [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)**.**

  Quando o tempo expira **OU** uma vela fecha:

  ```cs
  // Create a subscription to candles
  var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
  var timeInterval = TimeSpan.FromMilliseconds(5000);
  
  Connector
      .WhenIntervalElapsed(timeInterval)
      .Or(this.WhenCandlesStarted(subscription))
      .Do(() => this.AddInfoLog("Candle closed or time expired"))
      .Once()
      .Apply(this);
      
  // Send subscription request
  Subscribe(subscription);
  ```

  Ou neste formato:

  ```cs
  // Create a subscription to candles
  var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
  var timeInterval = TimeSpan.FromMilliseconds(5000);
  
  MarketRuleHelper
      .Or(new IMarketRule[] {
          Connector.WhenIntervalElapsed(timeInterval), 
          this.WhenCandlesStarted(subscription)
      })
      .Do(() => this.AddInfoLog("Candle closed or time expired"))
      .Once()
      .Apply(this);
      
  // Send subscription request
  Subscribe(subscription);
  ```

  Quando o preço do último negócio está acima de 135000 **E** abaixo de 140000:

  ```cs
  // Create a subscription to tick trades
  var subscription = new Subscription(DataType.Ticks, Security);
  var priceMore = new Unit(135000m, UnitTypes.Limit);
  var priceLess = new Unit(140000m, UnitTypes.Limit);
  				
  MarketRuleHelper
      .And(new IMarketRule[] {
          subscription.WhenLastTradePriceMore(this, 135000m), 
          subscription.WhenLastTradePriceLess(this, 140000m)
      })
      .Do(() => this.AddInfoLog($"Last trade price is in the range from {priceMore} to {priceLess}"))
      .Apply(this);
      
  // Send subscription request
  Subscribe(subscription);
  ```

  > [!TIP]
  > O processador em [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** será chamado depois de a última regra adicionada através de [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** ser accionada.
  
- **Periodicidade de funcionamento da regra - [IMarketRule.Until](xref:StockSharp.Algo.IMarketRule.Until(System.Func{System.Boolean}))**(**[System.Func\<System.Boolean\>](xref:System.Func`1) canFinish **)**:**

  ```cs
  bool flag = false;
  
  // Create a subscription to tick trades
  var subscription = new Subscription(DataType.Ticks, Security);
  				
  subscription
      .WhenTickTradeReceived(this)
      .Do((tick) =>
      {
          if(condition) flag = true;
      })
      .Until(() => flag)			
      .Apply(this);
      
  // Send subscription request
  Subscribe(subscription);
  ```

## Exemplos de Utilização de Regras

### Regras em Velas

```cs
// Create a subscription to 5-minute candles
var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);

// Variable for counting candles
var i = 0;
var diff = "10%".ToUnit();

// Rule that activates when a new candle starts
this.WhenCandlesStarted(subscription)
	.Do((candle) =>
	{
		i++;

		// Nested rule: check when total volume exceeds threshold
		this
			.WhenTotalVolumeMore(candle, diff)
			.Do((candle1) =>
			{
				LogInfo($"Rule WhenCandlesStarted and WhenTotalVolumeMore candle={candle1}");
				LogInfo($"Rule WhenCandlesStarted and WhenTotalVolumeMore i={i}");
			})
			.Once().Apply(this);

	}).Apply(this);
	
// Send subscription request
Subscribe(subscription);
```

### Regras em Livros de Ordens (Profundidade de Mercado)

```cs
// Subscription to order book data
var mdSub = new Subscription(DataType.MarketDepth, Security);

// Method 1: Creating a rule in a chain
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// Method 2: First create a rule variable
var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

whenMarketDepthChanged.Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// Rule within a rule
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

	// Rule without specifying Once()
	mdSub.WhenOrderBookReceived(this).Do((depth1) =>
	{
		LogInfo($"Rule WhenOrderBookReceived #4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
	}).Apply(this);
}).Once().Apply(this);

// Send subscription request
Subscribe(mdSub);
```

### Regras com Condição de Conclusão

```cs
// Subscription to order book data
var mdSub = new Subscription(DataType.MarketDepth, Security);

// Counter
var i = 0;

// Create a rule that processes order books until i reaches 10
mdSub.WhenOrderBookReceived(this).Do(depth =>
{
	i++;
	LogInfo($"Rule WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	LogInfo($"Rule WhenOrderBookReceived i={i}");
})
.Until(() => i >= 10)
.Apply(this);

// Send subscription request
Subscribe(mdSub);
```

### Regras em Ordens

```cs
// Subscription to tick trades
var sub = new Subscription(DataType.Ticks, Security);

// When we receive the first tick, we'll create an order
sub.WhenTickTradeReceived(this).Do(() =>
{
	var order = CreateOrder(Sides.Buy, default, 1);

	var ruleReg = order.WhenRegistered(this);
	var ruleRegFailed = order.WhenRegisterFailed(this);

	ruleReg
		.Do(() => LogInfo("Order #1 registered"))
		.Once()
		.Apply(this)
		.Exclusive(ruleRegFailed);  // Rules are mutually exclusive

	ruleRegFailed
		.Do(() => LogInfo("Order #1 not registered"))
		.Once()
		.Apply(this)
		.Exclusive(ruleReg);  // Rules are mutually exclusive

	RegisterOrder(order);
}).Once().Apply(this);

// Send subscription request
Subscribe(sub);
```

### Regras em Alterações de Preço

```cs
// Subscription to tick trades
var sub = new Subscription(DataType.Ticks, Security);

// Rule activates on the first tick and creates another rule
sub.WhenTickTradeReceived(this).Do(t =>
{
	// Create a rule that activates when the price moves 2 points in any direction
	sub
		.WhenLastTradePriceMore(this, t.Price + 2)
		.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
		.Do(t =>
		{
			LogInfo($"Rule WhenLastTradePriceMore or WhenLastTradePriceLess triggered: tick={t}");
		})
		.Apply(this);
})
.Once() // call this rule only once
.Apply(this);

// Send subscription request
Subscribe(sub);
```
