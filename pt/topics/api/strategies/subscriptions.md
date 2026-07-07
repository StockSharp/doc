# Subscrições de Dados de Mercado em Estratégias

Em StockSharp, as estratégias usam um mecanismo de subscrição para receber dados de mercado. Esta abordagem é o método principal e preferencial para obter dados em estratégias de negociação.

## Noções Básicas de Subscrições

As subscrições em estratégias baseiam-se no [mecanismo geral de subscrições do StockSharp](../market_data/subscriptions.md). Fornecem uma forma centralizada e unificada de obter todos os tipos de dados de mercado.

## Criar uma Subscrição numa Estratégia

No método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) da estratégia, pode criar e iniciar uma subscrição para os dados necessários:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Criar uma subscrição para candles de 5 minutos diretamente através de DataType
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		Security);
	
	// Se forem necessários parâmetros adicionais, pode configurar a subscrição
	subscription.From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(7));
	
	// Criar uma regra para processar candles recebidas
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);
	
	// Iniciar a subscrição
	Connector.Subscribe(subscription);
}
```

Neste exemplo, é criada uma subscrição para candles de 5 minutos usando um construtor conveniente que aceita `DataType` e `Security`. Se necessário, pode configurar adicionalmente os parâmetros da subscrição, como o período de histórico.

## Vantagens das Subscrições em Estratégias

Usar subscrições em estratégias tem várias vantagens em comparação com a subscrição direta de eventos de [Strategy.Connector](xref:StockSharp.Algo.Strategies.Strategy.Connector):

1. **Isolamento** - cada subscrição funciona de forma independente, permitindo receber diferentes tipos de dados para diferentes instrumentos sem interferência mútua. Isto também protege a estratégia contra a receção de dados destinados a outras estratégias executadas em paralelo. Com a subscrição direta de eventos do conector, teria de filtrar adicionalmente os dados para excluir informação de outras estratégias.

2. **Gestão de Estado** - as subscrições têm estados claros ([SubscriptionStates](xref:StockSharp.Messages.SubscriptionStates)), que permitem determinar com precisão se os dados históricos estão atualmente a ser recebidos ou se a subscrição já transitou para o modo online.

3. **Controlo Automático do Estado da Estratégia** - a estratégia acompanha automaticamente o estado de todas as suas subscrições e transita para o modo online ([IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline)) apenas quando todas as subscrições ficaram online.

4. **Uniformidade do Código** - as subscrições usam uma abordagem unificada, independente do tipo de dados solicitado.

5. **Integração com Regras** - as subscrições integram-se facilmente com o [modelo de eventos](event_model.md) da estratégia através de regras.

6. **Gestão Automática de Subscrições** - quando a estratégia para, todas as suas subscrições são automaticamente canceladas, libertando recursos.

7. **Suporte para Dados Históricos** - capacidade de carregar dados históricos antes de transitar para dados em tempo real.

## Monitorizar Estados de Subscrição

A estratégia acompanha automaticamente o estado de todas as subscrições para controlar o seu modo de operação:

```cs
private void CheckRefreshOnlineState()
{
	bool nowOnline = ProcessState == ProcessStates.Started;

	if (nowOnline)
		nowOnline = _subscriptions.CachedKeys
			.Where(s => !s.SubscriptionMessage.IsHistoryOnly())
			.All(s => s.State == SubscriptionStates.Online);
	
	// Atualizar o estado IsOnline da estratégia
	IsOnline = nowOnline;
}
```

A propriedade [Strategy.IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) será `true` apenas quando todas as subscrições da estratégia tiverem transitado para o estado [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online). Isto permite à estratégia compreender o momento em que está a trabalhar com dados de mercado atuais.

## Tipos de Subscrições

Em estratégias, pode usar subscrições para vários tipos de dados de mercado:

```cs
// Subscrição de candles
var candleSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	Security);

// Subscrição de profundidade de mercado
var depthSubscription = new Subscription(
	DataType.MarketDepth,
	Security);

// Subscrição de negócios tick
var tickSubscription = new Subscription(
	DataType.Ticks,
	Security);

// Subscrição de Level1 (melhor bid/ask e outra informação básica)
var level1Subscription = new Subscription(
	DataType.Level1,
	Security);
```

## Processar Dados de Subscrição Através de Regras

Para processar dados recebidos através de uma subscrição, recomenda-se usar [regras](event_model.md):

```cs
// Subscrição de candles
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), Security);

// Criar uma regra para processar candles recebidas
Connector
	.WhenCandlesFinished(subscription)  // Ativação da regra quando é recebida uma candle concluída
	.Do(ProcessCandle)                   // Chamar o método de processamento
	.Apply(this);                        // Aplicar a regra à estratégia

// Iniciar a subscrição
Connector.Subscribe(subscription);
```

No exemplo acima, é criada uma regra que chamará o método `ProcessCandle` quando cada candle concluída for recebida.

## Solicitar Dados Históricos

A estratégia define automaticamente o período de carregamento do histórico através da propriedade [Strategy.HistorySize](xref:StockSharp.Algo.Strategies.Strategy.HistorySize):

```cs
// Definir o período de carregamento do histórico para 30 dias
strategy.HistorySize = TimeSpan.FromDays(30);
```

Ao criar uma subscrição, a estratégia define automaticamente o parâmetro `From` para carregar histórico, caso este não tenha sido especificado explicitamente.

## Cancelar Subscrições

As subscrições podem ser canceladas manualmente chamando o método [UnSubscribe](xref:StockSharp.BusinessEntities.ISubscriptionProvider.UnSubscribe(StockSharp.BusinessEntities.Subscription)):

```cs
// Cancelar subscrição
Connector.UnSubscribe(subscription);
```

Ao parar a estratégia, se o parâmetro [UnsubscribeOnStop](xref:StockSharp.Algo.Strategies.Strategy.UnsubscribeOnStop) estiver definido como `true` (predefinição), todas as subscrições serão canceladas automaticamente.

## Ver Também

- [Subscrições de Dados de Mercado](../market_data/subscriptions.md)
- [Modelo de Eventos](event_model.md)
- [Compatibilidade da Estratégia com Plataformas](compatibility.md)

