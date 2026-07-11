# DPI

**Disparity Index (DPI)** ist ein technischer Indikator, der die relative Abweichung des aktuellen Preises von einem gleitenden Durchschnitt über einen bestimmten Zeitraum misst, ausgedrückt als Prozentsatz.

Um den Indikator verwenden zu können, müssen Sie die Klasse [DisparityIndex](xref:StockSharp.Algo.Indicators.DisparityIndex) verwenden.

## Beschreibung

Der Disparity Index (DPI) misst den Grad der Preisabweichung von seinem gleitenden Durchschnitt. Dieser Indikator hilft zu bestimmen, wie weit der Preis im Verhältnis zu seinem Durchschnittswert „gestreckt“ ist, und kann zur Identifizierung potenzieller überkaufter oder überverkaufter Bedingungen verwendet werden.

DPI basiert auf der Annahme, dass der Preis nach einer erheblichen Abweichung tendenziell zu seinem Mittelwert zurückkehrt. Je größer die Abweichung, desto höher ist die Wahrscheinlichkeit, dass sich der Preis anschließend in die entgegengesetzte Richtung bewegt und sich dem Durchschnitt annähert.

Der Disparity Index ist nützlich für:
- Identifizierung extremer Preisabweichungen vom Mittelwert
- Erkennen möglicher Umkehrpunkte
- Messung der Stärke des aktuellen Trends
- Erstellen von Handelsstrategien basierend auf der Mean-Reversion

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Zeitraum zur Berechnung des gleitenden Durchschnitts (Standardwert: 14)

## Berechnung

Die Disparity Index-Berechnungsformel ist ganz einfach:

```
DPI = ((Price / MA) - 1) * 100
```

Dabei gilt:
- Price – aktueller Preis (normalerweise Schlusskurs)
- MA – gleitender Durchschnitt des Preises über den Zeitraum Length
- Das Ergebnis wird als Prozentsatz ausgedrückt

Positive DPI-Werte zeigen an, dass der Preis über seinem gleitenden Durchschnitt liegt, während negative Werte darauf hinweisen, dass der Preis unter seinem gleitenden Durchschnitt liegt.

## Interpretation

Der Disparity Index kann wie folgt interpretiert werden:

1. **Extremwerte**:
   - Hohe positive Werte (z. B. über +10 %) können auf überkaufte Marktbedingungen hinweisen
   - Hohe negative Werte (z. B. unter -10 %) können auf überverkaufte Marktbedingungen hinweisen

2. **Nulllinienübergänge**:
   - Ein Übergang von unten nach oben (von negativen zu positiven Werten) zeigt an, dass der Preis seinen gleitenden Durchschnitt von unten nach oben überschritten hat, was als bullisches Signal angesehen werden kann
   - Ein Übergang von oben nach unten (von positiven zu negativen Werten) zeigt an, dass der Preis seinen gleitenden Durchschnitt von oben nach unten überschritten hat, was als bärisches Signal angesehen werden kann

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis erreicht ein neues Tief, aber DPI bildet ein höheres Tief
   - Bärische Divergenz: Der Preis erreicht ein neues Hoch, aber DPI bildet ein niedrigeres Hoch

4. **Trendanalyse**:
   - Durchweg positive DPI-Werte deuten auf einen starken Aufwärtstrend hin
   - Durchweg negative DPI-Werte deuten auf einen starken Abwärtstrend hin
   - Schwankungen um Null können auf einen Seitwärtstrend oder eine Konsolidierung hinweisen

5. **Mean-Reversion-Strategien**:
   - Extreme DPI-Werte können verwendet werden, um Positionen gegen die aktuelle Preisbewegung zu eröffnen und eine Rückkehr zum Mittelwert zu erwarten

![indicator_disparity_index](../../../../images/indicator_disparity_index.png)

## Siehe auch

[SMA](sma.md)
[RSI](rsi.md)
[StochasticOscillator](stochastic_oscillator.md)
[BollingerBands](bollinger_bands.md)
