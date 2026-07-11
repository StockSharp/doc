# WTO

**Wellentrend-Oszillator (WTO)** ist ein technischer Indikator zur Erkennung überkaufter und überverkaufter Marktniveaus sowie zyklischer Preisschwankungen. WTO kombiniert Elemente von Kanälen und Oszillatoren und ist dadurch ein wirksames Werkzeug zur Identifikation von Markt-Momentum und möglichen Wendepunkten.

Um den Indikator zu verwenden, nutzen Sie die Klasse [WaveTrendOscillator](xref:StockSharp.Algo.Indicators.WaveTrendOscillator).

## Beschreibung

Der Wellentrend-Oszillator ist darauf ausgelegt, Marktrauschen zu filtern und primäre Preisbewegungen hervorzuheben. Der Indikator oszilliert um die Nulllinie und erzeugt Wellenmuster, die mit zyklischen Preisbewegungen korrelieren.

Wichtige Eigenschaften von WTO:
- Oszillationen um die Nulllinie, wobei positive Werte einen Aufwärtstrend und negative Werte einen Abwärtstrend anzeigen
- Überkaufte Bereiche (üblicherweise über +60) und überverkaufte Bereiche (üblicherweise unter -60)
- Fähigkeit, Preisrauschen zu filtern und primäre Bewegungen hervorzuheben

Wichtige Indikatorsignale:
- Kreuzen der Nulllinie (Änderung der Trendrichtung)
- Verlassen überkaufter oder überverkaufter Zonen
- Divergenzen zwischen WTO und Preis (mögliche Umkehrungen)
- Spezifische Wellenmuster

## Parameter

- **EsaPeriod** - EMA-Periode zur Berechnung des ESA-Werts (typischerweise 10)
- **DPeriod** - Periode zur Berechnung der Abweichung (typischerweise 21)
- **AveragePeriod** - Periode zur Berechnung des Durchschnitts des endgültigen Oszillators (typischerweise 4)

## Berechnung

Die Berechnung des Wellentrend-Oszillator umfasst mehrere Schritte:

1. Typischen Preis berechnen:
   ```
   AP = (High + Low + Close) / 3
   ```

2. Geglätteten und absoluten ersten Messwert erstellen:
   ```
   ESA = EMA(AP, EsaPeriod)
   D = EMA(Abs(AP - ESA), DPeriod)
   ```

3. Erste Oszillatorlinie berechnen:
   ```
   CI = (AP - ESA) / (0.015 * D)
   ```

4. Oszillator glätten, um den endgültigen WTO-Wert zu erhalten:
   ```
   WTO = EMA(CI, AveragePeriod)
   ```

Typische Werte für die Indikatorparameter sind: EsaPeriod = 10, DPeriod = 21, AveragePeriod = 4; sie können jedoch an unterschiedliche Zeitrahmen und Instrumente angepasst werden.

![WTO Diagramm](../../../../images/indicator_wave_trend_oscillator.png)

## Siehe auch

[MACD](macd.md)
[Stochastischer Oszillator](stochastic_oscillator.md)

