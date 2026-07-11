# PP

**Pivot Points (PP)** ist ein technischer Indikator, der frühere Höchst-, Tiefst- und Schlusskurse verwendet, um potenzielle Unterstützungs- und Widerstandsniveaus für die aktuelle Handelsperiode zu bestimmen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [PivotPoints](xref:StockSharp.Algo.Indicators.PivotPoints) verwenden.

## Beschreibung

Pivot Points (Pivot-Punkte) sind eine der ältesten und am weitesten verbreiteten Methoden zur Identifizierung wichtiger Marktniveaus. Der Indikator berechnet den zentralen Drehpunkt (PP) und mehrere Unterstützungsniveaus (S1, S2, S3) und Widerstandsniveaus (R1, R2, R3) basierend auf den Daten der vorherigen Periode.

Ursprünglich wurden Pivot Points von Händlern auf Börsenparketts verwendet, um anhand der Daten des Vortages Schlüsselniveaus für den aktuellen Handelstag zu ermitteln. Heute wurde diese Methode für verschiedene Zeitrahmen angepasst – von Intraday bis monatlich.

Die Hauptidee hinter Pivot Points besteht darin, dass der Markt dazu neigt, auf diese vorberechneten Niveaus zu reagieren und sie als Umkehrpunkte oder Zonen zu nutzen, in denen eine Konsolidierung stattfinden kann. Händler nutzen diese Niveaus, um Entscheidungen über den Markteintritt und -austritt zu treffen sowie Zielniveaus und Stop-Losses festzulegen.

## Berechnung

Es gibt verschiedene Methoden zur Berechnung von Pivot Points, darunter Standard, Fibonacci, Woodie, Camarilla und DeMark. Nachfolgend finden Sie die Standardberechnungsmethode:

1. Berechnen Sie den Hauptdrehpunkt (PP):
   ```
   PP = (High + Low + Close) / 3
   ```

2. Berechnen Sie die ersten Widerstands- (R1) und Unterstützungsniveaus (S1):
   ```
   R1 = (2 * PP) - Low
   S1 = (2 * PP) - High
   ```

3. Berechnen Sie die zweiten Widerstands- (R2) und Unterstützungsniveaus (S2):
   ```
   R2 = PP + (High - Low)
   S2 = PP - (High - Low)
   ```

4. Berechnen Sie die dritten Widerstands- (R3) und Unterstützungsniveaus (S3):
   ```
   R3 = High + 2 * (PP - Low)
   S3 = Low - 2 * (High - PP)
   ```

Dabei gilt:
- High – höchster Preis der Vorperiode
- Low - niedrigster Preis der Vorperiode
- Close – Schlusskurs der Vorperiode

## Interpretation

Pivot Points kann wie folgt interpretiert werden:

1. **Hauptdrehpunkt (PP)**:
   - PP dient als primäre Referenz zur Bestimmung der allgemeinen Marktstimmung
   - Wenn der Preis über PP liegt, deutet dies auf eine bullische Stimmung hin
   - Wenn der Preis unter PP liegt, deutet dies auf eine rückläufige Stimmung hin
   - PP kann auch als Unterstützungs- oder Widerstandsniveau dienen

2. **Widerstandsstufen (R1, R2, R3)**:
   - Diese Niveaus stellen potenzielle Widerstandszonen in einem bullischen Markt dar
   - Ein Ausbruch aus einem Level kann zu einer weiteren Bewegung zum nächsten Level führen
   - Ein Abprall von einem Niveau kann zu einer Abwärtsumkehr führen

3. **Unterstützungsstufen (S1, S2, S3)**:
   - Diese Niveaus stellen potenzielle Unterstützungszonen in einem rückläufigen Markt dar
   - Ein Ausbruch aus einem Level kann zu einer weiteren Bewegung zum nächsten Level führen
   - Ein Absprung von einem Niveau kann zu einer Aufwärtsumkehr führen

4. **Handelsstrategien**:
   - **Rückprall-Handel**: Eröffnen Sie eine Position, wenn der Preis von einem Unterstützungs- oder Widerstandsniveau abprallt
   - **Ausbruchshandel**: Eröffnen Sie eine Position nach einem bestätigten Ausbruch aus einem Niveau
   - **Seitwärtshandel**: Kaufen Sie bei Unterstützungsniveaus und verkaufen Sie bei Widerstandsniveaus
   - **Zielsetzung**: Nutzen Sie das nächste Level als Gewinnmitnahmeziel
   - **Stop-Loss-Platzierung**: Platzieren Sie Stop-Loss-Werte über den entsprechenden Niveaus

5. **Kombination mit anderen Indikatoren**:
   - Pivot Points werden häufig in Kombination mit anderen technischen Indikatoren zur Bestätigung von Signalen verwendet
   - Besonders effektiv in Kombination mit Momentum-Indikatoren (RSI, Stochastik) und Trendindikatoren (MA, MACD)

6. **Zeitrahmen**:
   - Tägliche Pivot Points werden basierend auf dem vorherigen Handelstag berechnet
   - Wöchentliche Pivot Points werden basierend auf der Vorwoche berechnet
   - Monatliche Pivot Points werden basierend auf dem Vormonat berechnet
   - Die Auswahl des Zeitrahmens hängt vom Handelsstil und Zeithorizont ab

![indicator_pivot_points](../../../../images/indicator_pivot_points.png)

## Siehe auch

[FibonacciRetracement](fibonacci_retracement.md)
