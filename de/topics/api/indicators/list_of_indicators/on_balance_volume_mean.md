# OBVM

**On-Balance-Volumen-Durchschnitt (OBVM)** ist ein technischer Indikator, der einen gleitenden Durchschnitt des Indikators On-Balance-Volumen (OBV) darstellt und so klarere Trendsignale basierend auf dem Volumen ermöglicht.

Um den Indikator verwenden zu können, müssen Sie die Klasse [OnBalanceVolumeMean](xref:StockSharp.Algo.Indicators.OnBalanceVolumeMean) verwenden.

## Beschreibung

On-Balance-Volumen-Durchschnitt (OBVM) ist eine Modifikation des klassischen Indikators On-Balance-Volumen (OBV), der einen gleitenden Durchschnitt auf OBV-Werte anwendet, um Schwankungen zu glätten und klarere Trends zu erkennen. Der Indikator behält das Kernkonzept von OBV bei – Volumenakkumulation basierend auf Preisrichtungsänderungen, fügt jedoch eine zusätzliche Filterebene hinzu.

OBVM trägt dazu bei, das im ursprünglichen OBV vorhandene Rauschen zu eliminieren und macht langfristige Volumenstromtrends deutlicher wahrnehmbar. Dies ist besonders nützlich in volatilen Märkten oder bei der Analyse von Instrumenten mit unregelmäßigen Handelsvolumina.

Der Hauptvorteil von OBVM ist seine Fähigkeit, im Vergleich zum klassischen OBV klarere und weniger anfällig für falsche Signale zu generieren. Der Indikator kann auch verwendet werden, um Überschneidungen zwischen OBV und seinem Mittelwert zu identifizieren, was zusätzliche Handelsmöglichkeiten bietet.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Zeitraum für die Berechnung des gleitenden Durchschnitts (Standardwert: 20)

## Berechnung

Die Berechnung des On-Balance-Volumen-Durchschnitts umfasst die folgenden Schritte:

1. Berechnen Sie den Basisindikator On-Balance-Volumen (OBV):
   ```
   Wenn Close[current] > Close[previous]:
       OBV[current] = OBV[previous] + Volume[current]
   Wenn Close[current] < Close[previous]:
       OBV[current] = OBV[previous] - Volume[current]
   Wenn Close[current] = Close[previous]:
       OBV[current] = OBV[previous]
   ```

2. Wenden Sie den gleitenden Durchschnitt auf die OBV-Werte an:
   ```
   OBVM = SMA(OBV, Length)
   ```

Dabei gilt:
- Close - Schlusskurs
- Volume - Handelsvolumen
- OBV - On-Balance-Volumen
- SMA – einfacher gleitender Durchschnitt
- Length – gleitender Durchschnittszeitraum

Hinweis: Anstelle von SMA können auch andere Arten von gleitenden Durchschnitten wie EMA (exponentieller gleitender Durchschnitt), WMA (gewichteter gleitender Durchschnitt) usw. verwendet werden.

## Interpretation

Der On-Balance-Volumen-Durchschnitt kann wie folgt interpretiert werden:

1. **Trendanalyse**:
   - Steigender OBVM weist auf einen Aufwärtstrend mit starker Volumenunterstützung hin
   - Ein fallender OBVM weist auf einen rückläufigen Trend mit starker Volumenunterstützung hin
   - Der flache OBVM weist auf keinen ausgeprägten Trend hin

2. **OBV- und OBVM-Kreuzungen**:
   - Wenn OBV OBVM von unten nach oben kreuzt, kann dies als bullisches Signal angesehen werden
   - Wenn OBV OBVM von oben nach unten kreuzt, kann dies als rückläufiges Signal angesehen werden
   - Diese Kreuzungen weisen oft auf den Beginn neuer Trends oder bedeutender Preisbewegungen hin

3. **Abweichungen**:
   - Bullische Divergenz: Der Preis bildet ein neues Tief, während OBVM ein höheres Tief bildet
   - Bärische Divergenz: Der Preis bildet ein neues Hoch, während OBVM ein niedrigeres Hoch bildet
   - Divergenzen gehen oft deutlichen Trendumkehrungen voraus

4. **Bestätigung des Preistrends**:
   - Wenn sich OBVM in die gleiche Richtung wie der Preis bewegt, bestätigt dies den aktuellen Preistrend
   - Wenn sich OBVM und der Preis in entgegengesetzte Richtungen bewegen, kann dies auf eine mögliche Trendumkehr hinweisen

5. **Unterstützungs- und Widerstandsstufen**:
   - Der OBVM-Chart kann seine eigenen Unterstützungs- und Widerstandsniveaus bilden
   - Der Ausbruch dieser Niveaus kann ähnlichen Ausbrüchen im Preisdiagramm vorausgehen

6. **Vergleich mit anderen Volumenindikatoren**:
   - OBVM kann mit anderen Volumenindikatoren verglichen werden, um Signale zu bestätigen
   - Die Konsistenz der Signale mehrerer Volumenindikatoren erhöht deren Zuverlässigkeit

7. **Auswahl des Längenparameters**:
   - Kürzere Zeiträume (z. B. 10–15) machen OBVM empfindlicher gegenüber kurzfristigen Änderungen
   - Längere Zeiträume (z. B. 30–50) ermöglichen eine bessere Identifizierung langfristiger Trends
   - Der optimale Zeitraum hängt vom Handelszeithorizont und den spezifischen Instrumenteigenschaften ab

![indicator_on_balance_volume_mean](../../../../images/indicator_on_balance_volume_mean.png)

## Siehe auch

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)
