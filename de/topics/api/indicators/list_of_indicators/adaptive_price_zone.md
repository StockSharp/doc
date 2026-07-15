# APZ

**adaptive Preiszone (APZ)** ist ein von Lee Leibfarth entwickelter technischer Indikator, der dynamische Unterstützungs- und Widerstandszonen bildet und sich an die Marktvolatilität anpasst.

Zur Verwendung des Indikators müssen Sie die Klasse [AdaptivePriceZone](xref:StockSharp.Algo.Indicators.AdaptivePriceZone) verwenden.

## Beschreibung

Der APZ-Indikator besteht aus zwei Linien (obere und untere), die eine Preiszone um den Durchschnittspreis bilden. Diese Zone dehnt sich je nach aktueller Marktvolatilität aus oder zieht sich zusammen. Wenn der Markt volatiler wird, erweitert sich die Zone; wenn die Volatilität abnimmt, verengt sie sich.

APZ ist besonders nützlich für:
- Erkennen potenzieller Unterstützungs- und Widerstandsniveaus
- Erkennen möglicher Trendumkehrpunkte
- Sichtbarmachen von Phasen erhöhter und verringerter Volatilität
- Erstellen von Handelssystemen auf Basis von Ausbrüchen aus Preiszonen

## Parameter

Der Indikator hat die folgenden Parameter:
- **Zeitraum** - Berechnungsperiode (Standardwert: 5)
- **Bandbreite in Prozent** - Prozentanteil der Spanne zur Definition der Bandbreite (Standardwert: 2 %)

## Berechnung

Die APZ-Berechnung basiert auf dem exponentiellen gleitenden Durchschnitt (EMA) und der durchschnittlichen wahren Spanne (ATR):

1. Zunächst wird der EMA des Preises für die angegebene Periode berechnet:
   ```
   EMA = exponentieller gleitender Durchschnitt des Preises über Period
   ```

2. Danach wird die Volatilität mit ATR berechnet:
   ```
   Volatilität = exponentieller gleitender Durchschnitt des ATR über Period
   ```

3. Die oberen und unteren APZ-Linien werden wie folgt berechnet:
   ```
   obere Linie = EMA + (Volatilität * BandPercentage)
   untere Linie = EMA - (Volatilität * BandPercentage)
   ```

Wenn der Preis über der oberen APZ-Linie liegt, kann dies als Aufwärtstrend betrachtet werden. Wenn der Preis unter der unteren APZ-Linie liegt, kann dies auf einen Abwärtstrend hindeuten. Wenn sich der Preis innerhalb der APZ-Zone bewegt, kann sich der Markt in einer Konsolidierungs- oder Seitwärtsphase befinden.

![APZ Diagramm](../../../../images/indicator_adaptive_price_zone.png)

## Siehe auch

[BollingerBands](bollinger_bands.md)
[KeltnerChannels](keltner_channels.md)
[DonchianChannels](donchian_channels.md)
