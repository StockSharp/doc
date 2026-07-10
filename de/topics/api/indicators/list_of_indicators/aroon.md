# Aroon

Der **Aroon-Indikator** ist ein technischer Indikator, der 1995 von Tushar Chande entwickelt wurde, um Trendwechsel und Trendstärke zu erkennen. Der Name "Aroon" stammt aus dem Sanskrit und bedeutet "Morgendämmerung einer neuen Ära".

Zur Verwendung des Indikators müssen Sie die Klasse [Aroon](xref:StockSharp.Algo.Indicators.Aroon) verwenden.

## Beschreibung

Der Aroon-Indikator besteht aus zwei Linien:
- **Aroon Up** - misst die Stärke eines Aufwärtstrends
- **Aroon Down** - misst die Stärke eines Abwärtstrends

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

1. Aroon Up wird mit der Formel berechnet:
   ```
   Aroon Up = ((Length - Periods since high) / Length) * 100
   ```

2. Aroon Down wird mit der Formel berechnet:
   ```
   Aroon Down = ((Length - Periods since low) / Length) * 100
   ```

Wobei:
- Length - ausgewählte Periode
- "Periods since high" - Anzahl der Perioden seit Erreichen des höchsten Preises innerhalb der Length-Periode
- "Periods since low" - Anzahl der Perioden seit Erreichen des niedrigsten Preises innerhalb der Length-Periode

Beide Aroon-Linien oszillieren zwischen 0 und 100:
- Ein Wert von 100 bedeutet, dass das Hoch/Tief in der jüngsten Periode erreicht wurde
- Ein Wert von 0 bedeutet, dass das Hoch/Tief vor Length Perioden erreicht wurde

## Interpretation

- **Starker Aufwärtstrend**: Aroon Up liegt nahe 100 und Aroon Down nahe 0
- **Starker Abwärtstrend**: Aroon Down liegt nahe 100 und Aroon Up nahe 0
- **Seitwärtsbewegung**: beide Linien bewegen sich auf niedrigen Niveaus parallel zueinander
- **Potenzielle Trendumkehr**: Kreuzung der Linien Aroon Up und Aroon Down
- **Konsolidierung**: beide Linien oszillieren um 50

![indicator_aroon](../../../../images/indicator_aroon.png)

## Siehe auch

[ADX](adx.md)
[DMI](dmi.md)
