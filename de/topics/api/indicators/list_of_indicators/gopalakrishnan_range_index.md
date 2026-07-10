# GAPO

**Gopalakrishnan Range Index (GAPO)** ist ein technischer Indikator, der von Tushar Gopalakrishnan entwickelt wurde, um die Marktvolatilität mithilfe einer logarithmischen Skala zu messen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [GopalakrishnanRangeIndex](xref:StockSharp.Algo.Indicators.GopalakrishnanRangeIndex) verwenden.

## Beschreibung

Der Gopalakrishnan Range Index (GAPO) ist ein Volatilitätsindikator, der eine logarithmische Skala verwendet, um die gesamte Preisspanne über einen bestimmten Zeitraum zu messen. Es wurde von Tushar Gopalakrishnan entwickelt und in der Zeitschrift „Technical Analysis of Stocks & Commodities“ vorgestellt.

GAPO bewertet extreme Marktbewegungen, indem es das logarithmische Verhältnis zwischen den Höchst- und Tiefstpreisen über einen bestimmten Zeitraum misst. Durch diesen Ansatz kann der Indikator die erhöhte Volatilität genauer widerspiegeln, insbesondere in Zeiten starker Preisbewegungen.

Der GAPO-Indikator ist besonders nützlich für:
- Identifizieren von Perioden hoher und niedriger Volatilität
- Erkennen möglicher Umkehrpunkte nach extremen Bewegungen
- Anpassung von Parametern für andere volatilitätsbasierte Indikatoren
- Anpassung der Handelsstrategien an die aktuellen Marktbedingungen

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Berechnungszeitraum (Standardwert: 10)

## Berechnung

Die Gopalakrishnan Range Index-Berechnung ist ganz einfach:

```
GAPO = log(N) * log(Highest High - Lowest Low)
```

Dabei gilt:
- log - natürlicher Logarithmus
- N - Anzahl der Perioden (Length)
- Höchstes High – höchstes Hoch im Length-Zeitraum
- Niedrigster Low – niedrigster Tiefststand im Length-Zeitraum

## Interpretation

Der Gopalakrishnan Range Index kann wie folgt interpretiert werden:

1. **Absolute Werte**:
   - High GAPO-Werte weisen auf Perioden hoher Volatilität hin
   - Niedrige GAPO-Werte weisen auf Perioden geringer Volatilität hin
   - Extrem hohe Werte können auf eine mögliche Überdehnung des Marktes und eine mögliche Umkehr hinweisen

2. **GAPO-Trends**:
   - Steigende GAPO-Werte deuten auf eine zunehmende Volatilität hin
   - Sinkende GAPO-Werte weisen auf eine abnehmende Volatilität hin
   - Ein starker GAPO-Sprung könnte den Beginn einer neuen Trendbewegung signalisieren

3. **Relative Ebenen**:
   - Der Vergleich des aktuellen GAPO-Werts mit seinen historischen Werten ermöglicht die Beurteilung der relativen Volatilität
   - Werte über dem 95. Perzentil des historischen Bereichs können auf extreme Volatilität hinweisen
   - Werte unterhalb des 5. Perzentils des historischen Bereichs können auf eine ungewöhnlich niedrige Volatilität hinweisen

4. **Handelsstrategien**:
   - In Zeiten hoher Volatilität (hohe GAPO-Werte) kann es angebracht sein, den Stop-Loss und die angestrebten Gewinngrößen zu erhöhen
   - In Zeiten geringer Volatilität (niedrige GAPO-Werte) können Range-Trading-Strategien besser geeignet sein
   - Extreme GAPO-Werte können als gegensätzliche Indikatoren zur Suche nach Umkehrpunkten verwendet werden

5. **Kombination mit anderen Indikatoren**:
   - GAPO kann zum Filtern von Signalen anderer Indikatoren verwendet werden
   - In Zeiten hoher Volatilität können Trendindikatorsignale zuverlässiger sein
   - In Zeiten geringer Volatilität können Oszillatorsignale effektiver sein

![indicator_gopalakrishnan_range_index](../../../../images/indicator_gopalakrishnan_range_index.png)

## Siehe auch

[ATR](atr.md)
[ChoppinessIndex](choppiness_index.md)
[Wahre Spanne](true_range.md)
