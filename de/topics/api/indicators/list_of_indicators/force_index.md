# FI

**Kraftindex (FI)** ist ein von Dr. Alexander Elder entwickelter technischer Indikator, der die Stärke jeder Preisbewegung basierend auf ihrer Richtung, ihrem Ausmaß und ihrem Handelsvolumen misst.

Um den Indikator verwenden zu können, müssen Sie die Klasse [ForceIndex](xref:StockSharp.Algo.Indicators.ForceIndex) verwenden.

## Beschreibung

Der Kraftindex ist ein Oszillator, der die Stärke von „Bullen“ (Käufern) oder „Bären“ (Verkäufern) bei jeder Preisbewegung misst. Es kombiniert drei wichtige Elemente von Marktinformationen: Richtung der Preisbewegung, Ausmaß der Bewegung und Handelsvolumen.

Die Grundidee des Indikators ist, dass die Marktbewegung umso stärker ist, je größer die Preisänderung und je größer das Handelsvolumen ist. Positive Kraftindex-Werte weisen auf eine Vorherrschaft des Käufers hin (bullischer Druck), während negative Werte auf eine Vorherrschaft des Verkäufers hindeuten (bärischer Druck).

Der Kraftindex ist besonders nützlich für:
- Bestimmung der Stärke des aktuellen Trends
- Identifizieren potenzieller Umkehrpunkte
- Bestätigung von Ausbrüchen
- Erkennen von Divergenzen zwischen Preis und Bewegungsstärke

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Glättungszeitraum (Standardwert: 13)

## Berechnung

Die Kraftindex-Berechnung umfasst die folgenden Schritte:

1. Berechnung des Einzelperioden-Kraftindex:
   ```
   Einperioden-Kraftindex = (Close[current] - Close[previous]) * Volume[current]
   ```

2. Glättung mit einem exponentiellen gleitenden Durchschnitt (EMA):
   ```
   Kraftindex = EMA(Einperioden-Kraftindex, Length)
   ```

Dabei gilt:
- Close - Schlusskurs
- Volume - Handelsvolumen
- EMA – exponentieller gleitender Durchschnitt
- Length - Glättungszeitraum

## Interpretation

Der Kraftindex kann auf verschiedene Arten interpretiert werden:

1. **Nulllinienübergänge**:
   - Der Übergang von negativen zu positiven Werten weist auf einen erhöhten Aufwärtsdruck hin und kann als Kaufsignal gewertet werden
   - Der Übergang von positiven zu negativen Werten weist auf einen erhöhten Abwärtsdruck hin und kann als Verkaufssignal angesehen werden

2. **Extreme Werte**:
   - Hohe positive Werte deuten auf einen starken Aufwärtsdruck hin, der zu überkauften Marktbedingungen führen kann
   - Hohe negative Werte deuten auf einen starken Abwärtsdruck hin, der zu überverkauften Marktbedingungen führen kann

3. **Abweichungen**:
   - Eine bullische Divergenz (der Preis bildet ein neues Tief, während Kraftindex ein höheres Tief bildet) könnte eine mögliche Aufwärtsumkehr signalisieren
   - Eine rückläufige Divergenz (der Preis bildet ein neues Hoch, während Kraftindex ein niedrigeres Hoch bildet) könnte eine mögliche Abwärtsumkehr signalisieren

4. **Trendbestätigung**:
   - Durchweg positive Kraftindex-Werte bestätigen die Stärke eines Aufwärtstrends
   - Durchweg negative Kraftindex-Werte bestätigen die Stärke eines Abwärtstrends

5. **Dreifacher Nutzen** (laut Elder):
   - Kurzfristiger Kraftindex (2 Tage): zur Identifizierung kurzfristiger Chancen
   - Mittelfristiger Kraftindex (13 Tage): zur Ermittlung mittelfristiger Trends und Korrekturen
   - Langfristiger Kraftindex (100 Tage): zur Identifizierung des Haupttrends

6. **Korrekturkennzeichnung**:
   - Bei einem Aufwärtstrend können Tage mit negativem Kraftindex auf vorübergehende Korrekturen hinweisen
   - Bei einem Abwärtstrend können Tage mit positivem Kraftindex auf vorübergehende Aufschwünge hinweisen

![FI Diagramm](../../../../images/indicator_force_index.png)

## Siehe auch

[EMA](ema.md)
[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
