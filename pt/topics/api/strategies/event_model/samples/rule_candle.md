# Regra para uma Única Vela

## Visão Geral

`SimpleCandleRulesStrategy` é uma estratégia que demonstra a utilização de regras para velas no StockSharp. Acompanha os volumes das velas e regista informação quando determinadas condições são cumpridas.

## Componentes Principais

```cs
// Componentes principais
public class SimpleCandleRulesStrategy : Strategy
{
}
```

## Método OnStarted

Chamado quando a estratégia inicia:

- Inicializa uma subscrição de velas de 5 minutos
- Define regras para processar velas

```cs
// Método OnStarted
protected override void OnStarted2(DateTime time)
{
	var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security)
	{
		// velas prontas são muito mais rápidas que compressão em tempo real
		// turn off compression to boost optimizer (!!! make sure you have candles)

		//MarketData =
		//{
		//    BuildMode = MarketDataBuildModes.Build,
		//    BuildFrom = DataType.Ticks,
		//}
	};
	Subscribe(subscription);

	var i = 0;
	var diff = "10%".ToUnit();

	this.WhenCandlesStarted(subscription)
		.Do((candle) =>
		{
			i++;

			this
				.WhenTotalVolumeMore(candle, diff)
				.Do((candle1) =>
				{
	LogInfo($"Regra WhenCandlesStarted e WhenTotalVolumeMore vela={candle1}");
	LogInfo($"Regra WhenCandlesStarted e WhenTotalVolumeMore i={i}");
				})
				.Once().Apply(this);

		}).Apply(this);

	base.OnStarted2(time);
}
```

## Lógica

- A estratégia subscreve velas de 5 minutos
- Quando cada vela começa a formar-se, é definida uma regra
- A regra é accionada quando o volume total da vela excede 10% (usando um valor percentual)
- Quando a regra é accionada, a informação sobre a vela e o contador é adicionada ao log
- Depois do primeiro accionamento, a regra deixa de funcionar graças ao método `Once()`

## Funcionalidades

- Demonstra a utilização das regras `WhenCandlesStarted` e `WhenTotalVolumeMore`
- Usa o mecanismo de subscrição de velas
- Mostra um exemplo de criação de um valor percentual através de `"10%".ToUnit()`
- Mostra um exemplo de registo de informação numa estratégia usando o método `LogInfo`
- Contém código comentado para configurar a construção de velas a partir de ticks
