# EPMA

**Endpunkt-Gleitender Durchschnitt (EPMA)** ist ein technischer Indikator, der eine Modifikation des standardmäßigen gleitenden Durchschnitts darstellt und darauf ausgelegt ist, Verzögerungen bei der Trenderkennung zu reduzieren.

Um den Indikator verwenden zu können, müssen Sie die Klasse [EndpointMovingAverage](xref:StockSharp.Algo.Indicators.EndpointMovingAverage) verwenden.

## Beschreibung

Der Endpunkt-Gleitende Durchschnitt (EPMA) ist eine spezielle Form des gleitenden Durchschnitts, der sich auf die Endpunktdatenpunkte konzentriert. Im Gegensatz zu standardmäßigen gleitenden Durchschnitten, die alle Punkte in einem bestimmten Zeitraum gleichmäßig gewichten, gibt EPMA den Endpunktpunkten mehr Gewicht und ermöglicht so eine schnellere Reaktion auf Trendänderungen.

Der Hauptzweck von EPMA besteht darin, die Verzögerung herkömmlicher gleitender Durchschnitte zu verringern und gleichzeitig die Fähigkeit zur Filterung von Marktstörungen beizubehalten. Aufgrund seiner Berechnungsmethodik reagiert EPMA häufig schneller auf Preisrichtungsänderungen, was es zu einem wertvollen Werkzeug für Händler macht, die Trendumkehrungen früher erkennen möchten.

EPMA ist besonders nützlich für:
- Früheres Erkennen von Trendänderungen
- Reduzierung der Signalverzögerung
- Schaffung sensiblerer Handelssysteme
- Bestätigung von Signalen anderer Indikatoren mit weniger Verzögerung

## Parameter

Der Indikator hat die folgenden Parameter:
- **Länge** – Berechnungszeitraum (Standardwert: 14)

## Berechnung

Die Berechnung des Endpunkt-Gleitenden Durchschnitts basiert auf der linearen Regressionsmethode und konzentriert sich auf die Endpunkte des betrachteten Zeitraums:

1. Bestimmung des linearen Trends zwischen Anfangs- und Endpunkt der Periode:
   ```
   Startwert = Price[current - Length + 1]
   Endwert = Price[current]
   ```

2. Berechnung der Steigung der Trendlinie:
   ```
   Slope = (Endwert - Startwert) / (Length - 1)
   ```

3. Berechnen von EPMA als Projektion der Trendlinie zum aktuellen Punkt:
   ```
   EPMA = Startwert + Slope * (Length - 1)
   ```

Tatsächlich entspricht EPMA dem letzten Preiswert (Endwert) im betrachteten Zeitraum, konzeptionell handelt es sich jedoch um eine Projektion des durch die Endpunktpunkte definierten linearen Trends.

## Interpretation

Der Endpunkt-Gleitende Durchschnitt wird ähnlich wie andere gleitende Durchschnitte interpretiert, jedoch unter Berücksichtigung seiner erhöhten Empfindlichkeit:

1. **EPMA Richtung**:
   - Aufwärts EPMA zeigt einen Aufwärtstrend an
   - Abwärts EPMA zeigt einen Abwärtstrend an

2. **Kreuzungen mit dem Preis**:
   - Wenn der Preis EPMA von unten nach oben kreuzt, kann dies als bullisches Signal angesehen werden
   - Wenn der Preis EPMA von oben nach unten kreuzt, kann dies als rückläufiges Signal angesehen werden

3. **Mehrere EPMA-Kreuzungen**:
   - Das Kreuzen eines kurzen EPMA mit einem langen EPMA von unten nach oben kann den Beginn eines Aufwärtstrends anzeigen
   - Das Kreuzen eines kurzen EPMA mit einem langen EPMA von oben nach unten kann auf den Beginn eines Abwärtstrends hinweisen

4. **Divergenz mit anderen gleitenden Durchschnitten**:
   - Aufgrund seiner erhöhten Empfindlichkeit kann EPMA früher auf Trendänderungen reagieren als herkömmliche gleitende Durchschnitte
   - Abweichungen zwischen EPMA und anderen Arten von gleitenden Durchschnitten können als Frühwarnung für eine mögliche Trendwende dienen

5. **Signalfilterung**:
   - Aufgrund seiner erhöhten Empfindlichkeit kann EPMA in Seitwärtskonsolidierungsperioden mehr falsche Signale erzeugen
   - Es wird empfohlen, zusätzliche Filter oder Bestätigungen anderer Indikatoren zu verwenden, um die Signalzuverlässigkeit zu verbessern

![EPMA Diagramm](../../../../images/indicator_endpoint_moving_average.png)

## Siehe auch

[SMA](sma.md)
[EMA](ema.md)
[ZLEMA](zero_lag_exponential_moving_average.md)
[DEMA](dema.md)
