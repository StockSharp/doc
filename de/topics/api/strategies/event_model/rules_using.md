# Regeln verwenden

## Regeln erstellen

- **Erstellen einer Regel für die Bedingung der Orderregistrierung:**

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

     // Orderregistrierung
     Connector.RegisterOrder(order);
  }
  ```

  Wenn nun das Ereignis ausgelöst wird (die Order wird an der Börse registriert), wird die über die Methode [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** angegebene Aktion aufgerufen.

  Am Ende der Regelbildung wird die Methode [MarketRuleHelper.Apply](xref:StockSharp.Algo.MarketRuleHelper.Apply(StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule **)** aufgerufen. Bis diese Methode für die Regel aufgerufen wurde, ist sie inaktiv (der Handler in [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** wird nicht aufgerufen).

- **Erstellen von Regeln innerhalb einer Strategie:**

  ```cs
  class FirstStrategy : Strategy
  {
      protected override void OnStarted2(DateTime time)
      {
          // Abonnement für Candles
          var candleSubscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
          this
              .WhenCandlesStarted(candleSubscription)
              .Do(ProcessCandle)
              .Apply(this);

          // Abonnement für Tick-Trades
          var tickSubscription = new Subscription(DataType.Ticks, Security);
          tickSubscription
              .WhenTickTradeReceived(this)
              .Do(ProcessTick)
              .Apply(this);

          // Abonnementanfragen senden
          Subscribe(candleSubscription);
          Subscribe(tickSubscription);

          base.OnStarted2(time);
      }

      // Methoden zur Ereignisverarbeitung
      private void ProcessCandle(ICandleMessage candle) { /* ... */ }
      private void ProcessTick(ITickTradeMessage tick) { /* ... */ }
  }
  ```

- **Entfernen nicht benötigter Regeln.**

  [IMarketRule](xref:StockSharp.Algo.IMarketRule) besitzt [IMarketRule.Token](xref:StockSharp.Algo.IMarketRule.Token) - ein Token der Regel, mit dem sie verknüpft ist. Für die Regel [WhenCanceled](xref:StockSharp.Algo.MarketRuleHelper.WhenCanceled(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ISubscriptionProvider)) ist das Token beispielsweise die Order.

  Wenn eine Regel für erfolgreiche Orderstornierung ausgelöst wurde, ist es besser, alle anderen mit dieser Order verbundenen Regeln zu entfernen:

  ```cs
  var order = this.CreateOrder(direction, (decimal)Security.GetCurrentPrice(direction), Volume);
  var ruleCanceled = order.WhenCanceled(Connector);
  ruleCanceled
      .Do(() =>
      {
          this.AddInfoLog("Order successfully canceled");
          // Alle mit der Order verbundenen Regeln entfernen
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
  // Orderregistrierung
  RegisterOrder(order);
  ```

- **Kombinieren von Regeln mit der Bedingung [MarketRuleHelper.Or](xref:StockSharp.Algo.MarketRuleHelper.Or(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** / [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)**.**

  Wenn die Zeit abläuft **ODER** eine Candle schließt:

  ```cs
  // Abonnement für Candles erstellen
  var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
  var timeInterval = TimeSpan.FromMilliseconds(5000);

  Connector
      .WhenIntervalElapsed(timeInterval)
      .Or(this.WhenCandlesStarted(subscription))
      .Do(() => this.AddInfoLog("Candle closed or time expired"))
      .Once()
      .Apply(this);

  // Abonnementanfrage senden
  Subscribe(subscription);
  ```

  Oder in diesem Format:

  ```cs
  // Abonnement für Candles erstellen
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

  // Abonnementanfrage senden
  Subscribe(subscription);
  ```

  Wenn der Preis des letzten Trades über 135000 **UND** unter 140000 liegt:

  ```cs
  // Abonnement für Tick-Trades erstellen
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

  // Abonnementanfrage senden
  Subscribe(subscription);
  ```

  > [!TIP]
  > Der Handler in [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** wird aufgerufen, nachdem die letzte über [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** hinzugefügte Regel ausgelöst wurde.

- **Periodizität der Regelausführung - [IMarketRule.Until](xref:StockSharp.Algo.IMarketRule.Until(System.Func{System.Boolean}))**(**[System.Func\<System.Boolean\>](xref:System.Func`1) canFinish **)**:**

  ```cs
  bool flag = false;

  // Abonnement für Tick-Trades erstellen
  var subscription = new Subscription(DataType.Ticks, Security);

  subscription
      .WhenTickTradeReceived(this)
      .Do((tick) =>
      {
          if(condition) flag = true;
      })
      .Until(() => flag)
      .Apply(this);

  // Abonnementanfrage senden
  Subscribe(subscription);
  ```

## Beispiele für die Verwendung von Regeln

### Regeln auf Candles

```cs
// Abonnement für 5-Minuten-Candles erstellen
var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);

// Variable zum Zählen von Candles
var i = 0;
var diff = "10%".ToUnit();

// Regel, die aktiviert wird, wenn eine neue Candle startet
this.WhenCandlesStarted(subscription)
	.Do((candle) =>
	{
		i++;

		// Verschachtelte Regel: prüfen, wann das Gesamtvolumen den Schwellenwert überschreitet
		this
			.WhenTotalVolumeMore(candle, diff)
			.Do((candle1) =>
			{
				LogInfo($"Rule WhenCandlesStarted and WhenTotalVolumeMore candle={candle1}");
				LogInfo($"Rule WhenCandlesStarted and WhenTotalVolumeMore i={i}");
			})
			.Once().Apply(this);

	}).Apply(this);

// Abonnementanfrage senden
Subscribe(subscription);
```

### Regeln auf Orderbücher (Market Depth)

```cs
// Abonnement für Orderbuchdaten
var mdSub = new Subscription(DataType.MarketDepth, Security);

// Methode 1: Regel in einer Kette erstellen
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// Methode 2: Zuerst eine Regelvariable erstellen
var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

whenMarketDepthChanged.Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// Regel innerhalb einer Regel
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

	// Regel ohne Angabe von Once()
	mdSub.WhenOrderBookReceived(this).Do((depth1) =>
	{
		LogInfo($"Rule WhenOrderBookReceived #4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
	}).Apply(this);
}).Once().Apply(this);

// Abonnementanfrage senden
Subscribe(mdSub);
```

### Regeln mit Abschlussbedingung

```cs
// Abonnement für Orderbuchdaten
var mdSub = new Subscription(DataType.MarketDepth, Security);

// Zähler
var i = 0;

// Regel erstellen, die Orderbücher verarbeitet, bis i den Wert 10 erreicht
mdSub.WhenOrderBookReceived(this).Do(depth =>
{
	i++;
	LogInfo($"Rule WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	LogInfo($"Rule WhenOrderBookReceived i={i}");
})
.Until(() => i >= 10)
.Apply(this);

// Abonnementanfrage senden
Subscribe(mdSub);
```

### Regeln auf Orders

```cs
// Abonnement für Tick-Trades
var sub = new Subscription(DataType.Ticks, Security);

// Wenn wir den ersten Tick erhalten, erstellen wir eine Order
sub.WhenTickTradeReceived(this).Do(() =>
{
	var order = CreateOrder(Sides.Buy, default, 1);

	var ruleReg = order.WhenRegistered(this);
	var ruleRegFailed = order.WhenRegisterFailed(this);

	ruleReg
		.Do(() => LogInfo("Order #1 registered"))
		.Once()
		.Apply(this)
		.Exclusive(ruleRegFailed);  // Regeln schließen sich gegenseitig aus

	ruleRegFailed
		.Do(() => LogInfo("Order #1 not registered"))
		.Once()
		.Apply(this)
		.Exclusive(ruleReg);  // Regeln schließen sich gegenseitig aus

	RegisterOrder(order);
}).Once().Apply(this);

// Abonnementanfrage senden
Subscribe(sub);
```

### Regeln auf Preisänderungen

```cs
// Abonnement für Tick-Trades
var sub = new Subscription(DataType.Ticks, Security);

// Regel wird beim ersten Tick aktiviert und erstellt eine weitere Regel
sub.WhenTickTradeReceived(this).Do(t =>
{
	// Regel erstellen, die aktiviert wird, wenn sich der Preis um 2 Punkte in eine beliebige Richtung bewegt
	sub
		.WhenLastTradePriceMore(this, t.Price + 2)
		.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
		.Do(t =>
		{
			LogInfo($"Rule WhenLastTradePriceMore or WhenLastTradePriceLess triggered: tick={t}");
		})
		.Apply(this);
})
.Once() // Diese Regel nur einmal aufrufen
.Apply(this);

// Abonnementanfrage senden
Subscribe(sub);
```
