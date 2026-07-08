# PVO

**Percentage Volume Oscillator (PVO)** ist ein technischer Indikator ähnlich wie MACD, der jedoch auf das Handelsvolumen statt auf den Preis angewendet wird und die Differenz zwischen schnellen und langsamen exponentiellen gleitenden Durchschnitten des Volumens in Prozent ausdrückt.

Um den Indikator verwenden zu können, müssen Sie die Klasse [PercentageVolumeOscillator](xref:StockSharp.Algo.Indicators.PercentageVolumeOscillator) verwenden.

## Beschreibung

Der Percentage Volume Oscillator (PVO) ist eine Modifikation des Indikators MACD (Moving Average Convergence Divergence), der auf das Handelsvolumen statt auf den Preis angewendet wird. Ähnlich wie PPO (Percentage Price Oscillator) drückt PVO die Differenz zwischen schnellen und langsamen exponentiellen gleitenden Durchschnitten als Prozentsatz und nicht in absoluten Einheiten aus. Dies macht PVO besonders nützlich, wenn Sie verschiedene Instrumente mit unterschiedlichen Lautstärkepegeln vergleichen oder ein einzelnes Instrument über einen längeren Zeitraum analysieren.

PVO besteht aus drei Komponenten:
1. **PVO-Linie** – prozentuale Differenz zwischen schneller und langsamer Lautstärke EMA
2. **Signalleitung** – EMA der PVO-Leitung
3. **Histogramm** – Differenz zwischen der PVO-Linie und der Signallinie

Der PVO-Indikator hilft bei der Identifizierung von Volumenanomalien, die erheblichen Preisbewegungen vorausgehen können. Es ist auch nützlich, um Preistrends zu bestätigen und potenzielle Umkehrpunkte zu identifizieren.

## Parameter

Der Indikator hat die folgenden Parameter:
- **ShortPeriod** – Zeitraum zur Berechnung des Short-Volumens EMA (Standardwert: 12)
- **LongPeriod** – Zeitraum zur Berechnung des Long-Volumens EMA (Standardwert: 26)

## Berechnung

Die Percentage Volume Oscillator-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie kurze und länge exponentielle gleitende Durchschnitte des Volumens:
   ```
   Short EMA = EMA(Volume, ShortPeriod)
   Long EMA = EMA(Volume, LongPeriod)
   ```

2. Berechnen Sie die PVO-Linie als prozentuale Differenz zwischen Short- und Long-EMA:
   ```
   PVO Line = ((Short EMA - Long EMA) / Long EMA) * 100
   ```

3. Berechnen Sie die Signalleitung (typischerweise 9-Perioden-EMA- oder PVO-Leitung):
   ```
   Signal Line = EMA(PVO Line, 9)
   ```

4. Histogramm berechnen:
   ```
   Histogram = PVO Line - Signal Line
   ```

Dabei gilt:
- Volume - Handelsvolumen
- EMA – exponentieller gleitender Durchschnitt
- ShortPeriod – Punkt für Kurzform EMA
- LongPeriod – Zeitraum für langes EMA

## Interpretation

Der Percentage Volume Oscillator kann wie folgt interpretiert werden:

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

6. **Price Trendbestätigung**:
   - Steigender PVO bestätigt einen Aufwärtstrend des Preises
   - Der fallende PVO bestätigt einen Abwärtstrend des Preises
   - Divergenz zwischen PVO-Richtung und Preis kann auf eine mögliche Umkehr hinweisen

7. **Volume-Spikes**:
   - Starke PVO-Sprünge deuten auf erhebliche Volumenänderungen hin, die oft mit wichtigen Marktereignissen einhergehen
   - Solche Spitzen können Ausbrüchen wichtiger Preisniveaus vorausgehen oder diese begleiten

![indicator_percentage_volume_oscillator](../../../../images/indicator_percentage_volume_oscillator.png)

## Siehe auch

[PPO](percentage_price_oscillator.md)
[OBV](on_balance_volume.md)
[MACD](macd.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
