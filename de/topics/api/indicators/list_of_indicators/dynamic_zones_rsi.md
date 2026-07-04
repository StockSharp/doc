# DZRSI

**Dynamic Zones RSI (DZRSI)** ist eine Modifikation des klassischen Relative Strength Index (RSI), der sich dynamisch ändernde überkaufte und überverkaufte Niveaus anstelle statischer Werte verwendet.

Um den Indikator verwenden zu können, müssen Sie die Klasse [DynamicZonesRSI](xref:StockSharp.Algo.Indicators.DynamicZonesRSI) verwenden.

## Beschreibung

Der Dynamic Zones RSI (DZRSI) basiert auf dem traditionellen RSI, weist jedoch eine wichtige Verbesserung auf: Anstatt feste Überkauf- und Überverkauft-Werte zu verwenden (normalerweise 70 und 30), passt der DZRSI diese Werte an die aktuellen Marktbedingungen an.

Die Grundidee von DZRSI besteht darin, dass unterschiedliche Marktbedingungen unterschiedliche Schwellenwerte für die Bestimmung von überkauften und überverkauften Zuständen erfordern. In einem starken Aufwärtstrend kann RSI über einen längeren Zeitraum über dem traditionellen überkauften Niveau von 70 bleiben, ohne genaue Ein- oder Ausstiegssignale zu liefern. Ebenso kann RSI in einem starken Abwärtstrend lange Zeit unter dem überverkauften Niveau von 30 bleiben.

DZRSI löst dieses Problem, indem es diese Niveaus basierend auf dem historischen Verhalten des RSI selbst dynamisch anpasst, wodurch der Indikator anpassungsfähiger an verschiedene Marktregime wird.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Zeitraum zur Berechnung der Basis RSI (Standardwert: 14)
- **OverboughtLevel** – anfängliches Überkaufniveau (Standardwert: 70)
- **OversoldLevel** – anfängliches überverkauftes Niveau (Standardwert: 30)

## Berechnung

Die DZRSI-Berechnung umfasst mehrere Schritte:

1. Berechnen Sie den Standard-RSI über den angegebenen Length-Zeitraum:
   ```
   RSI = 100 - (100 / (1 + RS))
   RS = Average Positive Change / Average Negative Change
   ```

2. Bestimmen Sie den RSI-Schwingungsbereich über einen bestimmten historischen Zeitraum.

3. Passen Sie die überkauften und überverkauften Niveaus basierend auf dieser Spanne an:
   ```
   Dynamic Overbought Level = Base Overbought Level + Adjustment Based on Historical Data
   Dynamic Oversold Level = Base Oversold Level - Adjustment Based on Historical Data
   ```

4. Passen Sie dynamische Zonen basierend auf der Stärke des aktuellen Trends an.

## Interpretation

DZRSI wird ähnlich wie herkömmliches RSI interpretiert, jedoch unter Berücksichtigung dynamischer Zonen:

1. **Überkauft- und Überverkauft-Signale**:
   - Wenn DZRSI über das aktuelle dynamische überkaufte Niveau steigt, kann dies auf überkaufte Bedingungen am Markt hinweisen
   - Wenn DZRSI unter das aktuelle dynamische überverkaufte Niveau fällt, kann dies auf überverkaufte Bedingungen am Markt hinweisen

2. **Umkehrsignale**:
   - Eine Umkehr vom dynamischen Überkauft-Niveau nach unten kann als Verkaufssignal angesehen werden
   - Eine Umkehr vom dynamischen überverkauften Niveau nach oben kann als Kaufsignal gewertet werden

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während DZRSI ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während DZRSI ein niedrigeres Hoch bildet

4. **Trendanalyse**:
   - Bei einem Aufwärtstrend kann das dynamische überverkaufte Niveau höher als die traditionellen 30 sein
   - Bei einem Abwärtstrend kann das dynamische überkaufte Niveau unter den traditionellen 70 liegen

5. **Mittellinien-(50)-Überkreuzungen**:
   - Ein Übergang von unten nach oben kann als bullisches Signal angesehen werden
   - Ein Übergang von oben nach unten kann als bärisches Signal angesehen werden

![indicator_dynamic_zones_rsi](../../../../images/indicator_dynamic_zones_rsi.png)

## Siehe auch

[RSI](rsi.md)
[ConnorsRSI](connors_rsi.md)
[LRSI](laguerre_rsi.md)
