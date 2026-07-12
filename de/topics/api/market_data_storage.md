# Marktdatenspeicher

[S#](../api.md) stellt Funktionen zum Speichern und Laden von Daten bereit. Datenspeicherung kann in mehreren Fällen notwendig sein:

1. Während der Arbeit einer Handelsstrategie, wenn der Zustand gespeichert werden muss (Orders, Positionen, Einstellungen usw.);
2. Speichern von Marktdaten aus dem Handelsterminal für Algorithmustests in Echtzeit;
3. Sammeln von Daten für Analyse und Mustererkennung.

[S#](../api.md) macht den Speichermechanismus durch Zugriff auf höherer Ebene und das Verbergen technischer Details verständlich (weitere Details siehe [API](market_data_storage/api.md)). Außerdem ist er vielseitig und bietet die Möglichkeit, die Typen unterstützter Speicher zu erweitern.

## Empfohlene Inhalte

[Arbeiten mit der API](market_data_storage/api.md)

[Arbeiten mit Remote-Speicher](market_data_storage/remote.md)

