# BOP

**Balance of Power (BOP)** ist ein Indikator, der die Stärke der Bullen (Käufer) im Verhältnis zu den Bären (Verkäufern) misst, indem er bewertet, wie gut die Bullen den Preis vom Tief zum Hoch anheben können.

Zur Verwendung des Indikators müssen Sie die Klasse [BalanceOfPower](xref:StockSharp.Algo.Indicators.BalanceOfPower) verwenden.

## Beschreibung

Der Balance-of-Power-Indikator (BOP) zeigt das Kräfteverhältnis zwischen Käufern und Verkäufern im Markt. Er basiert auf der Annahme, dass Käufer (Bullen) oder Verkäufer (Bären) in einem Trend den Preis während der Sitzung kontrollieren können. Durch den Vergleich der Differenz zwischen Schluss- und Eröffnungskurs mit der gesamten Preisspanne (High-Low) lässt sich beurteilen, wer den Markt aktuell dominiert.

BOP hilft Tradern:
- Richtung und Stärke des aktuellen Trends zu bestimmen
- Potenzielle Umkehrpunkte zu erkennen
- Divergenzen zwischen Preis und Indikator zu erkennen
- Überkaufte und überverkaufte Niveaus zu bestimmen

## Berechnung

Die Formel zur Berechnung des Balance-of-Power-Indikators (BOP) ist recht einfach:

```
BOP = (Close - Open) / (High - Low)
```

Wobei:
- Close - Schlusskurs
- Open - Eröffnungskurs
- High - höchster Preis der Periode
- Low - niedrigster Preis der Periode

Wenn (High - Low) null ist, wird BOP auf null gesetzt, um eine Division durch null zu vermeiden.

BOP wird häufig zusätzlich mit einem gleitenden Durchschnitt geglättet, um Volatilität zu reduzieren und Signale besser lesbar zu machen.

## Interpretation

- **Positive BOP-Werte** (über null) zeigen an, dass Käufer (Bullen) den Markt kontrollieren, was auf einen Aufwärtstrend hindeuten kann.
- **Negative BOP-Werte** (unter null) zeigen an, dass Verkäufer (Bären) den Markt kontrollieren, was auf einen Abwärtstrend hindeuten kann.
- **Kreuzung der Nulllinie** kann als Signal für eine mögliche Änderung der Trendrichtung betrachtet werden.
- **Extremwerte** (stark positiv oder stark negativ) können auf überkaufte oder überverkaufte Marktbedingungen hinweisen.
- **Divergenzen** zwischen BOP und Preis können eine mögliche Trendumkehr signalisieren:
  - Wenn der Preis steigt und BOP fällt, kann dies vor einer Abschwächung des Aufwärtstrends warnen.
  - Wenn der Preis fällt und BOP steigt, kann dies auf ein mögliches Ende des Abwärtstrends hindeuten.

![indicator_balance_of_power](../../../../images/indicator_balance_of_power.png)

## Siehe auch

[BalanceOfMarketPower](balance_of_market_power.md)
[ForceIndex](force_index.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
