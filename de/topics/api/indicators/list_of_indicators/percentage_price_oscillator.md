# PPO

**Prozentualer Preisoszillator (PPO)** ist ein technischer Indikator ähnlich wie MACD, der jedoch die Differenz zwischen zwei exponentiellen gleitenden Durchschnitten als Prozentsatz und nicht als absolute Werte ausdrückt.

Um den Indikator verwenden zu können, müssen Sie die Klasse [PercentagePriceOscillator](xref:StockSharp.Algo.Indicators.PercentagePriceOscillator) verwenden.

## Beschreibung

Der prozentuale Preisoszillator (PPO) ist eine Variante des bekannteren Indikators MACD (Konvergenz/Divergenz gleitender Durchschnitte). Der Hauptunterschied besteht darin, dass PPO die Differenz zwischen zwei exponentiellen gleitenden Durchschnitten als Prozentsatz und nicht in absoluten Einheiten ausdrückt. Dies macht PPO besonders nützlich, wenn Sie verschiedene Instrumente mit unterschiedlichen Preisniveaus vergleichen oder ein einzelnes Instrument über einen langen Zeitraum hinweg analysieren, wenn sich sein Preis erheblich geändert hat.

PPO besteht aus drei Komponenten:
1. **PPO-Linie** – Differenz zwischen schnellem und langsamem EMA, ausgedrückt als Prozentsatz
2. **Signalleitung** – EMA der PPO-Leitung
3. **Histogramm** – Differenz zwischen der PPO-Linie und der Signallinie

Der PPO-Indikator schwankt um die Nulllinie, wobei positive Werte auf eine bullische Marktstimmung und negative Werte auf eine bärische Marktstimmung hinweisen. Das Ausmaß der Abweichung von Null spiegelt die Stärke des aktuellen Trends wider.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Kurzer Zeitraum** – Zeitraum zur Berechnung des kurzen EMA (Standardwert: 12)
- **Langer Zeitraum** – Zeitraum für die Berechnung des langen EMA (Standardwert: 26)

## Berechnung

Die PPO-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie kurze und lange exponentielle gleitende Durchschnitte:
   ```
   Kurze EMA = EMA(Preis, ShortPeriod)
   Lange EMA = EMA(Preis, LongPeriod)
   ```

2. Berechnen Sie die PPO-Linie als prozentuale Differenz zwischen dem kurzen und dem langen EMA:
   ```
   PPO-Linie = ((kurze EMA - lange EMA) / lange EMA) * 100
   ```

3. Berechnen Sie die Signalleitung (typischerweise 9-Perioden-EMA- oder PPO-Leitung):
   ```
   Signallinie = EMA(PPO-Linie, 9)
   ```

4. Histogramm berechnen:
   ```
   Histogramm = PPO-Linie - Signallinie
   ```

Dabei gilt:
- Price - Preis (normalerweise Schlusskurs)
- EMA – exponentieller gleitender Durchschnitt
- ShortPeriod – Punkt für Kurzform EMA
- LongPeriod – Zeitraum für langes EMA

## Interpretation

Der PPO kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - Die PPO-Linie, die die Nulllinie von unten nach oben kreuzt, kann als bullisches Signal angesehen werden
   - Die PPO-Linie, die die Nulllinie von oben nach unten kreuzt, kann als rückläufiges Signal angesehen werden

2. **Signalleitungskreuzungen**:
   - Die PPO-Linie, die die Signallinie von unten nach oben kreuzt, kann als bullisches Signal angesehen werden
   - Die PPO-Linie, die die Signallinie von oben nach unten kreuzt, kann als rückläufiges Signal angesehen werden

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während PPO ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während PPO ein niedrigeres Hoch bildet

4. **Überkauft/Überverkauft**:
   - Extrem hohe positive PPO-Werte können auf überkaufte Marktbedingungen hinweisen
   - Extrem niedrige negative PPO-Werte können auf überverkaufte Marktbedingungen hinweisen

5. **Histogrammanalyse**:
   - Die Erweiterung des Histogramms weist auf eine Verstärkung des aktuellen Trends hin
   - Die Kontraktion des Histogramms weist auf eine Abschwächung des aktuellen Trends hin
   - Eine Änderung der Farbe (oder des Vorzeichens) des Histogramms weist auf eine Änderung der kurzfristigen Dynamik hin

6. **Instrumentenvergleich**:
   - Im Gegensatz zu MACD kann PPO zum direkten Vergleich verschiedener Instrumente verwendet werden
   - Höhere PPO-Werte für ein Instrument im Vergleich zu einem anderen können auf eine stärkere relative Dynamik hinweisen

7. **Signalfilterung**:
   - Signallinien-Kreuzungssignale sind zuverlässiger, wenn PPO mit dem Haupttrend übereinstimmt
   - Beispielsweise sind bullische Signale zuverlässiger, wenn PPO positiv ist, und bärische Signale sind zuverlässiger, wenn PPO negativ ist

![PPO Diagramm](../../../../images/indicator_percentage_price_oscillator.png)

## Siehe auch

[MACD](macd.md)
[EMA](ema.md)
[PPO-Signal](percentage_price_oscillator_signal.md)
[PPO-Histogramm](percentage_price_oscillator_histogram.md)
[Prozentualer Volumenoszillator](percentage_volume_oscillator.md)
[TRIX](trix.md)
