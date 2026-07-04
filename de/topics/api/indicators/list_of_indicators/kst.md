# KST

**Know Sure Thing (KST)** ist ein von Martin Pring entwickelter technischer Indikator, der die Summe von vier geglätteten Änderungsraten (ROC) mit unterschiedlichen Zeiträumen darstellt, um langfristige Marktzyklen zu identifizieren.

Um den Indikator verwenden zu können, müssen Sie die Klasse [KnowSureThing](xref:StockSharp.Algo.Indicators.KnowSureThing) verwenden.

## Beschreibung

Der Know Sure Thing (KST)-Indikator ist ein von Martin Pring entwickelter Oszillator zur Identifizierung von Trends durch Messung der Preisdynamik über verschiedene Zeiträume hinweg. Der Indikator kombiniert vier Messungen der Änderungsrate (ROC) mit unterschiedlichen Zeiträumen und verleiht längeren Zeiträumen mehr Bedeutung.

KST basiert auf der Theorie, dass Marktzyklen unterschiedlicher Dauer gemeinsam die Preisbewegung beeinflussen. Durch die Kombination von ROC aus verschiedenen Zeiträumen zielt KST darauf ab, langfristige zyklische Trends zu identifizieren und potenzielle Umkehrpunkte zu bestimmen.

Der Indikator wird typischerweise von einer Signallinie (gleitender Durchschnitt von KST) begleitet, und deren Überschneidungen können zur Generierung von Handelssignalen verwendet werden.

## Berechnung

Die Berechnung des KST-Indikators umfasst die folgenden Schritte:

1. Berechnen Sie vier Messungen der Änderungsrate (ROC) mit unterschiedlichen Zeiträumen:
   ```
   ROC1 = ((Close / Close[n1 periods ago]) - 1) * 100
   ROC2 = ((Close / Close[n2 periods ago]) - 1) * 100
   ROC3 = ((Close / Close[n3 periods ago]) - 1) * 100
   ROC4 = ((Close / Close[n4 periods ago]) - 1) * 100
   ```

2. Glätten Sie jeden ROC mit einem einfachen gleitenden Durchschnitt (SMA):
   ```
   RCMA1 = SMA(ROC1, m1)
   RCMA2 = SMA(ROC2, m2)
   RCMA3 = SMA(ROC3, m3)
   RCMA4 = SMA(ROC4, m4)
   ```

3. Gewichtete Summierung, um KST zu erhalten:
   ```
   KST = (RCMA1 * 1) + (RCMA2 * 2) + (RCMA3 * 3) + (RCMA4 * 4)
   ```

4. Signalleitung berechnen:
   ```
   Signal Line = SMA(KST, signal period)
   ```

Dabei gilt:
- Close - Schlusskurs
- n1, n2, n3, n4 – Perioden für die ROC-Berechnung (Standardwerte: 10, 15, 20, 30)
- m1, m2, m3, m4 – Perioden für die ROC-Glättung (Standardwerte: 10, 10, 10, 15)
- Signalperiode - Signalleitungsperiode (Standardwert: 9)

## Interpretation

Der KST-Indikator kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - Wenn KST die Nulllinie von unten nach oben überschreitet, kann dies als bullisches Signal angesehen werden
   - Wenn KST die Nulllinie von oben nach unten kreuzt, kann dies als rückläufiges Signal angesehen werden

2. **Signalleitungskreuzungen**:
   - Wenn KST die Signallinie von unten nach oben kreuzt, kann dies als bullisches Signal angesehen werden (empfindlicher als der Nullliniendurchgang).
   - Wenn KST die Signallinie von oben nach unten kreuzt, kann dies als bärisches Signal angesehen werden (empfindlicher als der Nullliniendurchgang).

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während KST ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während KST ein niedrigeres Hoch bildet

4. **Extreme Werte**:
   - Hohe positive KST-Werte können auf überkaufte Marktbedingungen hinweisen
   - Hohe negative KST-Werte können auf überverkaufte Marktbedingungen hinweisen

5. **Bewegungsrichtung**:
   - Der steigende KST-Trend deutet auf eine insgesamt bullische Marktstimmung hin
   - Der fallende KST-Trend deutet auf eine insgesamt rückläufige Marktstimmung hin

6. **Trendbestätigung**:
   - KST kann zur Bestätigung von Signalen anderer Indikatoren verwendet werden
   - Die Konsistenz zwischen KST-Richtung und Preis bestätigt die Stärke des aktuellen Trends

7. **Analyse der Marktstimmung**:
   - Positive KST-Werte deuten auf eine vorherrschende bullische Stimmung hin
   - Negative KST-Werte deuten auf eine vorherrschende bärische Stimmung hin

![indicator_kst](../../../../images/indicator_kst.png)

## Siehe auch

[ROC](roc.md)
[MACD](macd.md)
[Momentum](momentum.md)
[RSI](rsi.md)
