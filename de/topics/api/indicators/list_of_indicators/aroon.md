# Aroon

Der **Aroon-Indikator** ist ein technischer Indikator, der 1995 von Tushar Chande entwickelt wurde, um Trendwechsel und Trendstärke zu erkennen. Der Name "Aroon" stammt aus dem Sanskrit und bedeutet "Morgendämmerung einer neuen Ära".

Zur Verwendung des Indikators müssen Sie die Klasse [Aroon](xref:StockSharp.Algo.Indicators.Aroon) verwenden.

## Beschreibung

Der Aroon-Indikator besteht aus zwei Linien:
- **Aroon-Aufwärts** - misst die Stärke eines Aufwärtstrends
- **Aroon-Abwärts** - misst die Stärke eines Abwärtstrends

Aroon hilft beim Bestimmen von:
- Beginn eines neuen Trends
- Stärke des aktuellen Trends
- Konsolidierung und Seitwärtsbewegung
- Potenziellen Trendumkehrungen

Der Indikator ist besonders nützlich, um frühe Phasen einer neuen Trendbildung und Konsolidierungsperioden zu erkennen.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** - Berechnungsperiode (typischerweise werden 14-25 Perioden verwendet)

## Berechnung

Die Berechnung des Aroon-Indikators basiert darauf, die Zeit (Anzahl der Perioden) seit dem Erreichen des höchsten bzw. niedrigsten Preises innerhalb der angegebenen Periode zu bestimmen:

1. Aroon-Aufwärts wird mit der Formel berechnet:
   ```
   Aroon-Aufwärts = ((Length - Perioden seit Hoch) / Length) * 100
   ```

2. Aroon-Abwärts wird mit der Formel berechnet:
   ```
   Aroon-Abwärts = ((Length - Perioden seit Tief) / Length) * 100
   ```

Wobei:
- Length - ausgewählte Periode
- "Perioden seit Hoch" - Anzahl der Perioden seit Erreichen des höchsten Preises innerhalb der Length-Periode
- "Perioden seit Tief" - Anzahl der Perioden seit Erreichen des niedrigsten Preises innerhalb der Length-Periode

Beide Aroon-Linien oszillieren zwischen 0 und 100:
- Ein Wert von 100 bedeutet, dass das Hoch/Tief in der jüngsten Periode erreicht wurde
- Ein Wert von 0 bedeutet, dass das Hoch/Tief vor Length Perioden erreicht wurde

## Interpretation

- **Starker Aufwärtstrend**: Aroon-Aufwärts liegt nahe 100 und Aroon-Abwärts nahe 0
- **Starker Abwärtstrend**: Aroon-Abwärts liegt nahe 100 und Aroon-Aufwärts nahe 0
- **Seitwärtsbewegung**: beide Linien bewegen sich auf niedrigen Niveaus parallel zueinander
- **Potenzielle Trendumkehr**: Kreuzung der Linien Aroon-Aufwärts und Aroon-Abwärts
- **Konsolidierung**: beide Linien oszillieren um 50

![Aroon Diagramm](../../../../images/indicator_aroon.png)

## Siehe auch

[ADX](adx.md)
[DMI](dmi.md)
