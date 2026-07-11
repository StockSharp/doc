# DeMarker

Der **DeMarker (DeM)**-Indikator bewertet den Kauf- und Verkaufsdruck, indem er die Extrema des aktuellen Balkens mit dem vorherigen Balken vergleicht.
Er hebt überkaufte und überverkaufte Zonen hervor und hilft, potenzielle Wendepunkte zu erkennen.

Verwenden Sie die Klasse [DeMarker](xref:StockSharp.Algo.Indicators.DeMarker), um mit diesem Indikator zu arbeiten.

## Berechnung

1. Berechnen Sie für jeden Balken Zwischenwerte:
   `DeMax = max(High − PreviousHigh, 0)`
   `DeMin = max(PreviousLow − Low, 0)`
2. Glätten Sie `DeMax` und `DeMin` mit einem gleitenden Durchschnitt der Länge **Length**.
3. Berechnen Sie den Endwert:
   `DeMarker = SMA(DeMax, Length) / (SMA(DeMax, Length) + SMA(DeMin, Length))`.

Die Ausgabe wird zwischen 0 und 1 normalisiert.

## Parameter

- **Length** – Glättungszeitraum, der die Reaktionsfähigkeit des Indikators steuert.

## Interpretation

- **Über 0,7** – überkaufte Bedingungen, mögliche Abwärtskorrektur.
- **Unter 0,3** – überverkaufte Bedingungen, mögliche Umkehr nach oben.
- **Divergenz** zwischen Preis und Indikator warnt vor einer Trendwende.

DeMarker kann für Einstiege in den Gegentrend sowie zur Bestätigung von Signalen von Momentum-Oszillatoren verwendet werden.

![DeMarker Diagramm](../../../../images/indicator_demarker.png)

## Siehe auch

[RSI](rsi.md)
[Stochastischer Oszillator](stochastic_oscillator.md)
[Impuls](momentum.md)
