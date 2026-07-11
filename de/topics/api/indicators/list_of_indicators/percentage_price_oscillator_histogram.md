# PPO-Histogramm

**PPO-Histogramm (PPOH)** zeigt den Abstand zwischen der PPO-Linie und ihrer Signallinie als Histogramm an und hilft Händlern so, das Momentum-Gleichgewicht sofort zu beurteilen.

Um den Indikator zu verwenden, verwenden Sie die Klasse [PercentagePriceOscillatorHistogram](xref:StockSharp.Algo.Indicators.PercentagePriceOscillatorHistogram).

## Beschreibung

Das PPO-Histogramm ist vom Standard-PPO abgeleitet. Anstatt sowohl die PPO- als auch die Signallinie darzustellen, wird deren Differenz als Balken um den Nullpegel herum visualisiert. Positive Balken zeigen an, dass die PPO-Linie über der Signallinie liegt (bullisches Momentum), während negative Balken zeigen, dass die PPO-Linie unter der Signallinie liegt (bärisches Momentum).

Das Histogramm reagiert schnell auf Änderungen in der Spanne zwischen PPO und der Signallinie und eignet sich daher zum Erkennen früher Verschiebungen der Trendstärke oder zum Erkennen von Momentumdivergenzen.

## Berechnung

1. Berechnen Sie die PPO-Linie und ihre Signallinie mit den gewünschten Perioden.
2. Subtrahieren Sie die Signallinie von der PPO-Linie, um den Histogrammwert zu erhalten.

```
Histogram = PPO - Signallinie
```

Werte über Null verdeutlichen den Aufwärtsdruck, während Werte unter Null den Abwärtsdruck widerspiegeln. Die Geschwindigkeit, mit der sich die Histogrammbalken ausdehnen oder zusammenziehen, gibt Hinweise auf die Impulsbeschleunigung oder -verzögerung.

## Interpretation

- **Nulllinienübergänge.** Eine Bewegung über Null bestätigt, dass die PPO-Linie die Signallinie überschritten hat, was auf eine zinsbullische Verschiebung hindeutet. Ein Rückgang unter Null weist auf einen rückläufigen Crossover hin.
- **Momentum steigt.** Das schnelle Wachstum positiver Balken deutet auf eine Verstärkung der Aufwärtsdynamik hin; Schrumpfende Balken deuten auf eine nachlassende Stärke und eine mögliche Umkehr hin.
- **Divergenzen.** Divergenzen zwischen der Preisbewegung und dem Histogramm können Händler auf eine mögliche Trenderschöpfung aufmerksam machen, bevor sie in den Preisdiagrammen sichtbar wird.

![PPO-Histogramm Diagramm](../../../../images/indicator_percentage_price_oscillator_histogram.png)

## Siehe auch

- [PPO](percentage_price_oscillator.md)
- [PPO-Signal](percentage_price_oscillator_signal.md)
