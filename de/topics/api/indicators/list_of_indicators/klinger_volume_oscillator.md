# KVO

**Klinger-Volumenoszillator (KVO)** ist ein von Stephen Klinger entwickelter technischer Indikator, der Volumen und Preis nutzt, um langfristige Trends und kurzfristige Umkehrungen im Markt zu erkennen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [KlingerVolumeOscillator](xref:StockSharp.Algo.Indicators.KlingerVolumeOscillator) verwenden.

## Beschreibung

Der Klinger-Volumenoszillator (KVO) wurde von Stephen Klinger entwickelt, um die Divergenz zwischen Volumen und Preis zu messen. Der Indikator basiert auf dem Konzept, dass Preisbewegungen durch das Volumen bestätigt werden. KVO versucht nicht nur die Trendrichtung, sondern auch seine Stärke und mögliche Umkehrpunkte zu bestimmen.

KVO kombiniert Preisinformationen mit Volumen mithilfe eines Volumenkraft-Indikators, der sowohl die Richtung und das Ausmaß der Preisbewegung als auch das Handelsvolumen berücksichtigt. Anschließend wendet es exponentielle gleitende Durchschnitte (EMA) mit zwei unterschiedlichen Perioden auf diesen Geldfluss an und berechnet die Differenz zwischen ihnen.

Der Indikator ist ein Oszillator, der oberhalb und unterhalb der Nulllinie schwankt. Positive KVO-Werte zeigen an, dass Käufer den Markt kontrollieren, während negative Werte darauf hinweisen, dass Verkäufer einen Vorteil haben.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Kurzer Zeitraum** – Zeitraum zur Berechnung des kurzen EMA (Standardwert: 34)
- **Langer Zeitraum** – Zeitraum für die Berechnung des langen EMA (Standardwert: 55)

## Berechnung

Die Klinger-Volumenoszillator-Berechnung umfasst mehrere Schritte:

1. Bestimmen Sie den Trend für jeden Zeitraum:
   ```
   Trend = +1, wenn (High + Low + Close) > (High[previous] + Low[previous] + Close[previous])
   Trend = -1, andernfalls
   ```

2. Berechnen Sie den Volumenkraft-Indikator:
   ```
   Volumenkraft = Volume * Trend * abs(2 * ((Close - Low) - (High - Close)) / (High - Low))
   ```
   Wenn (High - Low) Null ist, wird Volumenkraft auf Volumen multipliziert mit Trend gesetzt.

3. Berechnen Sie EMA für zwei Zeiträume:
   ```
   Kurze EMA = EMA(Volumenkraft, ShortPeriod)
   Lange EMA = EMA(Volumenkraft, LongPeriod)
   ```

4. Endgültige KVO-Berechnung:
   ```
   KVO = kurze EMA - lange EMA
   ```

5. Signalleitung berechnen (optional):
   ```
   Signallinie = EMA(KVO, 13)
   ```

Dabei gilt:
- High, Low, Close – Höchst-, Tiefst- und Schlusskurse
- Volume - Handelsvolumen
- EMA – exponentieller gleitender Durchschnitt
- ShortPeriod – Punkt für Kurzform EMA
- LongPeriod – Zeitraum für langes EMA

## Interpretation

Der Klinger-Volumenoszillator kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - Das Überqueren der Nulllinie von KVO von unten nach oben kann als bullisches Signal angesehen werden
   - Das Überqueren der Nulllinie von KVO von oben nach unten kann als rückläufiges Signal angesehen werden

2. **Signalleitungskreuzungen**:
   - KVO, das die Signallinie von unten nach oben kreuzt, kann als bullisches Einstiegssignal angesehen werden
   - KVO, das die Signallinie von oben nach unten kreuzt, kann als rückläufiges Einstiegssignal angesehen werden

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während KVO ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während KVO ein niedrigeres Hoch bildet

4. **Trendbestätigung**:
   - Positive KVO-Werte bestätigen einen Aufwärtstrend
   - Negative KVO-Werte bestätigen einen Abwärtstrend

5. **Trendstärke**:
   - Ein steigender KVO-Wert (sowohl positiv als auch negativ) weist auf eine Verstärkung des aktuellen Trends hin
   - Ein sinkender KVO-Wert deutet auf eine Abschwächung des aktuellen Trends hin

6. **Mögliche Umkehrungen**:
   - Extreme KVO-Werte können auf überkaufte oder überverkaufte Marktbedingungen und eine mögliche Umkehr hinweisen
   - Eine Verlangsamung des KVO-Anstiegs oder -Falls kann einer Trendumkehr vorausgehen

7. **Volumen und Preis**:
   - KVO ermöglicht die Beurteilung der Konsistenz von Preis- und Volumenbewegungen
   - Starkes Volumen in Trendrichtung führt zu extremeren KVO-Werten

![KVO Diagramm](../../../../images/indicator_klinger_volume_oscillator.png)

## Siehe auch

[OBV](on_balance_volume.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ADL](accumulation_distribution_line.md)
[ForceIndex](force_index.md)
