# CHOP

**Choppiness Index (CHOP)** ist ein Indikator, der ermitteln soll, ob sich der Markt in einer Seitwärtsbewegung (innerhalb einer Spanne) oder in einem Trendzustand befindet.

Um den Indikator verwenden zu können, müssen Sie die Klasse [ChoppinessIndex](xref:StockSharp.Algo.Indicators.ChoppinessIndex) verwenden.

## Beschreibung

Der Choppiness Index (CHOP) wurde entwickelt, um die Volatilität quantitativ zu bewerten und die Art der Marktbewegung zu bestimmen. Im Gegensatz zu vielen anderen Indikatoren ist CHOP nicht dazu gedacht, eine Trendrichtung zu erkennen oder Kauf- oder Verkaufssignale zu generieren. Stattdessen hilft es Händlern zu bestimmen, ob sich der Markt in einer Konsolidierung (Seitwärtsbewegung) oder in einem Richtungstrend befindet.

Der CHOP-Indikator schwankt zwischen 0 und 100:
- Werte näher an 100 deuten auf eine starke Konsolidierung hin (hohe „Abhackigkeit“).
- Werte näher bei 0 weisen auf einen starken Richtungstrend hin (geringe „Abgehacktheit“)

CHOP ist besonders nützlich für:
- Bestimmung einer geeigneten Handelsstrategie basierend auf dem Marktcharakter
- Identifizieren von Übergängen von Seitwärtsbewegung zu Trend und umgekehrt
- Bestätigung oder Widerlegung von Signalen anderer Indikatoren
- Vermeidung falscher Signale bei der Konsolidierung

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Berechnungszeitraum (Standardwert: 14)

## Berechnung

Die Choppiness Index-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie die Summe der wahren Reichweiten über den ausgewählten Zeitraum:
   ```
   Sum of TR = Sum(TR(i)) für i von 1 bis Length
   ```

2. Berechnen Sie den höchsten High und den niedrigsten Low über den ausgewählten Zeitraum:
   ```
   Highest High = maximaler High-Wert über Length-Periode
   Lowest Low = minimaler Low-Wert über Length-Periode
   ```

3. Berechnen Sie den CHOP-Index:
   ```
   CHOP = 100 * LOG10(Sum TR / (Highest High - Lowest Low)) / LOG10(Length)
   ```

Dabei gilt:
- TR – wahre Spanne für jede Kerze
- Length - ausgewählter Zeitraum
- LOG10 - Dezimallogarithmus

## Interpretation

- **High CHOP-Werte (über 60-70)** zeigen an, dass sich der Markt in einer Seitwärtsbewegung (Konsolidierung) befindet. In dieser Zeit ist es besser, Trendstrategien zu vermeiden und Range-Trading-Strategien in Betracht zu ziehen.

- **Niedrige CHOP-Werte (unter 30-40)** weisen auf einen starken Richtungstrend hin. Dies ist ein guter Zeitpunkt, Trendstrategien anzuwenden und Preisbewegungen zu verfolgen.

- **Übergänge zwischen hohen und niedrigen Werten** können auf eine Veränderung des Marktcharakters hinweisen. Ein Rückgang des CHOP von hohen Werten könnte den Beginn eines neuen Trends signalisieren. Ein CHOP-Anstieg ausgehend von niedrigen Werten könnte ein Anzeichen für eine Erschöpfung des Trends und einen Übergang zur Konsolidierung sein.

- **Schwellenwerte festlegen**: Typischerweise werden die folgenden Schwellenwerte verwendet:
  - Über 60-70: High „abgehacktes Gefühl“ (Seitwärtsbewegung)
  - 30-60: Mäßiges „Rubbeln“ (Übergangszustand)
  - Unter 30: Low „abgehackt“ (starker Trend)

![indicator_choppiness_index](../../../../images/indicator_choppiness_index.png)

## Siehe auch

[ATR](atr.md)
[ADX](adx.md)
[VHF](vhf.md)
[TrueRange](true_range.md)
