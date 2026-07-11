# MGD

**McGinley-Dynamik (MGD)** ist ein von John R. McGinley entwickelter technischer Indikator, der eine fortschrittliche Form des gleitenden Durchschnitts darstellt und seine Geschwindigkeit automatisch an Änderungen der Marktgeschwindigkeit anpasst.

Um den Indikator verwenden zu können, müssen Sie die Klasse [McGinleyDynamic](xref:StockSharp.Algo.Indicators.McGinleyDynamic) verwenden.

## Beschreibung

McGinley-Dynamik (MGD) wurde von John McGinley entwickelt, um einige Nachteile traditioneller gleitender Durchschnitte zu überwinden, wie z. B. Verzögerungen und die Unfähigkeit, sich an Änderungen der Marktgeschwindigkeit anzupassen. Der Indikator passt seine Reaktionszeit automatisch an die Geschwindigkeit der Preisbewegung an, wodurch er empfindlicher auf schnelle Änderungen reagiert und weniger anfällig für falsche Signale ist.

Im Gegensatz zu einfachen und exponentiellen gleitenden Durchschnitten beinhaltet MGD eine Abstimmungskonstante und das Verhältnis des Preises zum vorherigen Indikatorwert. Dadurch kann MGD schneller auf erhebliche Preisänderungen reagieren und gleichzeitig die Stabilität bei langsameren Bewegungen gewährleisten.

Die Grundidee besteht darin, dass MGD bei schnellen Marktbewegungen „beschleunigt“ und in Konsolidierungsphasen „verlangsamt“ wird, was eine genauere Preisverfolgung im Vergleich zu herkömmlichen gleitenden Durchschnitten ermöglicht.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Berechnungszeitraum (Standardwert: 14)

## Berechnung

Die McGinley-Dynamik-Berechnung wird rekursiv mit der folgenden Formel durchgeführt:

```
MGD = MGD[previous] + (Price - MGD[previous]) / (Length * ((Price / MGD[previous])^4))
```

Dabei gilt:
- Price – aktueller Preis (normalerweise Schlusskurs)
- MGD[vorheriger] – vorheriger Indikatorwert
- Length – Periodenparameter

Für den anfänglichen MGD-Wert wird normalerweise ein einfacher gleitender Durchschnitt über den angegebenen Zeitraum verwendet:

```
Erste Berechnung: MGD = SMA(Price, Length)
```

## Interpretation

McGinley-Dynamik kann ähnlich wie andere gleitende Durchschnitte interpretiert werden, weist jedoch verbesserte Eigenschaften auf:

1. **Trendbestimmung**:
   - Wenn der Preis über MGD liegt, deutet dies auf einen Aufwärtstrend hin
   - Wenn der Preis unter MGD liegt, deutet dies auf einen Abwärtstrend hin
   - Eine steile MGD-Steigung weist auf einen starken Trend hin

2. **Kreuzungen mit dem Preis**:
   - Price, das MGD von unten nach oben kreuzt, kann als bullisches Signal angesehen werden
   - Price, das MGD von oben nach unten kreuzt, kann als rückläufiges Signal angesehen werden
   - Aufgrund seiner adaptiven Natur bilden sich diese Überkreuzungen typischerweise früher als bei herkömmlichen gleitenden Durchschnitten

3. **Mehrere MGD-Frequenzweichen**:
   - Es können mehrere MGDs mit unterschiedlichen Perioden verwendet werden (z. B. MGD(14) und MGD(30))
   - Der kurze MGD, der den langen MGD von unten nach oben kreuzt, kann als Bestätigung des Aufwärtstrends angesehen werden
   - Short MGD, das Long MGD von oben nach unten kreuzt, kann als Bestätigung des Abwärtstrends angesehen werden

4. **Unterstützungs- und Widerstandsstufen**:
   - MGD dient oft als dynamisches Unterstützungsniveau in einem Aufwärtstrend
   - MGD dient oft als dynamisches Widerstandsniveau in einem Abwärtstrend
   - Mehrere Abpraller von MGD bestätigen die Trendstärke

5. **Preisbeziehung**:
   - Der Abstand zwischen Preis und MGD kann auf überkaufte oder überverkaufte Marktbedingungen hinweisen
   - Wenn der Preis erheblich von MGD abweicht, kann dies auf eine mögliche Umkehr oder Korrektur hinweisen

6. **Kombination mit anderen Indikatoren**:
   - MGD funktioniert gut mit Oszillatoren (RSI, Stochastik)
   - Kann als Trendfilter für andere Handelssysteme verwendet werden

7. **Auswahl des Längenparameters**:
   - Kleinere Length-Werte (z. B. 8-12) machen MGD empfindlicher gegenüber Preisänderungen und eignen sich für den kurzfristigen Handel
   - Größere Length-Werte (z. B. 20-50) machen MGD flüssiger und eignen sich für den langfristigen Handel

![MGD Diagramm](../../../../images/indicator_mcginley_dynamic.png)

## Siehe auch

[SMA](sma.md)
[EMA](ema.md)
[DEMA](dema.md)
[HMA](hma.md)
