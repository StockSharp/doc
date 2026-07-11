# A/D

**Acceleration/Deceleration (A/D)** ist ein von Bill Williams entwickelter Oszillator. Er misst die Beschleunigung und Verlangsamung des Trendmomentums.

Zur Verwendung des Indikators sollte die Klasse [Acceleration](xref:StockSharp.Algo.Indicators.Acceleration) verwendet werden.
##### Berechnung

Das A/D-Histogramm ist die Differenz zwischen dem Wert des 5/34-Histogramms der treibenden Kraft und dem 5-periodigen einfachen gleitenden Durchschnitt dieses Histogramms. Die Werte beziehen sich auf den klassischen Oszillator; in den Einstellungen können jederzeit eigene Parameter angegeben werden.

MEDIANPREIS = (HIGH + LOW) / 2
AO = SMA (MEDIANPREIS, 5) - SMA (MEDIANPREIS, 34)
A/D = AO - SMA (AO, 5)

wobei:

MEDIANPREIS - Medianpreis;
HIGH - höchster Preis des Balkens;
LOW - niedrigster Preis des Balkens;
SMA - einfacher gleitender Durchschnitt;
AO - Indikator [Awesome Oscillator](ao.md).

Die Parameter werden als Werte der SMA-Perioden festgelegt.

![IndicatorAcceleration](../../../../images/indicatoracceleration.png)

## Siehe auch

[Alligator](alligator.md)
