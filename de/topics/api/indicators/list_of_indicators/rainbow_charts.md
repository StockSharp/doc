# RC

**Regenbogencharts (RC)** ist ein Indikator für die technische Analyse, der aus einer Reihe gleitender Durchschnitte mit unterschiedlichen Zeiträumen besteht, die in einem einzigen Diagramm angezeigt werden. Optisch ähnelt der Indikator einem Regenbogen, daher der Name.

Um den Indikator verwenden zu können, müssen Sie die Klasse [RainbowCharts](xref:StockSharp.Algo.Indicators.RainbowCharts) verwenden.

## Beschreibung

Regenbogencharts basieren auf der Verwendung mehrerer gleitender Durchschnitte (normalerweise einfacher SMA) mit progressiv zunehmenden Perioden. Verschiedene gleitende Durchschnittslinien sind in unterschiedlichen Farben eingefärbt, wodurch ein Regenbogeneffekt auf dem Diagramm entsteht.

Der Indikator hilft bei der Bestimmung der Trendrichtung und -stärke:
- Wenn die Linien divergieren, deutet dies auf eine Trendverstärkung hin
- Wenn Linien zusammenlaufen, kann dies auf eine Abschwächung des Trends oder eine mögliche Umkehr hinweisen
- Wenn der Preis über allen Linien liegt, deutet dies auf einen starken Aufwärtstrend hin
- Wenn der Preis unter allen Linien liegt, deutet dies auf einen starken Abwärtstrend hin

## Parameter

- **Lines** – Anzahl der im Regenbogendiagramm verwendeten gleitenden Durchschnitte SMA.

## Berechnung

Regenbogencharts bestehen aus mehreren gleitenden Durchschnitten (SMA), wobei sich die Periode jeder nachfolgenden Linie um einen bestimmten Schritt erhöht. Für n Zeilen mit einer Basisperiode p werden die Perioden wie folgt berechnet:

```
Period(i) = p + i * step
```

Dabei gilt:
- i - Zeilennummer (von 0 bis n-1)
- Schritt – Periodenerhöhungsschritt (normalerweise 1)

![IndicatorRainbowCharts](../../../../images/indicator_rainbow_charts.png)

## Siehe auch

[SMA](sma.md)
