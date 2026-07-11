# NVI

**Negativer Volumenindex (NVI)** ist ein von Paul Dysart entwickelter technischer Indikator, der sich auf Preisänderungen an Tagen konzentriert, an denen das Handelsvolumen im Vergleich zum Vortag abnimmt.

Um den Indikator verwenden zu können, müssen Sie die Klasse [NegativeVolumeIndex](xref:StockSharp.Algo.Indicators.NegativeVolumeIndex) verwenden.

## Beschreibung

Der Negativer Volumenindex (NVI) basiert auf der Idee, dass „Smart Money“ an Tagen mit geringem Handelsvolumen aktiv ist, während die „Crowd“ (nicht professionelle Händler) an Tagen mit hohem Handelsvolumen aktiver ist. NVI ändert sich nur an Tagen, an denen das aktuelle Volumen niedriger ist als das Volumen des Vortages.

Das Konzept des Indikators legt nahe, dass Preisbewegungen bei reduziertem Volumen bedeutender sind und häufig die Handlungen informierter Anleger widerspiegeln. NVI versucht, diese Bewegungen zu verfolgen und ignoriert Preisänderungen an Tagen mit erhöhtem Volumen.

NVI wird häufig zusammen mit dem komplementären positiven Volume-Index (PVI) verwendet, der im Gegensatz dazu nur Tage mit steigendem Volumen berücksichtigt.

## Berechnung

Die Negativer Volumenindex-Berechnung umfasst die folgenden Schritte:

1. Legen Sie den anfänglichen NVI-Wert fest (normalerweise 1000):
   ```
   NVI[initial] = 1000
   ```

2. Für jede weitere Periode:
   ```
   Wenn Volume[current] < Volume[previous], dann:
       NVI[current] = NVI[previous] * (1 + (Price[current] - Price[previous]) / Price[previous])
   Otherwise:
       NVI[current] = NVI[previous]
   ```

Dabei gilt:
- Price - Preis (normalerweise Schlusskurs)
- Volume - Handelsvolumen

Mit anderen Worten: NVI ändert sich nur an Tagen, an denen das Handelsvolumen abnimmt, und bleibt an Tagen mit steigendem oder unverändertem Handelsvolumen unverändert.

## Interpretation

Der Negativer Volumenindex kann wie folgt interpretiert werden:

1. **Trendanalyse**:
   - Steigender NVI deutet darauf hin, dass „Smart Money“ kauft, was einen zukünftigen Preisanstieg vorhersagen könnte
   - Ein fallender NVI deutet darauf hin, dass „Smart Money“ verkauft wird, was einen zukünftigen Preisrückgang vorhersagen könnte

2. **Verschieben von Average-Frequenzweichen**:
   - NVI wird häufig mit seinem gleitenden 255-Tage-Durchschnitt (ungefähr ein Handelsjahr) verglichen.
   - Wenn NVI über seinem 255-Tage-SMA liegt, gilt dies als bullisches Signal
   - Wenn NVI unter seinem 255-Tage-SMA liegt, gilt dies als rückläufiges Signal

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während NVI ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während NVI ein niedrigeres Hoch bildet

4. **Kombination mit PVI**:
   - Wenn sowohl NVI als auch PVI steigen, ist das ein starkes bullisches Signal
   - Wenn sowohl NVI als auch PVI fallen, ist das ein starkes rückläufiges Signal
   - Wenn NVI steigt und PVI fällt, kann dies auf „Smart Money“-Käufe hinweisen, während die „Masse“ verkauft (möglicherweise bullisches Szenario).
   - Wenn NVI fällt und PVI steigt, kann dies darauf hindeuten, dass „Smart Money“ verkauft, während die „Masse“ kauft (möglicherweise bärisches Szenario).

5. **Langfristige Änderungen**:
   - NVI wird oft als langfristiger Indikator angesehen
   - Eine anhaltende Änderung der NVI-Richtung könnte auf eine deutliche Änderung der Marktstimmung hinweisen

6. **Bestätigung anderer Indikatoren**:
   - NVI funktioniert am besten, wenn es mit anderen technischen Indikatoren und Analysemethoden kombiniert wird
   - NVI-Signale werden zuverlässiger, wenn sie durch andere Indikatoren bestätigt werden

7. **Schwellenwerte festlegen**:
   - Einige Händler legen Schwellenwerte für NVI fest (z. B. 5 % über oder unter dem gleitenden Durchschnitt).
   - Das Überschreiten dieser Schwellenwerte kann als stärkeres Signal angesehen werden als einfache Überkreuzungen

![indicator_negative_volume_index](../../../../images/indicator_negative_volume_index.png)

## Siehe auch

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)
