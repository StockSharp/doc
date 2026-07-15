# CHV

**Chaikin-Volatilität (CHV)** gibt die Differenz zwischen dem Maximum der Käufe und dem Minimum der Verkäufe über einen bestimmten Zeitraum an. Der Indikator ermöglicht eine qualitative Analyse von Preisänderungen und der Bandbreite zwischen Preismaximum und -minimum. CHV berücksichtigt bei seiner Berechnung keine Preislücken, was in gewisser Weise als Nachteil angesehen werden kann.

Um den Indikator zu verwenden, sollte die Klasse [ChaikinVolatility](xref:StockSharp.Algo.Indicators.ChaikinVolatility) verwendet werden.
##### Berechnung

Die Berechnung des Chaikin-Volatilität-Indikators beginnt mit der Bestimmung des Spreads – der Differenz zwischen dem Maximum und dem Minimum des aktuellen Balkens. Das erhaltene Ergebnis wird mit einem exponentiellen gleitenden Durchschnitt über den entsprechenden Zeitraum geglättet. Die Volatilität wird nach der Formel berechnet:

CHV = (EMA(H-L(i), n) — EMA(H-L(i-n), n)) / EMA(H-L(i-n), n) x 100.

In diesem Fall stellt H-L(i) die Preisdifferenz auf dem aktuellen Balken dar und H-L(i-n) – die Preisdifferenz auf einem Balken, der n Perioden zuvor aufgetreten ist. Somit haben wir die Möglichkeit, visuell zu sehen, wie sich der Preis im Laufe der Zeit verändert hat.

Hauptparameter des Indikators:

- **ROC-Zeitraum** – die Periodennummer, relativ zu der die Berechnung durchgeführt wird. Ursprünglich auf 5 eingestellt.
- **Glättungszeitraum** – der Zeitraum des gleitenden Durchschnitts. Standardmäßig ist es auf 32 eingestellt.

![CHV Diagramm](../../../../images/indicatorchaikinvolatility.png)

## Siehe auch

[CMO](cmo.md)
