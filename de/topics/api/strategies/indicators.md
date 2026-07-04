# Indikatoren in Strategien

In StockSharp stellt die Klasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) einen speziellen Mechanismus für die Arbeit mit Indikatoren bereit. Damit können Sie den Bildungszustand der Indikatoren kontrollieren und bestimmen, wann die Strategie arbeitsbereit ist.

## Indicators-Eigenschaft

Die Eigenschaft [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) ist eine Sammlung der in der Strategie verwendeten Indikatoren. Diese Sammlung ist dafür vorgesehen, den Zustand der Indikatorbildung (Warm-up) automatisch zu verfolgen.

```cs
// Zugriff auf die Indikatorsammlung
INotifyList<IIndicator> indicators = strategy.Indicators;
```

## IsFormed-Eigenschaft

Standardmäßig prüft die Implementierung der Eigenschaft [Strategy.IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed), ob alle Indikatoren in der Sammlung [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) gebildet sind:

```cs
// Standardimplementierung in der Klasse Strategy
public virtual bool IsFormed => _indicators.AllFormed;
```

Eine Strategie gilt als "aufgewärmt" und arbeitsbereit, wenn alle Indikatoren in der Sammlung gebildet sind, also ihre Eigenschaft [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) `true` zurückgibt.

## Hinzufügen von Indikatoren zur Sammlung

Damit korrekt bestimmt werden kann, wann die Strategie bereit ist, müssen Sie die verwendeten Indikatoren zur Sammlung [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) hinzufügen:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Indikatoren erstellen
	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };
	
	// Indikatoren zur Sammlung hinzufügen
	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);
	
	// ...
}
```

## Welche Indikatoren hinzugefügt werden sollten

Sie sollten nur **unabhängige Indikatoren** zur Sammlung [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) hinzufügen. Dies ist eine wichtige Regel, die unnötiges Warten vermeidet und korrekt bestimmt, wann die Strategie bereit ist.

### Regeln zum Hinzufügen von Indikatoren:

1. **Unabhängige Indikatoren** - fügen Sie Indikatoren hinzu, die Marktdaten direkt verarbeiten (Kerzen, Ticks usw.):

   ```cs
   // Unabhängige Indikatoren
   var sma = new SimpleMovingAverage { Length = 20 };
   var rsi = new RelativeStrengthIndex { Length = 14 };
   
   Indicators.Add(sma);
   Indicators.Add(rsi);
   ```

2. **Indikatorketten** - wenn Sie eine Indikatorkette verwenden, bei der die Ausgabe eines Indikators die Eingabe für einen anderen ist, fügen Sie nur den **ersten Indikator in der Kette** zur Sammlung hinzu:

   ```cs
   // Indikatorkette
   var sma = new SimpleMovingAverage { Length = 20 };
   var stdev = new StandardDeviation { Length = 20 };
   var bollingerBands = new BollingerBands 
   { 
       SmaIndicator = sma,
       DeviationIndicator = stdev
   };
   
   // Nur den ersten Indikator in der Kette hinzufügen
   Indicators.Add(sma);
   // Keine Indikatoren hinzufügen, die von anderen Indikatoren abhängen
   // Indicators.Add(stdev); - falsch
   // Indicators.Add(bollingerBands); - falsch
   ```

3. **Kombinierte Indikatoren** - fügen Sie bei Indikatoren, die mehrere unabhängige Indikatoren verwenden (z. B. MACD), alle diese Indikatoren hinzu:

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
   
   // Basisindikatoren hinzufügen
   Indicators.Add(fastEma);
   Indicators.Add(slowEma);
   ```

## Verwendungsbeispiele

### Einfaches Beispiel mit zwei gleitenden Durchschnitten

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
		
		// Indikatoren zur Sammlung hinzufügen, um ihren Zustand zu verfolgen
		Indicators.Add(_longSma);
		Indicators.Add(_shortSma);
		
		// ...
	}
	
	private void ProcessCandle(ICandleMessage candle)
	{
		// Indikatoren verarbeiten
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);
		
		// Prüfen, ob die Strategie bereit ist, bevor Handelslogik ausgeführt wird
		if (!IsFormed)
			return;
			
		// Handelslogik
		// ...
	}
}
```

### Beispiel mit IsFormedAndOnline

Zur Prüfung, ob die Strategie handelsbereit ist, wird häufig die Methode [IsFormedAndOnlineAndAllowTrading](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) verwendet. Sie kombiniert die Prüfung der Indikatorbildung, des Online-Status und der Handelsberechtigung:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Indikatoren verarbeiten
	var longValue = _longSma.Process(candle);
	var shortValue = _shortSma.Process(candle);
	
	// Umfassende Prüfung der Strategiebereitschaft
	if (!IsFormedAndOnlineAndAllowTrading())
		return;
		
	// Handelslogik
	// ...
}
```

## Optimierung der Indikatorverwendung

In komplexeren Strategien ist es wichtig, die Arbeit mit Indikatoren korrekt zu organisieren:

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
		
		// Indikatoren erstellen
		_sma = new SimpleMovingAverage { Length = 20 };
		_rsi = new RelativeStrengthIndex { Length = 14 };
		
		_stdev = new StandardDeviation { Length = 20 };
		_bollinger = new BollingerBands 
		{ 
			SmaIndicator = _sma,
			DeviationIndicator = _stdev 
		};
		
		// Nur unabhängige Indikatoren hinzufügen
		Indicators.Add(_sma);
		Indicators.Add(_rsi);
		// _stdev und _bollinger nicht hinzufügen, da sie von _sma abhängen
		
		// ...
	}
	
	// ...
}
```

## Erweiterte Funktionen

Sie können die Eigenschaft [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) in Ihrer Strategie überschreiben, wenn das Standardverhalten nicht ausreicht:

```cs
public override bool IsFormed
{
	get
	{
		// Standardprüfung der Indikatoren
		if (!base.IsFormed)
			return false;
			
		// Zusätzliche Bedingungen für die Strategiebereitschaft
		return _customCondition && _additionalCheck;
	}
}
```

## Siehe auch

- [Liste der Indikatoren](../indicators/list_of_indicators.md)
- [Benutzerdefinierter Indikator](../indicators/custom_indicator.md)
- [Strategiekompatibilität mit Plattformen](compatibility.md)

