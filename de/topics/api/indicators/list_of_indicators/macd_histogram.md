# MACD-Histogramm

**Konvergenz/Divergenz gleitender Durchschnitte (MACD)** ist ein Momentum-Indikator, der die Beziehung zwischen zwei gleitenden Durchschnitten des Kurses eines Wertpapiers anzeigt, dargestellt als Histogramm.

Um den Indikator zu verwenden, sollte die Klasse [MovingAverageConvergenceDivergenceHistogram](xref:StockSharp.Algo.Indicators.MovingAverageConvergenceDivergenceHistogram) verwendet werden.

Zur Berechnung des Indikators werden drei exponentielle gleitende Durchschnitte mit unterschiedlichen Perioden verwendet. Der schnell gleitende Durchschnitt mit kürzerer Periode (EMA_s) wird vom langsam gleitenden Durchschnitt mit längerer Periode (EMA_l) subtrahiert. Die MACD-Linie wird aus den erhaltenen Werten erstellt.

MACD = EMA_s(P) − EMA_l(P)

Die Standardperioden sind 12 und 26. Diese Linie wird dann durch einen dritten exponentiellen gleitenden Durchschnitt (EMA_a) geglättet, typischerweise mit einer Periode von 9, was zur sogenannten MACD-Signallinie (Signal) führt.

Signal = EMA_a(EMA_s(P) − EMA_l(P))

Diese beiden resultierenden Kurven stellen den regulären linearen MACD dar. Auch die Nulllinie, relativ zu der die Kurven schwanken, ist üblicherweise im Anzeigefenster markiert.

Beim Erstellen des MACD-Histogramms (MACD-Histogramm) zeigen die Histogrammbalken die Differenz zwischen den Signal- und MACD-Linien an, was die Wahrnehmung des Indikators weiter vereinfacht.

![IndicatorMovingAverageConvergenceDivergenceHistogram](../../../../images/indicatormovingaverageconvergencedivergencehistogram.png)

## Siehe auch

[MACD mit Signallinie](macd_with_signal_line.md)
