# OMA

**Oscillator of Moving Average (OMA)** ist ein technischer Indikator, der die Differenz zwischen zwei gleitenden Durchschnitten mit unterschiedlichen Perioden misst, um die Dynamik und mögliche Umkehrpunkte zu bestimmen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [OscillatorOfMovingAverage](xref:StockSharp.Algo.Indicators.OscillatorOfMovingAverage) verwenden.

## Beschreibung

Der Oscillator of Moving Average (OMA) stellt die Differenz zwischen einem kurzen und einem langen gleitenden Durchschnitt dar. Dieser Indikator hilft bei der Bestimmung der Trendstärke und ihrer möglichen Änderungen, indem er die Beziehung zwischen gleitenden Durchschnitten verschiedener Zeiträume analysiert.

OMA funktioniert nach einem ähnlichen Prinzip wie MACD (Konvergenz/Divergenz gleitender Durchschnitte), jedoch in einer einfacheren Form, da es keine Signalleitung enthält. Der Indikator schwingt um die Nulllinie, wobei positive Werte darauf hinweisen, dass der kurze gleitende Durchschnitt über dem langen gleitenden Durchschnitt liegt (bullischer Zustand), und negative Werte anzeigen, dass der kurze gleitende Durchschnitt unter dem langen gleitenden Durchschnitt liegt (bärischer Zustand).

Die Hauptstärke von OMA liegt in seiner Fähigkeit, Veränderungen in der Trenddynamik zu erkennen und Handelssignale basierend auf Nulllinienkreuzungen und Preisdivergenzen zu generieren.

## Parameter

Der Indikator hat die folgenden Parameter:
- **ShortPeriod** – Zeitraum für den kurzen gleitenden Durchschnitt (Standardwert: 12)
- **LongPeriod** – Zeitraum für den langen gleitenden Durchschnitt (Standardwert: 26)

## Berechnung

Die Oscillator of Moving Average-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie den kurzen gleitenden Durchschnitt:
   ```
   Short MA = SMA(Price, ShortPeriod)
   ```

2. Berechnen Sie den langen gleitenden Durchschnitt:
   ```
   Long MA = SMA(Price, LongPeriod)
   ```

3. Berechnen Sie OMA als Differenz zwischen kurzen und langen gleitenden Durchschnitten:
   ```
   OMA = Short MA - Long MA
   ```

Dabei gilt:
- Price - Preis (normalerweise Schlusskurs)
- SMA – einfacher gleitender Durchschnitt
- ShortPeriod – Zeitraum für den kurzen gleitenden Durchschnitt
- LongPeriod – Zeitraum für den langen gleitenden Durchschnitt

Hinweis: Anstelle von SMA können auch andere Arten von gleitenden Durchschnitten wie EMA (exponentieller gleitender Durchschnitt), WMA (gewichteter gleitender Durchschnitt) usw. verwendet werden.

## Interpretation

Der Oscillator of Moving Average kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - OMA, das die Nulllinie von unten nach oben kreuzt (Short-MA kreuzt Long-MA von unten nach oben), kann als bullisches Signal angesehen werden
   - OMA, das die Nulllinie von oben nach unten kreuzt (Short-MA kreuzt Long-MA von oben nach unten), kann als bärisches Signal angesehen werden

2. **Extreme Werte**:
   - Hohe positive OMA-Werte weisen darauf hin, dass der Markt möglicherweise überkauft ist
   - Hohe negative OMA-Werte weisen darauf hin, dass der Markt möglicherweise überverkauft ist
   - Extremwerte gehen oft Korrekturen oder Trendumkehrungen voraus

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während OMA ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während OMA ein niedrigeres Hoch bildet
   - Divergenzen gehen oft deutlichen Trendumkehrungen voraus

4. **Trendbestätigung**:
   - Positive OMA-Werte bestätigen einen Aufwärtstrend
   - Negative OMA-Werte bestätigen einen Abwärtstrend
   - Ein steigender absoluter OMA-Wert deutet auf eine Verstärkung des aktuellen Trends hin

5. **Mittellinie (0)**:
   - Wenn OMA um die Nulllinie schwankt, kann dies auf das Fehlen eines ausgeprägten Trends oder einer Konsolidierung hinweisen

6. **Änderungsrate**:
   - Die OMA-Steigung gibt die Geschwindigkeit der Trendänderung an
   - Ein steiler Anstieg weist auf eine schnelle Trendänderung hin
   - Eine flache Steigung weist auf eine langsame Trendänderung hin

7. **Kombination mit anderen Indikatoren**:
   - OMA wird häufig in Kombination mit anderen Indikatoren zur Bestätigung von Signalen verwendet
   - Besonders effektiv in Kombination mit überkauften/überverkauften Indikatoren wie RSI oder Stochastic

![indicator_oscillator_of_moving_average](../../../../images/indicator_oscillator_of_moving_average.png)

## Siehe auch

[MACD](macd.md)
[MovingAverageCrossover](moving_average_crossover.md)
[SMA](sma.md)
[EMA](ema.md)
