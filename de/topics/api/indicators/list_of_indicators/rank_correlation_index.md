# Rank Correlation Index

Der **Rank Correlation Index (RCI)** ist ein Oszillator, der auf dem Rangkorrelationskoeffizienten nach Spearman basiert. Es vergleicht die Preisklassen
Die Zeit rangiert innerhalb des Bewegungsfensters und zeigt an, wie nah die aktuelle Bewegung an einer perfekt steigenden oder fallenden Sequenz ist.

Verwenden Sie die Klasse [RankCorrelationIndex](xref:StockSharp.Algo.Indicators.RankCorrelationIndex), um auf den Indikator zuzugreifen.

## Berechnung

1. Weisen Sie jedem Datenpunkt innerhalb des **Length**-Fensters einen Zeitrang zu (1 für den ältesten Wert, `Length` für den aktuellsten).
2. Ordnen Sie die Preise nach ihrem Wert (1 für den niedrigsten Preis, `Length` für den höchsten).
3. Berechnen Sie die Differenz `d = RankTime − RankPrice` für jeden Balken.
4. Wenden Sie die Spearman-Formel an:  
   `RCI = 1 − (6 × Σ d²) / (Length × (Length² − 1))`.

Bei Multiplikation mit 100 liegt der Indikator zwischen −100 und +100.

## Parameter

- **Length** – Fenstergröße für das Ranking-Verfahren.

## Interpretation

- **RCI ≈ +100** – perfekt steigende Sequenz (starker Aufwärtstrend).
- **RCI ≈ −100** – perfekt fallende Sequenz (starker Abwärtstrend).
- **RCI um 0** – Zufalls- oder Seitwärtsmarkt.
- Abweichungen zwischen Preis und RCI warnen vor möglichen Umkehrungen.

Der Indikator ist hilfreich für die kurzfristige Trendbewertung und das Erkennen von Wendepunkten, insbesondere in Kombination mit Momentum-Tools.

![indicator_rank_correlation_index](../../../../images/indicator_rank_correlation_index.png)

## Siehe auch

[Momentum](momentum.md)
[ROC](roc.md)
[RSI](rsi.md)
