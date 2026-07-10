# Uso de reglas

## Creación de reglas

- **Crear una regla para la condición de registro de una orden:**

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
         .Do(() => Connector.AddInfoLog("Orden registrada correctamente"))
         .Once()
         .Apply(this);
      
     // registro de la orden
     Connector.RegisterOrder(order);
  }
  ```

  Ahora, cuando se produzca el evento (la orden se registre en el exchange), se llamará a la acción especificada mediante el método [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)**.

  Al final de la formación de la regla, se llama al método [MarketRuleHelper.Apply](xref:StockSharp.Algo.MarketRuleHelper.Apply(StockSharp.Algo.IMarketRule))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule **)**. Hasta que se llame a este método para la regla, esta permanece inactiva (no se llamará al controlador de [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)**).
  
- **Crear reglas dentro de una estrategia:**

  ```cs
  class FirstStrategy : Strategy
  {
      protected override void OnStarted2(DateTime time)
      {
          // Suscripción a velas
          var candleSubscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
          this
              .WhenCandlesStarted(candleSubscription)
              .Do(ProcessCandle)
              .Apply(this);
              
          // Suscripción a operaciones tick
          var tickSubscription = new Subscription(DataType.Ticks, Security);
          tickSubscription
              .WhenTickTradeReceived(this)
              .Do(ProcessTick)
              .Apply(this);
              
          // Enviar solicitudes de suscripción
          Subscribe(candleSubscription);
          Subscribe(tickSubscription);
              
          base.OnStarted2(time);
      }
      
      // Métodos para procesar eventos
      private void ProcessCandle(ICandleMessage candle) { /* ... */ }
      private void ProcessTick(ITickTradeMessage tick) { /* ... */ }
  }    
  ```
  
- **Eliminar reglas innecesarias.**

  [IMarketRule](xref:StockSharp.Algo.IMarketRule) tiene [IMarketRule.Token](xref:StockSharp.Algo.IMarketRule.Token): un token de la regla con el que está asociada. Por ejemplo, para la regla [WhenCanceled](xref:StockSharp.Algo.MarketRuleHelper.WhenCanceled(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ISubscriptionProvider)), el token será la orden.

  Cuando se ha activado una regla de cancelación correcta de una orden, es mejor eliminar todas las demás reglas relacionadas con esta orden:

  ```cs
  var order = this.CreateOrder(direction, (decimal)Security.GetCurrentPrice(direction), Volume);
  var ruleCanceled = order.WhenCanceled(Connector);
  ruleCanceled
      .Do(() =>
      {
          this.AddInfoLog("Orden cancelada correctamente");
          // eliminar todas las reglas asociadas a la orden
          Rules.RemoveRulesByToken(ruleCanceled, (IMarketRule)ruleCanceled.Token);
      })
      .Once()
      .Apply(this);
  order
      .WhenRegistered(Connector)
      .Do(() => this.AddInfoLog("Orden registrada correctamente"))
      .Once()
      .Apply(this);
  order
      .WhenRegisterFailed(Connector)
      .Do(() => this.AddInfoLog("Orden no aceptada por la bolsa"))
      .Once()
      .Apply(this);
  order
      .WhenMatched(Connector)
      .Do(() => this.AddInfoLog("Order fully executed"))
      .Once()
      .Apply(this);
  // registro de la orden
  RegisterOrder(order);
  ```
  
- **Combinar reglas con la condición [MarketRuleHelper.Or](xref:StockSharp.Algo.MarketRuleHelper.Or(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** / [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)**.**

  Cuando expira el tiempo **OR** se cierra una vela:

  ```cs
  // Crear una suscripción a velas
  var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
  var timeInterval = TimeSpan.FromMilliseconds(5000);
  
  Connector
      .WhenIntervalElapsed(timeInterval)
      .Or(this.WhenCandlesStarted(subscription))
      .Do(() => this.AddInfoLog("Vela cerrada o tiempo expirado"))
      .Once()
      .Apply(this);
      
  // Enviar solicitud de suscripción
  Subscribe(subscription);
  ```

  O en este formato:

  ```cs
  // Crear una suscripción a velas
  var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
  var timeInterval = TimeSpan.FromMilliseconds(5000);
  
  MarketRuleHelper
      .Or(new IMarketRule[] {
          Connector.WhenIntervalElapsed(timeInterval), 
          this.WhenCandlesStarted(subscription)
      })
      .Do(() => this.AddInfoLog("Vela cerrada o tiempo expirado"))
      .Once()
      .Apply(this);
      
  // Enviar solicitud de suscripción
  Subscribe(subscription);
  ```

  Cuando el precio de la última operación está por encima de 135000 **AND** por debajo de 140000:

  ```cs
  // Crear una suscripción a operaciones tick
  var subscription = new Subscription(DataType.Ticks, Security);
  var priceMore = new Unit(135000m, UnitTypes.Limit);
  var priceLess = new Unit(140000m, UnitTypes.Limit);
  				
  MarketRuleHelper
      .And(new IMarketRule[] {
          subscription.WhenLastTradePriceMore(this, 135000m), 
          subscription.WhenLastTradePriceLess(this, 140000m)
      })
      .Do(() => this.AddInfoLog($"El precio de la última operación está en el rango de {priceMore} a {priceLess}"))
      .Apply(this);
      
  // Enviar solicitud de suscripción
  Subscribe(subscription);
  ```

  > [!TIP]
  > El controlador de [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** se llamará después de que se active la última regla agregada mediante [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)**.
  
- **Periodicidad de funcionamiento de una regla: [IMarketRule.Until](xref:StockSharp.Algo.IMarketRule.Until(System.Func{System.Boolean}))**(**[System.Func\<System.Boolean\>](xref:System.Func`1) canFinish **)**:**

  ```cs
  bool flag = false;
  
  // Crear una suscripción a operaciones tick
  var subscription = new Subscription(DataType.Ticks, Security);
  				
  subscription
      .WhenTickTradeReceived(this)
      .Do((tick) =>
      {
          if(condition) flag = true;
      })
      .Until(() => flag)			
      .Apply(this);
      
  // Enviar solicitud de suscripción
  Subscribe(subscription);
  ```

## Ejemplos de uso de reglas

### Reglas sobre velas

```cs
// Crear una suscripción a velas de 5 minutos
var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);

// Variable para contar velas
var i = 0;
var diff = "10%".ToUnit();

// Regla que se activa cuando comienza una nueva vela
this.WhenCandlesStarted(subscription)
	.Do((candle) =>
	{
		i++;

		// Regla anidada: comprobar cuándo el volumen total supera el umbral
		this
			.WhenTotalVolumeMore(candle, diff)
			.Do((candle1) =>
			{
				LogInfo($"Regla WhenCandlesStarted and WhenTotalVolumeMore vela={candle1}");
				LogInfo($"Regla WhenCandlesStarted and WhenTotalVolumeMore i={i}");
			})
			.Once().Apply(this);

	}).Apply(this);
	
// Enviar solicitud de suscripción
Subscribe(subscription);
```

### Reglas sobre libros de órdenes (Market Depth)

```cs
// Suscripción a datos del libro de órdenes
var mdSub = new Subscription(DataType.MarketDepth, Security);

// Método 1: Crear una regla en una cadena
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// Método 2: Crear primero una variable de regla
var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

whenMarketDepthChanged.Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// Regla dentro de una regla
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

	// Regla sin especificar Once()
	mdSub.WhenOrderBookReceived(this).Do((depth1) =>
	{
		LogInfo($"Rule WhenOrderBookReceived #4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
	}).Apply(this);
}).Once().Apply(this);

// Enviar solicitud de suscripción
Subscribe(mdSub);
```

### Reglas con condición de finalización

```cs
// Suscripción a datos del libro de órdenes
var mdSub = new Subscription(DataType.MarketDepth, Security);

// Contador
var i = 0;

// Crear una regla que procesa libros de órdenes hasta que i alcance 10
mdSub.WhenOrderBookReceived(this).Do(depth =>
{
	i++;
	LogInfo($"Rule WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	LogInfo($"Rule WhenOrderBookReceived i={i}");
})
.Until(() => i >= 10)
.Apply(this);

// Enviar solicitud de suscripción
Subscribe(mdSub);
```

### Reglas sobre órdenes

```cs
// Suscripción a operaciones tick
var sub = new Subscription(DataType.Ticks, Security);

// Cuando recibimos el primer tick, crearemos una orden
sub.WhenTickTradeReceived(this).Do(() =>
{
	var order = CreateOrder(Sides.Buy, default, 1);

	var ruleReg = order.WhenRegistered(this);
	var ruleRegFailed = order.WhenRegisterFailed(this);

	ruleReg
		.Do(() => LogInfo("Orden #1 registrada"))
		.Once()
		.Apply(this)
		.Exclusive(ruleRegFailed);  // Las reglas son mutuamente excluyentes

	ruleRegFailed
		.Do(() => LogInfo("Orden #1 no registrada"))
		.Once()
		.Apply(this)
		.Exclusive(ruleReg);  // Las reglas son mutuamente excluyentes

	RegisterOrder(order);
}).Once().Apply(this);

// Enviar solicitud de suscripción
Subscribe(sub);
```

### Reglas sobre cambios de precio

```cs
// Suscripción a operaciones tick
var sub = new Subscription(DataType.Ticks, Security);

// La regla se activa en el primer tick y crea otra regla
sub.WhenTickTradeReceived(this).Do(t =>
{
	// Crear una regla que se activa cuando el precio se mueve 2 puntos en cualquier dirección
	sub
		.WhenLastTradePriceMore(this, t.Price + 2)
		.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
		.Do(t =>
		{
			LogInfo($"Regla WhenLastTradePriceMore o WhenLastTradePriceLess activada: tick={t}");
		})
		.Apply(this);
})
.Once() // llamar esta regla solo una vez
.Apply(this);

// Enviar solicitud de suscripción
Subscribe(sub);
```
