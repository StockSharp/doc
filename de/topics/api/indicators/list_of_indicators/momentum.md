# Momentum

Der **Momentum**-Indikator misst das Ausmaß der Preisänderung eines Finanzinstruments über einen bestimmten Zeitraum. Es zeigt überkaufte und überverkaufte Momente an, in denen die Kurve Höchst- oder Tiefstwerte erreicht. Das Hinzufügen eines geglätteten gleitenden Durchschnitts zum Indikator verbessert die Interpretation von Trendänderungen.

Um den Indikator zu verwenden, sollte die Klasse [Momentum](xref:StockSharp.Algo.Indicators.Momentum) verwendet werden.
##### Berechnung
  
Momentum ist definiert als das Verhältnis des heutigen Preises zum Preis vor n Perioden:
 
MOMENTUM = CLOSE(i) / CLOSE(i - n) * 100  

Dabei gilt:
CLOSE(i) – der Schlusskurs des aktuellen Balkens;  
CLOSE(i - n) – der Schlusskurs von n Balken zurück.


![IndicatorMomentum](../../../../images/indicatormomentum.png)

## Siehe auch

[Money Flow Index](money_flow_index.md)
