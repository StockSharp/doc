# PGO

**Pretty-Good-Oszillator (PGO)** ist ein von Mark Johnson entwickelter technischer Indikator, der den aktuellen Schlusskurs mit früheren Preisen unter Berücksichtigung der Volatilität vergleicht, um überkaufte oder überverkaufte Marktbedingungen zu ermitteln.

Um den Indikator verwenden zu können, müssen Sie die Klasse [PrettyGoodOscillator](xref:StockSharp.Algo.Indicators.PrettyGoodOscillator) verwenden.

## Beschreibung

Der Pretty-Good-Oszillator (PGO) ist ein Indikator, der die Stärke des aktuellen Schlusskurses im Verhältnis zu seinen historischen Werten über einen bestimmten Zeitraum bewertet. PGO berücksichtigt nicht nur die Position des aktuellen Preises im historischen Bereich, sondern auch die Volatilität dieses Bereichs und macht ihn so anpassungsfähiger an sich ändernde Marktbedingungen.

Der Name „Pretty-Good-Oszillator“ spiegelt den pragmatischen Ansatz seines Erfinders wider – der Indikator erhebt nicht den Anspruch, ein perfektes Werkzeug zu sein, sondern bietet eine „ziemlich gute“ Möglichkeit, die aktuelle Marktsituation einzuschätzen.

PGO eignet sich besonders zur Identifizierung überkaufter und überverkaufter Bedingungen sowie zur Erkennung von Divergenzen, die einer Trendumkehr vorausgehen können.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Berechnungszeitraum (Standardwert: 14)

## Berechnung

Die Pretty-Good-Oszillator-Berechnung umfasst die folgenden Schritte:

1. Bestimmen Sie den höchsten Höchstwert (höchstes Hoch) und den niedrigsten Tiefstwert (tiefstes Tief) über den angegebenen Zeitraum:
   ```
   höchstes Hoch = Highest(High, Length)
   tiefstes Tief = Lowest(Low, Length)
   ```

2. Berechnen Sie die Standardabweichung der Schlusskurse über den angegebenen Zeitraum:
   ```
   Standardabweichung = StdDev(Close, Length)
   ```

3. Berechnen Sie den Pretty-Good-Oszillator:
   ```
   PGO = (Close - (höchstes Hoch + tiefstes Tief) / 2) / Standardabweichung
   ```

Dabei gilt:
- Close – aktueller Schlusskurs
- High - höchster Preis
- Low - niedrigster Preis
- Length - Berechnungszeitraum
- StdDev - Standardabweichung

## Interpretation

Der Pretty-Good-Oszillator kann wie folgt interpretiert werden:

1. **Überkaufte und überverkaufte Niveaus**:
   - Werte über +2 weisen häufig auf überkaufte Marktbedingungen hin
   - Werte unter -2 weisen häufig auf überverkaufte Marktbedingungen hin
   - Extreme Werte (+3/-3 und darüber/unten) können auf deutlich überkaufte/überverkaufte Bedingungen und eine mögliche Umkehr hinweisen

2. **Nulllinienübergänge**:
   - Das Überqueren der Nulllinie von PGO von unten nach oben kann als bullisches Signal angesehen werden
   - Das Überqueren der Nulllinie von PGO von oben nach unten kann als rückläufiges Signal angesehen werden

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während PGO ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während PGO ein niedrigeres Hoch bildet
   - Divergenzen gehen oft deutlichen Trendumkehrungen voraus

4. **Trendbestätigung**:
   - Positive PGO-Werte zeigen an, dass der Preis über der durchschnittlichen Spanne liegt, was für einen Aufwärtstrend charakteristisch ist
   - Negative PGO-Werte weisen darauf hin, dass der Preis unter der durchschnittlichen Spanne liegt, was für einen Abwärtstrend charakteristisch ist

5. **Bewertung der Trendstärke**:
   - Je weiter der PGO-Wert von Null entfernt ist, desto stärker ist der aktuelle Trend
   - Die Annäherung von PGO an die Nulllinie könnte auf eine Abschwächung des Trends hinweisen

6. **Signalfilterung**:
   - PGO kann zum Filtern von Signalen anderer Indikatoren verwendet werden
   - Berücksichtigen Sie beispielsweise nur bullische Signale, wenn PGO positiv ist, und nur bärische Signale, wenn PGO negativ ist

7. **Positionsausgänge**:
   - Extreme PGO-Werte können als Signale für Gewinnmitnahmen genutzt werden
   - Verlassen Sie beispielsweise Kaufpositionen, wenn PGO +2 überschreitet, und Verkaufspositionen, wenn PGO unter -2 fällt

![PGO Diagramm](../../../../images/indicator_pretty_good_oscillator.png)

## Siehe auch

[RSI](rsi.md)
[Stochastischer Oszillator](stochastic_oscillator.md)
[CCI](cci.md)
[StandardDeviation](standard_deviation.md)
