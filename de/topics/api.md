# API-Dokumentation

## Übersicht

Die StockSharp-API, auch bekannt als S#-API, ist ein Software Development Kit (SDK) zur Erstellung von Handelsanwendungen wie [Designer](designer.md), [Terminal](terminal.md) und anderen benutzerdefinierten Handelswerkzeugen. Sie stellt die Kerninfrastruktur für Marktdaten, Orderrouting, Strategieausführung, Tests und Handels-UI-Komponenten bereit.

## Funktionen

- **Strategie-Skripting**: Mit der StockSharp-API können Benutzer Handelsstrategien direkt im [Designer](designer/strategies/using_code.md) schreiben und ausführen. Strategien können mit C#, F# oder Python entwickelt, getestet und bereitgestellt werden.

- **Analysetools**: Die API integriert sich mit Hydra für detaillierte [Marktdatenanalysen](hydra/analytics.md). Sie unterstützt Datenverarbeitung, Speicherung und benutzerdefinierte Analyse-Workflows.

- **Entwicklung eigener Anwendungen**: Entwickler können die StockSharp-API nutzen, um eigenständige [Handelslösungen](api/examples.md) zu erstellen, anstatt sich ausschließlich auf das integrierte Skripting der Anwendungen zu verlassen.

- **Connectors und Desktop-Steuerelemente**: Die API enthält viele [Konnektoren](api/connectors.md) für Echtzeit-Marktdaten und Handelszugang. Sie bietet außerdem anpassbare [Desktop-Steuerelemente](api/graphical_user_interface.md) zum Aufbau professioneller Handelsplattformen.

## Architektur

Die StockSharp-API ist auf Modularität und [Erweiterbarkeit](api/connectors/creating_own_connector.md) ausgelegt. Entwickler können sie mit Plugins und zusätzlichen Modulen erweitern, ohne das Kernsystem zu verändern. Diese Architektur hilft beim Aufbau skalierbarer und wartbarer Handelsanwendungen.

## Quelloffen

Der Kern der StockSharp-API ist quelloffen. Der Quellcode ist auf GitHub verfügbar, sodass Entwickler ihn studieren, modifizieren und Verbesserungen beisteuern können.

## GitHub-Repository

Der offizielle Quellcode der StockSharp-API ist im GitHub-Repository verfügbar:

[StockSharp GitHub-Repository](https://github.com/stocksharp/stocksharp)
