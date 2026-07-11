# TWAP

**Zeitgewichteter Durchschnittspreis (TWAP)** ist ein Indikator, der den zeitgewichteten Durchschnittspreis eines Finanzinstruments über einen bestimmten Zeitraum berechnet. TWAP wird häufig von institutionellen Anlegern verwendet, um große Orders mit möglichst geringer Marktbeeinflussung auszuführen.

Um den Indikator zu verwenden, nutzen Sie die Klasse [TimeWeightedAveragePrice](xref:StockSharp.Algo.Indicators.TimeWeightedAveragePrice).

## Beschreibung

TWAP ist einer der gängigsten Algorithmen zur Orderausführung. Er teilt eine große Order in eine Reihe kleinerer Orders auf, die gleichmäßig über die Zeit verteilt werden. Ziel von TWAP ist es, über ein bestimmtes Zeitintervall einen Durchschnittspreis zu erzielen und dabei die Marktbeeinflussung zu minimieren.

Wichtigste Anwendungsfälle von TWAP:
- Benchmark-Preis zur Bewertung der Qualität der Orderausführung
- Ausführungsalgorithmus zur Minimierung der Marktbeeinflussung
- Werkzeug für Marktanalyse und Handelsentscheidungen

Im Unterschied zu VWAP (volumengewichteter Durchschnittspreis) berücksichtigt TWAP keine Handelsvolumina, sondern konzentriert sich ausschließlich auf den Zeitaspekt.

## Berechnung

Die TWAP-Berechnung erfolgt, indem Preise in gleichen Zeitabständen summiert und durch die Anzahl der Zeitintervalle geteilt werden:

```
TWAP = (P1 + P2 + P3 + ... + Pn) / n
```

wobei gilt:
- P1, P2, ..., Pn - Preise zu aufeinanderfolgenden Zeitpunkten
- n - Anzahl der Zeitintervalle

In der praktischen Umsetzung werden am häufigsten typische Preise für jede Periode (Candle) verwendet:

```
Typischer Preis = (High + Low + Close) / 3
TWAP = Sum(typischer Preis) / Anzahl der Perioden
```

Zur Bestimmung des aktuellen TWAP-Werts in Echtzeit kann auch eine rekursive Formel verwendet werden:

```
TWAP(current) = (TWAP(previous) * (n-1) + P(current)) / n
```

wobei n die Anzahl der Beobachtungen im TWAP-Fenster ist.

![TWAP Diagramm](../../../../images/indicator_time_weighted_average_price.png)
