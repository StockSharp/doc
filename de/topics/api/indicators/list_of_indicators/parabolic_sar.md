# parabolischer SAR

**parabolischer SAR (SAR)** – Ein Trendindikator, der Preisstopp- und Umkehrpunkte sowie die Trendrichtung anzeigt.

Um den Indikator zu verwenden, sollte die Klasse [ParabolicSar](xref:StockSharp.Algo.Indicators.ParabolicSar) verwendet werden.
##### Indikatorberechnung

Der Preis des Indikatorpunkts (SAR) für die nächste Periode (Kerze) wird anhand der folgenden Formeln berechnet:

SAR(n+1) = SAR(n) + a * (hoch – SAR(n)), für einen Aufwärtstrend;
SAR(n+1) = SAR(n) + a * (niedrig – SAR(n)), für einen Abwärtstrend, wobei:

SAR(n+1) — Preis für den Zeitraum n+1;
SAR(n) – Preis für den Zeitraum n;
hoch und niedrig – neues Maximum bzw. neues Minimum (Extreme). Sie werden für die Zeitspanne zwischen der Aktivierung des vorherigen Anzeigesignals und dem aktuellen Zeitpunkt berücksichtigt;

a – Beschleunigungsfaktor.

Der Beschleunigungsfaktor ist ein variabler Koeffizient, der durch minimale, maximale Werte und einen Änderungsschritt gekennzeichnet ist.

Am Umkehrpunkt nimmt der Faktor einen Mindestwert von einer Stufe an, und sobald der Preis je nach Trend einen neuen Extremwert (Hoch oder Tief) erreicht, wird der Faktor um eine Stufe erhöht. Wenn der Faktor seinen Maximalwert erreicht, wird sein Wachstum angehalten.

![IndicatorParabolicSar](../../../../images/indicatorparabolicsar.png)

## Siehe auch

[Peak](peak.md)
