# Regla para una sola vela

## Descripción general

`SimpleCandleRulesStrategy` es una estrategia que demuestra el uso de reglas para velas en StockSharp. Realiza seguimiento de los volúmenes de las velas y registra información cuando se cumplen determinadas condiciones.

## Componentes principales

```cs
// Componentes principales
public class SimpleCandleRulesStrategy : Strategy
{
}
```

## Método OnStarted

Se llama cuando se inicia la estrategia:

- Inicializa una suscripción a velas de 5 minutos
- Establece reglas para procesar velas

```cs
// Método OnStarted
protected override void OnStarted2(DateTime time)
{
	var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security)
	{
		// velas listas para usar mucho más rápido que la compresión sobre la marcha
		// desactive la compresión para acelerar el optimizador (!!! asegúrese de tener velas)

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
	LogInfo($"Regla WhenCandlesStarted y WhenTotalVolumeMore vela={candle1}");
	LogInfo($"Regla WhenCandlesStarted y WhenTotalVolumeMore i={i}");
				})
				.Once().Apply(this);

		}).Apply(this);

	base.OnStarted2(time);
}
```

## Lógica

- La estrategia se suscribe a velas de 5 minutos
- Cuando cada vela empieza a formarse, se establece una regla
- La regla se activa cuando el volumen total de la vela supera el 10% (mediante un valor porcentual)
- Cuando la regla se activa, se agrega al registro información sobre la vela y el contador
- Después de la primera activación, la regla deja de funcionar gracias al método `Once()`

## Características

- Demuestra el uso de las reglas `WhenCandlesStarted` y `WhenTotalVolumeMore`
- Usa el mecanismo de suscripción a velas
- Muestra un ejemplo de creación de un valor porcentual mediante `"10%".ToUnit()`
- Muestra un ejemplo de registro de información en una estrategia mediante el método `LogInfo`
- Contiene código comentado para configurar la construcción de velas a partir de ticks
