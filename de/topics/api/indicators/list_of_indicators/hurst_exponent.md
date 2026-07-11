# HurstExponent

**Hurst Exponent** ist ein statistisches Maß, das zur Bewertung der Tendenz einer Zeitreihe verwendet wird, entweder einen Trend zu entwickeln oder zum Mittelwert zurückzukehren.

Um den Indikator verwenden zu können, müssen Sie die Klasse [HurstExponent](xref:StockSharp.Algo.Indicators.HurstExponent) verwenden.

## Beschreibung

Der Hurst-Exponent (H) liegt zwischen 0 und 1 und beschreibt das Langzeitgedächtnis einer Serie:
- **H > 0,5** weist auf ein anhaltendes Trendverhalten hin.
- **H < 0,5** zeigt eine Tendenz zur Mittelwertsumkehr.
- **H ≈ 0,5** entspricht einem Random Walk.

Es kann dabei helfen, die Markteffizienz abzuschätzen und sich abzeichnende Zyklen oder Trends aufzudecken.

## Parameter

- **Length** – die Anzahl der in der Berechnung verwendeten Balken.

## Berechnung

Eine gängige Methode basiert auf dem R/S-Ansatz (Rescaled Range):
1. Berechnen Sie für jedes Fenster von `Length`-Punkten die kumulativen Abweichungen vom Mittelwert.
2. Ermitteln Sie den Bereich `R` als Differenz zwischen maximaler und minimaler kumulativer Abweichung.
3. Berechnen Sie die Standardabweichung `S` des Fensters.
4. Bewerten Sie den neu skalierten Bereich `R/S`.
5. Schätzen Sie `H` als Steigung von `log(R/S)` gegenüber `log(Length)`.

Höhere Werte des Exponenten deuten auf ein stärkeres Trendverhalten hin, während niedrigere Werte auf eine stärkere Mittelwertumkehr hinweisen.

![indicator_hurst_exponent](../../../../images/indicator_hurst_exponent.png)

## Siehe auch

[Fraktaler adaptiver gleitender Durchschnitt](fractal_adaptive_moving_average.md)
[MMI](market_meanness_index.md)
