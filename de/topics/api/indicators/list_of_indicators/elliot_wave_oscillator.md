# EWO

**Elliot Wave Oscillator (EWO)** ist ein technischer Indikator, der auf der Elliott-Wellen-Theorie basiert und Händlern dabei hilft, die Wellenstruktur und potenzielle Marktumkehrpunkte zu bestimmen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [ElliotWaveOscillator](xref:StockSharp.Algo.Indicators.ElliotWaveOscillator) verwenden.

## Beschreibung

Der Elliot Wave Oscillator (EWO) wurde entwickelt, um Händlern bei der Anwendung der Elliott-Wellen-Theorie in der Marktanalyse zu helfen. Die Elliott-Wellen-Theorie geht davon aus, dass sich Märkte in vorhersehbaren Zyklen bewegen, die aus fünf Wellen in Trendrichtung (Impulswellen) und drei Wellen gegen den Trend (Korrekturwellen) bestehen.

EWO basiert auf der Differenz zwischen schnellen und langsamen gleitenden Durchschnitten und ist darauf ausgelegt, Impuls- und Korrekturwellen gemäß Elliotts Theorie zu identifizieren. Es hilft festzustellen, wann sich der Markt in einer Impuls- oder Korrekturphase befindet, und schlägt mögliche Umkehrpunkte vor.

Der Elliot Wave Oscillator ist besonders nützlich für:
- Identifizierung der aktuellen Elliott-Wellenstruktur
- Bestimmung möglicher Enden von Impuls- und Korrekturwellen
- Bestätigung der manuellen Wellenanalyse
- Vorhersage potenzieller Umkehrpunkte

## Parameter

Der Indikator hat die folgenden Parameter:
- **ShortPeriod** – Zeitraum für den kurzen gleitenden Durchschnitt (Standardwert: 5)
- **LongPeriod** – Zeitraum für den langen gleitenden Durchschnitt (Standardwert: 35)

## Berechnung

Die Elliot Wave Oscillator-Berechnung ist ganz einfach:

```
EWO = EMA(Close, ShortPeriod) - EMA(Close, LongPeriod)
```

Dabei gilt:
- EMA – exponentieller gleitender Durchschnitt
- Close - Schlusskurs
- ShortPeriod – kurzer Zeitraum (normalerweise 5)
- LongPeriod – langer Zeitraum (normalerweise 35)

## Interpretation

Der Elliot Wave Oscillator kann wie folgt interpretiert werden:

1. **Positive und negative Werte**:
   - Positive Werte (EWO über Null) zeigen an, dass der Short-EMA über dem Long-EMA liegt, was häufig einem Aufwärtstrend oder einer Aufwärtsimpulswelle entspricht
   - Negative Werte (EWO unter Null) weisen darauf hin, dass der Short-EMA unter dem Long-EMA liegt, was häufig einem Abwärtstrend oder einer Abwärtsimpulswelle entspricht

2. **Nulllinienübergänge**:
   - Das Überschreiten der Nulllinie von unten nach oben kann den Beginn einer neuen Aufwärtsimpulswelle signalisieren
   - Das Überschreiten der Nulllinie von oben nach unten kann den Beginn einer neuen Abwärtsimpulswelle signalisieren

3. **Oszillator-Extreme**:
   - Spitzen und Täler des Oszillators können dem Ende von Impulswellen entsprechen
   - Auf das Erreichen eines Extrems folgt oft eine Korrekturphase

4. **Abweichungen**:
   - Eine bullische Divergenz (der Preis bildet ein neues Tief, während EWO ein höheres Tief bildet) könnte auf ein mögliches Ende einer Abwärtsimpulswelle hinweisen
   - Eine rückläufige Divergenz (der Preis bildet ein neues Hoch, während EWO ein niedrigeres Hoch bildet) könnte auf ein mögliches Ende einer Aufwärtsimpulswelle hinweisen

5. **Wellenstruktur**:
   - Bei Impulswellen (Wellen 1, 3, 5) zeigt EWO typischerweise starke Werte in Trendrichtung
   - In Korrekturwellen (Wellen 2, 4, A, B, C) zeigt EWO typischerweise schwächere Werte oder bewegt sich in eine Richtung, die dem Haupttrend entgegengesetzt ist

6. **Welle 3-Identifikation**:
   - Welle 3, die normalerweise die stärkste Impulswelle in der Elliott-Wellen-Theorie ist, zeichnet sich häufig durch die höchsten EWO-Werte aus

![indicator_elliot_wave_oscillator](../../../../images/indicator_elliot_wave_oscillator.png)

## Siehe auch

[EMA](ema.md)
[MACD](macd.md)
[ZigZag](zigzag.md)
[WaveTrendOscillator](wave_trend_oscillator.md)
