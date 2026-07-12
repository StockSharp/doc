# Operações de Negociação em Estratégias

Em StockSharp, a classe [Strategy](xref:StockSharp.Algo.Strategies.Strategy) fornece vários métodos para trabalhar com ordens, tornando conveniente a implementação de estratégias de negociação.

## Métodos de Colocação de Ordens

Existem várias formas de colocar ordens em estratégias StockSharp:

### 1. Usar Métodos de Alto Nível

A forma mais simples é usar métodos incorporados que criam e registam uma ordem numa única chamada:

```cs
// Comprar ao preço de mercado
BuyMarket(volume);

// Vender ao preço de mercado
SellMarket(volume);

// Comprar a preço limite
BuyLimit(price, volume);

// Vender a preço limite
SellLimit(price, volume);

// Fechar a posição atual ao preço de mercado
ClosePosition();
```

Estes métodos fornecem máxima simplicidade e legibilidade do código. Automaticamente:
- Criam um objeto de ordem com os parâmetros especificados
- Preenchem os campos necessários (instrumento, portefólio, etc.)
- Registam a ordem no sistema de negociação

### 2. Usar CreateOrder + RegisterOrder

Uma abordagem mais flexível é separar a criação e o registo das ordens:

```cs
// Criar um objeto de ordem
var order = CreateOrder(Sides.Buy, price, volume);

// Definições adicionais da ordem
order.Comment = "Minha ordem especial";
order.TimeInForce = TimeInForce.MatchOrCancel;

// Registar a ordem
RegisterOrder(order);
```

O método [CreateOrder](xref:StockSharp.Algo.Strategies.Strategy.CreateOrder(StockSharp.Messages.Sides,System.Decimal,System.Nullable{System.Decimal})) cria um objeto de ordem inicializado que pode ser personalizado adicionalmente antes do registo.

### 3. Criação e Registo Diretos de uma Ordem

Para controlo máximo, pode criar diretamente um objeto de ordem e registá-lo:

```cs
// Criar diretamente um objeto de ordem
var order = new Order
{
	Security = Security,
	Portfolio = Portfolio,
	Side = Sides.Buy,
	Type = OrderTypes.Limit,
	Price = price,
	Volume = volume,
	Comment = "Ordem personalizada"
};

// Registar a ordem
RegisterOrder(order);
```

Para mais detalhes sobre trabalhar com ordens, veja a secção [Ordens](../orders_management.md).

## Tratar Eventos de Ordem

Depois de registar uma ordem, é importante acompanhar o seu estado. Numa estratégia, pode:

### 1. Usar Event Handlers

```cs
// Subscrever o evento de ordem recebida
OrderReceived += OnOrderReceived;

// Subscrever o evento de falha de registo de ordem
OrderRegisterFailed += OnOrderRegisterFailed;

private void OnOrderReceived(Order order)
{
	if (order.State == OrderStates.Done)
	{
		// Ordem executada - executar a lógica correspondente
	}
}

private void OnOrderRegisterFailed(OrderFail fail)
{
	// Tratar erro de registo da ordem
	LogError($"Erro ao registrar a ordem: {fail.Error}");
}
```

### 2. Usar Regras para Ordens

Uma abordagem mais poderosa é usar [regras](event_model.md) para ordens:

```cs
// Criar uma ordem
var order = BuyLimit(price, volume);

// Criar uma regra que será acionada quando a ordem for executada
order
	.WhenMatched(this)
	.Do(() => {
		// Ações após a execução da ordem
		LogInfo($"Ordem {order.TransactionId} executada");
		
		// Por exemplo, colocar uma stop order
		var stopOrder = SellLimit(price * 0.95, volume);
	})
	.Apply(this);

// Regra para tratar erro de registo
order
	.WhenRegisterFailed(this)
	.Do(fail => {
		LogError($"Erro ao registrar a ordem: {fail.Error}");
		// Possivelmente tentar novamente com parâmetros diferentes
	})
	.Apply(this);
```

Exemplos detalhados de utilização de regras com ordens podem ser encontrados na secção [Exemplos de Regras de Ordem](event_model/samples/rule_order.md).

## Gestão de Posição

A estratégia também fornece métodos para gestão de posição:

```cs
// Obter posição atual
decimal currentPosition = Position;

// Fechar posição atual
ClosePosition();

// Proteger posição com stop-loss e take-profit
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute),   // take-profit
	stopLoss: new Unit(20, UnitTypes.Absolute),     // stop-loss
	isStopTrailing: true,                        // stop móvel
	useMarketOrders: true                        // usar ordens de mercado
);
```

## Estado da Estratégia Antes de Negociar

Antes de executar operações de negociação, é importante garantir que a estratégia está no estado correto. StockSharp fornece várias propriedades e métodos para verificar a prontidão da estratégia:

### Propriedade IsFormed

A propriedade [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) indica se todos os indicadores usados na estratégia estão formados (aquecidos). Por predefinição, verifica se todos os indicadores adicionados à coleção [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) estão no estado [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) = `true`.

Mais informação sobre trabalhar com indicadores numa estratégia pode ser encontrada na secção [Indicadores em Estratégia](indicators.md).

### Propriedade IsOnline

A propriedade [IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) mostra se a estratégia está em modo de tempo real. Torna-se `true` apenas quando a estratégia está iniciada e todas as suas subscrições de dados de mercado transitaram para o estado [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online).

Mais detalhes sobre subscrições de dados de mercado em estratégias podem ser encontrados na secção [Subscrições de Dados de Mercado em Estratégias](subscriptions.md).

### Propriedade TradingMode

A propriedade [TradingMode](xref:StockSharp.Algo.Strategies.Strategy.TradingMode) define o modo de negociação da estratégia. Valores possíveis:

- [StrategyTradingModes.Full](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Full) - todas as operações de negociação são permitidas (modo predefinido)
- [StrategyTradingModes.Disabled](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Disabled) - a negociação está completamente desativada
- [StrategyTradingModes.CancelOrdersOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.CancelOrdersOnly) - apenas o cancelamento de ordens é permitido
- [StrategyTradingModes.ReducePositionOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.ReducePositionOnly) - apenas são permitidas operações de redução de posição

Esta propriedade pode ser configurada através dos parâmetros da estratégia:

```cs
public SmaStrategy()
{
	_tradingMode = Param(nameof(TradingMode), StrategyTradingModes.Full)
					.SetDisplay("Modo de negociação", "Operações de negociação permitidas", "Definições básicas");
}
```

### Métodos Auxiliares para Verificação de Estado

Para verificar convenientemente a prontidão da estratégia para negociar, StockSharp fornece métodos auxiliares:

- [IsFormedAndOnline()](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnline) - verifica se a estratégia está no estado `IsFormed = true` e `IsOnline = true`

- [IsFormedAndOnlineAndAllowTrading(StrategyTradingModes)](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) - verifica se a estratégia está formada, está em modo online e tem as permissões de negociação necessárias

O método `IsFormedAndOnlineAndAllowTrading` aceita um parâmetro opcional `required` do tipo [StrategyTradingModes](xref:StockSharp.Algo.Strategies.StrategyTradingModes):

```cs
public bool IsFormedAndOnlineAndAllowTrading(StrategyTradingModes required = StrategyTradingModes.Full)
```

Este parâmetro permite especificar o nível mínimo de permissões de negociação necessário para uma operação específica:

1. **StrategyTradingModes.Full** (valor predefinido) - devolve `true` apenas se a estratégia estiver em modo de negociação completa (`TradingMode = StrategyTradingModes.Full`). Usado para operações que podem aumentar uma posição.

2. **StrategyTradingModes.ReducePositionOnly** - devolve `true` se a estratégia estiver em modo de negociação completa ou apenas em modo de redução de posição. Usado para operações de fecho de posição ou fecho parcial.

3. **StrategyTradingModes.CancelOrdersOnly** - devolve `true` com qualquer modo de negociação ativo (exceto `Disabled`). Usado para operações de cancelamento de ordens.

Isto permite permitir ou proibir seletivamente várias operações de negociação dependendo do modo de negociação atual:

```cs
// Para colocar uma nova ordem que aumenta uma posição, é necessário o modo de negociação completa
if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.Full))
{
	// Podemos colocar quaisquer ordens
	RegisterOrder(CreateOrder(Sides.Buy, price, volume));
}
// Para fechar uma posição, o modo de redução de posição é suficiente
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly) && Position != 0)
{
	// Só podemos fechar a posição
	ClosePosition();
}
// Para cancelar ordens ativas, o modo de cancelamento de ordens é suficiente
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
{
	// Só podemos cancelar ordens
	CancelActiveOrders();
}
```

Assim, este método permite implementar um mecanismo seguro de controlo de acesso para funções de negociação, em que operações mais críticas (como abrir novas posições) requerem um nível mais elevado de permissões, e operações menos críticas (cancelar ordens) são realizadas mesmo num modo de negociação limitado.

É uma boa prática usar estes métodos antes de executar operações de negociação:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Verificar se a estratégia está formada e em modo online,
	// e se a negociação é permitida
	if (!IsFormedAndOnlineAndAllowTrading())
		return;
	
	// Lógica de negociação
	// ...
}
```

## Exemplo de Operações de Negociação

Abaixo está um exemplo que demonstra diferentes formas de colocar ordens numa estratégia e tratar a sua execução:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Subscrever candles
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		Security);
	
	// Criar uma regra para processar candles
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);
	
	Connector.Subscribe(subscription);
}

private void ProcessCandle(ICandleMessage candle)
{
	// Verificar se a estratégia está pronta para negociar
	if (!this.IsFormedAndOnlineAndAllowTrading())
		return;
	
	// Exemplo de lógica de negociação baseada no preço de fecho
	if (candle.ClosePrice > _previousClose * 1.01)
	{
		// Opção 1: Usar um método de alto nível
		var order = BuyLimit(candle.ClosePrice, Volume);
		
		// Criar uma regra para tratar a execução da ordem
		order
			.WhenMatched(this)
			.Do(() => {
				// Quando a ordem é executada, definir stop-loss e take-profit
				StartProtection(
					takeProfit: new Unit(50, UnitTypes.Absolute),
					stopLoss: new Unit(20, UnitTypes.Absolute)
				);
			})
			.Apply(this);
	}
	else if (candle.ClosePrice < _previousClose * 0.99)
	{
		// Opção 2: Criação e registo separados
		var order = CreateOrder(Sides.Sell, candle.ClosePrice, Volume);
		RegisterOrder(order);
		
		// Forma alternativa de tratar através do evento
		OrderReceived += (o) => {
			if (o == order && o.State == OrderStates.Done)
			{
				// Ações após a execução
			}
		};
	}
	
	_previousClose = candle.ClosePrice;
}
```

## Ver Também

- [Ordens](../orders_management.md)
- [Regras de Ordem](event_model/samples/rule_order.md)
- [Modelo de Eventos](event_model.md)
- [Proteção de Posição](take_profit_and_stop_loss.md)
