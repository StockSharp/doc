# MOMA

**Momentum des gleitenden Durchschnitts (MOMA)** ist ein technischer Indikator, der die Änderungsrate des gleitenden Durchschnittspreises misst und die Konzepte von Momentum und gleitenden Durchschnitten kombiniert.

Um den Indikator verwenden zu können, müssen Sie die Klasse [MomentumOfMovingAverage](xref:StockSharp.Algo.Indicators.MomentumOfMovingAverage) verwenden.

## Beschreibung

Das Momentum des gleitenden Durchschnitts (MOMA) ist eine Kombination aus zwei Indikatoren – einem Momentum-Indikator und einem gleitenden Durchschnitt. Zunächst wird ein gleitender Durchschnitt der Preisreihe berechnet und anschließend das Momentum (Änderungsrate) dieses gleitenden Durchschnitts gemessen.

Die Hauptidee von MOMA besteht darin, zunächst die Preisreihe mithilfe eines gleitenden Durchschnitts zu glätten und dadurch Marktrauschen zu beseitigen, und dann die Geschwindigkeit und Richtung der Änderung in dieser geglätteten Kurve zu analysieren. Dies ermöglicht es, ein klareres Signal über die Änderung des Trendmomentums zu erhalten, als das Momentum direkt anhand des Preises zu berechnen.

MOMA hilft bei der Bestimmung der Trendstärke und potenzieller Umkehrpunkte, indem es sich auf Änderungen in der Dynamik des gleitenden Durchschnitts und nicht auf den Preis selbst konzentriert. Positive MOMA-Werte weisen auf ein Aufwärtsmomentum des gleitenden Durchschnitts hin, während negative Werte auf ein Abwärtsmomentum hinweisen.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Zeitraum für die Berechnung des gleitenden Durchschnitts (Standardwert: 14)
- **MomentumPeriod** – Zeitraum für die Impulsberechnung (Standardwert: 10)

## Berechnung

Die Berechnung des Momentums des gleitenden Durchschnitts umfasst die folgenden Schritte:

1. Berechnen Sie den gleitenden Durchschnitt für die Preisreihe:
   ```
   MA = SMA(Price, Length)
   ```

2. Berechnen Sie den gleitenden Durchschnittsimpuls:
   ```
   MOMA = MA[current] - MA[current - MomentumPeriod]
   ```

Dabei gilt:
- Price - Preis (normalerweise Schlusskurs)
- SMA – einfacher gleitender Durchschnitt
- Length – Zeitraum für den gleitenden Durchschnitt
- MomentumPeriod – Zeitraum für die Impulsberechnung

Hinweis: Anstelle von SMA können auch andere Arten von gleitenden Durchschnitten wie EMA (exponentieller gleitender Durchschnitt), WMA (gewichteter gleitender Durchschnitt) usw. verwendet werden.

## Interpretation

Das Momentum des gleitenden Durchschnitts kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - Das Überqueren der Nulllinie von MOMA von unten nach oben kann als bullisches Signal gewertet werden, das den Beginn oder die Verstärkung eines Aufwärtstrends anzeigt
   - Das Überqueren der Nulllinie von MOMA von oben nach unten kann als rückläufiges Signal angesehen werden, das den Beginn oder die Verstärkung eines Abwärtstrends anzeigt

2. **Absolute Werte**:
   - Hohe positive MOMA-Werte deuten auf ein starkes Aufwärtsmomentum des gleitenden Durchschnitts hin
   - High-negative MOMA-Werte weisen auf eine starke Abwärtsdynamik des gleitenden Durchschnitts hin
   - Werte nahe Null deuten auf keine ausgeprägte Dynamik oder einen Seitwärtstrend hin

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während MOMA ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während MOMA ein niedrigeres Hoch bildet

4. **Richtungsänderung**:
   - Wenn MOMA die Bewegungsrichtung ändert (von steigend zu fallend oder umgekehrt), kann dies auf eine mögliche Änderung des gleitenden Durchschnittstrends hinweisen
   - Solche Umkehrungen gehen häufig Änderungen in der Preisrichtung voraus

5. **Trendbestätigung**:
   - Positive MOMA-Werte bestätigen einen Aufwärtstrend
   - Negative MOMA-Werte bestätigen einen Abwärtstrend
   - Steigende MOMA-Werte deuten auf eine Verstärkung des aktuellen Trends hin
   - Sinkende MOMA-Werte deuten auf eine Abschwächung des aktuellen Trends hin

6. **Signalfilterung**:
   - MOMA kann zum Filtern von Signalen anderer Indikatoren verwendet werden
   - Berücksichtigen Sie beispielsweise nur bullische Signale, wenn MOMA positiv ist, und nur bärische Signale, wenn MOMA negativ ist

7. **Parameterauswahl**:
   - Kürzere Zeiträume für Length und MomentumPeriod machen MOMA empfindlicher, aber auch anfälliger für Fehlsignale
   - Längere Zeiträume machen MOMA flüssiger, können aber zu verzögerten Signalen führen

![MOMA](../../../../images/indicator_momentum_of_moving_average.png)

## Siehe auch

[Impuls](momentum.md)
[SMA](sma.md)
[EMA](ema.md)
[RoC](roc.md)
