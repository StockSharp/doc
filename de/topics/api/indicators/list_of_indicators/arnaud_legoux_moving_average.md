# ALMA

**Gleitender Durchschnitt nach Arnaud Legoux (ALMA)** ist ein von Arnaud Legoux entwickelter Indikator, der optimiert wurde, um Marktrauschen zu eliminieren und Signalverzögerungen zu reduzieren.

Zur Verwendung des Indikators müssen Sie die Klasse [ArnaudLegouxMovingAverage](xref:StockSharp.Algo.Indicators.ArnaudLegouxMovingAverage) verwenden.

## Beschreibung

ALMA kombiniert die Vorteile zweier Ansätze zur Datenglättung:
1. Eliminierung von Marktrauschen (wie die meisten gleitenden Durchschnitte)
2. Minimierung der Verzögerung (typisch für viele Glättungsindikatoren)

Der ALMA-Indikator verwendet eine Normalverteilung (Gauß-Verteilung) als Gewichtungsfunktion, die mit den Parametern Offset und Sigma fein abgestimmt werden kann. Dadurch ist er ein sehr flexibles und effektives Werkzeug für die technische Analyse.

ALMA wird verwendet für:
- Bestimmen des aktuellen Trends
- Erkennen von Umkehrpunkten
- Erstellen von Handelssystemen auf Basis von Kreuzungen

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** - Berechnungsperiode (Anzahl der zu analysierenden Kerzen)
- **Sigma** - Sigma, ein Parameter zur Steuerung der Form der Gauß-Kurve (empfohlener Wert: 6)
- **Offset** - Offset, ein Parameter zur Steuerung von Glättung und Reaktionsgeschwindigkeit (empfohlener Wert: 0,85)

## Berechnung

Die ALMA-Berechnung erfolgt in mehreren Schritten:

1. Bestimmung der Gewichte für jeden Datenpunkt im Fenster anhand der Normalverteilung (Gauß-Verteilung):
   ```
   m = floor(Offset * (Length - 1))
   s = Length / Sigma

   Für jedes i von 0 bis Length-1:
   w(i) = exp(-((i - m)^2) / (2 * s^2))
   ```

2. Normalisierung der Gewichte:
   ```
   Sum_of_weights = Summe aller w(i)

   Für jedes i von 0 bis Length-1:
   w_norm(i) = w(i) / Sum_of_weights
   ```

3. Berechnung von ALMA als gewichtete Summe:
   ```
   ALMA = sum(Price(t-i) * w_norm(i)) für alle i von 0 bis Length-1
   ```

Wobei:
- Length - ALMA-Periode
- Offset - Offset-Parameter (von 0 bis 1)
- Sigma - Sigma-Parameter (normalerweise von 2 bis 8)

![ALMA Diagramm](../../../../images/indicator_arnaud_legoux_moving_average.png)

## Siehe auch

[SMA](sma.md)
[EMA](ema.md)
[T3MA](t3_moving_average.md)
[ZLEMA](zero_lag_exponential_moving_average.md)
