# PVO

**Prozentualer Volumenoszillator (PVO)** ist ein technischer Indikator ähnlich wie MACD, der jedoch auf das Handelsvolumen statt auf den Preis angewendet wird und die Differenz zwischen schnellen und langsamen exponentiellen gleitenden Durchschnitten des Volumens in Prozent ausdrückt.

Um den Indikator verwenden zu können, müssen Sie die Klasse [PercentageVolumeOscillator](xref:StockSharp.Algo.Indicators.PercentageVolumeOscillator) verwenden.

## Beschreibung

Der prozentuale Volumenoszillator (PVO) ist eine Modifikation des Indikators MACD (Konvergenz/Divergenz gleitender Durchschnitte), der auf das Handelsvolumen statt auf den Preis angewendet wird. Ähnlich wie PPO (prozentualer Preisoszillator) drückt PVO die Differenz zwischen schnellen und langsamen exponentiellen gleitenden Durchschnitten als Prozentsatz und nicht in absoluten Einheiten aus. Dies macht PVO besonders nützlich, wenn Sie verschiedene Instrumente mit unterschiedlichen Volumenniveaus vergleichen oder ein einzelnes Instrument über einen längeren Zeitraum analysieren.

PVO besteht aus drei Komponenten:
1. **PVO-Linie** – prozentuale Differenz zwischen schneller und langsamer Volumen-EMA
2. **Signalleitung** – EMA der PVO-Leitung
3. **Histogramm** – Differenz zwischen der PVO-Linie und der Signallinie

Der PVO-Indikator hilft bei der Identifizierung von Volumenanomalien, die erheblichen Preisbewegungen vorausgehen können. Es ist auch nützlich, um Preistrends zu bestätigen und potenzielle Umkehrpunkte zu identifizieren.

## Parameter

Der Indikator hat die folgenden Parameter:
- **ShortPeriod** – Zeitraum zur Berechnung des kurzen Volumen-EMA (Standardwert: 12)
- **LongPeriod** – Zeitraum zur Berechnung des langen Volumen-EMA (Standardwert: 26)

## Berechnung

Die Berechnung des prozentualen Volumenoszillators umfasst die folgenden Schritte:

1. Berechnen Sie kurze und lange exponentielle gleitende Durchschnitte des Volumens:
   ```
   Kurze EMA = EMA(Volumen, ShortPeriod)
   Lange EMA = EMA(Volumen, LongPeriod)
   ```

2. Berechnen Sie die PVO-Linie als prozentuale Differenz zwischen dem kurzen und dem langen Volumen-EMA:
   ```
   PVO-Linie = ((kurze EMA - lange EMA) / lange EMA) * 100
   ```

3. Berechnen Sie die Signalleitung (typischerweise 9-Perioden-EMA- oder PVO-Leitung):
   ```
   Signallinie = EMA(PVO-Linie, 9)
   ```

4. Histogramm berechnen:
   ```
   Histogramm = PVO-Linie - Signallinie
   ```

Dabei gilt:
- Volume - Handelsvolumen
- EMA – exponentieller gleitender Durchschnitt
- ShortPeriod – Punkt für Kurzform EMA
- LongPeriod – Zeitraum für langes EMA

## Interpretation

Der prozentuale Volumenoszillator kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - Die PVO-Linie, die die Nulllinie von unten nach oben kreuzt, weist auf eine überdurchschnittliche Volumenbeschleunigung hin, die eine Aufwärtsbewegung ankündigen könnte
   - Die PVO-Linie, die die Nulllinie von oben nach unten kreuzt, weist auf eine unterdurchschnittliche Volumenverlangsamung hin, die eine rückläufige Bewegung ankündigen könnte

2. **Signalleitungskreuzungen**:
   - Die PVO-Linie, die die Signallinie von unten nach oben kreuzt, kann als bullisches Signal angesehen werden
   - Die PVO-Linie, die die Signallinie von oben nach unten kreuzt, kann als rückläufiges Signal angesehen werden

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während PVO ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während PVO ein niedrigeres Hoch bildet

4. **Extreme Werte**:
   - Sehr hohe PVO-Werte können auf ein übermäßiges Volumen hinweisen, das häufig bei Marktspitzen oder Panik auftritt
   - Sehr niedrige PVO-Werte können auf ein unzureichendes Volumen hinweisen, was häufig bei Marktflaute auftritt

5. **Histogrammanalyse**:
   - Ein zunehmend positives Histogramm deutet auf eine Verstärkung der zinsbullischen Volumendynamik hin
   - Ein zunehmendes negatives Histogramm deutet auf eine Verstärkung der rückläufigen Volumendynamik hin
   - Die Kontraktion des Histogramms weist auf eine Abschwächung der aktuellen Volumendynamik hin

6. **Bestätigung des Preistrends**:
   - Steigender PVO bestätigt einen Aufwärtstrend des Preises
   - Der fallende PVO bestätigt einen Abwärtstrend des Preises
   - Divergenz zwischen PVO-Richtung und Preis kann auf eine mögliche Umkehr hinweisen

7. **Volumenspitzen**:
   - Starke PVO-Sprünge deuten auf erhebliche Volumenänderungen hin, die oft mit wichtigen Marktereignissen einhergehen
   - Solche Spitzen können Ausbrüchen wichtiger Preisniveaus vorausgehen oder diese begleiten

![PVO](../../../../images/indicator_percentage_volume_oscillator.png)

## Siehe auch

[PPO](percentage_price_oscillator.md)
[OBV](on_balance_volume.md)
[MACD](macd.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
