# Indicadores

[S#](../api.md) proporciona más de 140 indicadores de análisis técnico estándar. Esto le permite utilizar indicadores ya preparados en lugar de crearlos desde cero. También puede crear sus propios indicadores basados ​​en los existentes, como se muestra en la sección [Indicador personalizado](indicators/custom_indicator.md). Todas las clases base para trabajar con indicadores, así como los propios indicadores, se encuentran en el espacio de nombres [StockSharp.Algo.Indicators](xref:StockSharp.Algo.Indicators).

## Integración de indicadores en un algoritmo de trading

1. Primero, necesitas crear un indicador. Un indicador se crea como un objeto .NET normal:

   ```cs
   var longSma = new SimpleMovingAverage { Length = 80 };
   var shortSma = new SimpleMovingAverage { Length = 30 };
   
   // Se recomienda agregar indicadores a la colección de estrategias.
   Indicators.Add(longSma);
   Indicators.Add(shortSma);
   ```

2. A continuación, debe procesar los datos de mercado para los indicadores. El enfoque más eficaz es utilizar el resultado devuelto por el método [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)):

   ```cs
   private void ProcessCandle(ICandleMessage candle)
   {
       // Procese la vela con indicadores y guarde inmediatamente los resultados.
       var longValue = longSma.Process(candle);
       var shortValue = shortSma.Process(candle);
       
       // Utilice los resultados para tomar decisiones de trading
       if (shortValue.GetValue<decimal>() > longValue.GetValue<decimal>())
       {
           // señal de compra
           BuyAtMarket();
       }
   }
   ```

   Un indicador acepta [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) como entrada. Algunos indicadores funcionan con un número simple, como [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage). Otros requieren una vela completa, como [MedianPrice](xref:StockSharp.Algo.Indicators.MedianPrice). Por lo tanto, los valores de entrada deben convertirse a [DecimalIndicatorValue](xref:StockSharp.Algo.Indicators.DecimalIndicatorValue) o [CandleIndicatorValue](xref:StockSharp.Algo.Indicators.CandleIndicatorValue). El valor resultante del indicador sigue las mismas reglas que el valor de entrada.

3. Tanto el valor resultante como el de entrada del indicador tienen la propiedad [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal), que indica que el valor es final y el indicador no cambiará en este momento. Por ejemplo, el indicador [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) se forma en función del precio de cierre de la vela, pero en este momento el precio de cierre final se desconoce y cambia. En este caso, el valor resultante de [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) será falso. Si pasa una vela completa al indicador, tanto el valor de entrada como el resultante de [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) serán verdaderos.

4. **Enfoque recomendado**: utilice directamente los valores obtenidos al llamar al método [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)), en lugar de llamar posteriormente a [GetCurrentValue](xref:StockSharp.Algo.Indicators.IndicatorHelper.GetCurrentValue(StockSharp.Algo.Indicators.IIndicator)):

   ```cs
   // Ejemplo de estrategia con dos medias móviles
   private void ProcessCandle(ICandleMessage candle)
   {
       // Procese la vela con indicadores y guarde inmediatamente los resultados.
       var longValue = _longSma.Process(candle);
       var shortValue = _shortSma.Process(candle);
       
       // dibujar en el gráfico
       DrawCandlesAndIndicators(candle, longValue, shortValue);
       
       if (!IsFormedAndOnlineAndAllowTrading()) 
           return;
           
       // Utilice los valores obtenidos para comparar.
       var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
       var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);
       
       // Compruebe si se produjo un cruce
       if (isShortLessCurrent == isShortLessPrev) 
           return;
       
       var volume = Volume + Math.Abs(Position);
       
       // Acciones de trading basadas en la señal.
       if (isShortLessCurrent)
           SellMarket(volume);
       else
           BuyMarket(volume);
   }
   ```

   Este enfoque tiene las siguientes ventajas:
   - Se alinea con el modelo de procesamiento de datos en streaming (recibir → procesar → usar el resultado)
   - Es más eficiente ya que evita acceder repetidamente al contenedor de valores acumulados.
   - Elimina posibles problemas de sincronización entre la llamada al proceso y las llamadas posteriores a GetCurrentValue.

5. Enfoque no recomendado (menos eficiente):

   ```cs
   // Enfoque subóptimo
   foreach (var candle in candles)
   {
       // Procesa la vela pero ignora el valor devuelto.
       _longSma.Process(candle);
       _shortSma.Process(candle);
   }
   
   // Luego intente obtener valores a través de GetCurrentValue()
   var isShortLessThenLong = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
   ```
   
   Con este enfoque, hay un acceso adicional al contenedor de valores históricos de los indicadores, lo que introduce retrasos e interrumpe el modelo de transmisión de procesamiento de datos.

6. Todos los indicadores tienen la propiedad [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed), que indica si el indicador está listo para su uso. Por ejemplo, el indicador [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) tiene un período y hasta que el indicador procese un número de velas igual al período del indicador, se considerará que el indicador no está listo para su uso. Y la propiedad [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed) será falsa.

## Ejemplo de una estrategia completa con medias móviles

A continuación se muestra un ejemplo de una estrategia que utiliza correctamente indicadores, procesa velas y utiliza los resultados del método de proceso:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;

	private IChartIndicatorElement _longSmaIndicatorElement;
	private IChartIndicatorElement _shortSmaIndicatorElement;
	private IChartCandleElement _chartCandleElement;
	private IChartTradeElement _tradesElem;
	private IChart _chart;

	public SmaStrategy()
	{
		base.Name = "SMA strategy";

		// Inicializar parámetros de estrategia
		_longSmaLength = Param(nameof(LongSmaLength), 80);
		_shortSmaLength = Param(nameof(ShortSmaLength), 30);
		_series = Param(nameof(Series), DataType.TimeFrame(TimeSpan.FromMinutes(15)));
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Crear indicadores
		_shortSma = new SimpleMovingAverage { Length = _shortSmaLength.Value };
		_longSma = new SimpleMovingAverage { Length = _longSmaLength.Value };

		// Agregar indicadores a la colección de estrategias
		Indicators.Add(_shortSma);
		Indicators.Add(_longSma);

		// Inicializar gráfico
		_chart = GetChart();
		if (_chart != null)
		{
			InitChart();
		}
		
		// Suscríbete a velas
		var subscription = new Subscription(_series.Value, Security);

		Connector
			.WhenCandlesFinished(subscription)
			.Do(ProcessCandle)
			.Apply(this);

		Connector.Subscribe(subscription);
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		// Procesa la vela con indicadores y guarda los resultados.
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);
		
		// dibujar en el gráfico
		DrawCandlesAndIndicators(candle, longValue, shortValue);
		
		// Consultar condiciones para operar
		if (!IsFormedAndOnlineAndAllowTrading()) 
			return;

		// Comparar los valores de los indicadores actuales y anteriores
		var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
		var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

		// comprobar si hay cruce
		if (isShortLessCurrent == isShortLessPrev) 
			return;

		var volume = Volume + Math.Abs(Position);

		// Acciones de trading basadas en la señal.
		if (isShortLessCurrent)
			SellMarket(volume);
		else
			BuyMarket(volume);
	}

	private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
	{
		if (_chart == null) return;
		var data = _chart.CreateData();
		data.Group(candle.OpenTime)
			.Add(_chartCandleElement, candle)
			.Add(_longSmaIndicatorElement, longSma)
			.Add(_shortSmaIndicatorElement, shortSma);
		_chart.Draw(data);
	}

	// Otros métodos de inicialización de gráficos se omiten por motivos de brevedad.
}
```

Este ejemplo demuestra el enfoque correcto para trabajar con indicadores en el modelo de transmisión StockSharp.
