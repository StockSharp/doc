# MI

**Mass Index (MI)** ist ein von Donald Dorsey entwickelter technischer Indikator, der potenzielle Trendumkehrungen durch die Analyse von Preisspannenausweitungen und -schrumpfungen identifiziert.

Um den Indikator verwenden zu können, müssen Sie die Klasse [MassIndex](xref:StockSharp.Algo.Indicators.MassIndex) verwenden.

## Beschreibung

Der Mass Index (MI) ist ein technisches Analysetool, das dabei hilft, potenzielle Trendumkehrungen zu erkennen, indem es Änderungen in der Preisspanne (Differenz zwischen Höchst- und Mindestpreisen) verfolgt. Der Indikator wurde von Donald Dorsey auf der Grundlage der Annahme entwickelt, dass Trendumkehrungen typischerweise eine Ausweitung und anschließende Verengung der Preisspanne vorausgehen.

MI misst die Volatilität anhand exponentieller gleitender Durchschnitte (EMA) der Preisspanne. Es sagt nicht die Umkehrrichtung voraus, sondern nur deren Wahrscheinlichkeit. Aus diesem Grund wird MI häufig in Verbindung mit anderen Richtungsanzeigern verwendet.

Das Grundkonzept besteht darin, dass die Wahrscheinlichkeit einer aktuellen Trendumkehr steigt, wenn der Massenindex einen bestimmten Schwellenwert erreicht und dann unter diesen Wert fällt.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Hauptberechnungszeitraum (Standardwert: 25)
- **EmaLength** – Zeitraum für Preisspanne EMA (Standardwert: 9)

## Berechnung

Die Mass Index-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie den High-Low-Bereich für jeden Zeitraum:
   ```
   Range = High - Low
   ```

2. Berechnen Sie den 9-Perioden-EMA des Bereichs:
   ```
   EMA1 = EMA(Range, EmaLength)
   ```

3. Berechnen Sie den 9-Perioden-EMA des EMA des Bereichs:
   ```
   EMA2 = EMA(EMA1, EmaLength)
   ```

4. Berechnen Sie das Verhältnis:
   ```
   Ratio = EMA1 / EMA2
   ```

5. Sum-Verhältnisse über 25 Perioden:
   ```
   MI = Sum(Ratio über die letzten Length-Perioden)
   ```

Dabei gilt:
- High – Höchster Preis des Zeitraums
- Low – niedrigster Preis des Zeitraums
- EMA – exponentieller gleitender Durchschnitt
- Length - Summationsperiode (normalerweise 25)
- EmaLength - EMA-Periode (normalerweise 9)

## Interpretation

Der Mass Index wird wie folgt interpretiert:

1. **„Umkehrbuckel“**:
   - Das klassische „Umkehrbuckel“-Signal entsteht, wenn der Massenindex über 27 steigt und dann unter 26,5 fällt
   - Dieses Muster weist auf eine mögliche Trendumkehr hin, sagt jedoch nicht deren Richtung voraus

2. **Indexniveaus**:
   - Werte über 27 deuten auf eine Ausweitung der Preisspanne und eine erhöhte Volatilität hin
   - Hohen Werten, denen ein Rückgang folgt, kann eine Trendumkehr vorausgehen
   - Low-Werte (unter 20) weisen auf eine Verengung der Preisspanne und eine verringerte Volatilität hin

3. **Trendrichtung**:
   - Der Massenindex zeigt weder die Trendrichtung noch deren Umkehr an
   - Zur Richtungsbestimmung sind zusätzliche Indikatoren oder Analysemethoden erforderlich (z. B. gleitende Durchschnitte oder Unterstützungs-/Widerstandsniveaus)

4. **Abweichungen**:
   - Divergenzen zwischen Preis und Massenindex sind weniger signifikant als das „Umkehrbuckel“-Muster
   - Allerdings könnten Diskrepanzen zwischen den neuen Höchst-/Tiefstständen der Preise und den rückläufigen Höchst-/Tiefstständen des Massenindex auf eine Trendabschwächung hinweisen

5. **Kombination mit anderen Indikatoren**:
   - Der Massenindex funktioniert am besten in Kombination mit Trendrichtungsindikatoren
   - Beliebte Kombinationen sind EMA, MACD oder RSI zur Bestimmung der möglichen Umkehrrichtung

6. **Volatilitätsänderungen**:
   - Ein starker Anstieg des Massenindex deutet auf eine deutliche Ausweitung der Preisspanne hin, die einer starken Bewegung vorausgehen könnte
   - Ein allmählicher Rückgang des Index deutet auf eine Verengung der Spanne und eine mögliche Konsolidierung hin

7. **Parameteroptimierung**:
   - Standardparameter (9 für EMA, 25 für Summierung) funktionieren in den meisten Zeitrahmen gut
   - Eine Verkürzung der Perioden kann zu schnelleren Signalen führen, kann aber auch zu einer Zunahme falscher Signale führen

![indicator_mass_index](../../../../images/indicator_mass_index.png)

## Siehe auch

[ATR](atr.md)
[BollingerBands](bollinger_bands.md)
[ChoppinessIndex](choppiness_index.md)
[TrueRange](true_range.md)
