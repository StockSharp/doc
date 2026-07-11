# HLI

**Hoch-Tief-Index (HLI)** ist ein technischer Indikator, der das Verhältnis der Anzahl der Aktien, die neue Höchststände erreichen, zur Anzahl der Aktien, die über einen bestimmten Zeitraum neue Tiefststände erreichen, misst.

Um den Indikator verwenden zu können, müssen Sie die Klasse [HighLowIndex](xref:StockSharp.Algo.Indicators.HighLowIndex) verwenden.

## Beschreibung

Der Hoch-Tief-Index (HLI) ist ein Marktbreitenindikator, der die gesamte Marktaktivität analysiert, indem er die Anzahl der Instrumente, die neue Höchststände erreichen, mit der Anzahl der Instrumente vergleicht, die neue Tiefststände erreichen. Dies ermöglicht die Beurteilung der internen Stärke oder Schwäche des Marktes.

Die Grundidee des Indikators besteht darin, dass ein gesunder Markt dadurch gekennzeichnet ist, dass mehr Wertpapiere neue Höchststände als neue Tiefststände erreichen. Umgekehrt wird ein schwächer werdender Markt dazu führen, dass mehr Wertpapiere neue Tiefststände erreichen.

HLI ist besonders nützlich für:
- Beurteilung der allgemeinen Marktlage
- Identifizierung von Abweichungen zwischen dem Index und einzelnen Marktinstrumenten
- Ermittlung potenzieller Marktumkehrpunkte
- Bestätigung von Signalen anderer Indikatoren

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Berechnungszeitraum (Standardwert: 14)

## Berechnung

Die Hoch-Tief-Index-Berechnung umfasst die folgenden Schritte:

1. Zählen Sie die Anzahl der Wertpapiere, die im Length-Zeitraum neue Höchststände erreicht haben:
   ```
   New Highs = Anzahl der Instrumente mit neuen Hochs über die Length-Periode
   ```

2. Zählen Sie die Anzahl der Wertpapiere, die im Length-Zeitraum neue Tiefststände erreicht haben:
   ```
   New Lows = Anzahl der Instrumente mit neuen Tiefs über die Length-Periode
   ```

3. Berechnen Sie Hoch-Tief-Index als Verhältnis der Differenz zwischen neuen Höchst- und Tiefstständen zu ihrer Summe:
   ```
   HLI = ((New Highs - New Lows) / (New Highs + New Lows)) * 100
   ```

Hinweis: Wenn (Neue Hochs + Neue Tiefs) gleich Null ist, wird HLI auf Null gesetzt, um eine Division durch Null zu vermeiden.

## Interpretation

Der Hoch-Tief-Index wird wie folgt interpretiert:

1. **Wertebereich**:
   - HLI schwankt zwischen -100 und +100
   - Positive Werte deuten darauf hin, dass mehr Wertpapiere neue Höchststände als neue Tiefststände erreichen
   - Negative Werte deuten darauf hin, dass mehr Wertpapiere neue Tiefststände als neue Höchststände erreichen

2. **Nulllinienübergänge**:
   - Der Übergang von negativen zu positiven Werten kann als bullisches Signal angesehen werden
   - Der Übergang von positiven zu negativen Werten kann als bärisches Signal angesehen werden

3. **Extreme Werte**:
   - Werte nahe +100 deuten auf einen starken bullischen Markt hin (möglicherweise überkaufter Zustand)
   - Werte nahe -100 deuten auf einen starken rückläufigen Markt hin (möglicherweise überverkaufter Zustand)

4. **Abweichungen**:
   - Bullische Divergenz: Der Marktindex erreicht ein neues Tief, aber HLI bildet ein höheres Tief
   - Bärische Divergenz: Der Marktindex erreicht ein neues Hoch, aber HLI bildet ein niedrigeres Hoch

5. **HLI-Trends**:
   - Anhaltendes HLI-Wachstum deutet auf eine Stärkung eines bullischen Marktes hin
   - Der anhaltende HLI-Rückgang deutet auf eine Stärkung eines rückläufigen Marktes hin

6. **Bestätigung des Markttrends**:
   - Wenn der Marktindex steigt und auch HLI steigt, bestätigt dies die Stärke eines Aufwärtstrends
   - Wenn der Marktindex fällt und auch HLI fällt, bestätigt dies die Stärke eines Abwärtstrends

![indicator_high_low_index](../../../../images/indicator_high_low_index.png)

## Siehe auch

[McClellanOscillator](mcclellan_oscillator.md)
