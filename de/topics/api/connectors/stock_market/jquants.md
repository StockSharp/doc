# J-Quants

**J-Quants** verbindet StockSharp mit der J-Quants API V2 für Marktdaten der Tokioter Börse.

## Wichtige Funktionen

- Im Quellcode geprüfte Funktionen: Suche nach TSE-Aktien, -Futures und -Optionen, Level-1-Werte, Tickdaten und historische Kerzen. Der Adapter liefert ausschließlich Marktdaten und unterstützt keine Transaktionen.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.

## Typische Verwendung

Geeignet zur Suche japanischer Instrumente und zum Herunterladen von Kursen, Abschlüssen und Kerzenhistorie für Analysezwecke.

Datenabdeckung, Historientiefe, Anfragelimits und Verfügbarkeit werden vom J-Quants-Abonnement bestimmt.

## Siehe auch

[Connector-Konfiguration](jquants/configuration_jquants.md)

[Grafische Konfiguration](jquants/graphical_configuration_jquants.md)

[Adapterinitialisierung](jquants/adapter_initialization_jquants.md)
