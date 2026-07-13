# Indicadores en estrategias

En StockSharp, la clase [Strategy](xref:StockSharp.Algo.Strategies.Strategy) proporciona un mecanismo especial para trabajar con indicadores, que permite controlar su estado de formación y determinar cuándo la estrategia está lista para trabajar.

## Propiedad Indicators

La propiedad [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) es una colección de indicadores usados en la estrategia. Esta colección está diseñada para realizar seguimiento automático del estado de formación de los indicadores (warm-up).

```cs
// Acceso a la colección de indicadores
INotifyList<IIndicator> indicators = strategy.Indicators;
```

## Propiedad IsFormed

De forma predeterminada, la implementación de la propiedad [Strategy.IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) comprueba si todos los indicadores de la colección [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) están formados:

```cs
// Implementación estándar en la clase Strategy
public virtual bool IsFormed => _indicators.AllFormed;
```

Una estrategia se considera "calentada" y lista para trabajar cuando todos los indicadores de la colección están formados (su propiedad [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) devuelve `true`).

## Agregar indicadores a la colección

Para determinar correctamente cuándo la estrategia está lista, debe agregar los indicadores que usa a la colección [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators):

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Crear indicadores
	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };

	// Agregar indicadores a la colección
	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);

	// ...
}
```

## Qué indicadores agregar

Debe agregar solo **indicadores independientes** a la colección [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators). Esta es una regla importante que ayuda a evitar esperas innecesarias y a determinar correctamente cuándo la estrategia está lista.

### Reglas para agregar indicadores:

1. **Indicadores independientes** - agregue indicadores que procesan directamente datos de mercado (velas, ticks, etc.):

   ```cs
   // Indicadores independientes
   var sma = new SimpleMovingAverage { Length = 20 };
   var rsi = new RelativeStrengthIndex { Length = 14 };

   Indicators.Add(sma);
   Indicators.Add(rsi);
   ```

2. **Cadenas de indicadores** - al usar una cadena de indicadores (donde la salida de uno es la entrada de otro), agregue a la colección solo el **primer indicador de la cadena**:

   ```cs
   // Cadena de indicadores
   var sma = new SimpleMovingAverage { Length = 20 };
   var stdev = new StandardDeviation { Length = 20 };
   var bollingerBands = new BollingerBands
   {
       SmaIndicator = sma,
       DeviationIndicator = stdev
   };

   // Agregar solo el primer indicador de la cadena
   Indicators.Add(sma);
   // NO agregue indicadores dependientes de otros indicadores
   // Indicators.Add(stdev); - incorrecto
   // Indicators.Add(bollingerBands); - incorrecto
   ```

3. **Indicadores combinados** - para indicadores que usan varios indicadores independientes (por ejemplo, MACD), agréguelos todos:

   ```cs
   var fastEma = new ExponentialMovingAverage { Length = 12 };
   var slowEma = new ExponentialMovingAverage { Length = 26 };
   var signalEma = new ExponentialMovingAverage { Length = 9 };
   var macd = new MovingAverageConvergenceDivergence
   {
       FastEma = fastEma,
       SlowEma = slowEma,
       SignalEma = signalEma
   };

   // Agregar indicadores base
   Indicators.Add(fastEma);
   Indicators.Add(slowEma);
   ```

## Ejemplos de uso

### Ejemplo básico con dos medias móviles

```cs
public class SmaStrategy : Strategy
{
	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;

	// ...

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		_longSma = new SimpleMovingAverage { Length = LongSmaLength };
		_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

		// Agregar indicadores a la colección para seguir su estado
		Indicators.Add(_longSma);
		Indicators.Add(_shortSma);

		// ...
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		// Procesar indicadores
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);

		// Comprobar si la estrategia está lista antes de ejecutar la lógica de negociación
		if (!IsFormed)
			return;

		// Lógica de negociación
		// ...
	}
}
```

### Ejemplo usando IsFormedAndOnline

Para comprobar si la estrategia está lista para operar, a menudo se usa el método [IsFormedAndOnlineAndAllowTrading](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)), que combina la comprobación de formación de indicadores, estado online y permiso de negociación:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Procesar indicadores
	var longValue = _longSma.Process(candle);
	var shortValue = _shortSma.Process(candle);

	// Comprobación integral de preparación de la estrategia
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Lógica de negociación
	// ...
}
```

## Optimización del uso de indicadores

En estrategias más complejas, es importante organizar correctamente el trabajo con indicadores:

```cs
public class ComplexStrategy : Strategy
{
	private SimpleMovingAverage _sma;
	private RelativeStrengthIndex _rsi;
	private BollingerBands _bollinger;
	private StandardDeviation _stdev;

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Crear indicadores
		_sma = new SimpleMovingAverage { Length = 20 };
		_rsi = new RelativeStrengthIndex { Length = 14 };

		_stdev = new StandardDeviation { Length = 20 };
		_bollinger = new BollingerBands
		{
			SmaIndicator = _sma,
			DeviationIndicator = _stdev
		};

		// Agregar solo indicadores independientes
		Indicators.Add(_sma);
		Indicators.Add(_rsi);
		// No agregue _stdev ni _bollinger, ya que dependen de _sma

		// ...
	}

	// ...
}
```

## Funciones avanzadas

Puede sobrescribir la propiedad [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) en su estrategia si el comportamiento estándar no es suficiente:

```cs
public override bool IsFormed
{
	get
	{
		// Comprobación estándar de indicadores
		if (!base.IsFormed)
			return false;

		// Condiciones adicionales de preparación de la estrategia
		return _customCondition && _additionalCheck;
	}
}
```

## Ver también

- [Lista de indicadores](../indicators/list_of_indicators.md)
- [Indicador personalizado](../indicators/custom_indicator.md)
- [Compatibilidad de estrategias con plataformas](compatibility.md)
