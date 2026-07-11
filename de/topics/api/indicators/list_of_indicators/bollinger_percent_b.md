# BBP

**Bollinger-Prozent B (BBP)** ist ein von John Bollinger entwickelter Indikator als Ergänzung zum Bollinger-Bands-Indikator. BBP zeigt die Lage des Preises relativ zum oberen und unteren Bollinger-Band.

Zur Verwendung des Indikators müssen Sie die Klasse [BollingerPercentB](xref:StockSharp.Algo.Indicators.BollingerPercentB) verwenden.

## Beschreibung

Der Bollinger-Percent-B-Indikator bestimmt die Preisposition relativ zum oberen und unteren Bollinger-Band als Prozentwert von 0 bis 1 (oder von 0 % bis 100 %). Dadurch kann die Preisposition im Kontext der Bollinger-Bänder genauer bestimmt werden:

- Ein Wert von 1 (oder 100 %) bedeutet, dass der Preis am oberen Bollinger-Band liegt.
- Ein Wert von 0 (oder 0 %) bedeutet, dass der Preis am unteren Bollinger-Band liegt.
- Ein Wert von 0.5 (oder 50 %) bedeutet, dass der Preis am mittleren Bollinger-Band (SMA) liegt.

BBP kann auch Werte außerhalb des Bereichs 0-1 annehmen:
- Werte über 1 zeigen an, dass der Preis über dem oberen Bollinger-Band liegt.
- Werte unter 0 zeigen an, dass der Preis unter dem unteren Bollinger-Band liegt.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** - SMA-Berechnungsperiode (Standardwert: 20)
- **StdDevMultiplier** - Multiplikator der Standardabweichung zur Berechnung der Bollinger-Bänder (Standardwert: 2)

## Berechnung

Die Berechnung von Bollinger-Prozent B basiert auf der Formel:

```
BBP = (Price - Lower Bollinger-Band) / (Upper Bollinger-Band - Lower Bollinger-Band)
```

Wobei:
- Price - aktueller Preis (normalerweise Schlusskurs)
- Unteres Bollinger-Band = SMA - (StdDevMultiplier * Standardabweichung)
- Oberes Bollinger-Band = SMA + (StdDevMultiplier * Standardabweichung)
- SMA - einfacher gleitender Durchschnitt über die Length-Periode
- Standardabweichung - Standardabweichung des Preises über die Length-Periode

## Verwendung

Bollinger-Prozent B kann auf verschiedene Weise verwendet werden:

1. **Erkennen überkaufter/überverkaufter Bedingungen**:
   - Werte über 1 zeigen einen überkauften Markt an
   - Werte unter 0 zeigen einen überverkauften Markt an

2. **Umkehrsignale**:
   - Wenn BBP in den Bereich 0-1 zurückkehrt, nachdem er ihn verlassen hatte
   - Divergenzen zwischen BBP und Preis

3. **Trendbestimmung**:
   - BBP-Werte dauerhaft über 0.5 deuten auf einen Aufwärtstrend hin
   - BBP-Werte dauerhaft unter 0.5 deuten auf einen Abwärtstrend hin

4. **Finden versteckter Unterstützungs- und Widerstandsniveaus**:
   - Die Niveaus 0.8 und 0.2 werden häufig als zusätzliche Unterstützungs- und Widerstandsniveaus verwendet

![indicator_bollinger_percent_b](../../../../images/indicator_bollinger_percent_b.png)

## Siehe auch

[BollingerBands](bollinger_bands.md)
[StdDev](standard_deviation.md)
[RSI](rsi.md)
