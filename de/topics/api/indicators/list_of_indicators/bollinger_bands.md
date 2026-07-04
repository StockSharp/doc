# Bollinger Bands

**Bollinger Bands** sind ein oszillierender Indikator zur Messung der Marktvolatilität. Er ermöglicht die Einschätzung, ob der Preis im Vergleich zum gleitenden Durchschnitt hoch oder niedrig ist. Das mittlere Band entspricht dem einfachen gleitenden Durchschnitt des Preises. Die oberen und unteren Bänder sind Niveaus, auf denen der Preis relativ zum gleitenden Durchschnitt als hoch oder niedrig betrachtet werden kann.

Zur Verwendung des Indikators sollte die Klasse [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands) verwendet werden.
##### Berechnung
  
Zur Berechnung der Bollinger Bands werden die folgenden Parameter mit entsprechenden Einstellungen verwendet:  
- Typ der Standardabweichung - normalerweise double;  
- Periode des Moving Average - nach Ermessen des Traders.  

Der Indikator besteht somit aus drei Linien: mittlere, obere und untere Linie, jeweils mit eigener Formel:  
  
Middle Line (ML) = Moving Average (SMA (Close, N))  
Upper Band = ML + (D x Standard Deviation)  
Lower Band = ML - (D x Standard Deviation), wobei  
  
D - die in den Einstellungen festgelegte Kanalbreite, Standard Deviation (StdDev) - Standardabweichung, berechnet mit der Formel: SQRT(Sum(Close, n))^2, n)/n), wobei  
Sum - Summe über n Perioden, n - Berechnungsperiode, SQRT - Quadratwurzel, Close - Schlusskurs.  

![IndicatorBollingerBands](../../../../images/indicatorbollingerbands.png)

## Siehe auch

[CHV](chv.md)
