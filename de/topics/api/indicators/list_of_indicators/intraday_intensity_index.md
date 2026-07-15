# III

**Intraday-Intensitätsindex (III)** ist ein von David Bostian entwickelter technischer Indikator, der die Beziehung zwischen Schlusskurs, Preisspanne und Handelsvolumen innerhalb eines Handelstages bewertet.

Um den Indikator verwenden zu können, müssen Sie die Klasse [IntradayIntensityIndex](xref:StockSharp.Algo.Indicators.IntradayIntensityIndex) verwenden.

## Beschreibung

Der Intraday-Intensitätsindex (III) kombiniert Informationen über Preisbewegungen und Handelsvolumen, um die Intensität des Kauf- oder Verkaufsdrucks innerhalb eines Handelstages zu bewerten. Der Indikator basiert auf der Annahme, dass die Position des Schlusskurses im Verhältnis zur Preisspanne des Tages in Kombination mit dem Volumen Aufschluss über die Richtung und Stärke der Marktbewegung geben kann.

III eignet sich besonders zur Identifizierung von Intraday-Änderungen der Marktstimmung und zur Bestimmung potenzieller Umkehrpunkte. Positive Indikatorwerte deuten auf Kaufdruck hin (Schlusskurs näher am Tageshoch), während negative Werte auf Verkaufsdruck hinweisen (Schlusskurs näher am Tagestief).

Der Intraday-Intensitätsindex ist besonders effektiv für:
- Identifizierung von Intraday-Änderungen der Marktstimmung
- Ermittlung möglicher Umkehrpunkte
- Bestätigung von Signalen anderer Indikatoren
- Einschätzung der Stärke des aktuellen Trends

## Parameter

Der Indikator hat die folgenden Parameter:
- **Länge** – Glättungszeitraum (Standardwert: 14)

## Berechnung

Die Intraday-Intensitätsindex-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie den individuellen III-Wert für jede Periode:
   ```
   roher III = ((2 * Close - High - Low) / ((High - Low) * Volume)) * Volume
   ```

2. Glätten Sie mit einem einfachen gleitenden Durchschnitt:
   ```
   III = SMA(roher III, Length)
   ```

Dabei gilt:
- Close - Schlusskurs
- High – Höchster Preis des Zeitraums
- Low – niedrigster Preis des Zeitraums
- Volume - Handelsvolumen
- SMA – einfacher gleitender Durchschnitt
- Length - Glättungszeitraum

## Interpretation

Der Intraday-Intensitätsindex kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - Der Übergang von negativen zu positiven Werten kann als bullisches Signal angesehen werden, das auf erhöhten Kaufdruck hinweist
   - Der Übergang von positiven zu negativen Werten kann als bärisches Signal angesehen werden, das auf erhöhten Verkaufsdruck hinweist

2. **Extreme Werte**:
   - Hohe positive Werte weisen auf starken Kaufdruck hin
   - Hohe negative Werte weisen auf starken Verkaufsdruck hin
   - Extreme Werte können auf überkaufte oder überverkaufte Marktbedingungen hinweisen

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während III ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während III ein niedrigeres Hoch bildet

4. **III-Trends**:
   - Durchweg positive III-Werte bestätigen einen Aufwärtstrend
   - Durchweg negative III-Werte bestätigen einen Abwärtstrend
   - Schwankungen um die Nulllinie können auf einen Seitwärtstrend oder Unsicherheit hinweisen

5. **Kombination mit anderen Indikatoren**:
   - III wird häufig in Kombination mit anderen technischen Indikatoren zur Bestätigung von Signalen verwendet
   - Besonders effektiv in Kombination mit Trend- und Volumenindikatoren

6. **Wertänderungen**:
   - Ein schneller Wechsel von negativen zu positiven Werten kann auf eine starke Veränderung der Marktstimmung hinweisen
   - Eine allmähliche Annäherung an die Nulllinie könnte auf eine Abschwächung der aktuellen Dynamik hinweisen

![III Diagramm](../../../../images/indicator_intraday_intensity_index.png)

## Siehe auch

[IntradayMomentumIndex](intraday_momentum_index.md)
[BalanceOfPower](balance_of_power.md)
[ForceIndex](force_index.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
