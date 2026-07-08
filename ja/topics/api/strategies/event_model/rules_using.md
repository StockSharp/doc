# ルールの使用

## ルールの作成

- **注文登録条件のルールの作成:**

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
      
     // 注文登録
     Connector.RegisterOrder(order);
  }
  ```

  これで、イベントが発生したとき（注文が取引所に登録されたとき）、[IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** メソッドで指定したアクションが呼び出されます。

  ルールの構築の最後に、[MarketRuleHelper.Apply](xref:StockSharp.Algo.MarketRuleHelper.Apply(StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule **)** メソッドを呼び出します。このメソッドがルールに対して呼び出されるまで、ルールは非アクティブです（[IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** のハンドラーは呼び出されません）。
  
- **ストラテジー内でのルールの作成:**

  ```cs
  class FirstStrategy : Strategy
  {
      protected override void OnStarted2(DateTime time)
      {
          // ローソク足へのサブスクリプション
          var candleSubscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
          this
              .WhenCandlesStarted(candleSubscription)
              .Do(ProcessCandle)
              .Apply(this);
              
          // ティック取引へのサブスクリプション
          var tickSubscription = new Subscription(DataType.Ticks, Security);
          tickSubscription
              .WhenTickTradeReceived(this)
              .Do(ProcessTick)
              .Apply(this);
              
          // サブスクリプション要求を送信
          Subscribe(candleSubscription);
          Subscribe(tickSubscription);
              
          base.OnStarted2(time);
      }
      
      // イベント処理用メソッド
      private void ProcessCandle(ICandleMessage candle) { /* ... */ }
      private void ProcessTick(ITickTradeMessage tick) { /* ... */ }
  }    
  ```
  
- **不要なルールの削除。**

  [IMarketRule](xref:StockSharp.Algo.IMarketRule) には [IMarketRule.Token](xref:StockSharp.Algo.IMarketRule.Token) があります。これは、そのルールが関連付けられているルールのトークンです。たとえば、[WhenCanceled](xref:StockSharp.Algo.MarketRuleHelper.WhenCanceled(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ISubscriptionProvider)) ルールの場合、トークンは注文になります。

  注文キャンセル成功のルールがトリガーされたら、この注文に関連する他のすべてのルールを削除することを推奨します。

  ```cs
  var order = this.CreateOrder(direction, (decimal)Security.GetCurrentPrice(direction), Volume);
  var ruleCanceled = order.WhenCanceled(Connector);
  ruleCanceled
      .Do(() =>
      {
          this.AddInfoLog("Order successfully canceled");
          // 注文に関連付けられたすべてのルールを削除
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
  // 注文登録
  RegisterOrder(order);
  ```
  
- **条件 [MarketRuleHelper.Or](xref:StockSharp.Algo.MarketRuleHelper.Or(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** / [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** によるルールの結合。**

  時間が経過した **OR** ローソク足がクローズした場合:

  ```cs
  // ローソク足へのサブスクリプションを作成
  var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
  var timeInterval = TimeSpan.FromMilliseconds(5000);
  
  Connector
      .WhenIntervalElapsed(timeInterval)
      .Or(this.WhenCandlesStarted(subscription))
      .Do(() => this.AddInfoLog("Candle closed or time expired"))
      .Once()
      .Apply(this);
      
  // サブスクリプション要求を送信
  Subscribe(subscription);
  ```

  または、次の形式です。

  ```cs
  // ローソク足へのサブスクリプションを作成
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
      
  // サブスクリプション要求を送信
  Subscribe(subscription);
  ```

  直近約定価格が 135000 より高く **AND** 140000 より低い場合:

  ```cs
  // ティック取引へのサブスクリプションを作成
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
      
  // サブスクリプション要求を送信
  Subscribe(subscription);
  ```

  > [!TIP]
  > [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** のハンドラーは、[MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** で追加された最後のルールがトリガーされた後に呼び出されます。
  
- **ルール動作の周期性 - [IMarketRule.Until](xref:StockSharp.Algo.IMarketRule.Until(System.Func{System.Boolean}))**(**[System.Func\<System.Boolean\>](xref:System.Func`1) canFinish **)**:**

  ```cs
  bool flag = false;
  
  // ティック取引へのサブスクリプションを作成
  var subscription = new Subscription(DataType.Ticks, Security);
  				
  subscription
      .WhenTickTradeReceived(this)
      .Do((tick) =>
      {
          if(condition) flag = true;
      })
      .Until(() => flag)			
      .Apply(this);
      
  // サブスクリプション要求を送信
  Subscribe(subscription);
  ```

## ルール使用例

### ローソク足のルール

```cs
// 5分足へのサブスクリプションを作成
var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);

// ローソク足をカウントする変数
var i = 0;
var diff = "10%".ToUnit();

// 新しいローソク足が開始したときにアクティブになるルール
this.WhenCandlesStarted(subscription)
	.Do((candle) =>
	{
		i++;

		// 入れ子のルール: 合計出来高がしきい値を超えたか確認
		this
			.WhenTotalVolumeMore(candle, diff)
			.Do((candle1) =>
			{
				LogInfo($"Rule WhenCandlesStarted and WhenTotalVolumeMore candle={candle1}");
				LogInfo($"Rule WhenCandlesStarted and WhenTotalVolumeMore i={i}");
			})
			.Once().Apply(this);

	}).Apply(this);
	
// サブスクリプション要求を送信
Subscribe(subscription);
```

### オーダーブック（Market Depth）のルール

```cs
// オーダーブックデータへのサブスクリプション
var mdSub = new Subscription(DataType.MarketDepth, Security);

// 方法 1: チェーン内でルールを作成
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// 方法 2: 先にルール変数を作成
var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

whenMarketDepthChanged.Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// ルール内のルール
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

	// Once() を指定しないルール
	mdSub.WhenOrderBookReceived(this).Do((depth1) =>
	{
		LogInfo($"Rule WhenOrderBookReceived #4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
	}).Apply(this);
}).Once().Apply(this);

// サブスクリプション要求を送信
Subscribe(mdSub);
```

### 完了条件付きのルール

```cs
// オーダーブックデータへのサブスクリプション
var mdSub = new Subscription(DataType.MarketDepth, Security);

// カウンター
var i = 0;

// i が 10 に達するまでオーダーブックを処理するルールを作成
mdSub.WhenOrderBookReceived(this).Do(depth =>
{
	i++;
	LogInfo($"Rule WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	LogInfo($"Rule WhenOrderBookReceived i={i}");
})
.Until(() => i >= 10)
.Apply(this);

// サブスクリプション要求を送信
Subscribe(mdSub);
```

### 注文のルール

```cs
// ティック取引へのサブスクリプション
var sub = new Subscription(DataType.Ticks, Security);

// 最初のティックを受信したら、注文を作成する
sub.WhenTickTradeReceived(this).Do(() =>
{
	var order = CreateOrder(Sides.Buy, default, 1);

	var ruleReg = order.WhenRegistered(this);
	var ruleRegFailed = order.WhenRegisterFailed(this);

	ruleReg
		.Do(() => LogInfo("Order #1 registered"))
		.Once()
		.Apply(this)
		.Exclusive(ruleRegFailed);  // ルールは相互排他的

	ruleRegFailed
		.Do(() => LogInfo("Order #1 not registered"))
		.Once()
		.Apply(this)
		.Exclusive(ruleReg);  // ルールは相互排他的

	RegisterOrder(order);
}).Once().Apply(this);

// サブスクリプション要求を送信
Subscribe(sub);
```

### 価格変化のルール

```cs
// ティック取引へのサブスクリプション
var sub = new Subscription(DataType.Ticks, Security);

// ルールは最初のティックでアクティブになり、別のルールを作成する
sub.WhenTickTradeReceived(this).Do(t =>
{
	// 価格がいずれかの方向に 2 ポイント動いたときにアクティブになるルールを作成
	sub
		.WhenLastTradePriceMore(this, t.Price + 2)
		.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
		.Do(t =>
		{
			LogInfo($"Rule WhenLastTradePriceMore or WhenLastTradePriceLess triggered: tick={t}");
		})
		.Apply(this);
})
.Once() // このルールは一度だけ呼び出す
.Apply(this);

// サブスクリプション要求を送信
Subscribe(sub);
```
