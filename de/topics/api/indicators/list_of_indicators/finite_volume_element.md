# FVE

**Finite-Volume-Element (FVE)** ist ein technischer Indikator, der entwickelt wurde, um die Beziehung zwischen Preis und Volumen zu analysieren und dabei zu helfen, den Käufer- und Verkäuferdruck auf dem Markt einzuschätzen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [FiniteVolumeElement](xref:StockSharp.Algo.Indicators.FiniteVolumeElement) verwenden.

## Beschreibung

Der Finite-Volume-Element (FVE) analysiert die Beziehung zwischen Preisänderungen und Handelsvolumina, um die potenzielle Stärke der Preisbewegung zu bestimmen. Es basiert auf der Annahme, dass Preisänderungen dann am signifikantesten sind, wenn sie durch entsprechende Volumina bestätigt werden.

Der FVE-Indikator wandelt Preisänderungen, gewichtet nach Volumen, in einen Oszillator um, der dabei hilft, das relative Gleichgewicht zwischen Käufern und Verkäufern auf dem Markt zu bestimmen. Positive FVE-Werte weisen auf eine Dominanz des Käufers hin, während negative Werte auf eine Dominanz des Verkäufers hinweisen.

FVE ist besonders nützlich für:
- Beurteilung der Stärke und Nachhaltigkeit des aktuellen Trends
- Identifizieren potenzieller Umkehrpunkte
- Ermittlung von Abweichungen zwischen Preis und Volumen
- Bestätigung von Signalen anderer Indikatoren

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Glättungszeitraum (Standardwert: 22)

## Berechnung

Die Berechnung des FVE-Indikators umfasst mehrere Schritte:

1. Berechnen Sie den typischen Preis für aktuelle und frühere Zeiträume:
   ```
   Typischer Preis = (High + Low + Close) / 3
   ```

2. Berechnen Sie die typische Preisänderung:
   ```
   Preisänderung = typischer Preis[current] - typischer Preis[previous]
   ```

3. Berechnen Sie die volumengewichtete Preisänderung:
   ```
   Volumengewichtete Preisänderung = Preisänderung * Volume[current]
   ```

4. Normalisieren, um der Marktgröße Rechnung zu tragen:
   ```
   Normalisierter Wert = volumengewichtete Preisänderung / (durchschnittliches Volumen über die Periode * Preisvolatilität)
   ```

5. Kumulierte Summierung und Glättung:
   ```
   FVE = SMA(kumulierte Summe der normalisierten Werte, Length)
   ```

Dabei gilt:
- High, Low, Close – Höchst-, Tiefst- und Schlusskurse
- Volume - Handelsvolumen
- SMA – einfacher gleitender Durchschnitt
- Length - Glättungszeitraum

## Interpretation

Der FVE-Indikator kann wie folgt interpretiert werden:

1. **Nulllinienübergänge**:
   - Der Übergang von negativen zu positiven Werten weist auf einen erhöhten Käuferdruck hin und kann als bullisches Signal gewertet werden
   - Der Übergang von positiven zu negativen Werten weist auf einen erhöhten Verkäuferdruck hin und kann als bärisches Signal gewertet werden

2. **Extreme Werte**:
   - Hohe positive Werte (über +3) können auf überkaufte Marktbedingungen hinweisen
   - Hohe negative Werte (unter -3) können auf überverkaufte Marktbedingungen hinweisen

3. **Abweichungen**:
   - Eine zinsbullische Divergenz (der Preis bildet ein neues Tief, während FVE ein höheres Tief bildet) könnte auf eine mögliche Aufwärtsumkehr hinweisen
   - Eine rückläufige Divergenz (der Preis bildet ein neues Hoch, während FVE ein niedrigeres Hoch bildet) könnte auf eine mögliche Abwärtsumkehr hinweisen

4. **Trendbestätigung**:
   - Durchweg positive FVE-Werte bestätigen die Stärke eines Aufwärtstrends
   - Durchweg negative FVE-Werte bestätigen die Stärke eines Abwärtstrends

5. **FVE-Änderungsrate**:
   - Ein schneller Anstieg oder Rückgang der FVE-Werte kann auf eine starke Dynamik der Preisbewegung hinweisen
   - Eine Verlangsamung der FVE-Wertänderungen kann auf eine mögliche Verlangsamung der Dynamik hinweisen

6. **Unterstützungs- und Widerstandsstufen**:
   - Historische Umkehrpunkte auf dem FVE-Chart können als Richtlinien für zukünftige Umkehrungen dienen

![FVE Diagramm](../../../../images/indicator_finite_volume_element.png)

## Siehe auch

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ForceIndex](force_index.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
