# WAD

**Williams-Akkumulation/-Distribution (WAD)** ist ein von Larry Williams entwickelter Volumenindikator. Anders als die klassische Akkumulations-/Distributionslinie konzentriert sich WAD auf das Verhältnis zwischen dem Schlusskurs der aktuellen Periode und dem Schlusskurs der vorherigen Periode, um Kauf- oder Verkaufsdruck zu bestimmen.

Um den Indikator zu verwenden, nutzen Sie die Klasse [WilliamsAccumulationDistribution](xref:StockSharp.Algo.Indicators.WilliamsAccumulationDistribution).

## Beschreibung

Der Williams-Akkumulation/-Distribution-Indikator dient dazu, Abweichungen zwischen Preis und Volumen zu identifizieren, die auf mögliche Trendumkehrungen hinweisen können. WAD ist besonders nützlich, um Schwäche in der aktuellen Preisbewegung sichtbar zu machen.

Wichtige Eigenschaften von WAD:
- Positive Werte zeigen Akkumulation an (Kaufdruck)
- Negative Werte zeigen Distribution an (Verkaufsdruck)
- Divergenzen zwischen WAD und Preis können Preisumkehrungen vorausgehen

Wichtige Anwendungsfälle des Indikators:
- Bestätigung des aktuellen Trends
- Identifikation möglicher Preisumkehrungen
- Bestimmung von Kauf- oder Verkaufsdruck

## Berechnung

Der Williams-Akkumulation/-Distribution-Indikator wird nach folgender Logik berechnet:

1. Schutz der wahren Spanne (TRP) für die aktuelle Periode bestimmen:
   ```
   TRP = Max(High - Low, |High - Close_prev|, |Low - Close_prev|)
   ```

2. Accumulation/Distribution-Wert (AD) für die aktuelle Periode berechnen:
   - Wenn Close > Close_prev (steigender Markt):
      ```
      AD = Close - Min(Low, Close_prev)
      ```
   - Wenn Close < Close_prev (fallender Markt):
      ```
      AD = Close - Max(High, Close_prev)
      ```
   - Wenn Close = Close_prev:
      ```
      AD = 0
      ```

3. WAD-Wert durch Akkumulation der AD-Werte berechnen:
   ```
   WAD = vorheriger WAD-Wert + AD
   ```

Der Indikator akkumuliert positive und negative Werte und bildet eine kumulative Linie, die mit der Preisbewegung verglichen werden kann.

![IndicatorWilliamsAccumulationDistribution](../../../../images/indicator_williams_accumulation_distribution.png)

## Siehe auch

[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)

