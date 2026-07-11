# MP

**Momentum Pinball (MP)** ist ein technischer Indikator, der die Preisdynamik und ihre Änderungen analysiert, um potenzielle Umkehrpunkte und Trendstärken auf dem Markt zu identifizieren.

Um den Indikator verwenden zu können, müssen Sie die Klasse [MomentumPinball](xref:StockSharp.Algo.Indicators.MomentumPinball) verwenden.

## Beschreibung

Der Momentum Pinball (MP)-Indikator ist ein spezieller Oszillator, der die Preisdynamik verfolgt und potenzielle Umkehrpunkte identifiziert. Der Name „Flipper“ spiegelt die Fähigkeit des Indikators wider, Momente zu erkennen, in denen der Preis, wie eine Kugel in einem Flipperautomaten, von extremen Positionen abprallt.

MP analysiert die Beziehung zwischen der aktuellen Dynamik und ihren historischen Extremwerten und bestimmt, wann der Markt überkaufte oder überverkaufte Bedingungen erreicht. Der Indikator hilft auch dabei, Momente zu erkennen, in denen die Dynamik nachzulassen beginnt, was einer Trendumkehr vorausgehen kann.

Die Grundidee besteht darin, dass extreme Impulswerte oft instabil sind und nach dem Erreichen solcher Extreme in der Regel eine Korrektur oder Umkehr folgt. MP hilft bei der Visualisierung dieses Prozesses, indem es sowohl die Dynamik selbst als auch ihre Änderungen verfolgt.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Berechnungszeitraum (Standardwert: 14)

## Berechnung

Die Berechnung des Momentum Pinball-Indikators umfasst die folgenden Schritte:

1. Berechnen Sie das Basismomentum als Differenz zwischen dem aktuellen Preis und dem Preis vor N Perioden:
   ```
   Momentum = Price[current] - Price[current - Length]
   ```

2. Bestimmen Sie den historischen maximalen und minimalen Impuls über den angegebenen Zeitraum:
   ```
   Max_Momentum = Maximum(Momentum) über Length-Periode
   Min_Momentum = Minimum(Momentum) über Length-Periode
   ```

3. Normalisieren Sie die aktuelle Dynamik relativ zu historischen Extremen:
   ```
   Normalized_Momentum = (Momentum - Min_Momentum) / (Max_Momentum - Min_Momentum)
   ```

4. Berechnen Sie die Änderungsrate für den normalisierten Impuls:
   ```
   Momentum_Change = Normalized_Momentum[current] - Normalized_Momentum[current - 1]
   ```

5. Endgültige MP-Berechnung als Kombination aus normalisiertem Impuls und seiner Änderung:
   ```
   MP = Normalized_Momentum + Momentum_Change
   ```

Dabei gilt:
- Price - Preis (normalerweise Schlusskurs)
- Length - Berechnungszeitraum
- Momentum – Grundimpuls
- Normalized_Momentum – normalisierter Impuls
- Momentum_Change – Impulsänderung

## Interpretation

Der Momentum Pinball-Indikator kann wie folgt interpretiert werden:

1. **Extremwerte**:
   - Werte über 0,8 weisen auf überkaufte Marktbedingungen hin
   - Werte unter 0,2 weisen auf überverkaufte Marktbedingungen hin
   - Wenn MP diese Niveaus erreicht, steigt die Wahrscheinlichkeit einer Umkehr oder Korrektur

2. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während MP ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während MP ein niedrigeres Hoch bildet
   - Divergenzen gehen oft deutlichen Trendumkehrungen voraus

3. **Mittellinienkreuzungen**:
   - Das Überschreiten der 0,5-Marke von MP von unten nach oben kann als bullisches Signal gewertet werden
   - Das Überschreiten der 0,5-Marke durch MP von oben nach unten kann als rückläufiges Signal angesehen werden

4. **Rebounds von Extremen**:
   - Eine Umkehr des MP von überkauften oder überverkauften Niveaus kann Markteintrittssignale erzeugen
   - Besonders starke Signale entstehen, wenn solche Umkehrungen mit Divergenzen einhergehen

5. **Trendanalyse**:
   - Anhaltende MP-Werte über 0,5 bestätigen einen Aufwärtstrend
   - Anhaltende MP-Werte unter 0,5 bestätigen einen Abwärtstrend
   - MP-Schwankungen um das 0,5-Niveau weisen auf einen Seitwärtstrend oder Unsicherheit hin

6. **Momentumstärke**:
   - Eine steile MP-Steigung weist auf eine starke Dynamik hin
   - Eine flache MP-Steigung weist auf einen schwachen Impuls hin
   - Eine Verlangsamung des MP-Anstiegs oder -Falls kann einer Trendumkehr vorausgehen

7. **Kombination mit anderen Indikatoren**:
   - MP wird häufig in Kombination mit Trendindikatoren verwendet
   - Beispielsweise können gleitende Durchschnitte zur Bestimmung der Trendrichtung verwendet werden, während MP für Ein- und Ausstiegspunkte verwendet werden kann

![indicator_momentum_pinball](../../../../images/indicator_momentum_pinball.png)

## Siehe auch

[Momentum](momentum.md)
[RSI](rsi.md)
[StochasticOscillator](stochastic_oscillator.md)
[PrettyGoodOscillator](pretty_good_oscillator.md)
