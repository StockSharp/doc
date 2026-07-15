# FDI

**Fraktaldimensionsindex (FDI)** quantifiziert die Rauheit einer Preisreihe.

Um den Indikator verwenden zu können, müssen Sie die Klasse [FractalDimension](xref:StockSharp.Algo.Indicators.FractalDimension) verwenden.

## Beschreibung

Der FDI reicht von 1 bis 2 und spiegelt das Marktverhalten wider:
- Werte nahe 1 weisen auf einen anhaltenden Trend hin (glatterer Verlauf).
- Werte um 1,5 entsprechen einem Random Walk.
- Werte nahe 2 weisen auf einen schwankenden oder lauten Markt hin.

Der Indikator basiert auf fraktaler Geometrie und misst, wie komplex der Preispfad ist.

## Parameter

Der Indikator hat den folgenden Parameter:
- **Länge** – Berechnungszeitraum (Standardwert: 30)

## Berechnung

FDI wird durch Vergleich der gesamten Preispfadlänge mit der gesamten Hoch-Tief-Spanne berechnet:

1. Sum absolute Differenzen zwischen aufeinanderfolgenden Preisen über den Zeitraum, um die Länge des Preispfads zu ermitteln.
2. Ermitteln Sie die Differenz zwischen dem Höchst- und Tiefstwert für den Zeitraum.
3. Berechnen Sie FDI mit:
   ```
   FDI = 1 + (log(PathLength) - log(Range)) / log(2 * (Length - 1))
   ```
4. Klemmen Sie das Ergebnis zwischen 1 und 2.

## Interpretation

- **FDI nahe 1** – starkes Trendverhalten.
- **FDI etwa 1,5** – Random Walk; Die Trendstärke ist neutral.
- **FDI näher an 2** – unruhiger oder seitwärts gerichteter Markt.

![FDI Diagramm](../../../../images/indicator_fractal_dimension.png)

## Siehe auch

[Hurst-Exponent](hurst_exponent.md)

[Fraktaler adaptiver gleitender Durchschnitt](fractal_adaptive_moving_average.md)
