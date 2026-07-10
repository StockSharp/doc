# CKS

**Chande Kroll Stop (CKS)** ist ein von Tushar Chande und Stanley Kroll entwickelter Indikator zur Bestimmung von Stop-Loss-Niveaus, der sich an die Marktvolatilität anpasst und Händlern hilft, Positionsausstiegspunkte festzulegen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [ChandeKrollStop](xref:StockSharp.Algo.Indicators.ChandeKrollStop) verwenden.

## Beschreibung

Der Chande Kroll Stop-Indikator wurde als dynamisches Tool zum Festlegen von Stop-Loss-Levels entwickelt, das auf Änderungen der Marktvolatilität und des Markttrends reagiert. Es besteht aus zwei Linien: einer oberen Stopplinie (für Short-Positionen) und einer unteren Stopplinie (für Long-Positionen).

Der Hauptvorteil von CKS liegt in seiner Fähigkeit, sich an aktuelle Marktbedingungen anzupassen. In Zeiten hoher Volatilität werden die Stop-Linien weiter vom Preis entfernt positioniert, um eine vorzeitige Schließung von Positionen aufgrund von Marktgeräuschen zu vermeiden. In Zeiten geringer Volatilität bewegen sich die Stopplinien näher an den Preis heran und sorgen so für eine engere Trendverfolgung.

CKS ist besonders nützlich für:
- Festlegung von Stop-Loss-Levels für Long- und Short-Positionen
- Mit adaptiver Risikokontrolle dem Trend folgen
- Identifizierung potenzieller Trendumkehrpunkte
- Schaffung mechanischer Handelssysteme mit klaren Ausstiegsregeln

## Parameter

Der Indikator hat die folgenden Parameter:
- **Period** – Hauptperiode zur Berechnung von Extremwerten (Standardwert: 10)
- **Multiplier** – Multiplikator für ATR, der den Abstand von Extrema bestimmt (Standardwert: 1,5)
- **StopPeriod** – Zeitraum zur Berechnung der Stop-Levels (Standardwert: 20)

## Berechnung

Die Chande Kroll Stop-Berechnung umfasst die folgenden Schritte:

1. Bestimmung hoher und niedriger Extremwerte über dem Period:
   ```
   HighestHigh = höchster High-Wert über Period
   LowestLow = niedrigster Low-Wert über Period
   ```

2. Berechnung der wahren Reichweite von Average (ATR) über Period:
   ```
   ATR = durchschnittlicher TR-Wert über Period
   ```

3. Berechnen der oberen und unteren Bänder:
   ```
   Upper Band = HighestHigh - (Multiplier * ATR)
   Lower Band = LowestLow + (Multiplier * ATR)
   ```

4. Festlegung der Endhaltelinien anhand von StopPeriod:
   ```
   Upper Stop = höchster Wert des oberen Bands über StopPeriod
   Lower Stop = niedrigster Wert des unteren Bands über StopPeriod
   ```

## Interpretation

- **Upper Stop** wird für Short-Positionen verwendet. Übersteigt der Schlusskurs den oberen Stop, kann dies als Signal gewertet werden, eine Short-Position zu schließen oder eine Long-Position zu eröffnen.

- **Lower Stop** wird für Long-Positionen verwendet. Fällt der Schlusskurs unter den unteren Stop, kann dies als Signal gewertet werden, eine Long-Position zu schließen oder eine Short-Position zu eröffnen.

- Das Überqueren der Stopplinien **Price** kann auf eine mögliche Trendumkehr oder den Beginn einer neuen Dynamik hinweisen.

- **Abrupte Änderungen der Stopplinien** können bei erheblichen Änderungen der Marktvolatilität auftreten.

- **Verwendung mit anderen Indikatoren**: CKS funktioniert am besten in Kombination mit anderen Trend- und Momentumindikatoren, die bei der Bestimmung der Markteintrittsrichtung helfen.

![indicator_chande_kroll_stop](../../../../images/indicator_chande_kroll_stop.png)

## Siehe auch

[ATR](atr.md)
[ParabolicSAR](parabolic_sar.md)
[DonchianChannels](donchian_channels.md)
