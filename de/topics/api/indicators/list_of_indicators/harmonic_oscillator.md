# HO

**Harmonischer Oszillator (HO)** ist ein technischer Indikator, der auf der Theorie der harmonischen Schwingungen basiert und dabei hilft, zyklische Komponenten in der Preisbewegung zu identifizieren.

Um den Indikator verwenden zu können, müssen Sie die Klasse [HarmonicOscillator](xref:StockSharp.Algo.Indicators.HarmonicOscillator) verwenden.

## Beschreibung

Der Harmonischer Oszillator (HO) ist ein Indikator, der entwickelt wurde, um die Periodizität und zyklische Natur von Marktpreisbewegungen zu identifizieren. Es basiert auf dem Prinzip, dass viele Preisbewegungen harmonische (periodische) Komponenten enthalten, die isoliert und zur Vorhersage zukünftiger Preisbewegungen verwendet werden können.

Der Indikator wendet Spektralanalysemethoden an, um die Preisreihe in harmonische Komponenten zu zerlegen und dominante Zyklen hervorzuheben. Anschließend werden diese zyklischen Komponenten als Oszillator angezeigt, der Händlern dabei hilft, zu bestimmen, wann der Preis innerhalb der identifizierten Zyklen lokale Höchst- oder Tiefststände erreichen könnte.

HO ist besonders nützlich für:
- Bestimmung der zyklischen Natur des Marktes
- Identifizieren potenzieller Umkehrpunkte
- Marktgeräusche filtern
- Vorhersage von Momenten, in denen der Preis seine Richtung ändern könnte

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Analysezeitraum (Standardwert: 30)

## Berechnung

Die Harmonischer Oszillator-Berechnung umfasst die folgenden Schritte:

1. Vorverarbeitung der Price-Serie (Trendbeseitigung):
   ```
   Trendbereinigter Preis = Price - SMA(Price, Length)
   ```

2. Anwendung der Spektralanalyse zur Identifizierung dominanter Zyklen:
   ```
   Spektralkomponenten = FFT(Trendbereinigter Preis)
   ```

3. Extrahieren der wichtigsten harmonischen Komponenten:
   ```
   Dominante Zyklen = Top-N-Spektralkomponenten anhand der Amplitude extrahieren
   ```

4. Synthese des Harmonischer Oszillator basierend auf dominanten Zyklen:
   ```
   HO = Rekonstruktion dominanter Zyklen durch inverse FFT
   ```

Dabei gilt:
- Price - Preis (normalerweise Schlusskurs)
- SMA – einfacher gleitender Durchschnitt
- FFT – Schnelle Fourier-Transformation
- Length - Analysezeitraum

## Interpretation

Der Harmonischer Oszillator kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - Wenn HO die Nulllinie von unten nach oben überschreitet, kann dies als bullisches Signal angesehen werden
   - Wenn HO die Nulllinie von oben nach unten kreuzt, kann dies als rückläufiges Signal angesehen werden

2. **Oszillator-Extreme**:
   - Wenn HO ein lokales Maximum erreicht, kann dies auf eine mögliche Preisspitze hinweisen
   - Wenn HO ein lokales Minimum erreicht, kann dies auf eine mögliche Preisuntergrenze hinweisen

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während HO ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während HO ein niedrigeres Hoch bildet

4. **Zyklusprojektion**:
   - Regelmäßige HO-Spitzen und -Täler können zur Prognose zukünftiger Umkehrpunkte verwendet werden
   - Die Analyse der Dauer zwischen Spitzen/Tiefpunkten kann dabei helfen, die dominante Zykluslänge zu bestimmen

5. **Amplitudenänderungen**:
   - Eine erhöhte HO-Schwingungsamplitude kann auf eine Verstärkung der zyklischen Komponente hinweisen
   - Eine verringerte HO-Schwingungsamplitude kann auf eine Abschwächung der zyklischen Komponente hinweisen

6. **Kombination mit anderen Indikatoren**:
   - HO funktioniert am besten in Kombination mit Trendindikatoren
   - In Trendmärkten können HO-Signale verwendet werden, um Einstiegspunkte in die Trendrichtung zu bestimmen

![HO Diagramm](../../../../images/indicator_harmonic_oscillator.png)

## Siehe auch

[SineWave](sine_wave.md)
[CenterOfGravityOscillator](center_of_gravity_oscillator.md)
[FisherTransform](ehlers_fisher_transform.md)
[DetrendedSyntheticPrice](detrended_synthetic_price.md)
