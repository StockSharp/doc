# VI

**Vortex Indicator (VI)** ist ein technischer Indikator, der 2009 von Etienne und Julia Boisse entwickelt wurde. Der Indikator besteht aus zwei Linien, VI+ und VI-, die aufwärts- und abwärtsgerichtete Preisbewegungen anzeigen und dabei helfen, den Beginn neuer Trends zu erkennen und bestehende Trends zu bestätigen.

Um den Indikator zu verwenden, nutzen Sie die Klasse [VortexIndicator](xref:StockSharp.Algo.Indicators.VortexIndicator).

## Beschreibung

Der Vortex Indicator ist von den Prinzipien wirbelartiger Bewegungen in der Natur inspiriert und soll die zyklische Natur von Marktbewegungen abbilden. Er besteht aus zwei Linien:

- **VI+** (positiver Vortex Indicator) - misst aufwärtsgerichtete Preisbewegungen
- **VI-** (negativer Vortex Indicator) - misst abwärtsgerichtete Preisbewegungen

Wichtige Indikatorsignale:
- Kaufen, wenn VI+ VI- von unten nach oben kreuzt
- Verkaufen, wenn VI- VI+ von unten nach oben kreuzt
- Der Abstand zwischen den Linien zeigt die Trendstärke an

Der Vortex Indicator ist besonders nützlich für:
- Bestimmung des Beginns neuer Trends
- Einschätzung der Stärke eines bestehenden Trends
- Identifikation möglicher Wendepunkte

## Parameter

- **Length** - Berechnungsperiode, typischerweise mit dem Wert 14.

## Berechnung

Die Berechnung des Vortex Indicator erfolgt in mehreren Schritten:

1. Positive und negative Bewegung berechnen:
   ```
   VM+ = |Current High - Previous Low|
   VM- = |Current Low - Previous High|
   ```

2. True Range berechnen:
   ```
   TR = Max(High - Low, |High - Previous Close|, |Low - Previous Close|)
   ```

3. Werte von VM+ und VM- über die Length-Periode summieren:
   ```
   Sum_VM+ = Sum(VM+, Length)
   Sum_VM- = Sum(VM-, Length)
   ```

4. True Range über die Length-Periode summieren:
   ```
   Sum_TR = Sum(TR, Length)
   ```

5. Normalisierte Werte VI+ und VI- berechnen:
   ```
   VI+ = Sum_VM+ / Sum_TR
   VI- = Sum_VM- / Sum_TR
   ```

Das Kreuzen dieser beiden Linien erzeugt Handelssignale: Wenn VI+ über VI- steigt, signalisiert dies einen bullischen Trend; umgekehrt signalisiert ein Anstieg von VI- über VI+ einen bärischen Trend.

![IndicatorVortexIndicator](../../../../images/indicator_vortex_indicator.png)

## Siehe auch

[ADX](adx.md)
[DMI](dmi.md)

