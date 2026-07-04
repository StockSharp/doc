# EIS

**Elder Impulse System (EIS)** ist ein von Dr. Alexander Elder entwickelter technischer Indikator, der einen Trendindikator und einen Momentumoszillator kombiniert, um die Richtung und Stärke der Marktbewegung zu bestimmen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [ElderImpulseSystem](xref:StockSharp.Algo.Indicators.ElderImpulseSystem) verwenden.

## Beschreibung

Der Elder Impulse System (EIS) ist ein einfaches, aber leistungsstarkes Tool zur Visualisierung der Marktdynamik. Es kombiniert zwei Indikatoren:
1. **Exponentielle Bewegung Average (EMA)** – zur Bestimmung der Trendrichtung
2. **MACD-Histogramm** – zur Messung der Stärke und Dynamik der Preisbewegung

EIS klassifiziert jede Kerze im Preisdiagramm in eine von drei Kategorien (normalerweise durch unterschiedliche Farben gekennzeichnet):
- **Grün (starker Aufwärtsimpuls)** – wenn beide Indikatoren steigen
- **Rot (starker rückläufiger Impuls)** – wenn beide Indikatoren fallen
- **Blau oder neutral (kein klarer Impuls)** – wenn sich die Indikatoren in entgegengesetzte Richtungen bewegen

EIS ist besonders nützlich für:
- Trendrichtung und -stärke schnell visuell bestimmen
- Identifizieren von Ein- und Ausstiegspunkten in Richtung des Haupttrends
- Identifizieren potenzieller Umkehrpunkte
- Filterung falscher Signale

## Berechnung

Die Elder Impulse System-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie den exponentiellen gleitenden Durchschnitt über 13 Perioden (EMA):
   ```
   EMA = EMA(Close, 13)
   ```

2. Berechnen Sie das MACD-Histogramm (Standardwerte: 12, 26, 9):
   ```
   MACD Line = EMA(Close, 12) - EMA(Close, 26)
   Signal Line = EMA(MACD Line, 9)
   MACD Histogram = MACD Line - Signal Line
   ```

3. Bestimmen Sie die Farbklassifizierung für die aktuelle Kerze:
   ```
   If EMA[current] > EMA[previous] AND MACD Histogram[current] > MACD Histogram[previous], then Green (Bullish Impulse)
   If EMA[current] < EMA[previous] AND MACD Histogram[current] < MACD Histogram[previous], then Red (Bearish Impulse)
   Otherwise Blue (No Impulse)
   ```

## Interpretation

Der Elder Impulse System wird wie folgt interpretiert:

1. **Grüne Kerzen (starker bullischer Impuls)**:
   - Zeigen Sie eine starke Aufwärtsdynamik an
   - Beste Zeit zum Kaufen oder Halten von Long-Positionen
   - Eine Reihe grüner Kerzen weist auf einen starken Aufwärtstrend hin

2. **Rote Kerzen (starker rückläufiger Impuls)**:
   - Zeigen Sie eine starke Abwärtsdynamik an
   - Beste Zeit zum Verkaufen oder Halten von Short-Positionen
   - Eine Reihe roter Kerzen weist auf einen starken Abwärtstrend hin

3. **Blaue Kerzen (kein klarer Impuls)**:
   - Zeigen Sie Unsicherheit oder Konsolidierung an
   - Signalisieren Sie eine mögliche Verlangsamung oder Trendumkehr
   - Tritt häufig während Konsolidierungsperioden oder vor einer Trendwende auf

4. **Handelsstrategien**:
   - Kaufen Sie, wenn Kerzen ihre Farbe von Blau nach Grün ändern
   - Verkaufen, wenn die Kerzen ihre Farbe von Blau nach Rot ändern
   - Close Long-Positionen, wenn Kerzen ihre Farbe von Grün in eine andere ändern
   - Close-Short-Positionen, wenn Kerzen ihre Farbe von Rot zu einer anderen ändern

5. **Trendbestätigung**:
   - Eine Reihe grüner Kerzen bestätigt einen Aufwärtstrend
   - Eine Reihe roter Kerzen bestätigt einen Abwärtstrend
   - Farbwechsel weisen auf einen Seitwärtstrend oder eine Unsicherheit hin

![indicator_elder_impulse_system](../../../../images/indicator_elder_impulse_system.png)

## Siehe auch

[EMA](ema.md)
[MACD](macd.md)
[MACDHistogram](macd_histogram.md)
[ForceIndex](force_index.md)
