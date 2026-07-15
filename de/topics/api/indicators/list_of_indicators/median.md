# Gleitender Median

Der Indikator **Gleitender Median** berechnet den Median der aktuellsten N-Werte. Im Vergleich zu gleitenden Durchschnitten reagiert er weniger empfindlich auf Ausreißer und bewahrt abrupte Preisänderungen, wodurch er in unruhigen Marktphasen nützlich ist.

Verwenden Sie die Klasse [Median](xref:StockSharp.Algo.Indicators.Median), um auf den Indikator zuzugreifen.

## Beschreibung

Ein Medianfilter sortiert die Preise innerhalb des beweglichen Fensters und wählt den mittleren Wert aus. Als Ergebnis:

- Einzelne Spitzen oder Tropfen verzerren die Ausgabe nicht.
- Die Verzögerung ist kleiner als bei vielen Glättungsfiltern.
- Die Form des Signals bleibt kantiger und hilft so, Umkehrungen zu erfassen.

## Parameter

- **Länge** – Fenstergröße, die zur Berechnung des Medians verwendet wird. Größere Fenster sorgen für eine stärkere Glättung, erhöhen aber die Verzögerung.

## Nutzung

- Wenden Sie Gleitender Median als Alternative zu gleitenden Durchschnitten an, wenn Preisdaten erhebliches Rauschen enthalten.
- Kreuzungen zwischen Preis und Median können als Trendwechselsignale behandelt werden.
- Kombinieren Sie den Median mit anderen Filtern, um Trends zu extrahieren und gleichzeitig wichtige Preissprünge beizubehalten.

![Gleitender Median Diagramm](../../../../images/indicator_median.png)

## Siehe auch

[SMA](sma.md)
[EMA](ema.md)
[Geglätteter gleitender Durchschnitt](smoothed_ma.md)
