# LRSI

**Laguerre-RSI (LRSI)** ist ein technischer Indikator, der auf den mathematischen Prinzipien des Laguerre-Filters basiert und von John Ehlers als erweiterte Version des traditionellen Relative-Stärke-Index (RSI) entwickelt wurde.

Um den Indikator verwenden zu können, müssen Sie die Klasse [LaguerreRSI](xref:StockSharp.Algo.Indicators.LaguerreRSI) verwenden.

## Beschreibung

Laguerre-RSI (LRSI) ist ein innovativer Oszillator, der die Laguerre-Filter-Mathematik nutzt, um im Vergleich zum herkömmlichen RSI einen empfindlicheren und weniger nacheilenden Indikator zu erzeugen. John Ehlers hat diesen Indikator entwickelt, um das Signalverzögerungsproblem zu lösen, das vielen technischen Indikatoren innewohnt.

LRSI kombiniert Laguerre-Polynomprinzipien mit dem Relative-Stärke-Index-Konzept. Dadurch kann der Indikator schneller auf Trendänderungen reagieren und klarere Handelssignale bilden. Wie der herkömmliche RSI oszilliert der LRSI zwischen 0 und 1 (oder 0 bis 100 bei Multiplikation mit 100), weist jedoch eine weniger laute Struktur und deutlichere Kurven auf.

Der Hauptvorteil von LRSI ist seine Fähigkeit, Trendänderungen schnell zu erkennen und gleichzeitig die Signalstabilität aufrechtzuerhalten. Dies macht es besonders nützlich für den kurzfristigen Handel und die Bestimmung von Ein- und Ausstiegspunkten.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Gamma** – Filterkoeffizient (Standardwert: 0,4, Bereich von 0,1 bis 0,9)

Der Parameter Gamma bestimmt den Grad der Filterung und beeinflusst die Empfindlichkeit des Indikators. Niedrigere Gamma-Werte führen zu einem glatteren und weniger empfindlichen Indikator, während höhere Werte den Indikator empfindlicher gegenüber Preisänderungen, aber möglicherweise auch lauter machen.

## Berechnung

Die Laguerre-RSI-Berechnung umfasst mehrere Schritte:

1. Initialisieren Sie beim ersten Durchlauf vier Laguerre-Filterwerte (L0, L1, L2, L3):
   ```
   L0 = L1 = L2 = L3 = 0
   ```

2. Filterwerte für jeden neuen Preis aktualisieren:
   ```
   L0_new = (1 - Gamma) * Price + Gamma * L0_old
   L1_new = -Gamma * L0_new + L0_old + Gamma * L1_old
   L2_new = -Gamma * L1_new + L1_old + Gamma * L2_old
   L3_new = -Gamma * L2_new + L2_old + Gamma * L3_old
   ```

3. Berechnen Sie die „kumulative Multiplikation“ der gefilterten Werte:
   ```
   CU = (L0_new + L1_new + L2_new + L3_new) / 4
   ```

4. Unterteilen Sie die Komponenten in „oben“ und „unten“:
   ```
   Wenn CU >= CU_old, dann:
     UP = CU - CU_old
     DN = 0
   Otherwise:
     UP = 0
     DN = CU_old - CU
   ```

5. Endgültige LRSI-Berechnung:
   ```
   LRSI = UP / (UP + DN)
   ```

   Wenn (UP + DN) Null ist, wird LRSI auf den vorherigen Wert gesetzt.

Dabei gilt:
- Price – Eingabepreis (normalerweise Schlusspreis)
- Gamma – Filterparameter
- CU - „kumulative Multiplikation“

## Interpretation

Laguerre-RSI wird ähnlich wie herkömmliches RSI interpretiert, jedoch mit erhöhter Empfindlichkeit:

1. **Überkaufte und überverkaufte Niveaus**:
   - Werte über 0,8 (oder 80) gelten typischerweise als überkauft
   - Werte unter 0,2 (oder 20) gelten typischerweise als überverkauft
   - Aufgrund der Eigenschaften des Laguerre-Filters können diese Werte je nach Marktvolatilität angepasst werden

2. **Mittellinienkreuzungen**:
   - Das Überschreiten der Marke von 0,5 (oder 50) von unten nach oben kann als bullisches Signal angesehen werden
   - Das Überschreiten der 0,5- (oder 50-) Marke von oben nach unten kann als bärisches Signal angesehen werden

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während LRSI ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während LRSI ein niedrigeres Hoch bildet

4. **Rebounds von Extremen**:
   - Die Umkehr des LRSI von überkauften oder überverkauften Niveaus kann als Markteintrittssignal dienen

5. **Trendbestätigung**:
   - LRSI-Werte über 0,5 bestätigen einen Aufwärtstrend
   - LRSI-Werte unter 0,5 bestätigen einen Abwärtstrend

6. **Gamma Parameteroptimierung**:
   - Für schnellere Signale - Gamma erhöhen (näher an 0,9)
   - Für glattere Signale - Gamma verringern (näher an 0,1)

![LRSI Diagramm](../../../../images/indicator_laguerre_rsi.png)

## Siehe auch

[RSI](rsi.md)
[AdaptiveLaguerreFilter](adaptive_laguerre_filter.md)
[ConnorsRSI](connors_rsi.md)
