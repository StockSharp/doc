# T3MA

**Gleitender T3-Durchschnitt (T3MA)** ist ein fortgeschrittener gleitender Durchschnitt, der von Tim Tillson entwickelt wurde. T3 stellt einen dreifach geglätteten exponentiellen gleitenden Durchschnitt (EMA) mit einem Volumenfaktor dar, wodurch er im Vergleich zu herkömmlichen gleitenden Durchschnitten glatter und weniger anfällig für falsche Signale ist.

Um den Indikator verwenden zu können, müssen Sie die Klasse [T3MovingAverage](xref:StockSharp.Algo.Indicators.T3MovingAverage) verwenden.

## Beschreibung

Der gleitende T3-Durchschnitt wurde entwickelt, um die Nachteile traditioneller gleitender Durchschnitte wie Verzögerungen und falsche Signale zu beseitigen. Durch mehrfache Glättung und einen einstellbaren Volumenfaktor sorgt T3MA für eine glattere Kurve, die der Preisentwicklung genauer folgt.

Hauptvorteile von T3MA:
- Weniger Verzögerung im Vergleich zu gewöhnlichen gleitenden Durchschnitten
- Glattere Kurve mit weniger Fehlsignalen
- Anpassungsfähigkeit an verschiedene Marktbedingungen durch einstellbaren Volumenfaktor

T3MA kann verwendet werden für:
- Bestimmung der Trendrichtung
- Finden von Ein- und Ausstiegspunkten, wenn der Preis die Indikatorlinie kreuzt
- Aufbau von Handelssystemen basierend auf Kreuzungen mehrerer T3MAs mit unterschiedlichen Perioden

## Parameter

- **Volumenfaktor** – Volumenfaktor, der den Grad der Glättung bestimmt (normalerweise ein Wert zwischen 0 und 1, empfohlener Wert ist 0,7).
- **Länge** – Berechnungszeitraum, ähnlich dem Zeitraum bei gewöhnlichen gleitenden Durchschnitten.

## Berechnung

Die Berechnung des gleitenden T3-Durchschnitts erfolgt in mehreren Schritten:

1. Berechnen Sie sechs aufeinanderfolgende exponentielle gleitende Durchschnitte mit derselben Periode:
   ```
   EMA1 = EMA(Price, Length)
   EMA2 = EMA(EMA1, Length)
   EMA3 = EMA(EMA2, Length)
   EMA4 = EMA(EMA3, Length)
   EMA5 = EMA(EMA4, Length)
   EMA6 = EMA(EMA5, Length)
   ```

2. Berechnen Sie T3 basierend auf dem erhaltenen EMAs und dem Volumenfaktor:
   ```
   c1 = -VolumeFactor^3
   c2 = 3 * VolumeFactor^2 + 3 * VolumeFactor^3
   c3 = -6 * VolumeFactor^2 - 3 * VolumeFactor - 3 * VolumeFactor^3
   c4 = 1 + 3 * VolumeFactor + VolumeFactor^3 + 3 * VolumeFactor^2

   T3 = c1 * EMA6 + c2 * EMA5 + c3 * EMA4 + c4 * EMA3
   ```

Wenn VolumeFactor = 0 ist, entspricht T3 EMA3 (dreifache EMA). Wenn VolumeFactor = 1 ist, wird T3 maximal geglättet.

![T3MA Diagramm](../../../../images/indicator_t3_moving_average.png)

## Siehe auch

[EMA](ema.md)
[DEMA](dema.md)
[TEMA](tema.md)
