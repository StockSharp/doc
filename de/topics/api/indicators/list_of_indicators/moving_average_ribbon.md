# MAR

**Band gleitender Durchschnitte (MAR)** ist ein technischer Indikator, der mehrere gleitende Durchschnitte mit progressiv zunehmenden Perioden anzeigt, um die Stärke und Richtung des Trends zu visualisieren.

Um den Indikator verwenden zu können, müssen Sie die Klasse [MovingAverageRibbon](xref:StockSharp.Algo.Indicators.MovingAverageRibbon) verwenden.

## Beschreibung

Das Band gleitender Durchschnitte (MAR) ist eine Reihe mehrerer gleitender Durchschnitte, die in einer „Band“- oder „Fächer“-Formation auf einem Diagramm angezeigt werden. Dieser Indikator hilft Händlern, den aktuellen Trendzustand und seine Stärke intuitiver zu visualisieren als die Verwendung von ein oder zwei gleitenden Durchschnitten.

MAR umfasst mehrere gleitende Durchschnitte (normalerweise 5 bis 10) mit progressiv zunehmenden Perioden. Das Intervall zwischen den Perioden kann gleichmäßig (z. B. 10, 20, 30, 40 ...) oder exponentiell (z. B. 5, 10, 20, 40 ...) sein.

Der Grundgedanke besteht darin, dass die gegenseitige Positionierung und Form dieser gleitenden Durchschnitte wertvolle Informationen über den Zustand und die Stärke des aktuellen Trends liefern und dabei helfen können, potenzielle Umkehrpunkte zu identifizieren.

## Parameter

Der Indikator hat die folgenden Parameter:
- **ShortPeriod** – anfänglicher (minimaler) Zeitraum für gleitende Durchschnitte (Standardwert: 10)
- **LongPeriod** – letzter (maximaler) Zeitraum für gleitende Durchschnitte (Standardwert: 100)
- **RibbonCount** – Anzahl der gleitenden Durchschnitte im Band (Standardwert: 10)

## Berechnung

Die Berechnung des Bands gleitender Durchschnitte umfasst die folgenden Schritte:

1. Bestimmen Sie die Periodenfolge für gleitende Durchschnitte:
   ```
   Step = (LongPeriod - ShortPeriod) / (RibbonCount - 1)
   Periods = [ShortPeriod, ShortPeriod + Step, ShortPeriod + 2*Step, ..., LongPeriod]
   ```

2. Berechnen Sie den gleitenden Durchschnitt für jeden Zeitraum:
   ```
   MAs = [SMA(Preis, Periode) für jede Periode in Perioden]
   ```

Dabei gilt:
- Price - Preis (normalerweise Schlusskurs)
- SMA – einfacher gleitender Durchschnitt
- ShortPeriod – Anfangszeitraum
- LongPeriod – letzte Periode
- RibbonCount – Anzahl der gleitenden Durchschnitte

Hinweis: Anstelle von SMA können auch andere Arten von gleitenden Durchschnitten wie EMA (exponentieller gleitender Durchschnitt), WMA (gewichteter gleitender Durchschnitt) usw. verwendet werden.

## Interpretation

Das Band gleitender Durchschnitte kann wie folgt interpretiert werden:

1. **Gegenseitige Positionierung der gleitenden Durchschnitte**:
   - Wenn alle Linien in aufsteigender Reihenfolge der Perioden angeordnet sind (kürzeste oben, längste unten), deutet dies auf einen starken Aufwärtstrend hin
   - Wenn alle Linien in absteigender Reihenfolge der Perioden angeordnet sind (kürzeste unten, längste oben), deutet dies auf einen starken Abwärtstrend hin
   - Wenn sich Linien schneiden und keine klare Reihenfolge haben, deutet dies auf einen Seitwärtstrend oder eine Unsicherheit hin

2. **Bandform**:
   - Ein sich ausdehnendes Band (größerer Abstand zwischen den Linien) weist auf eine Trendverstärkung hin
   - Ein schrumpfendes Band (kleinerer Abstand zwischen den Linien) weist auf eine Abschwächung des Trends hin
   - Eine enge Gruppierung der Linien weist auf eine Konsolidierung oder das Fehlen eines ausgeprägten Trends hin

3. **Kreuzungen gleitender Durchschnitte**:
   - Der Beginn von Linienkreuzungen kann auf eine mögliche Trendänderung hinweisen
   - Wenn kurze gleitende Durchschnitte beginnen, länge gleitende Durchschnitte zu kreuzen, kann dies ein frühes Signal für eine Trendumkehr sein

4. **Bandneigungswinkel**:
   - Ein steiler Winkel weist auf einen starken Trend hin
   - Flacher Winkel weist auf einen schwachen Trend hin
   - Die horizontale Positionierung des Bandes weist auf einen Seitwärtstrend hin

5. **Preisposition relativ zum Band**:
   - Wenn der Preis über dem gesamten Band liegt, bestätigt dies einen starken Aufwärtstrend
   - Wenn der Preis unter dem gesamten Band liegt, bestätigt dies einen starken Abwärtstrend
   - Wenn sich der Preis innerhalb des Bandes bewegt, kann dies auf einen Übergangszustand oder eine Konsolidierung hinweisen

6. **Handelsstrategien**:
   - Geben Sie eine Position ein, an der der Preis vom Rand des Bandes in Trendrichtung abprallt
   - Verlassen Sie eine Position, wenn sich die gleitenden Durchschnitte in die entgegengesetzte Richtung zu kreuzen beginnen
   - Nutzen Sie die Bandbreite, um Stop-Losses und Take-Profits festzulegen

![MAR](../../../../images/indicator_moving_average_ribbon.png)

## Siehe auch

[SMA](sma.md)
[EMA](ema.md)
[MovingAverageCrossover](moving_average_crossover.md)
[GuppyMultipleMovingAverage](guppy_multiple_moving_average.md)
