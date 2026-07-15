# CGO

**Schwerpunkt-Oszillator (CGO)** ist ein von John Ehlers entwickelter technischer Indikator, der auf dem Konzept des Schwerpunkts in der Physik basiert und auf die Analyse von Preisbewegungen auf dem Markt angewendet wird.

Um den Indikator verwenden zu können, müssen Sie die Klasse [CenterOfGravityOscillator](xref:StockSharp.Algo.Indicators.CenterOfGravityOscillator) verwenden.

## Beschreibung

Der Schwerpunkt-Oszillator (CGO) ist ein Frühindikator, der versucht, Marktumkehrpunkte zu identifizieren, indem er die Preisreihe als physikalisches System behandelt und seinen „Schwerpunkt“ bestimmt. Der Indikator berechnet, wo sich das „Gleichgewicht“ der aktuellen Preisbewegungen befindet, und nutzt diese Informationen, um zukünftige Trendrichtungsänderungen vorherzusagen.

CGO ist besonders nützlich für:
- Identifizieren potenzieller Umkehrpunkte, bevor sie auf dem Preisdiagramm erscheinen
- Aufdecken der Stärken und Schwächen des aktuellen Trends
- Erkennen versteckter Abweichungen zwischen Preis und Indikator
- Erstellen von Handelssystemen basierend auf führenden Signalen

## Parameter

Der Indikator hat die folgenden Parameter:
- **Länge** – Berechnungszeitraum (Standardwert: 10)

## Berechnung

Die Schwerpunkt-Oszillator (CGO)-Berechnung basiert auf der Formel:

```
CGO = - Sum(Price(i) * (i + 1)) / Sum(Price(i))
```

Dabei gilt:
- i - Preiswertindex im Zeitraum von 0 bis (Length-1)
- Price(i) – Preis (normalerweise Schlusskurs) für den entsprechenden Index i
- Sum – Summe aller Werte im Length-Zeitraum

In dieser Formel wird jeder Preis anhand seiner Position in der Zeitreihe gewichtet und dann mit der Gesamtsumme der Preise normalisiert. Das Minuszeichen vor der Formel wird hinzugefügt, um den Indikator bei steigendem Preis ansteigen zu lassen und ihn so intuitiver zu machen.

## Interpretation

- **Nullliniendurchgang**: Wenn CGO die Nulllinie von unten nach oben kreuzt, kann dies als bullisches Signal angesehen werden. Ein Übergang von oben nach unten kann auf ein rückläufiges Signal hinweisen.

- **Indikator-Extreme**: Wenn CGO Extreme (Höchst- oder Tiefstwerte) erreicht, kann dies auf eine mögliche Trendumkehr hinweisen.

- **Abweichungen**:
  - Bullische Divergenz: Wenn der Preis ein neues Tief bildet, CGO dies jedoch nicht bestätigt und ein höheres Tief bildet.
  - Bärische Divergenz: Wenn der Preis ein neues Hoch erreicht, CGO jedoch ein niedrigeres Hoch bildet.

- **Indikatorbewegung**: Eine schnelle CGO-Bewegung in eine Richtung kann den Beginn eines neuen Trends anzeigen. Wenn sich der Indikator langsam bewegt oder um die Nulllinie schwankt, kann dies auf eine Marktkonsolidierung hinweisen.

Da es sich bei CGO um einen Frühindikator handelt, erscheinen seine Signale häufig vor entsprechenden Änderungen im Preisdiagramm, was Händlern einen Vorteil bei Handelsentscheidungen verschafft.

![CGO Diagramm](../../../../images/indicator_center_of_gravity_oscillator.png)

## Siehe auch

[SineWave](sine_wave.md)
[HarmonicOscillator](harmonic_oscillator.md)
[FisherTransform](ehlers_fisher_transform.md)
[RVI](rvi.md)
