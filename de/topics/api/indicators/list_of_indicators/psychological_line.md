# PSY

**Psychologische Linie (PSY)** ist ein technischer Indikator, der den Anteil steigender Perioden (Kerzen, Balken) im Verhältnis zur Gesamtzahl der Perioden über ein bestimmtes Zeitintervall misst.

Um den Indikator verwenden zu können, müssen Sie die Klasse [PsychologicalLine](xref:StockSharp.Algo.Indicators.PsychologicalLine) verwenden.

## Beschreibung

Der Psychologische Linie (PSY) ist ein einfacher, aber effektiver Indikator, der die Marktstimmung widerspiegelt, indem er den Prozentsatz der Preisanstiegsperioden im Vergleich zur Gesamtzahl der betrachteten Perioden berechnet. Der Indikator basiert auf der Annahme, dass die Marktpsychologie und die Anlegerstimmung eine entscheidende Rolle bei Preisbewegungen spielen.

PSY ist ein Oszillator mit Werten zwischen 0 und 100, wobei:
- Ein Wert von 100 bedeutet, dass der Preis in allen betrachteten Zeiträumen gestiegen ist
- Ein Wert von 0 bedeutet, dass der Preis in allen betrachteten Zeiträumen gefallen ist
- Ein Wert von 50 bedeutet eine gleiche Anzahl an steigenden und fallenden Perioden

Der PSY-Indikator hilft festzustellen, ob sich der Markt in einem überkauften oder überverkauften Zustand befindet, und kann mögliche Trendumkehrungen vorhersagen.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Berechnungszeitraum (Standardwert: 12-14)

## Berechnung

Die Psychologische Linie-Berechnung ist sehr einfach:

```
PSY = (Anzahl steigender Perioden über Length-Perioden / Length) * 100
```

Dabei gilt:
- Eine steigende Periode ist als eine Periode definiert, in der der Schlusskurs höher ist als der Schlusskurs der vorherigen Periode
- Length - Anzahl der berücksichtigten Perioden

## Interpretation

Der Psychologische Linie kann wie folgt interpretiert werden:

1. **Überkaufte und überverkaufte Niveaus**:
   - Werte über 70-80 weisen auf überkaufte Bedingungen am Markt hin (zu viele Perioden waren gestiegen)
   - Werte unter 20–30 deuten auf überverkaufte Marktbedingungen hin (zu viele Perioden fielen)
   - Extremwerte gehen oft Trendumkehrungen voraus

2. **Mittellinie (50)**:
   - Das Überschreiten der 50-Marke von unten nach oben kann als bullisches Signal gewertet werden
   - Das Überschreiten der 50-Marke von oben nach unten kann als bärisches Signal gewertet werden
   - Eine anhaltende Bewegung über 50 deutet auf eine Bullendominanz hin
   - Eine anhaltende Bewegung unter 50 deutet auf eine Bärendominanz hin

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während PSY ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während PSY ein niedrigeres Hoch bildet

4. **Abpraller von extremen Niveaus**:
   - Eine Umkehr des PSY aus der überkauften Zone könnte auf eine mögliche bärische Umkehr hinweisen
   - Die Umkehr des PSY aus der überverkauften Zone könnte auf eine mögliche bullische Umkehr hinweisen

5. **Trendanalyse**:
   - In einem starken Aufwärtstrend bleibt PSY oft über 50, mit periodischen Erholungen aus der überkauften Zone
   - In einem starken Abwärtstrend bleibt PSY oft unter 50, mit periodischen Erholungen aus der überverkauften Zone

6. **Optimierung des Längenparameters**:
   - Kürzere Zeiträume (z. B. 5-8) machen PSY empfindlicher und für den kurzfristigen Handel geeignet
   - Längere Zeiträume (z. B. 20-25) machen PSY reibungsloser und für den langfristigen Handel geeignet

7. **Kombination mit anderen Indikatoren**:
   - PSY wird häufig in Kombination mit anderen Indikatoren zur Bestätigung von Signalen verwendet
   - Besonders nützlich in Kombination mit Trendindikatoren und Volumenindikatoren

![PSY](../../../../images/indicator_psychological_line.png)

## Siehe auch

[RSI](rsi.md)
[Stochastischer Oszillator](stochastic_oscillator.md)
[UltimateOscillator](uo.md)
[MomentumOscillator](momentum.md)
