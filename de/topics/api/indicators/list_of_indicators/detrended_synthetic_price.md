# DSP

**Trendbereinigter synthetischer Preis (DSP)** ist ein technischer Indikator, der den Gesamttrend aus einer Preisreihe entfernt und es Händlern ermöglicht, sich auf kurzfristige Preisschwankungen zu konzentrieren.

Um den Indikator verwenden zu können, müssen Sie die Klasse [DetrendedSyntheticPrice](xref:StockSharp.Algo.Indicators.DetrendedSyntheticPrice) verwenden.

## Beschreibung

Der DSP-Indikator wurde entwickelt, um den langfristigen Trend aus einem Preisdiagramm zu eliminieren und es Händlern zu ermöglichen, kurzfristige Zyklen und Schwankungen klarer zu erkennen. Es ist besonders nützlich, um kurzfristige Handelsmöglichkeiten zu identifizieren, die durch den vorherrschenden Trend verdeckt werden könnten.

Die Hauptidee von DSP besteht darin, dass es durch die Entfernung des Trends aus der Preisreihe einfacher wird, die zyklischen Komponenten der Preisbewegung zu identifizieren. Dies macht den Indikator besonders wertvoll für Händler, die sich auf den kurzfristigen Handel spezialisiert haben und zyklische Muster verwenden.

DSP ist nützlich für:
- Identifizierung kurzfristiger Marktzyklen
- Ermittlung möglicher Umkehrpunkte
- Abweichungen vom Preis erkennen
- Schaffung von Handelssystemen, die auf der zyklischen Natur der Märkte basieren

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Berechnungszeitraum (Standardwert: 10-20 Perioden)

## Berechnung

Die Berechnung des Trendbereinigter synthetischer Preis umfasst die folgenden Schritte:

1. Berechnen Sie den gleitenden Durchschnitt des Preises über den angegebenen Zeitraum:
   ```
   MA = SMA(Price, Length)
   ```

2. Bestimmen Sie den Offset für die synthetische Preisberechnung:
   ```
   Offset = (Length / 2) + 1
   ```

3. Berechnen Sie den synthetischen Preis, indem Sie den verschobenen gleitenden Durchschnitt vom aktuellen Preis subtrahieren:
   ```
   DSP = Price - MA[um (Length/2) + 1 Perioden zurück verschoben]
   ```

Dabei gilt:
- Price – aktueller Preis (normalerweise Schlusskurs)
- MA – einfacher gleitender Durchschnitt
- Length – ausgewählter Berechnungszeitraum

## Interpretation

Der DSP-Indikator schwingt um die Nulllinie und kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - Wenn DSP die Nulllinie von unten nach oben überschreitet, kann dies als bullisches Signal angesehen werden
   - Wenn DSP die Nulllinie von oben nach unten kreuzt, kann dies als rückläufiges Signal angesehen werden

2. **Indikator-Extreme**:
   - Wenn DSP extrem hohe Werte erreicht, kann dies auf überkaufte Bedingungen am Markt hinweisen
   - Wenn DSP extrem niedrige Werte erreicht, kann dies auf überverkaufte Marktbedingungen hinweisen

3. **Zyklische Analyse**:
   - Regelmäßige DSP-Oszillationen können verwendet werden, um die Periodizität des Marktzyklus zu bestimmen
   - Änderungen der Schwingungsamplitude können auf Verschiebungen in der Marktdynamik hinweisen

4. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während DSP ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während DSP ein niedrigeres Hoch bildet

5. **Musterbildung**:
   - Auf dem DSP-Chart können sich technische Muster (Kopf-Schulter-Muster, Doppelboden usw.) bilden, die möglicherweise zusätzliche Handelssignale liefern

![DSP Diagramm](../../../../images/indicator_detrended_synthetic_price.png)

## Siehe auch

[DetrendedPriceOscillator](dpo.md)
[CenterOfGravityOscillator](center_of_gravity_oscillator.md)
[SineWave](sine_wave.md)
