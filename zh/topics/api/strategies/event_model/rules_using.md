# 使用规则

## 制定规则

- **创建订单注册条件规则：**

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

现在，当事件触发时（订单在交易所登记），通过 [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** 方法指定的操作将被调用。

在规则形成结束时，会调用 [MarketRuleHelper.Apply](xref:StockSharp.Algo.MarketRuleHelper.Apply(StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule **)** 方法。在该方法为规则调用之前，该规则处于非活动状态（[IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** 中的处理程序不会被调用）。

- **在策略中创建规则：**

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
  
- **移除不必要的规则。**

[IMarketRule](xref:StockSharp.Algo.IMarketRule) 有 [IMarketRule.Token](xref:StockSharp.Algo.IMarketRule.Token) —— 一个与该规则相关的规则标记。例如，对于规则 [WhenCanceled](xref:StockSharp.Algo.MarketRuleHelper.WhenCanceled(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ISubscriptionProvider))，标记将是订单。

当成功取消订单的规则被触发时，最好删除与该订单相关的所有其他规则：

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
  
- **结合规则与条件 [MarketRuleHelper.Or](xref:StockSharp.Algo.MarketRuleHelper.Or(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) 规则, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) 规则 **)** / [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) 规则, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) 规则 **)**.

当时间到期 **或** K线收盘时：

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

或者这种格式：

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

当最后成交价高于135000 **且** 低于140000时：

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
  > [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** 中的处理程序将在最后通过 [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** 添加的规则触发之后被调用。
  
- **规则操作周期性 - [IMarketRule.Until](xref:StockSharp.Algo.IMarketRule.Until(System.Func{System.Boolean}))**（**[System.Func\<System.Boolean\>](xref:System.Func`1) canFinish **）**:**

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

## 使用规则的示例

### K线规则

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

### 关于订单簿（市场深度）的规则

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

### 带完成条件的规则

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

### 关于命令的规则

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

### 价格变动规则

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