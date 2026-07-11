# PVI

**Positiver Volumenindex (PVI)** ist ein von Paul Dysart entwickelter technischer Indikator, der sich auf Preisänderungen an Tagen konzentriert, an denen das Handelsvolumen im Vergleich zum Vortag steigt.

Um den Indikator verwenden zu können, müssen Sie die Klasse [PositiveVolumeIndex](xref:StockSharp.Algo.Indicators.PositiveVolumeIndex) verwenden.

## Beschreibung

Der Positiver Volumenindex (PVI) basiert auf der Idee, dass die „Crowd“ (nicht professionelle Händler) an Tagen mit hohem Handelsvolumen aktiver ist, während „Smart Money“ an Tagen mit geringem Handelsvolumen agiert. PVI ändert sich nur an Tagen, an denen das aktuelle Volumen höher ist als das Volumen des Vortages.

Der Indikator deutet darauf hin, dass die Preisbewegungen bei steigendem Volumen erheblich sind und häufig die Stimmung auf dem Massenmarkt widerspiegeln. PVI verfolgt diese Bewegungen und ignoriert Preisänderungen an Tagen mit geringerem Volumen.

PVI wird häufig zusammen mit dem komplementären negativen Volume-Index (NVI) verwendet, der im Gegensatz dazu nur Tage berücksichtigt, an denen das Volumen abnimmt.

## Berechnung

Die Positiver Volumenindex-Berechnung umfasst die folgenden Schritte:

1. Legen Sie den anfänglichen PVI-Wert fest (normalerweise 1000):
   ```
   PVI[initial] = 1000
   ```

2. Für jede weitere Periode:
   ```
   Wenn Volume[current] > Volume[previous], dann:
       PVI[current] = PVI[previous] * (1 + (Price[current] - Price[previous]) / Price[previous])
   Otherwise:
       PVI[current] = PVI[previous]
   ```

Dabei gilt:
- Price - Preis (normalerweise Schlusskurs)
- Volume - Handelsvolumen

Mit anderen Worten: PVI ändert sich nur an Tagen, an denen das Handelsvolumen steigt, und bleibt an Tagen mit sinkendem oder unverändertem Handelsvolumen unverändert.

## Interpretation

Der Positiver Volumenindex kann wie folgt interpretiert werden:

1. **Trendanalyse**:
   - Steigender PVI deutet darauf hin, dass die „Masse“ kauft, was einen zukünftigen Preisanstieg vorhersagen könnte
   - Ein fallender PVI deutet darauf hin, dass die „Masse“ verkauft, was einen zukünftigen Preisrückgang vorhersagen könnte

2. **Verschieben von Average-Frequenzweichen**:
   - PVI wird häufig mit seinem gleitenden 255-Tage-Durchschnitt (ungefähr ein Handelsjahr) verglichen.
   - Wenn PVI über seinem 255-Tage-SMA liegt, gilt dies als bullisches Signal
   - Wenn PVI unter seinem 255-Tage-SMA liegt, gilt dies als rückläufiges Signal

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während PVI ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während PVI ein niedrigeres Hoch bildet

4. **Kombination mit NVI**:
   - Wenn sowohl PVI als auch NVI steigen, ist das ein starkes bullisches Signal
   - Wenn sowohl PVI als auch NVI fallen, ist das ein starkes rückläufiges Signal
   - Wenn PVI steigt und NVI fällt, kann dies darauf hindeuten, dass „Massenkäufe“ erfolgen, während „Smart Money“ verkauft (potenziell bullisches Szenario).
   - Wenn PVI fällt und NVI steigt, kann dies darauf hindeuten, dass die „Menge“ verkauft, während „Smart Money“ kauft (potenziell rückläufiges Szenario).

5. **Langfristige Änderungen**:
   - PVI wird oft als langfristiger Indikator angesehen
   - Eine anhaltende Änderung der PVI-Richtung könnte auf eine deutliche Änderung der Marktstimmung hinweisen

6. **Bestätigung anderer Indikatoren**:
   - PVI funktioniert am besten, wenn es mit anderen technischen Indikatoren und Analysemethoden kombiniert wird
   - PVI-Signale werden zuverlässiger, wenn sie durch andere Indikatoren bestätigt werden

7. **Schwellenwerte festlegen**:
   - Einige Händler legen Schwellenwerte für PVI fest (z. B. 5 % über oder unter dem gleitenden Durchschnitt).
   - Das Überschreiten dieser Schwellenwerte kann als stärkeres Signal angesehen werden als einfache Überkreuzungen

![indicator_positive_volume_index](../../../../images/indicator_positive_volume_index.png)

## Siehe auch

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)
[NegativeVolumeIndex](negative_volume_index.md)
