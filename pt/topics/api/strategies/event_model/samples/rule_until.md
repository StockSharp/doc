# Regra Until

## Visão Geral

`SimpleRulesUntilStrategy` é uma estratégia que demonstra a utilização de uma regra com uma condição de terminação (`Until`) no StockSharp. Subscreve negócios e livros de ordens e, em seguida, define uma regra que é executada até uma determinada condição ser cumprida.

## Componentes Principais

```cs
// Componentes principais
public class SimpleRulesUntilStrategy : Strategy
{
}
```

## Método OnStarted

Chamado quando a estratégia inicia:

- Cria subscrições para ticks e livros de ordens
- Cria uma regra que é executada quando são recebidos dados do livro de ordens até uma determinada condição ser cumprida

```cs
// Método OnStarted
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	var i = 0;
	mdSub.WhenOrderBookReceived(this).Do(depth =>
	{
		i++;
		LogInfo($"The rule WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
		LogInfo($"The rule WhenOrderBookReceived i={i}");
	})
	.Until(() => i >= 10)
	.Apply(this);

	// Enviar solicitações de assinatura de dados de mercado.
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## Lógica

- Ao iniciar, a estratégia cria subscrições para ticks e livros de ordens
- É criada uma regra que é accionada sempre que são recebidos dados do livro de ordens
- Quando a regra é accionada:
  - O contador `i` é incrementado
  - A informação sobre os melhores preços bid e ask é adicionada ao log
  - O valor actual do contador `i` é adicionado ao log
- A regra é executada até o valor do contador `i` atingir ou exceder 10
- Depois de a condição ser cumprida, a regra pára automaticamente de funcionar

## Funcionalidades

- Demonstra a utilização do método `Until()` para limitar a execução da regra
- Usa subscrição de negócios e livros de ordens
- Mostra um exemplo de registo de informação sobre o livro de ordens e o estado do contador usando o método `LogInfo`
- Ilustra como limitar o número de execuções da regra com base numa condição específica
