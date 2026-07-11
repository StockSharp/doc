# SuperTrend

**Der SuperTrend-Indikator** ist ein Trendfolgeindikator, der auf der durchschnittlichen wahren Spanne (ATR) basiert. Er hilft, die aktuelle Trendrichtung und mögliche Umkehrpunkte zu identifizieren.

Um den Indikator zu verwenden, sollte die Klasse [SuperTrend](xref:StockSharp.Algo.Indicators.SuperTrend) verwendet werden.

## Beschreibung

SuperTrend wird aus dem Durchschnittspreis und dem ATR-Wert erstellt. Die Indikatorlinie wechselt von oberhalb des Preises nach unterhalb (und umgekehrt), wenn sich der Trend ändert. Auf diese Weise hebt SuperTrend den aktuellen Trend visuell hervor, bis der Preis die Indikatorlinie kreuzt.

## Parameter

- **ATR-Zeitraum** – der für die ATR-Berechnung verwendete Zeitraum.
- **Multiplier** – der Faktor, der definiert, wie weit die Linie vom Durchschnittspreis abweicht.

## Berechnung

1. Berechnen Sie ATR über den gewählten Zeitraum.
2. Berechnen Sie zwei Grenzen:
   ```
   UpperBand = (High + Low) / 2 + Multiplier * ATR
   LowerBand = (High + Low) / 2 - Multiplier * ATR
   ```
3. SuperTrend entspricht zunächst einem der Bänder, abhängig vom aktuellen Trend.
4. Wenn der Schlusskurs die SuperTrend-Linie kreuzt, ändert sich die Trendrichtung und die Linie bewegt sich auf die entgegengesetzte Seite.

![IndicatorSuperTrend](../../../../images/indicator_supertrend.png)

## Siehe auch

[ATR](atr.md)
[parabolischer SAR](parabolic_sar.md)
