# Ichimoku

**Ichimoku** ist ein Indikator, der durch eine Kombination aus fünf Linien dargestellt wird, von denen drei gleitende Durchschnitte und zwei Ableitungen davon sind. Ichimoku identifiziert das Vorhandensein eines Trends und zeigt Unterstützungs-/Widerstandszonen und Trend-Retracements an.

Um den Indikator zu verwenden, sollte die Klasse [Ichimoku](xref:StockSharp.Algo.Indicators.Ichimoku) verwendet werden.
##### Beschreibung des Ichimoku-Indikators

Grafisch besteht der Indikator aus fünf farbigen Linien, die einfachen gleitenden Durchschnitten ähneln:

- Tenkan (Umrechnungslinie) – die schnellste Linie, sie reagiert zuerst auf Preisänderungen. Sein Hauptzweck besteht darin, die Richtung des kurzfristigen Trends zu bestimmen. In der klassischen Version geht es um ein Segment von 9 Takten zurück. Er setzt sich aus der halben Summe des höchsten und niedrigsten Preises zusammen.

- Kijun (Basislinie) – zeigt den mittelfristigen Trend mit einem Zeitraum von 26 an.

- Senkou A und Senkou B – werden 26 Perioden in die Zukunft projiziert und angezeigt. Zusammen bilden sie die sogenannte Wolke (Kumo), die Unterstützungs- und Widerstandsbereiche anzeigt und eine Schlüsselkomponente des Indikators darstellt.

- Chikou (nacheilende Spanne) – stellt den letzten Schlusskurs dar, der um 26 Perioden nach hinten verschoben wurde. Es hilft, Signale zu bestätigen: Wenn es den Chart von unten nach oben kreuzt, ist es ein Kaufsignal, und von oben nach unten – ein Verkaufssignal. Chikou fungiert im Wesentlichen als Trendfilter.

![Ichimoku Diagramm](../../../../images/indicatorichimoku.png)

## Siehe auch

[JMA](jma.md)
