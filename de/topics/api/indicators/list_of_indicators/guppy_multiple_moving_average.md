# GMMA

**Mehrfacher gleitender Durchschnitt nach Guppy (GMMA)** ist ein von Daryl Guppy entwickelter technischer Indikator, der zwei Gruppen exponentieller gleitender Durchschnitte (EMA) verwendet, um die Interaktion zwischen kurzfristigen Händlern und langfristigen Anlegern aufzuzeigen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [GuppyMultipleMovingAverage](xref:StockSharp.Algo.Indicators.GuppyMultipleMovingAverage) verwenden.

## Beschreibung

Der mehrfache gleitende Durchschnitt nach Guppy (GMMA) besteht aus zwei Gruppen exponentieller gleitender Durchschnitte (EMA):
1. **Kurzfristige Gruppe** (typischerweise 3, 5, 8, 10, 12 und 15 Perioden) – repräsentiert die Aktivität kurzfristiger Händler
2. **Langfristige Gruppe** (typischerweise 30, 35, 40, 45, 50 und 60 Perioden) – stellt die Aktivität langfristiger Anleger dar

GMMA ermöglicht die Visualisierung der Interaktion zwischen diesen beiden Marktteilnehmergruppen und ermittelt, ob sich der Markt in einem Trend- oder Konsolidierungszustand befindet. Der Indikator hilft auch dabei, Momente zu erkennen, in denen kurzfristig orientierte Händler beginnen, der gleichen Richtung zu folgen wie langfristig orientierte Anleger, was häufig auf die Bildung oder Verstärkung eines Trends hinweist.

GMMA ist besonders nützlich für:
- Bestimmung der Stärke und Richtung des aktuellen Trends
- Identifizierung potenzieller Trendumkehrungen
- Erkennen, wann der Markt von einer Konsolidierung in einen Trend übergeht
- Ermittlung optimaler Einstiegspunkte in einen bestehenden Trend

## Berechnung

Bei der GMMA-Berechnung werden zwei Gruppen exponentieller gleitender Durchschnitte berechnet:

1. Kurzfristige EMA-Gruppe:
   ```
   EMA_3 = EMA(Price, 3)
   EMA_5 = EMA(Price, 5)
   EMA_8 = EMA(Price, 8)
   EMA_10 = EMA(Price, 10)
   EMA_12 = EMA(Price, 12)
   EMA_15 = EMA(Price, 15)
   ```

2. Langzeit-EMA-Gruppe:
   ```
   EMA_30 = EMA(Price, 30)
   EMA_35 = EMA(Price, 35)
   EMA_40 = EMA(Price, 40)
   EMA_45 = EMA(Price, 45)
   EMA_50 = EMA(Price, 50)
   EMA_60 = EMA(Price, 60)
   ```

Dabei gilt:
- EMA – exponentieller gleitender Durchschnitt
- Price - Preis (normalerweise Schlusskurs)

## Interpretation

Bei der GMMA-Interpretation werden sowohl einzelne Gruppen als auch deren Interaktion analysiert:

1. **Gruppenpositionierung**:
   - Wenn die kurzfristige Gruppe über der langfristigen Gruppe liegt, deutet dies auf einen Aufwärtstrend hin
   - Wenn die kurzfristige Gruppe unter der langfristigen Gruppe liegt, deutet dies auf einen Abwärtstrend hin

2. **Abstand zwischen Gruppen**:
   - Ein großer Abstand zwischen den Gruppen weist auf einen starken Trend hin
   - Kleine Distanzen oder Gruppenkreuzungen weisen auf einen schwachen Trend oder eine Konsolidierung hin

3. **Komprimierung und Erweiterung**:
   - Kompression (Konvergenz) von Linien innerhalb einer Gruppe weist auf Unsicherheit und mögliche Konsolidierung hin
   - Die Ausdehnung (Divergenz) der Linien innerhalb einer Gruppe weist auf eine Trendverstärkung hin

4. **Überkreuzungen**:
   - Kurzfristige Gruppe kreuzt die langfristige Gruppe von unten nach oben – starkes bullisches Signal
   - Kurzfristige Gruppe kreuzt die langfristige Gruppe von oben nach unten – starkes rückläufiges Signal

5. **Richtungsänderungen**:
   - Wenn die langfristige Gruppe beginnt, die Richtung zu ändern, deutet dies auf einen deutlichen Wandel in der Stimmung der langfristigen Anleger hin
   - Eine kurzfristige Gruppenumkehr ohne Veränderungen in der langfristigen Gruppe deutet oft auf eine vorübergehende Korrektur hin

6. **Optimale Einstiegspunkte**:
   - Nach einer starken Expansion kann es zu einer Kompression kommen, was auf eine Korrektur innerhalb des Trends hinweist
   - Das Ende einer solchen Kompression (neue Expansion) kann ein guter Einstiegspunkt in Richtung des Haupttrends sein

7. **Frühzeitige Umkehrwarnung**:
   - Kurzfristige Durchschnittswerte ändern zuerst die Richtung, dann beginnen sich Änderungen in den langfristigen Durchschnittswerten zu zeigen
   - Crossover zwischen Gruppen kann als Bestätigung einer Trendumkehr dienen

![GMMA Diagramm](../../../../images/indicator_guppy_multiple_moving_average.png)

## Siehe auch

[EMA](ema.md)
[MovingAverageRibbon](moving_average_ribbon.md)
[RainbowCharts](rainbow_charts.md)
[MACD](macd.md)
