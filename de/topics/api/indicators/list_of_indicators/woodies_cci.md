# WCCI

**Woodies CCI (WCCI)** ist eine Modifikation des Standard-Commodity Channel Index (CCI), entwickelt vom Trader Ken Wood (bekannt als "Woodies"). Diese CCI-Variante enthält zusätzliche Glättung und wird als Teil eines umfassenden Woodies-CCI-Handelssystems verwendet.

Um den Indikator zu verwenden, nutzen Sie die Klasse [WoodiesCCI](xref:StockSharp.Algo.Indicators.WoodiesCCI).

## Beschreibung

Woodies CCI ist eine modifizierte Version des klassischen CCI-Indikators und enthält zwei Linien:
- Die Haupt-CCI-Linie mit einer gewählten Periode (typischerweise 14)
- Eine geglättete CCI-Linie, die ein einfacher gleitender Durchschnitt der Haupt-CCI-Linie ist

Das Woodies-CCI-System verwendet diese beiden Linien zusammen mit mehreren wichtigen Niveaus zur Erzeugung von Handelssignalen. Die wichtigsten Niveaus sind:
- +100 und -100 (traditionelle überkaufte und überverkaufte Niveaus)
- +200 und -200 (stark überkaufte und stark überverkaufte Bedingungen)
- Nulllinie (wichtig für die Trendbestimmung)

Wichtige Signale im Woodies-CCI-System:
- "Zero-line Reject" - wenn sich CCI der Nulllinie nähert und dann davon abprallt, wobei die vorherige Richtung fortgesetzt wird
- "Trend-line Break" - wenn CCI eine wichtige Trendlinie durchbricht
- "umgekehrte Divergenz" - eine spezielle Art der Divergenz zwischen Preis und CCI

## Parameter

- **Length** - Berechnungsperiode für die Haupt-CCI-Linie (typischerweise 14)
- **SMALength** - Periode zur Glättung der Haupt-CCI-Linie, um die zweite Linie zu erhalten (typischerweise 9)

## Berechnung

Woodies CCI wird in mehreren Schritten berechnet:

1. Zuerst wird der Standard-CCI berechnet:
   ```
   Typischer Preis (TP) = (High + Low + Close) / 3
   Durchschnittswert (SMA) = SMA(TP, Length)
   Mittlere Abweichung (MD) = Sum(|TP - SMA|) / Length
   CCI = (TP - SMA) / (0.015 * MD)
   ```

2. Danach wird die geglättete CCI-Linie berechnet:
   ```
   Smooth CCI = SMA(CCI, SMALength)
   ```

Woodies CCI verwendet die Kombination dieser beiden Linien, um Handelssignale zu erzeugen. Im klassischen Woodies-System bilden das Kreuzen dieser Linien, ihre Interaktion mit Schlüsselniveaus und verschiedene Muster die Grundlage für Handelsentscheidungen.

![IndicatorWoodiesCCI](../../../../images/indicator_woodies_cci.png)

## Siehe auch

[CCI](cci.md)

