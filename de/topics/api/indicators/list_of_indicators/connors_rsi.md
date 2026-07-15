# CRSI

**Connors-RSI (CRSI)** ist ein umfassender technischer Indikator, der von Larry Connors entwickelt wurde und drei Komponenten kombiniert, um überkaufte und überverkaufte Marktbedingungen zu messen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [ConnorsRSI](xref:StockSharp.Algo.Indicators.ConnorsRSI) verwenden.

## Beschreibung

Connors-RSI ist eine erweiterte Version des traditionellen Relative-Stärke-Index (RSI), der zwei zusätzliche Komponenten hinzufügt, um genauere überkaufte und überverkaufte Signale zu liefern.

Im Gegensatz zum Standard-RSI, das nur Preisänderungen berücksichtigt, berücksichtigt Connors-RSI auch den Streak (Reihe aufeinanderfolgender Preisbewegungen in eine Richtung) und die Änderungsrate (ROC), wodurch es empfindlicher auf kurzfristige Änderungen reagiert und extreme Marktbedingungen zuverlässiger erkennt.

CRSI ist besonders nützlich für:
- Identifizierung kurzfristiger Ein- und Ausstiegsmöglichkeiten
- Bestimmung extremer überkaufter und überverkaufter Niveaus
- Erstellen von Handelssystemen basierend auf der Rückkehr zum Mittelwert
- Filtern von Signalen anderer Indikatoren

## Parameter

Der Indikator hat die folgenden Parameter:
- **RSI-Zeitraum** – Zeitraum zur Berechnung der RSI-Komponente (Standardwert: 3)
- **Serien-RSI-Zeitraum** – Zeitraum zur Berechnung der Streak-RSI-Komponente (Standardwert: 2)
- **ROC-RSI-Zeitraum** – Zeitraum zur Berechnung der Änderungsrate der RSI-Komponente (Standardwert: 100)

## Berechnung

Die Connors-RSI-Berechnung umfasst drei Komponenten, die dann gemittelt werden, um den Endwert zu erhalten:

1. **Preis-RSI-Komponente** – Standard-RSI, berechnet über einen kurzen Zeitraum (normalerweise 3 Tage):
   ```
   RSI = 100 - (100 / (1 + RS))
   wobei RS = durchschnittliche positive Änderung / durchschnittliche negative Änderung
   ```

2. **Streak RSI-Komponente**:
   - Berechnen Sie zunächst den Streak (Anzahl aufeinanderfolgender Tage mit Preisanstieg oder -abfall).
   - Wenden Sie anschließend RSI mit dem StreakRSIPeriod auf diese Streak-Serie an

3. **Änderungsrate RSI-Komponente (ROC RSI)**:
   - Berechnen Sie den Perzentilrang des aktuellen ROC gegenüber dem ROCRSIPeriod
   - Skalieren Sie den Perzentilrang von 0 bis 100

4. **Endgültiger Connors-RSI-Wert**:
   ```
   CRSI = (RSI + StreakRSI + ROCRSI) / 3
   ```

## Interpretation

Connors-RSI oszilliert zwischen 0 und 100, ähnlich dem Standard-RSI:

- **Extrem hohe Werte (über 90)** weisen auf stark überkaufte Bedingungen hin. Dies kann ein Signal zum Verkauf oder zum Eingehen einer Verkaufsposition sein.

- **Extrem niedrige Werte (unter 10)** weisen auf stark überverkaufte Bedingungen hin. Dies kann ein Signal zum Kauf oder zum Schließen einer Verkaufsposition sein.

- **Standardniveaus**:
  - Über 70-80: überkauft
  - Unter 20-30: überverkauft
  - 40-60: neutrale Zone

- **Abweichungen**:
  - Bullische Divergenz: Der Preis bildet ein neues Tief, während CRSI ein höheres Tief bildet
  - Bärische Divergenz: Der Preis bildet ein neues Hoch, während CRSI ein niedrigeres Hoch bildet

Connors-RSI funktioniert am besten auf Charts mit Zeitrahmen von täglich bis wöchentlich und in Handelsstrategien, die sich an der Rückkehr zum Mittelwert orientieren.

![CRSI Diagramm](../../../../images/indicator_connors_rsi.png)

## Siehe auch

[RSI](rsi.md)
[RMI](relative_momentum_index.md)
[LRSI](laguerre_rsi.md)
