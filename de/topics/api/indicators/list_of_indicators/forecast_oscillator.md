# FOSC

**Prognose-Oszillator (FOSC)** ist ein technischer Indikator, der die Abweichung des Preises von seinem durch lineare Regression ermittelten vorhergesagten Wert misst und diese Abweichung als Prozentsatz darstellt.

Um den Indikator verwenden zu können, müssen Sie die Klasse [ForecastOscillator](xref:StockSharp.Algo.Indicators.ForecastOscillator) verwenden.

## Beschreibung

Der Prognose-Oszillator (FOSC) basiert auf einer linearen Regression und soll den Grad der Abweichung des aktuellen Preises von seinem vorhergesagten Wert messen. Es hilft Händlern einzuschätzen, wie genau der aktuelle Preis dem erwarteten Trend entspricht oder davon abweicht.

Der Indikator berechnet eine Trendlinie mithilfe einer linearen Regression über einen bestimmten Zeitraum und vergleicht dann den tatsächlichen Schlusskurs mit dem vorhergesagten Wert auf dieser Linie. Die Differenz wird in Prozent ausgedrückt, was FOSC zu einem Oszillator macht, der um die Nulllinie schwankt.

Der Prognose-Oszillator ist besonders nützlich für:
- Bestimmung des Grades der Preisanpassung an den erwarteten Trend
- Identifizieren potenzieller Umkehrpunkte
- Erkennen extremer Preisabweichungen vom Trend
- Identifizieren von Zeiträumen, in denen sich der Preis schneller oder langsamer als erwartet bewegt

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Zeitraum für lineare Regressionsberechnung (Standardwert: 14)

## Berechnung

Die Prognose-Oszillator-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie die Prognoselinie mithilfe der linearen Regression über den angegebenen Zeitraum:
   ```
   Prognose = lineare Regressionslinie(Close, Length)
   ```

2. Berechnen Sie den Oszillator als prozentuales Verhältnis zwischen aktuellem Preis und Prognosewert:
   ```
   FOSC = ((Close - Prognose) / Prognose) * 100
   ```

Dabei gilt:
- Close – aktueller Schlusskurs
- Prognose – vorhergesagter Wert, der durch lineare Regression ermittelt wird
- Length – Zeitraum für die Berechnung der linearen Regression

## Interpretation

Der Prognose-Oszillator wird wie folgt interpretiert:

1. **Abweichung vom Nullpunkt**:
   - Positive Werte (FOSC > 0) zeigen an, dass der aktuelle Preis über dem vorhergesagten Wert liegt, was auf eine stärkere Aufwärtsbewegung als erwartet hinweisen kann
   - Negative Werte (FOSC < 0) weisen darauf hin, dass der aktuelle Preis unter dem vorhergesagten Wert liegt, was auf eine stärkere Abwärtsbewegung als erwartet hinweisen kann

2. **Extreme Werte**:
   - Sehr hohe positive Werte können auf überkaufte Marktbedingungen im Vergleich zum Trend hinweisen
   - Sehr niedrige negative Werte können auf überverkaufte Marktbedingungen im Verhältnis zum Trend hinweisen

3. **Zurück zur Null**:
   - Die Bewegung von FOSC von extremen Werten in Richtung Null kann auf eine mögliche Rückkehr des Preises zu seiner Trendlinie hinweisen

4. **Nulllinienübergänge**:
   - Das Überschreiten der Nulllinie von unten nach oben kann als bullisches Signal gewertet werden
   - Das Überschreiten der Nulllinie von oben nach unten kann als bärisches Signal gewertet werden

5. **Abweichungen**:
   - Eine bullische Divergenz (der Preis bildet ein neues Tief, während FOSC ein höheres Tief bildet) könnte auf eine mögliche Aufwärtsumkehr hinweisen
   - Eine rückläufige Divergenz (der Preis bildet ein neues Hoch, während FOSC ein niedrigeres Hoch bildet) könnte auf eine mögliche Abwärtsumkehr hinweisen

6. **Trendbestätigung**:
   - Wenn sich FOSC in die gleiche Richtung wie der Preis bewegt, bestätigt dies die Stärke des aktuellen Trends
   - Wenn sich FOSC in die entgegengesetzte Richtung zum Preis bewegt, kann dies auf eine Abschwächung des aktuellen Trends hinweisen

![indicator_forecast_oscillator](../../../../images/indicator_forecast_oscillator.png)

## Siehe auch

[LinearRegression](lrc.md)
[StandardError](standard_error.md)
[DisparityIndex](disparity_index.md)
