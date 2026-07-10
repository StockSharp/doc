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
         .Do(() => Connector.AddInfoLog("Ordem registrada com sucesso"))
         .Once()
         .Apply(this);
      
     // registro de ordem
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
          // Assinatura de velas
          var candleSubscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
          this
              .WhenCandlesStarted(candleSubscription)
              .Do(ProcessCandle)
              .Apply(this);
              
          // Assinatura de negociações tick
          var tickSubscription = new Subscription(DataType.Ticks, Security);
          tickSubscription
              .WhenTickTradeReceived(this)
              .Do(ProcessTick)
              .Apply(this);
              
          // Enviar solicitações de assinatura
          Subscribe(candleSubscription);
          Subscribe(tickSubscription);
              
          base.OnStarted2(time);
      }
      
      // Métodos para processamento de eventos
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
          this.AddInfoLog("Ordem cancelada com sucesso");
          // remover todas as regras associadas à ordem
          Rules.RemoveRulesByToken(ruleCanceled, (IMarketRule)ruleCanceled.Token);
      })
      .Once()
      .Apply(this);
  order
      .WhenRegistered(Connector)
      .Do(() => this.AddInfoLog("Ordem registrada com sucesso"))
      .Once()
      .Apply(this);
  order
      .WhenRegisterFailed(Connector)
      .Do(() => this.AddInfoLog("Ordem não aceita pela bolsa"))
      .Once()
      .Apply(this);
  order
      .WhenMatched(Connector)
      .Do(() => this.AddInfoLog("Order fully executed"))
      .Once()
      .Apply(this);
  // registro de ordem
  RegisterOrder(order);
  ```
  
- **Combinar regras com a condição [MarketRuleHelper.Or](xref:StockSharp.Algo.MarketRuleHelper.Or(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** / [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)**.**

  Quando o tempo expira **OU** uma vela fecha:

  ```cs
  // Criar assinatura de velas
  var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
  var timeInterval = TimeSpan.FromMilliseconds(5000);
  
  Connector
      .WhenIntervalElapsed(timeInterval)
      .Or(this.WhenCandlesStarted(subscription))
      .Do(() => this.AddInfoLog("Candle fechada ou tempo expirado"))
      .Once()
      .Apply(this);
      
  // Enviar solicitação de assinatura
  Subscribe(subscription);
  ```

  Ou neste formato:

  ```cs
  // Criar assinatura de velas
  var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);
  var timeInterval = TimeSpan.FromMilliseconds(5000);
  
  MarketRuleHelper
      .Or(new IMarketRule[] {
          Connector.WhenIntervalElapsed(timeInterval), 
          this.WhenCandlesStarted(subscription)
      })
      .Do(() => this.AddInfoLog("Candle fechada ou tempo expirado"))
      .Once()
      .Apply(this);
      
  // Enviar solicitação de assinatura
  Subscribe(subscription);
  ```

  Quando o preço do último negócio está acima de 135000 **E** abaixo de 140000:

  ```cs
  // Criar assinatura de negociações tick
  var subscription = new Subscription(DataType.Ticks, Security);
  var priceMore = new Unit(135000m, UnitTypes.Limit);
  var priceLess = new Unit(140000m, UnitTypes.Limit);
  				
  MarketRuleHelper
      .And(new IMarketRule[] {
          subscription.WhenLastTradePriceMore(this, 135000m), 
          subscription.WhenLastTradePriceLess(this, 140000m)
      })
      .Do(() => this.AddInfoLog($"O preço da última transação está no intervalo de {priceMore} a {priceLess}"))
      .Apply(this);
      
  // Enviar solicitação de assinatura
  Subscribe(subscription);
  ```

  > [!TIP]
  > O processador em [IMarketRule.Do](xref:StockSharp.Algo.IMarketRule.Do(System.Action))**(**[System.Action](xref:System.Action) action **)** será chamado depois de a última regra adicionada através de [MarketRuleHelper.And](xref:StockSharp.Algo.MarketRuleHelper.And(StockSharp.Algo.IMarketRule,StockSharp.Algo.IMarketRule[]))**(**[StockSharp.Algo.IMarketRule](xref:StockSharp.Algo.IMarketRule) rule, [StockSharp.Algo.IMarketRule\[\]](xref:StockSharp.Algo.IMarketRule[]) rules **)** ser accionada.
  
- **Periodicidade de funcionamento da regra - [IMarketRule.Until](xref:StockSharp.Algo.IMarketRule.Until(System.Func{System.Boolean}))**(**[System.Func\<System.Boolean\>](xref:System.Func`1) canFinish **)**:**

  ```cs
  bool flag = false;
  
  // Criar assinatura de negociações tick
  var subscription = new Subscription(DataType.Ticks, Security);
  				
  subscription
      .WhenTickTradeReceived(this)
      .Do((tick) =>
      {
          if(condition) flag = true;
      })
      .Until(() => flag)			
      .Apply(this);
      
  // Enviar solicitação de assinatura
  Subscribe(subscription);
  ```

## Exemplos de Utilização de Regras

### Regras em Velas

```cs
// Criar uma assinatura para velas de 5 minutos
var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);

// Variável para contar velas
var i = 0;
var diff = "10%".ToUnit();

// Regra que é ativada quando uma nova vela começa
this.WhenCandlesStarted(subscription)
	.Do((candle) =>
	{
		i++;

		// Regra aninhada: verificar quando o volume total excede o limite
		this
			.WhenTotalVolumeMore(candle, diff)
			.Do((candle1) =>
			{
				LogInfo($"Regra WhenCandlesStarted and WhenTotalVolumeMore vela={candle1}");
				LogInfo($"Regra WhenCandlesStarted and WhenTotalVolumeMore i={i}");
			})
			.Once().Apply(this);

	}).Apply(this);
	
// Enviar solicitação de assinatura
Subscribe(subscription);
```

### Regras em Livros de Ordens (Profundidade de Mercado)

```cs
// Assinatura de dados do livro de ofertas
var mdSub = new Subscription(DataType.MarketDepth, Security);

// Método 1: criar regra em cadeia
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// Método 2: primeiro criar variável de regra
var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

whenMarketDepthChanged.Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
}).Once().Apply(this);

// Regra dentro de regra
mdSub.WhenOrderBookReceived(this).Do((depth) =>
{
	LogInfo($"Rule WhenOrderBookReceived #3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

	// Rule without specifying Once()
	mdSub.WhenOrderBookReceived(this).Do((depth1) =>
	{
		LogInfo($"Rule WhenOrderBookReceived #4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
	}).Apply(this);
}).Once().Apply(this);

// Enviar solicitação de assinatura
Subscribe(mdSub);
```

### Regras com Condição de Conclusão

```cs
// Assinatura de dados do livro de ofertas
var mdSub = new Subscription(DataType.MarketDepth, Security);

// Counter
var i = 0;

// Criar regra que processa livros de ofertas até i chegar a 10
mdSub.WhenOrderBookReceived(this).Do(depth =>
{
	i++;
	LogInfo($"Rule WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	LogInfo($"Rule WhenOrderBookReceived i={i}");
})
.Until(() => i >= 10)
.Apply(this);

// Enviar solicitação de assinatura
Subscribe(mdSub);
```

### Regras em Ordens

```cs
// Assinatura de negociações tick
var sub = new Subscription(DataType.Ticks, Security);

// Quando recebermos o primeiro tick, criaremos uma ordem
sub.WhenTickTradeReceived(this).Do(() =>
{
	var order = CreateOrder(Sides.Buy, default, 1);

	var ruleReg = order.WhenRegistered(this);
	var ruleRegFailed = order.WhenRegisterFailed(this);

	ruleReg
		.Do(() => LogInfo("Ordem #1 registrada"))
		.Once()
		.Apply(this)
		.Exclusive(ruleRegFailed);  // Rules are mutually exclusive

	ruleRegFailed
		.Do(() => LogInfo("Ordem #1 não registrada"))
		.Once()
		.Apply(this)
		.Exclusive(ruleReg);  // Rules are mutually exclusive

	RegisterOrder(order);
}).Once().Apply(this);

// Enviar solicitação de assinatura
Subscribe(sub);
```

### Regras em Alterações de Preço

```cs
// Assinatura de negociações tick
var sub = new Subscription(DataType.Ticks, Security);

// A regra é ativada no primeiro tick e cria outra regra
sub.WhenTickTradeReceived(this).Do(t =>
{
	// Criar regra ativada quando o preço se move 2 pontos em qualquer direção
	sub
		.WhenLastTradePriceMore(this, t.Price + 2)
		.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
		.Do(t =>
		{
			LogInfo($"Regra WhenLastTradePriceMore ou WhenLastTradePriceLess acionada: tick={t}");
		})
		.Apply(this);
})
.Once() // call this rule only once
.Apply(this);

// Enviar solicitação de assinatura
Subscribe(sub);
```
