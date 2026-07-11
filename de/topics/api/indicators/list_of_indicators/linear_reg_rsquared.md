# R-Quadrat

**R-Quadrat der linearen Regression (R-Quadrat der linearen Regression)** ist ein technischer Indikator, der misst, wie gut die lineare Regression Preisdaten annähert und die Stärke des Markttrends bestimmt.

Um den Indikator verwenden zu können, müssen Sie die Klasse [LinearRegRSquared](xref:StockSharp.Algo.Indicators.LinearRegRSquared) verwenden.

## Beschreibung

R-Quadrat der linearen Regression (R²) ist ein statistisches Maß, mit dem der Grad der Übereinstimmung zwischen Preisdaten und einer durch diese Daten gezogenen linearen Regressionslinie bewertet wird. Im Rahmen der technischen Analyse zeigt R² an, wie gut die aktuelle Preisbewegung einem linearen Trend entspricht.

R²-Werte reichen von 0 bis 1 (oder 0 % bis 100 %):
- Ein Wert nahe 1 (100 %) zeigt an, dass die Preise sehr gut entlang der Trendlinie ausgerichtet sind, was auf einen starken Trend hinweist
- Ein Wert nahe 0 weist auf das Fehlen eines linearen Trends hin und ist charakteristisch für seitwärts gerichtete, chaotische oder zyklische Märkte

Der Indikator hilft Händlern, zwischen Phasen starker Trends und Phasen der Konsolidierung oder Seitwärtsbewegungen zu unterscheiden und ermöglicht ihnen so die Auswahl einer geeigneten Handelsstrategie.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Zeitraum für lineare Regressionsberechnung (Standardwert: 14)

## Berechnung

Die R-Quadrat der linearen Regression-Berechnung umfasst die folgenden Schritte:

1. Erstellen einer linearen Regressionslinie für Preisdaten über den Length-Zeitraum:
   ```
   y = a + b*x
   ```
   Dabei gilt:
   - y - Preis (abhängige Variable)
   - x - Periodenfolgenummer (unabhängige Variable)
   - a - freier Term (y-Achsenabschnitt)
   - b - Steigungskoeffizient

2. Berechnung der Summe der quadratischen Abweichungen von der Regression (SSE):
   ```
   SSE = Sum((tatsächlicher Preis - prognostizierter Preis)^2)
   ```
   Dabei gilt:
   - Tatsächlicher Price - tatsächlicher Preis
   - Vorhergesagter Price – vorhergesagter Preis aus der Regressionsgleichung

3. Berechnung der Gesamtsumme der Quadrate (SST):
   ```
   SST = Sum((tatsächlicher Preis - Durchschnittspreis)^2)
   ```
   Dabei ist Durchschnittspreis der Durchschnittspreis über den Length-Zeitraum

4. Berechnung von R²:
   ```
   R² = 1 - (SSE / SST)
   ```

## Interpretation

R-Quadrat der linearen Regression kann wie folgt interpretiert werden:

1. **Bewertung der Trendstärke**:
   - Werte über 0,7 (70 %) weisen auf einen starken Trend hin
   - Werte zwischen 0,3 und 0,7 (30-70 %) weisen auf einen moderaten Trend hin
   - Werte unter 0,3 (30 %) weisen auf einen schwachen oder keinen Trend hin

2. **Auswahl der Handelsstrategie**:
   - Bei hohen R²-Werten (starker Trend) sind Trendfolgestrategien wirksam
   - Bei niedrigen R²-Werten (Seitwärtsbewegung) sind Range-Trading-Strategien effektiv

3. **Übergangspunktsuche**:
   - Ein steigender R² könnte die Bildung eines neuen Trends signalisieren
   - Ein sinkender R² kann auf eine Abschwächung des Trends und eine mögliche Konsolidierung oder Umkehr hinweisen

4. **Signalfilterung**:
   - Signale von Trendindikatoren sind bei hohen R²-Werten zuverlässiger
   - Oszillatorsignale sind bei niedrigen R²-Werten zuverlässiger

5. **Kombination mit anderen Indikatoren**:
   - R² wird oft verwendet, um den Marktmodus zu bestimmen, woraufhin entsprechende Indikatoren angewendet werden
   - Verwenden Sie beispielsweise gleitende Durchschnitte mit hohem R² und stochastische Oszillatoren mit niedrigem R²

6. **Bewertung der Marktvorhersehbarkeit**:
   - High R²-Werte deuten auf eine vorhersehbarere kurzfristige Preisbewegung hin
   - Niedrige R²-Werte deuten auf chaotischere, unvorhersehbarere Bewegungen hin

7. **Zeitrahmen**:
   - R² kann in unterschiedlichen Zeitrahmen zu unterschiedlichen Ergebnissen führen
   - Der Vergleich von R² über Zeiträume hinweg kann zusätzliche Informationen über die Marktstruktur liefern

![indicator_linear_reg_r_squared](../../../../images/indicator_linear_reg_rsquared.png)

## Siehe auch

[LinearRegression](lrc.md)
[StandardError](standard_error.md)
[ChoppinessIndex](choppiness_index.md)
