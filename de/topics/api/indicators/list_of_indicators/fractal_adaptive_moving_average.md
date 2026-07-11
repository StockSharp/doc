# FRAMA

**Fraktaler adaptiver gleitender Durchschnitt (FRAMA)** ist ein von John Ehlers entwickelter technischer Indikator, der die Reaktionsgeschwindigkeit auf Preisänderungen basierend auf der fraktalen Dimension des Marktes anpasst.

Um den Indikator verwenden zu können, müssen Sie die Klasse [FractalAdaptiveMovingAverage](xref:StockSharp.Algo.Indicators.FractalAdaptiveMovingAverage) verwenden.

## Beschreibung

Der fraktale adaptive gleitende Durchschnitt (FRAMA) ist ein fortschrittlicher Typ des exponentiellen gleitenden Durchschnitts (EMA), der seine Empfindlichkeit gegenüber Preisänderungen basierend auf der fraktalen Dimension des Marktes automatisch anpasst. Der Indikator wurde von John Ehlers entwickelt und im Oktober 2000 in der Zeitschrift Technical Analysis of Stocks & Commodities vorgestellt.

FRAMA nutzt das Konzept der fraktalen Geometrie zur Analyse der Marktstruktur. Es bestimmt, wie „fraktal“ oder chaotisch der aktuelle Markt ist, und passt auf dieser Grundlage die Antwortrate des Indikators an:

- Unter trendigen (weniger fraktalen) Marktbedingungen reagiert FRAMA schnell auf Preisänderungen, ähnlich wie ein Short-EMA
- Bei seitwärts gerichteten (eher fraktalen) Marktbedingungen reagiert FRAMA langsamer, ähnlich wie ein langer EMA

Dadurch kann FRAMA schneller auf erhebliche Preisbewegungen reagieren und Marktgeräusche ignorieren, was ihn im Vergleich zu herkömmlichen gleitenden Durchschnitten effektiver macht.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Berechnungszeitraum (Standardwert: 10-20)

## Berechnung

Die FRAMA-Berechnung umfasst mehrere Schritte:

1. Berechnen Sie die fraktale Dimension (D) basierend auf dem logarithmischen Verhältnis der Länge des Hoch-Tief-Preises zur Anzahl der Perioden:
   ```
   N1 = High(1...Length/2) - Low(1...Length/2)
   N2 = High(Length/2+1...Length) - Low(Length/2+1...Length)
   N3 = High(1...Length) - Low(1...Length)

   D = (log(N1 + N2) - log(N3)) / log(2)
   ```

2. Konvertieren Sie die fraktale Dimension in einen Alpha-Faktor für die exponentielle Glättung:
   ```
   Smoothing Factor = exp(-4.6 * (D - 1))
   Alpha = Smoothing Factor * Smoothing Factor
   ```

3. Wenden Sie den Alpha-Faktor auf den aktuellen Preis und den vorherigen FRAMA-Wert an:
   ```
   FRAMA = Alpha * Price + (1 - Alpha) * FRAMA[previous]
   ```

Dabei gilt:
- High – Höchstpreis für den Zeitraum
- Low – Mindestpreis für den Zeitraum
- log - natürlicher Logarithmus

## Interpretation

FRAMA kann ähnlich wie andere gleitende Durchschnitte interpretiert werden, jedoch unter Berücksichtigung seiner adaptiven Natur:

1. **FRAMA Richtung**:
   - Aufwärts FRAMA zeigt einen Aufwärtstrend an
   - Abwärts FRAMA zeigt einen Abwärtstrend an

2. **Frequenzweichen mit Price**:
   - Wenn der Preis FRAMA von unten nach oben kreuzt, kann dies als bullisches Signal angesehen werden
   - Wenn der Preis FRAMA von oben nach unten kreuzt, kann dies als rückläufiges Signal angesehen werden

3. **Mehrere FRAMA-Frequenzweichen**:
   - Das Kreuzen eines kurzen FRAMA mit einem langen FRAMA von unten nach oben kann den Beginn eines Aufwärtstrends anzeigen
   - Das Kreuzen eines kurzen FRAMA mit einem langen FRAMA von oben nach unten kann auf den Beginn eines Abwärtstrends hinweisen

4. **FRAMA Neigungswinkel**:
   - Ein steiler Neigungswinkel weist auf einen starken Trend hin
   - Ein geringer Neigungswinkel weist auf einen schwachen Trend hin
   - Eine horizontale Bewegung weist auf einen Seitwärtstrend hin

5. **Signalfilterung**:
   - Aufgrund seiner adaptiven Natur erzeugt FRAMA weniger falsche Signale als herkömmliche gleitende Durchschnitte
   - Je kürzer der FRAMA-Zeitraum ist, desto empfindlicher reagiert der Indikator auf Preisänderungen

6. **Unterstützungs- und Widerstandsstufen**:
   - FRAMA kann in einem Aufwärtstrend als dynamisches Unterstützungsniveau dienen
   - FRAMA kann als dynamisches Widerstandsniveau in einem Abwärtstrend dienen

![indicator_fractal_adaptive_moving_average](../../../../images/indicator_fractal_adaptive_moving_average.png)

## Siehe auch

[EMA](ema.md)
[KAMA](kama.md)
[VIDYA](vidya.md)
