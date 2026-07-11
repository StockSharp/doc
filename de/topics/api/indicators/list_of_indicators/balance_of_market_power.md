# BMP

**Marktmacht-Balance (BMP)** ist ein Indikator, der die Stärke der Käufer im Verhältnis zu den Verkäufern misst, basierend auf einer Analyse von Preisbewegungen und Handelsvolumen.

Zur Verwendung des Indikators müssen Sie die Klasse [BalanceOfMarketPower](xref:StockSharp.Algo.Indicators.BalanceOfMarketPower) verwenden.

## Beschreibung

Der Balance-of-Market-Power-Indikator dient dazu, die aktuelle Kräfteverteilung zwischen Käufern und Verkäufern im Markt zu bewerten. Er analysiert, wie stark der Schlusskurs von seiner Spanne (Hoch-Tief) abweicht, und setzt dies in Beziehung zum Handelsvolumen.

BMP hilft Tradern:
- Die dominierende Marktseite zu bestimmen (Käufer oder Verkäufer)
- Potenzielle Trendumkehrungen zu erkennen
- Divergenzen zwischen Preis und Indikator zu erkennen
- Überkaufte und überverkaufte Niveaus zu finden

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** - Glättungsperiode (Standardwert: 14)

## Berechnung

Die BMP-Berechnung erfolgt in zwei Schritten:

1. Berechnung von BMP für jede einzelne Kerze:
   ```
   Roh-BMP = ((Schlusskurs - Eröffnungskurs) / (High - Low)) * Volume
   ```
   Wenn (High - Low) null ist, wird der rohe BMP auf null gesetzt.

2. Glättung von BMP mit einem einfachen gleitenden Durchschnitt (SMA):
   ```
   BMP = SMA(Roh-BMP, Length)
   ```

Wobei:
- Schlusskurs - Schlusskurs der aktuellen Kerze
- Eröffnungskurs - Eröffnungskurs der aktuellen Kerze
- High - höchster Preis der aktuellen Kerze
- Low - niedrigster Preis der aktuellen Kerze
- Volume - Handelsvolumen für die aktuelle Kerzenperiode
- Length - ausgewählte Glättungsperiode

## Interpretation

- **Positive BMP-Werte** zeigen die Dominanz der Käufer (Bullen) im Markt an
- **Negative BMP-Werte** zeigen die Dominanz der Verkäufer (Bären) im Markt an
- **Kreuzung der Nulllinie** kann als Signal für eine Trendänderung betrachtet werden
- **Extremwerte** (oberhalb oder unterhalb bestimmter Niveaus) können auf überkaufte oder überverkaufte Marktbedingungen hinweisen
- **Divergenzen** zwischen BMP und Preis können eine mögliche Trendumkehr signalisieren

![indicator_balance_of_market_power](../../../../images/indicator_balance_of_market_power.png)

## Siehe auch

[BalanceOfPower](balance_of_power.md)
[ForceIndex](force_index.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
