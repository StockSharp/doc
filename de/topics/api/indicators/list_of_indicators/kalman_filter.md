# KalmanFilter

**Kalman-Filter** ist ein rekursiver Algorithmus, der den zugrunde liegenden Zustand eines Systems anhand verrauschter Beobachtungen schätzt.

Um den Indikator verwenden zu können, müssen Sie die Klasse [KalmanFilter](xref:StockSharp.Algo.Indicators.KalmanFilter) verwenden.

## Beschreibung

Der Kalman-Filter wendet einen Vorhersage-Korrektur-Zyklus an, um Preisdaten zu glätten und Marktstörungen zu reduzieren. Es passt sich dynamisch an, wenn neue Informationen verfügbar werden, was es nützlich macht, Trends in volatilen Märkten zu verfolgen.

## Parameter

- **ProcessNoise** – erwartete Varianz im zugrunde liegenden Prozess.
- **ObservationNoise** – erwartete Varianz in den beobachteten Daten.

## Berechnung

Bei jedem Schritt führt der Filter Folgendes aus:
1. **Vorhersage** des nächsten Zustands basierend auf der vorherigen Schätzung.
2. **Aktualisierung** dieser Vorhersage unter Verwendung der neuesten Preisbeobachtung und der Rauschschätzungen.

Dadurch entsteht eine optimierte Schätzung, die schnell auf Preisänderungen reagiert und gleichzeitig kurzfristige Schwankungen herausfiltert.

![KalmanFilter Diagramm](../../../../images/indicator_kalman_filter.png)

## Siehe auch

[Kaufmans adaptiver gleitender Durchschnitt](kama.md)
[Adaptiver Laguerre-Filter](adaptive_laguerre_filter.md)
