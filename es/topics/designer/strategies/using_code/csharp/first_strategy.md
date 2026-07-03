# Ejemplo de estrategia C#

La creación de una estrategia desde código fuente se demostrará con el ejemplo de la estrategia SMA, similar al ejemplo de estrategia SMA ensamblada desde cubos en la sección [Creación de un algoritmo a partir de cubos](../../using_visual_designer/first_strategy.md).

Esta sección no describirá las construcciones del lenguaje C# (ni [Strategy](../../../../api/strategies.md), que se usa como base para crear estrategias), pero mencionará características específicas para trabajar con código en **Designer**.

> [!TIP]
> Las estrategias creadas en **Designer** son compatibles con las estrategias creadas en la [API](../../../../api.md) gracias al uso de la clase base común [Strategy](../../../../api/strategies.md). Esto facilita mucho más la ejecución de dichas estrategias fuera de **Designer** que la ejecución de [esquemas](../../../live_execution/running_strategies_outside_of_designer.md).

1. Los parámetros de la estrategia se crean usando un enfoque especial:

```cs
_candleTypeParam = this.Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);


private readonly StrategyParam<DataType> _candleTypeParam;

public DataType CandleType
{
	get => _candleTypeParam.Value;
	set => _candleTypeParam.Value = value;
}

private readonly StrategyParam<int> _long;

public int Long
{
	get => _long.Value;
	set => _long.Value = value;
}

private readonly StrategyParam<int> _short;

public int Short
{
	get => _short.Value;
	set => _short.Value = value;
}
```

Al usar la clase [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1), se aplica automáticamente el enfoque de guardar y restaurar configuración.

2. Al crear indicadores y suscribirse a datos de mercado, debe vincularlos para que los datos entrantes de la suscripción puedan actualizar los valores de los indicadores:

```cs
// ---------- crear indicadores -----------

var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

// ----------------------------------------

// --- vincular conjunto de velas e indicadores ----

var subscription = SubscribeCandles(CandleType);

subscription
	// vincular indicadores a las velas
	.Bind(longSma, shortSma, OnProcess)
	// iniciar procesamiento
	.Start();
```

3. Al trabajar con gráficos, es importante tener en cuenta que al ejecutar la estrategia [fuera de Designer](../../../live_execution/running_strategies_outside_of_designer.md), el objeto de gráfico puede estar ausente.

```cs
var area = CreateChartArea();

// area puede ser null si no hay GUI (estrategia alojada en Runner o en una app de consola propia)
if (area != null)
{
	DrawCandles(area, subscription);

	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);

	DrawOwnTrades(area);
}
```

4. Inicie la protección de posición mediante [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) si la lógica de la estrategia lo requiere:

```cs
// iniciar protección por take profit y/o stop loss
StartProtection(TakeValue, StopValue);
```

5. La propia lógica de la estrategia, implementada en el método OnProcess. El método es llamado por la suscripción creada en el paso 1:

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// en caso de que no nos hayamos suscrito solo a velas finalizadas
	if (candle.State != CandleStates.Finished)
		return;

	// calcular nuevos valores para short y long
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong)
	{
		// ocurrió un cruce

		// si short es menor que long, vender; en caso contrario, comprar
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// calcular tamaño para abrir posición o revertir
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		var priceStep = GetSecurity().PriceStep ?? 1;

		// calcular precio de orden como precio de cierre + desplazamiento
		var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		// guardar valores actuales para short y long
		_isShortLessThenLong = isShortLessThenLong;
	}
}
```
