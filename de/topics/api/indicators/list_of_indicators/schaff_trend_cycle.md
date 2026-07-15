# STC

**Schaff-Trendzyklus (STC)** ist ein von Doug Schaff entwickelter Momentum-Indikator. STC basiert auf der Annahme, dass sich Marktzyklen häufiger zwischen überkauften und überverkauften Bedingungen bewegen als in einem echten Trend.

Um den Indikator verwenden zu können, müssen Sie die Klasse [SchaffTrendCycle](xref:StockSharp.Algo.Indicators.SchaffTrendCycle) verwenden.

## Beschreibung

Der Schaff-Trendzyklus vereint die Vorteile des Stochastik-Oszillators, des MACD und der zyklischen Analyse. Dieser Indikator kann schneller auf Trendänderungen reagieren als herkömmliche Indikatoren wie MACD oder Stochastik.

STC schwankt zwischen 0 und 100:
- Werte über 75 weisen normalerweise auf überkaufte Bedingungen hin
- Werte unter 25 weisen auf überverkaufte Bedingungen hin
- Das Überschreiten der 50er-Marke könnte eine Trendwende signalisieren

Hauptindikatorsignale:
- Kaufen Sie, wenn STC die 25-Marke von unten nach oben überschreitet (wodurch die überverkaufte Zone verlassen wird).
- Verkaufen, wenn STC die 75-Marke von oben nach unten überschreitet (Verlassen der überkauften Zone)

## Parameter

- **Länge** – Hauptzeitraum für die Berechnung des Indikators.

## Berechnung

Die STC-Berechnung erfolgt in mehreren Schritten:

1. Berechnen Sie MACD:
   ```
   MACD = EMA(Close, Fast) - EMA(Close, Slow)
   Signal = EMA(MACD, Signal)
   ```
   wobei „Schnell“, „Langsam“ und „Signal“ typischerweise 23, 50 bzw. 10 sind.

2. Berechnen Sie den Stochastik-Oszillator basierend auf MACD:
   ```
   Stoch_K = 100 * ((MACD - Lowest(MACD, Length)) / (Highest(MACD, Length) - Lowest(MACD, Length)))
   Stoch_D = EMA(Stoch_K, 3)
   ```

3. Wiederholen Sie die stochastische Berechnung, um STC zu erhalten:
   ```
   STC = 100 * ((Stoch_D - Lowest(Stoch_D, Length)) / (Highest(Stoch_D, Length) - Lowest(Stoch_D, Length)))
   ```

Das Ergebnis ist ein Oszillator, der glatter ist als der klassische Stochastik und schneller auf Trendänderungen reagiert als der MACD.

![STC Diagramm](../../../../images/indicator_schaff_trend_cycle.png)

## Siehe auch

[MACD](macd.md)
[Stochastik](stochastic_oscillator.md)
