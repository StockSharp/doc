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
         .Do(() => Connector.AddInfoLog("订单已成功注册"))
         .Once()
         .Apply(this);
      
     // 订单注册
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
          // K线订阅
          var candleSubscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
          this
              .WhenCandlesStarted(candleSubscription)
              .Do(ProcessCandle)
              .Apply(this);
              
          // tick 成交订阅
          var tickSubscription = new Subscription(DataType.Ticks, Security);
          tickSubscription
              .WhenTickTradeReceived(this)
              .Do(ProcessTick)
              .Apply(this);
              
          // 发送订阅请求
          Subscribe(candleSubscription);
          Subscribe(tickSubscription);
              
          base.OnStarted2(time);
      }
      
      // 事件处理方法
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
          this.AddInfoLog("订单已成功撤销");
          // 删除与订单关联的所有规则
          Rules.RemoveRulesByToken(ruleCanceled, (IMarketRule)ruleCanceled.Token);
      })
      .Once()
      .Apply(this);
  order
      .WhenRegistered(Connector)
      .Do(() => this.AddInfoLog("订单已成功注册"))
      .Once()
      .Apply(this);
  order
      .WhenRegisterFailed(Connector)
      .Do(() => this.AddInfoLog("订单未被交易所接受"))
      .Once()
      .Apply(this);
  order
      .WhenMatched(Connector)
      .Do(() => this.AddInfoLog("Order fully executed"))
      .Once()
      .Apply(this);
  // 订单注册
  RegisterOrder(order);
  ```
  
- **结合规则与条件 [MarketRuleHelper.Or](xref:StockSharp.Algo.MarketRuleHelper.Or(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) 规则, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) 规则 **)** / [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) 规则, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) 规则 **)**.

当时间到期 **或** K线收盘时：

  ```cs
  // 创建 K线订阅
  var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
  var timeInterval = TimeSpan.FromMilliseconds(5000);
  
  Connector
      .WhenIntervalElapsed(timeInterval)
      .Or(this.WhenCandlesStarted(subscription))
      .Do(() => this.AddInfoLog("Candle closed or time expired"))
      .Once()
      .Apply(this);
      
  // 发送订阅请求
  Subscribe(subscription);
  ```

或者这种格式：

  ```cs
  // 创建 K线订阅
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
      
  // 发送订阅请求
  Subscribe(subscription);
  ```

当最后成交价高于135000 **且** 低于140000时：

  ```cs
  // 创建 tick 成交订阅
  var subscription = new Subscription(DataType.Ticks, Security);
  var priceMore = new Unit(135000m, UnitTypes.Limit);
  var priceLess = new Unit(140000m, UnitTypes.Limit);
  				
  MarketRuleHelper
      .And(new IMarketRule[] {
          subscription.WhenLastTradePriceMore(this, 135000m), 
          subscription.WhenLastTradePriceLess(this, 140000m)
      })
      .Do(() => this.AddInfoLog($"最后成交价在 {priceMore} 到 {priceLess} 的范围内"))
      .Apply(this);
      
  // 发送订阅请求
  Subscribe(subscription);
  ```

  > [!TIP]
  > [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** 中的处理程序将在最后通过 [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** 添加的规则触发之后被调用。
  
- **规则操作周期性 - [IMarketRule.Until](xref:StockSharp.Algo.IMarketRule.Until(System.Func{System.Boolean}))**（**[System.Func\<System.Boolean\>](xref:System.Func`1) canFinish **）**:**

  ```cs
  bool flag = false;
  
  // 创建 tick 成交订阅
  var subscription = new Subscription(DataType.Ticks, Security);
  				
  subscription
      .WhenTickTradeReceived(this)
      .Do((tick) =>
      {
          if(condition) flag = true;
      })
      .Until(() => flag)			
      .Apply(this);
      
  // 发送订阅请求
  Subscribe(subscription);
  ```

## 使用规则的示例

### K线规则

```cs
// 创建 5 分钟 K线订阅
var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);

// 用于统计 K线数量的变量
var i = 0;
var diff = "10%".ToUnit();

// 新 K线开始时激活的规则
this.WhenCandlesStarted(subscription)
	.Do((candle) =>
	{
		i++;

		// 嵌套规则：检查总成交量是否超过阈值
		this
			.WhenTotalVolumeMore(candle, diff)
			.Do((candle1) =>
			{
				LogInfo($"规则 WhenCandlesStarted and WhenTotalVolumeMore candle={candle1}");
				LogInfo($"规则 WhenCandlesStarted and WhenTotalVolumeMore i={i}");
			})
			.Once().Apply(this);

	}).Apply(this);
	
// 发送订阅请求
Subscribe(subscription);
```

### 关于订单簿（市场深度）的规则

```cs
// 订单簿数据订阅
var mdSub = new Subscription(DataType.MarketDepth, Security);

// 方法 1：在链中创建规则
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// 方法 2：先创建规则变量
var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

whenMarketDepthChanged.Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// 规则中的规则
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

	// 未指定 Once() 的规则
	mdSub.WhenOrderBookReceived(this).Do((depth1) =>
	{
		LogInfo($"Rule WhenOrderBookReceived #4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
	}).Apply(this);
}).Once().Apply(this);

// 发送订阅请求
Subscribe(mdSub);
```

### 带完成条件的规则

```cs
// 订单簿数据订阅
var mdSub = new Subscription(DataType.MarketDepth, Security);

// Counter
var i = 0;

// 创建处理订单簿直到 i 达到 10 的规则
mdSub.WhenOrderBookReceived(this).Do(depth =>
{
	i++;
	LogInfo($"Rule WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	LogInfo($"Rule WhenOrderBookReceived i={i}");
})
.Until(() => i >= 10)
.Apply(this);

// 发送订阅请求
Subscribe(mdSub);
```

### 关于命令的规则

```cs
// tick 成交订阅
var sub = new Subscription(DataType.Ticks, Security);

// 收到第一个 tick 时创建订单
sub.WhenTickTradeReceived(this).Do(() =>
{
	var order = CreateOrder(Sides.Buy, default, 1);

	var ruleReg = order.WhenRegistered(this);
	var ruleRegFailed = order.WhenRegisterFailed(this);

	ruleReg
		.Do(() => LogInfo("Order #1 registered"))
		.Once()
		.Apply(this)
		.Exclusive(ruleRegFailed);  // 规则互斥

	ruleRegFailed
		.Do(() => LogInfo("订单 #1 未注册"))
		.Once()
		.Apply(this)
		.Exclusive(ruleReg);  // 规则互斥

	RegisterOrder(order);
}).Once().Apply(this);

// 发送订阅请求
Subscribe(sub);
```

### 价格变动规则

```cs
// tick 成交订阅
var sub = new Subscription(DataType.Ticks, Security);

// 规则在第一个 tick 时激活并创建另一条规则
sub.WhenTickTradeReceived(this).Do(t =>
{
	// 创建价格向任意方向移动 2 点时激活的规则
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

// 发送订阅请求
Subscribe(sub);
```
