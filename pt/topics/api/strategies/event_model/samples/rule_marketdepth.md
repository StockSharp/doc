# Regras para Livros de Ordens e Negócios

## Visão Geral

`SimpleRulesStrategy` é uma estratégia que demonstra várias formas de criar e aplicar regras no StockSharp. Subscreve negócios e livros de ordens e, em seguida, define várias regras para processar os dados recebidos.

## Componentes Principais

```cs
// Main components
public class SimpleRulesStrategy : Strategy
{
}
```

## Método OnStarted

Chamado quando a estratégia inicia:

- Cria subscrições para negócios e livros de ordens
- Demonstra várias formas de criar e aplicar regras

```cs
// OnStarted method
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	//-----------------------Create a rule. Method №1-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//-----------------------Create a rule. Method №2-----------------------------------
	var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

	whenMarketDepthChanged.Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//----------------------Rule inside rule-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

		//----------------------not a Once rule-----------------------------------
		mdSub.WhenOrderBookReceived(this).Do((depth1) =>
		{
			LogInfo($"The rule WhenOrderBookReceived №4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
		}).Apply(this);
	}).Once().Apply(this);

	// Sending requests for subscribe to market data.
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## Lógica

### Método #1: Criar uma Regra

- Cria uma regra que é accionada quando é recebido um livro de ordens
- Regista o melhor bid e ask
- A regra é accionada apenas uma vez (`Once()`)

### Método #2: Criar uma Regra

- Demonstra uma forma alternativa de criar uma regra
- Funcionalmente idêntica ao Método #1

### Regra Dentro de uma Regra

- Cria uma regra que é accionada quando é recebido um livro de ordens
- Dentro desta regra, é criada outra regra
- A regra externa é accionada uma vez; a regra interna, sempre que é recebido um livro de ordens

## Funcionalidades

- Demonstra várias formas de criar e aplicar regras no StockSharp
- Usa subscrição de negócios e livros de ordens
- Mostra um exemplo de registo de informação numa estratégia usando o método `LogInfo`
- Ilustra a utilização de `Once()` para limitar o accionamento da regra
