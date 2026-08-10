# Quellcodes

Der Open-Source-Code von [S#](../api.md) ist auf mehrere Repositorys verteilt. Das [StockSharp-Kernrepository](https://github.com/StockSharp/StockSharp) enthält das Nachrichtenmodell, Geschäftsobjekte, gemeinsame Konnektorabstraktionen, Algorithmen, Testwerkzeuge und weitere Grundlagen der Plattform. Anbieterspezifische Konnektorimplementierungen werden nicht im Kernrepository gespeichert.

Alle offenen, anbieterspezifischen Konnektoren werden in [StockSharp\/Connectors](https://github.com/StockSharp/Connectors) gepflegt. Jeder Konnektor ist ein eigenständiges .NET-Projekt, und das Repository enthält `Connectors.slnx`, um sie gemeinsam zu erstellen.

Die eigenständige Browser-Diagramm-Engine und der Diagramm-Stack für Webterminals werden in [StockSharp\/JS-Charts](https://github.com/StockSharp/JS-Charts) gepflegt. Siehe [JavaScript-Diagramme](../api/graphical_user_interface/charts/javascript_charts.md).

[Anleitung zur Verwendung von GitHub](https://docs.github.com/de/get-started/start-your-journey/hello-world)

Liste der Komponenten, die mit Quellcode verfügbar sind:

- Allgemeine Klassen zum Erstellen eigener Verbindungen.
- Format des Marktdatenspeichers.
- Handelssimulator.
- Historischer Simulator (Rücktester).
- Indikatoren der technischen Analyse (mehr als 140).
- Algorithmen zur Berechnung von Gewinn/Verlust, Slippage und Verzögerung.
- Algorithmen zum Erstellen von Kerzen beliebiger Zeitrahmen sowie nicht zeitbasierter Kerzen (Tick, Range usw.).
- Protokollierung.
- Import und Export.

Die Quellcodes aller geschlossenen Komponenten sowie fertiger Programme sind nach dem Kauf verfügbar. Weitere Informationen zu den Kosten der Quellcodes finden Sie unter [Kosten der Quellcodes](https://stocksharp.com/de/store/?groups=22).

## Empfohlene Inhalte

[Installationsanleitung](../api/setup.md)
