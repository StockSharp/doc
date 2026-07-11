# CMF

**Chaikin-Geldfluss (CMF)** ist ein von Mark Chaikin entwickelter technischer Indikator, der die Stärke des Geldflusses (Akkumulation und Verteilung) auf dem Markt über einen bestimmten Zeitraum misst.

Um den Indikator verwenden zu können, müssen Sie die Klasse [ChaikinMoneyFlow](xref:StockSharp.Algo.Indicators.ChaikinMoneyFlow) verwenden.

## Beschreibung

Chaikin-Geldfluss (CMF) erweitert das Konzept der Akkumulations-/Verteilungslinie (A/D-Linie) und konzentriert sich auf einen bestimmten Zeitraum. Der Indikator misst das Geldflussvolumen, ausgedrückt als Prozentsatz des Gesamtvolumens über den angegebenen Zeitraum.

CMF hilft Händlern:
- Bestimmen Sie die Stärke des Kauf- und Verkaufsdrucks
- Identifizieren Sie Akkumulations- (Kauf) und Verteilungstrends (Verkauf).
- Erkennen Sie Divergenzen zwischen Preisbewegung und Geldfluss
- Bestätigen Sie den aktuellen Trend oder seine Schwäche

Die Grundidee von CMF besteht darin, dass der Schlusskurs bei einem starken Aufwärtstrend näher am Periodenhoch liegen sollte, während er bei einem starken Abwärtstrend näher am Periodentief liegen sollte.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** - Berechnungszeitraum (Standardwert: 20-21 Tage)

## Berechnung

Die CMF-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie den Geldfluss Multiplier für jede Periode:
   ```
   Geldflussmultiplikator = ((Close - Low) - (High - Close)) / (High - Low)
   ```

   Wenn (High - Low) = 0, dann ist der Geldfluss Multiplier = 0.

2. Berechnen Sie den Geldfluss Volume für den Zeitraum:
   ```
   Geldflussvolumen = Geldflussmultiplikator * Volume
   ```

3. Berechnen Sie Chaikin-Geldfluss:
   ```
   CMF = Sum(Geldflussvolumen über Length-Periode) / Sum(Volumen über Length-Periode)
   ```

## Interpretation

CMF oszilliert um die Nulllinie und liegt typischerweise im Bereich von -1 bis +1:

- **Positive CMF-Werte** (über Null):
  - Käuferdruck anzeigen (Akkumulation)
  - Je höher der Wert, desto stärker ist der Käuferdruck
  - Besonders bedeutsam, wenn es über einen längeren Zeitraum anhält

- **Negative CMF-Werte** (unter Null):
  - Verkäuferdruck angeben (Verteilung)
  - Je niedriger der Wert, desto stärker ist der Verkäuferdruck
  - Ein längerer Aufenthalt im negativen Bereich bestätigt einen Abwärtstrend

- **Nulllinienüberschreitung**:
  - Ein Übergang von unten nach oben kann den Beginn eines Aufwärtstrends anzeigen
  - Ein Übergang von oben nach unten kann den Beginn eines Abwärtstrends signalisieren

- **Abweichungen**:
  - Bullische Divergenz: Preis sinkt, während CMF steigt (potenzielle Aufwärtsumkehr)
  - Bärische Divergenz: Preis steigt, während CMF fällt (potenzielle Abwärtsumkehr)

- **Extremwerte**:
  - Werte über +0,25 können auf eine starke Akkumulation hinweisen
  - Werte unter -0,25 können auf eine starke Verteilung hinweisen

![CMF Diagramm](../../../../images/indicator_chaikin_money_flow.png)

## Siehe auch

[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
[ForceIndex](force_index.md)
[MFI](money_flow_index.md)
