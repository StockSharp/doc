# KER

**Kaufman Efficiency Ratio (KER)** ist ein von Perry Kaufman entwickelter technischer Indikator, der die Effizienz der Preisbewegung misst, indem er die gerichtete Preisbewegung mit der Gesamtvolatilität vergleicht.

Um den Indikator verwenden zu können, müssen Sie die Klasse [KaufmanEfficiencyRatio](xref:StockSharp.Algo.Indicators.KaufmanEfficiencyRatio) verwenden.

## Beschreibung

Der Kaufman Efficiency Ratio (KER) bewertet, wie „effizient“ sich der Preis im Vergleich zum gesamten zurückgelegten Weg in eine bestimmte Richtung bewegt. Es stellt das Verhältnis der Nettopreisbewegung zur Summe aller Preisänderungen über einen bestimmten Zeitraum dar.

KER wurde von Perry Kaufman entwickelt und ursprünglich als Komponente des Adaptive Moving Average (KAMA) verwendet. KER selbst ist jedoch ein wertvolles Tool, das dabei hilft, festzustellen, ob sich der Markt in einem Trend- oder Schwankungszustand befindet.

KER-Werte schwanken zwischen 0 und 1:
- Werte nahe 1 weisen auf eine hocheffiziente Preisbewegung hin (starker Trend)
- Werte nahe 0 weisen auf eine ineffiziente Preisbewegung hin (Seitwärtsmarkt oder hohe Volatilität)

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Zeitraum für Effizienzberechnung (Standardwert: 10)

## Berechnung

Die Kaufman Efficiency Ratio-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie die Richtungsbewegung (Nettoveränderung) über den Zeitraum:
   ```
   Direction = |Price[current] - Price[current - Length]|
   ```

2. Berechnen Sie die Gesamtbewegung (Summe aller Änderungen) über den Zeitraum:
   ```
   Volatility = Sum(|Price[i] - Price[i-1]|) für i von (current - Length + 1) bis current
   ```

3. Wirkungsgradverhältnis berechnen:
   ```
   KER = Direction / Volatility
   ```

Dabei gilt:
- Price – normalerweise Schlusskurs
- Length - Berechnungszeitraum
- | | - bezeichnet den absoluten Wert

Wenn die Volatilität Null ist (was unwahrscheinlich ist), wird KER auf Null gesetzt, um eine Division durch Null zu vermeiden.

## Interpretation

Der Kaufman Efficiency Ratio kann wie folgt interpretiert werden:

1. **Effizienzstufen**:
   - High KER-Werte (>0,6) weisen auf einen starken Trend hin
   - Mittlere KER-Werte (0,3–0,6) weisen auf einen moderaten Trend hin
   - Niedrige KER-Werte (<0,3) weisen auf einen Seitwärtsmarkt oder eine hohe Volatilität hin

2. **KER-Änderungen**:
   - Das Wachstum von KER kann die Entstehung oder Verstärkung eines Trends signalisieren
   - Der Rückgang des KER kann auf eine Abschwächung des Trends oder einen Übergang in eine Seitwärtsbewegung hinweisen

3. **Handelsstrategien**:
   - In Zeiten hoher Effizienz (hoher KER) sind Trendstrategien vorzuziehen
   - In Zeiten geringer Effizienz (niedriger KER) sind Range-Trading-Strategien vorzuziehen

4. **Signalfilterung**:
   - KER kann zum Filtern von Signalen anderer Indikatoren verwendet werden
   - Trendindikatorsignale sind bei hohem KER zuverlässiger
   - Oszillatorsignale sind bei niedrigem KER zuverlässiger

5. **Anpassung der Marktbedingungen**:
   - KER ermöglicht die Anpassung von Handelsstrategien an sich ändernde Marktbedingungen
   - Händler können Parameter anderer Indikatoren basierend auf KER-Werten dynamisch anpassen

6. **Vorläufer ändern**:
   - Starke KER-Änderungen gehen oft neuen Preisbewegungen voraus
   - Der Rückgang des KER nach einer Phase hoher Werte könnte auf eine mögliche Trendumkehr hinweisen

![indicator_kaufman_efficiency_ratio](../../../../images/indicator_kaufman_efficiency_ratio.png)

## Siehe auch

[KAMA](kama.md)
[ADX](adx.md)
[VHF](vhf.md)
[VIDYA](vidya.md)
