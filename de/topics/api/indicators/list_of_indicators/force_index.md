# FI

**Force Index (FI)** ist ein von Dr. Alexander Elder entwickelter technischer Indikator, der die Stärke jeder Preisbewegung basierend auf ihrer Richtung, ihrem Ausmaß und ihrem Handelsvolumen misst.

Um den Indikator verwenden zu können, müssen Sie die Klasse [ForceIndex](xref:StockSharp.Algo.Indicators.ForceIndex) verwenden.

## Beschreibung

Der Force Index ist ein Oszillator, der die Stärke von „Bullen“ (Käufern) oder „Bären“ (Verkäufern) bei jeder Preisbewegung misst. Es kombiniert drei wichtige Elemente von Marktinformationen: Richtung der Preisbewegung, Ausmaß der Bewegung und Handelsvolumen.

Die Grundidee des Indikators ist, dass die Marktbewegung umso stärker ist, je größer die Preisänderung und je größer das Handelsvolumen ist. Positive Force Index-Werte weisen auf eine Vorherrschaft des Käufers hin (bullischer Druck), während negative Werte auf eine Vorherrschaft des Verkäufers hindeuten (bärischer Druck).

Der Force Index ist besonders nützlich für:
- Bestimmung der Stärke des aktuellen Trends
- Identifizieren potenzieller Umkehrpunkte
- Bestätigung von Ausbrüchen
- Erkennen von Divergenzen zwischen Preis und Bewegungsstärke

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Glättungszeitraum (Standardwert: 13)

## Berechnung

Die Force Index-Berechnung umfasst die folgenden Schritte:

1. Berechnung des Einzelperioden-Force Index:
   ```
   1-Period Force Index = (Close[current] - Close[previous]) * Volume[current]
   ```

2. Glättung mit einem exponentiellen gleitenden Durchschnitt (EMA):
   ```
   Force Index = EMA(1-Period Force Index, Length)
   ```

Dabei gilt:
- Close - Schlusskurs
- Volume - Handelsvolumen
- EMA – exponentieller gleitender Durchschnitt
- Length - Glättungszeitraum

## Interpretation

Der Force Index kann auf verschiedene Arten interpretiert werden:

1. **Nulllinienübergänge**:
   - Der Übergang von negativen zu positiven Werten weist auf einen erhöhten Aufwärtsdruck hin und kann als Kaufsignal gewertet werden
   - Der Übergang von positiven zu negativen Werten weist auf einen erhöhten Abwärtsdruck hin und kann als Verkaufssignal angesehen werden

2. **Extreme Werte**:
   - Hohe positive Werte deuten auf einen starken Aufwärtsdruck hin, der zu überkauften Marktbedingungen führen kann
   - Hohe negative Werte deuten auf einen starken Abwärtsdruck hin, der zu überverkauften Marktbedingungen führen kann

3. **Abweichungen**:
   - Eine bullische Divergenz (der Preis bildet ein neues Tief, während Force Index ein höheres Tief bildet) könnte eine mögliche Aufwärtsumkehr signalisieren
   - Eine rückläufige Divergenz (der Preis bildet ein neues Hoch, während Force Index ein niedrigeres Hoch bildet) könnte eine mögliche Abwärtsumkehr signalisieren

4. **Trendbestätigung**:
   - Durchweg positive Force Index-Werte bestätigen die Stärke eines Aufwärtstrends
   - Durchweg negative Force Index-Werte bestätigen die Stärke eines Abwärtstrends

5. **Dreifacher Nutzen** (laut Elder):
   - Kurzfristiger Force Index (2 Tage): zur Identifizierung kurzfristiger Chancen
   - Mittelfristiger Force Index (13 Tage): zur Ermittlung mittelfristiger Trends und Korrekturen
   - Langfristiger Force Index (100 Tage): zur Identifizierung des Haupttrends

6. **Korrekturkennzeichnung**:
   - Bei einem Aufwärtstrend können Tage mit negativem Force Index auf vorübergehende Korrekturen hinweisen
   - Bei einem Abwärtstrend können Tage mit positivem Force Index auf vorübergehende Aufschwünge hinweisen

![indicator_force_index](../../../../images/indicator_force_index.png)

## Siehe auch

[EMA](ema.md)
[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
