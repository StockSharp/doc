# AFI

**Bestätigungsflussindex (AFI)** ist ein Indikator, der die Trendstärke anhand der Beziehung zwischen Volumen und Preisbewegung misst.

Zur Verwendung des Indikators müssen Sie die Klasse [ApprovalFlowIndex](xref:StockSharp.Algo.Indicators.ApprovalFlowIndex) verwenden.

## Beschreibung

Der Bestätigungsflussindex (AFI) hilft, die Intensität des Orderflows im Markt einzuschätzen und die Stärke des aktuellen Trends zu bestimmen. Dieser Indikator analysiert die Beziehung zwischen Handelsvolumen und Preisbewegung, um potenzielle Umkehrpunkte zu erkennen oder eine Trendfortsetzung zu bestätigen.

Der AFI-Indikator kann verwendet werden für:
- Bestimmen der Stärke des aktuellen Trends
- Erkennen von Divergenzen zwischen Preis und Indikator
- Suchen potenzieller Marktumkehrpunkte

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** - Berechnungsperiode des Indikators

## Berechnung

Die Berechnung des Bestätigungsflussindex basiert auf der Analyse von Preisänderung und Volumen über eine bestimmte Periode:

1. Zunächst wird die Preisänderung für die Periode berechnet
2. Danach wird diese Änderung zum Handelsvolumen in Beziehung gesetzt
3. Die resultierenden Werte werden über die ausgewählte Periode summiert (Parameter Length)

AFI soll bestimmen, in welchem Umfang das Handelsvolumen die Preisbewegung "bestätigt".

Positive AFI-Werte weisen auf einen starken Aufwärtstrend hin, während negative Werte auf einen Abwärtstrend hindeuten. Werte nahe null können auf das Fehlen eines ausgeprägten Trends hinweisen.

![AFI](../../../../images/indicator_approval_flow_index.png)

## Siehe auch

[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
