# Indikatoren

[S#](../api.md) stellt mehr als 140 Standardindikatoren der technischen Analyse bereit. Dadurch können Sie fertige Indikatoren verwenden, statt sie von Grund auf neu zu erstellen. Sie können außerdem eigene Indikatoren auf Basis vorhandener Indikatoren erstellen, wie im Abschnitt [Benutzerdefinierter Indikator](indicators/custom_indicator.md) gezeigt. Alle Basisklassen für die Arbeit mit Indikatoren sowie die Indikatoren selbst befinden sich im Namespace [StockSharp.Algo.Indicators](xref:StockSharp.Algo.Indicators).

## Integration von Indikatoren in einen Handelsalgorithmus

1. Zuerst müssen Sie einen Indikator erstellen. Ein Indikator wird wie ein normales .NET-Objekt erstellt:

   ```cs
   var longSma = new SimpleMovingAverage { Length = 80 };
   var shortSma = new SimpleMovingAverage { Length = 30 };
   
   // Es wird empfohlen, Indikatoren zur Strategie-Sammlung hinzuzufügen
   Indicators.Add(longSma);
   Indicators.Add(shortSma);
   ```

2. Danach müssen Marktdaten für die Indikatoren verarbeitet werden. Der effizienteste Ansatz ist die Verwendung des Ergebnisses, das von der Methode [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)) zurückgegeben wird:

   ```cs
   private void ProcessCandle(ICandleMessage candle)
   {
       // Candle mit Indikatoren verarbeiten und Ergebnisse sofort speichern
       var longValue = longSma.Process(candle);
       var shortValue = shortSma.Process(candle);
       
       // Ergebnisse für Handelsentscheidungen verwenden
       if (shortValue.GetValue<decimal>() > longValue.GetValue<decimal>())
       {
           // Kaufsignal
           BuyAtMarket();
       }
   }
   ```

   Ein Indikator akzeptiert [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) als Eingabe. Einige Indikatoren arbeiten mit einer einfachen Zahl, etwa [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage). Andere benötigen eine vollständige Candle, etwa [MedianPrice](xref:StockSharp.Algo.Indicators.MedianPrice). Deshalb müssen Eingabewerte entweder in [DecimalIndicatorValue](xref:StockSharp.Algo.Indicators.DecimalIndicatorValue) oder in [CandleIndicatorValue](xref:StockSharp.Algo.Indicators.CandleIndicatorValue) umgewandelt werden. Der resultierende Wert des Indikators folgt denselben Regeln wie der Eingabewert.

3. Sowohl der Ergebnis- als auch der Eingabewert des Indikators besitzen die Eigenschaft [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal), die angibt, dass der Wert endgültig ist und sich der Indikator zu diesem Zeitpunkt nicht mehr ändern wird. Der Indikator [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) wird beispielsweise auf Basis des Schlusskurses einer Candle gebildet; im aktuellen Moment ist der endgültige Schlusskurs jedoch unbekannt und verändert sich. In diesem Fall ist der Ergebniswert von [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) `false`. Wenn Sie eine abgeschlossene Candle an den Indikator übergeben, sind sowohl Eingabe- als auch Ergebniswert von [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) `true`.

4. **Empfohlener Ansatz**: Verwenden Sie direkt die Werte, die beim Aufruf der Methode [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)) zurückgegeben werden, anstatt anschließend [GetCurrentValue](xref:StockSharp.Algo.Indicators.IndicatorHelper.GetCurrentValue(StockSharp.Algo.Indicators.IIndicator)) aufzurufen:

   ```cs
   // Beispiel einer Strategie mit zwei gleitenden Durchschnitten
   private void ProcessCandle(ICandleMessage candle)
   {
       // Candle mit Indikatoren verarbeiten und Ergebnisse sofort speichern
       var longValue = _longSma.Process(candle);
       var shortValue = _shortSma.Process(candle);
       
       // Im Chart zeichnen
       DrawCandlesAndIndicators(candle, longValue, shortValue);
       
       if (!IsFormedAndOnlineAndAllowTrading()) 
           return;
           
       // Erhaltene Werte für den Vergleich verwenden
       var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
       var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);
       
       // Prüfen, ob eine Kreuzung aufgetreten ist
       if (isShortLessCurrent == isShortLessPrev) 
           return;
       
       var volume = Volume + Math.Abs(Position);
       
       // Handelsaktionen auf Basis des Signals
       if (isShortLessCurrent)
           SellMarket(volume);
       else
           BuyMarket(volume);
   }
   ```

   Dieser Ansatz hat folgende Vorteile:
   - Er entspricht dem Streaming-Modell der Datenverarbeitung (empfangen -> verarbeiten -> Ergebnis verwenden)
   - Er ist effizienter, da wiederholte Zugriffe auf den Container der akkumulierten Werte vermieden werden
   - Er beseitigt potenzielle Synchronisationsprobleme zwischen dem Aufruf von Process und nachfolgenden GetCurrentValue-Aufrufen

5. Nicht empfohlener Ansatz (weniger effizient):

   ```cs
   // Suboptimaler Ansatz
   foreach (var candle in candles)
   {
       // Candle verarbeiten, aber den zurückgegebenen Wert ignorieren
       _longSma.Process(candle);
       _shortSma.Process(candle);
   }
   
   // Später versuchen, Werte über GetCurrentValue() abzurufen
   var isShortLessThenLong = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
   ```
   
   Bei diesem Ansatz erfolgt ein zusätzlicher Zugriff auf den Container historischer Indikatorwerte. Das führt zu Verzögerungen und stört das Streaming-Modell der Datenverarbeitung.

6. Alle Indikatoren besitzen die Eigenschaft [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed), die angibt, ob der Indikator einsatzbereit ist. Der Indikator [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) besitzt beispielsweise eine Periode. Solange der Indikator nicht eine Anzahl von Candles verarbeitet hat, die der Indikatorperiode entspricht, gilt er als nicht einsatzbereit. Die Eigenschaft [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed) ist dann `false`.

## Beispiel einer vollständigen Moving-Average-Strategie

Unten sehen Sie ein Beispiel einer Strategie, die Indikatoren korrekt verwendet, Candles verarbeitet und die Ergebnisse der Process-Methode nutzt:

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

		// Strategieparameter initialisieren
		_longSmaLength = Param(nameof(LongSmaLength), 80);
		_shortSmaLength = Param(nameof(ShortSmaLength), 30);
		_series = Param(nameof(Series), DataType.TimeFrame(TimeSpan.FromMinutes(15)));
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Indikatoren erstellen
		_shortSma = new SimpleMovingAverage { Length = _shortSmaLength.Value };
		_longSma = new SimpleMovingAverage { Length = _longSmaLength.Value };

		// Indikatoren zur Strategie-Sammlung hinzufügen
		Indicators.Add(_shortSma);
		Indicators.Add(_longSma);

		// Chart initialisieren
		_chart = GetChart();
		if (_chart != null)
		{
			InitChart();
		}
		
		// Candles abonnieren
		var subscription = new Subscription(_series.Value, Security);

		Connector
			.WhenCandlesFinished(subscription)
			.Do(ProcessCandle)
			.Apply(this);

		Connector.Subscribe(subscription);
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		// Candle mit Indikatoren verarbeiten und Ergebnisse speichern
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);
		
		// Im Chart zeichnen
		DrawCandlesAndIndicators(candle, longValue, shortValue);
		
		// Handelsbedingungen prüfen
		if (!IsFormedAndOnlineAndAllowTrading()) 
			return;

		// Aktuelle und vorherige Indikatorwerte vergleichen
		var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
		var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

		// Auf Kreuzung prüfen
		if (isShortLessCurrent == isShortLessPrev) 
			return;

		var volume = Volume + Math.Abs(Position);

		// Handelsaktionen auf Basis des Signals
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

	// Weitere Chart-Initialisierungsmethoden wurden der Kürze halber weggelassen
}
```

Dieses Beispiel zeigt den korrekten Ansatz zur Arbeit mit Indikatoren im Streaming-Modell von StockSharp.

