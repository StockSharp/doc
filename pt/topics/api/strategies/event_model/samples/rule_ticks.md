# Regras para Negócios Tick

## Visão Geral

`SimpleTradeRulesStrategy` é uma estratégia que demonstra a utilização de regras combinadas para analisar preços de negócios no StockSharp. Subscreve negócios e cria uma regra que é accionada sob determinadas condições de preço.

## Componentes Principais

```cs
// Main components
public class SimpleTradeRulesStrategy : Strategy
{
}
```

## Método OnStarted

Chamado quando a estratégia inicia:

- Cria uma subscrição de ticks
- Cria uma regra combinada para analisar preços de negócios

```cs
// OnStarted method
protected override void OnStarted2(DateTime time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(t =>
	{
		sub
			.WhenLastTradePriceMore(this, t.Price + 2)
			.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
			.Do(t =>
			{
				LogInfo($"The rule WhenLastTradePriceMore Or WhenLastTradePriceLess tick={t}");
			})
			.Apply(this);
	})
	.Once() // call this rule only once
	.Apply(this);

	// Sending request for subscribe to market data.
	Subscribe(sub);

	base.OnStarted2(time);
}
```

## Lógica

- Quando o primeiro tick é recebido, é criada uma regra combinada
- Baseia-se no preço do tick recebido: cria uma regra que é accionada quando o preço se altera em +/- 2
- A regra é accionada quando o preço do último negócio fica acima de actual + 2 ou abaixo de actual - 2
- Quando a regra é accionada, a informação sobre o tick é adicionada ao log
- A regra externa é accionada apenas uma vez (`Once()`)

## Funcionalidades

- Demonstra a criação de regras combinadas usando `Or()`
- Usa `WhenLastTradePriceMore` e `WhenLastTradePriceLess` para análise de preço
- Mostra um exemplo de registo de informação sobre negócios usando o método `LogInfo`
- Ilustra a utilização de `Once()` para limitar o accionamento da regra
- Passa o parâmetro tick ao processador de eventos (ao contrário do exemplo na documentação)
