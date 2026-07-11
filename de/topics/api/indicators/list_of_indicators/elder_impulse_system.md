# EIS

**Elder-Impulse-System (EIS)** ist ein von Dr. Alexander Elder entwickelter technischer Indikator, der einen Trendindikator und einen Momentumoszillator kombiniert, um die Richtung und Stärke der Marktbewegung zu bestimmen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [ElderImpulseSystem](xref:StockSharp.Algo.Indicators.ElderImpulseSystem) verwenden.

## Beschreibung

Das Elder-Impulse-System (EIS) ist ein einfaches, aber leistungsstarkes Tool zur Visualisierung der Marktdynamik. Es kombiniert zwei Indikatoren:
1. **Exponentieller gleitender Durchschnitt (EMA)** – zur Bestimmung der Trendrichtung
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

Die Berechnung des Elder-Impulse-Systems umfasst die folgenden Schritte:

1. Berechnen Sie den exponentiellen gleitenden Durchschnitt über 13 Perioden (EMA):
   ```
   EMA = EMA(Close, 13)
   ```

2. Berechnen Sie das MACD-Histogramm (Standardwerte: 12, 26, 9):
   ```
   MACD-Linie = EMA(Close, 12) - EMA(Close, 26)
   Signallinie = EMA(MACD-Linie, 9)
   MACD-Histogramm = MACD-Linie - Signallinie
   ```

3. Bestimmen Sie die Farbklassifizierung für die aktuelle Kerze:
   ```
   Wenn EMA[aktuell] > EMA[vorherig] UND MACD-Histogramm[aktuell] > MACD-Histogramm[vorherig], dann Grün (Aufwärtsimpuls)
   Wenn EMA[aktuell] < EMA[vorherig] UND MACD-Histogramm[aktuell] < MACD-Histogramm[vorherig], dann Rot (Abwärtsimpuls)
   Andernfalls Blau (kein Impuls)
   ```

## Interpretation

Das Elder-Impulse-System wird wie folgt interpretiert:

1. **Grüne Kerzen (starker bullischer Impuls)**:
   - Zeigen Sie eine starke Aufwärtsdynamik an
   - Beste Zeit zum Kaufen oder Halten von Kaufpositionen
   - Eine Reihe grüner Kerzen weist auf einen starken Aufwärtstrend hin

2. **Rote Kerzen (starker rückläufiger Impuls)**:
   - Zeigen Sie eine starke Abwärtsdynamik an
   - Beste Zeit zum Verkaufen oder Halten von Verkaufspositionen
   - Eine Reihe roter Kerzen weist auf einen starken Abwärtstrend hin

3. **Blaue Kerzen (kein klarer Impuls)**:
   - Zeigen Sie Unsicherheit oder Konsolidierung an
   - Signalisieren Sie eine mögliche Verlangsamung oder Trendumkehr
   - Tritt häufig während Konsolidierungsperioden oder vor einer Trendwende auf

4. **Handelsstrategien**:
   - Kaufen Sie, wenn Kerzen ihre Farbe von Blau nach Grün ändern
   - Verkaufen, wenn die Kerzen ihre Farbe von Blau nach Rot ändern
   - Schließen Sie Kaufpositionen, wenn Kerzen ihre Farbe von Grün in eine andere ändern
   - Schließen Sie Verkaufspositionen, wenn Kerzen ihre Farbe von Rot zu einer anderen ändern

5. **Trendbestätigung**:
   - Eine Reihe grüner Kerzen bestätigt einen Aufwärtstrend
   - Eine Reihe roter Kerzen bestätigt einen Abwärtstrend
   - Farbwechsel weisen auf einen Seitwärtstrend oder eine Unsicherheit hin

![indicator_elder_impulse_system](../../../../images/indicator_elder_impulse_system.png)

## Siehe auch

[EMA](ema.md)
[MACD](macd.md)
[MACD-Histogramm](macd_histogram.md)
[FI](force_index.md)
