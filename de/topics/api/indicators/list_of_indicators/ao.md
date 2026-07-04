# AO

Der **Awesome Oscillator (AO)** ist ein klassischer technischer Indikator, der durch Subtraktion gleitender Durchschnitte (SMA) mit unterschiedlichen Perioden konstruiert wird.

Zur Verwendung des Indikators sollte die Klasse [AwesomeOscillator](xref:StockSharp.Algo.Indicators.AwesomeOscillator) verwendet werden.
##### Berechnung

Das Histogramm des Awesome Oscillator ist ein 34-periodiger einfacher gleitender Durchschnitt, der auf den Mittelpunktwerten der Balken (H+L) / 2 aufgebaut ist, abgezogen von einem 5-periodigen einfachen gleitenden Durchschnitt auf denselben Mittelpunktwerten (H+L) / 2. Somit wird die langsame gleitende Durchschnittslinie von der schnellen abgezogen, um eine Vorstellung von der Stärke der Preisbewegung und ihren weiteren Absichten zu erhalten.

MEDIAN PRICE = (HIGH + LOW) / 2  
AO = SMA (MEDIAN PRICE, 5) - SMA (MEDIAN PRICE, 34), wobei  
  
MEDIAN PRICE - Medianpreis  
HIGH - höchster Preis des Balkens  
LOW - niedrigster Preis des Balkens  
SMA - einfacher gleitender Durchschnitt

Die Werte beziehen sich auf den klassischen Indikator; in den Einstellungen können jederzeit eigene Parameter angegeben werden.

![IndicatorAwesomeOscillator](../../../../images/indicatorawesomeoscillator.png)

## Siehe auch

[Bollinger Bands](bollinger_bands.md)
