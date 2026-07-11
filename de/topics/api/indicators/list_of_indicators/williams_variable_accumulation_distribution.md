# WVAD

**variable Williams-Akkumulation/Distribution (WVAD)** ist ein von Larry Williams entwickelter kumulativer Volumenindikator. Er bewertet Kauf- und Verkaufsdruck, indem er das Verhältnis zwischen Eröffnungskurs, Schlusskurs, Hoch, Tief und Handelsvolumen analysiert.

Um den Indikator zu verwenden, nutzen Sie die Klasse [WilliamsVariableAccumulationDistribution](xref:StockSharp.Algo.Indicators.WilliamsVariableAccumulationDistribution).

## Beschreibung

Der WVAD-Indikator misst, in welchem Umfang Käufer oder Verkäufer die Preisbewegung innerhalb jedes Balkens kontrollieren, und gewichtet diesen Wert mit dem Volumen. Liegt der Schlusskurs über dem Eröffnungskurs, weist dies auf Käuferdominanz hin und umgekehrt. Die Hoch-Tief-Spanne dient als Normalisierungsfaktor.

Wichtige Anwendungsfälle des Indikators:
- Bestätigung des aktuellen Trends
- Identifikation von Divergenzen zwischen Indikator und Preis
- Bestimmung von Kauf- oder Verkaufsdruck
- Bewertung der Stärke der Preisbewegung unter Berücksichtigung des Volumens

## Berechnung

Der WVAD-Indikator wird mit folgender Formel berechnet:

```
WVAD = WVAD(previous) + ((Close - Open) / (High - Low)) * Volume
```

wobei gilt:
- Close - Schlusskurs der aktuellen Periode
- Open - Eröffnungskurs der aktuellen Periode
- High - höchster Preis der aktuellen Periode
- Low - niedrigster Preis der aktuellen Periode
- Volume - Handelsvolumen der aktuellen Periode
- WVAD(previous) - vorheriger Indikatorwert

Wenn High = Low ist (die Spanne ist null), wird der Wert für diese Periode nicht addiert.

Der Indikator ist kumulativ - die Werte werden mit jeder neuen Periode aufsummiert.

## Siehe auch

[WAD](williams_accumulation_distribution.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)

