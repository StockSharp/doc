# MCO

**McClellan Oscillator (MCO)** ist ein von Sherman und Marian McClellan entwickelter technischer Indikator, der die Marktbreite misst, indem er die Differenz zwischen den gleitenden Durchschnitten steigender und fallender Aktien analysiert.

Um den Indikator verwenden zu können, müssen Sie die Klasse [McClellanOscillator](xref:StockSharp.Algo.Indicators.McClellanOscillator) verwenden.

## Beschreibung

Der McClellan Oscillator (MCO) ist einer der bekanntesten Marktbreitenindikatoren, der dabei hilft, die allgemeine Marktlage zu beurteilen und potenzielle Umkehrpunkte zu identifizieren. Es wurde 1969 entwickelt und ist seitdem für viele technische Analysten zu einem wichtigen Werkzeug geworden.

MCO basiert auf der Analyse des Verhältnisses zwischen der Anzahl steigender und fallender Aktien auf dem Markt. Der Indikator berechnet die Differenz zwischen den exponentiellen gleitenden Durchschnitten der Nettozuwächse über 19 und 39 Perioden (Differenz zwischen der Anzahl der steigenden und fallenden Aktien).

Der McClellan Oscillator ist besonders nützlich für:
- Bestimmung der Gesamtmarktrichtung
- Identifizieren von überkauften und überverkauften Bedingungen
- Identifizieren potenzieller Umkehrpunkte
- Bestätigung der Stärke oder Schwäche des aktuellen Trends

## Berechnung

Die McClellan Oscillator-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie die Nettovorschüsse für jeden Handelstag:
   ```
   Net Advances = Advances - Declines
   ```
   Dabei ist Advances die Anzahl steigender Aktien und Declines die Anzahl fallender Aktien.

2. Berechnen Sie den exponentiellen gleitenden Durchschnitt der Nettovorschüsse über 19 Perioden:
   ```
   EMA19 = EMA(Net Advances, 19)
   ```

3. Berechnen Sie den exponentiellen gleitenden Durchschnitt der Nettovorschüsse über 39 Perioden:
   ```
   EMA39 = EMA(Net Advances, 39)
   ```

4. Berechnen Sie McClellan Oscillator als Differenz zwischen diesen beiden EMAs:
   ```
   MCO = EMA19 - EMA39
   ```

## Interpretation

Der McClellan Oscillator kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - Das Überqueren der Nulllinie von unten nach oben durch MCO kann als zinsbullisches Signal gewertet werden, das auf einen möglichen Beginn eines Aufwärtstrends hinweist
   - Das Überqueren der Nulllinie von MCO von oben nach unten kann als rückläufiges Signal angesehen werden, das auf einen möglichen Beginn eines Abwärtstrends hinweist

2. **Extreme Werte**:
   - Werte über +100 weisen häufig auf überkaufte Marktbedingungen hin
   - Werte unter -100 weisen häufig auf überverkaufte Marktbedingungen hin
   - Extreme Werte (+150/-150 und darüber/unten) können auf eine mögliche Marktumkehr hinweisen

3. **Abweichungen**:
   - Bullische Divergenz: Der Index bildet ein neues Tief, während der MCO ein höheres Tief bildet
   - Bärische Divergenz: Der Index bildet ein neues Hoch, während der MCO ein niedrigeres Hoch bildet

4. **Marktbreitenstatus**:
   - Positive MCO-Werte deuten darauf hin, dass die meisten Aktien auf dem Markt steigen
   - Negative MCO-Werte weisen darauf hin, dass die meisten Aktien auf dem Markt fallen

5. **Bewegungsbeschleunigung/-verzögerung**:
   - Steigende MCO-Werte (positiv oder negativ) weisen auf eine Beschleunigung der aktuellen Marktbewegung hin
   - Sinkende MCO-Werte deuten auf eine Verlangsamung der aktuellen Marktbewegung hin

6. **Kombination mit McClellan-Summationsindex**:
   - Der McClellan-Summationsindex (MSI) ist die kumulative Summe der MCO-Werte
   - Der Nulldurchgang von MSI kann MCO-Signale bestätigen und langfristige Trendänderungen anzeigen

7. **Bulnische/bärische Muster**:
   - „Bullischer Ausläufer“ – schneller MCO-Rückgang, gefolgt von einer schnellen Erholung, was oft auf einen möglichen Markttiefpunkt hindeutet
   - „Bärischer Ausläufer“ – schneller MCO-Anstieg, gefolgt von einem schnellen Rückgang, was oft auf ein potenzielles Markthoch hindeutet

![indicator_mcclellan_oscillator](../../../../images/indicator_mcclellan_oscillator.png)

## Siehe auch

[HighLowIndex](high_low_index.md)
