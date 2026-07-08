# Regra para Ordens

## Visão Geral

`SimpleOrderRulesStrategy` é uma estratégia que demonstra a utilização de regras para processar eventos relacionados com ordens no StockSharp. Subscreve negócios e cria regras para processar eventos de registo de ordens.

## Componentes Principais

```cs
// Componentes principais
public class SimpleOrderRulesStrategy : Strategy
{
}
```

## Método OnStarted

Chamado quando a estratégia inicia:

- Cria uma subscrição de ticks
- Cria dois conjuntos de regras para processar eventos de registo de ordens

```cs
// Método OnStarted
protected override void OnStarted2(DateTime time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 1);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("Order №1 Registered"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("Order №1 RegisterFailed"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 10000000);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("Order №2 Registered"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("Order №2 RegisterFailed"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	// Enviar solicitação de assinatura de dados de mercado.
	Subscribe(sub);

	base.OnStarted2(time);
}
```

## Lógica

### Primeiro Conjunto de Regras

- Quando é recebido um tick, cria uma ordem para comprar 1 unidade
- A ordem é criada usando o método `CreateOrder`, especificando direcção, preço (default = mercado) e volume
- Define regras para processar registo bem-sucedido e erros de registo
- As regras são mutuamente exclusivas e são accionadas apenas uma vez

### Segundo Conjunto de Regras

- Quando é recebido o tick seguinte, cria uma ordem para comprar 10.000.000 unidades
- Define, de forma semelhante, regras para processar registo bem-sucedido e erros de registo
- As regras também são mutuamente exclusivas e são accionadas apenas uma vez

## Funcionalidades

- Demonstra a criação de regras para processar eventos de registo de ordens
- Usa o mecanismo de regras mutuamente exclusivas (`Exclusive`)
- Mostra um exemplo de registo de informação sobre eventos de ordens usando o método `LogInfo`
- Ilustra a utilização de `Once()` para limitar o accionamento da regra
- Cria ordens com volumes diferentes para demonstrar vários cenários (registo bem-sucedido e erro de registo)
