# Verbindung IQFeed

Die Verbindung zu den IQ-Servern wird über die Anwendung IQConnect hergestellt, die entweder auf dem lokalen oder auf einem entfernten Computer installiert sein kann. Die Kommunikation zwischen der Clientanwendung und IQConnect sowie zwischen IQConnect und den Servern erfolgt über das TCP/IP-Protokoll.

Zum Abrufen der Daten verwendet die Clientanwendung vier Verbindungen über verschiedene Ports:

1. Level1 (Port 5009) - wird für Echtzeitdaten zu Instrumenten (Ticks, Eröffnungskurs, Schlusskurs, Volatilität usw.) und Nachrichten verwendet.
2. Level2 (Port 9200) - wird verwendet, um erweiterte Quotes zu Instrumenten abzurufen; für jedes ECN kann das beste Quote-Paar abgerufen werden.
3. Lookup (Port 9100) - wird für die Suche nach Instrumenten, das Abrufen historischer Daten und das Erhalten erweiterter Informationen zu Nachrichten verwendet.
4. Admin (Port 9300) - wird verwendet, um allgemeine Informationen zur Verbindung abzurufen und Einstellungen zu ändern.

Die in Klammern angegebenen Portnummern werden standardmäßig für die Verbindung mit IQConnect verwendet. Für Clientverbindungen können Sie die Portnummern in der Registry ändern, zum Beispiel für Level1 unter folgendem Pfad: \[HKEY\_CURRENT\_USER\\SOFTWARE\\DTN\\IQFEED\\Startup\\Level1Port\]. Die Portnummern für die Verbindung zu IQ-Servern können nicht geändert werden.
