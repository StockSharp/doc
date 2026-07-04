# Verbindung IQFeed

Die Verbindung zu den IQ-Servern wird ueber die Anwendung IQConnect hergestellt, die entweder auf dem lokalen oder auf einem entfernten Computer installiert sein kann. Die Kommunikation zwischen der Clientanwendung und IQConnect sowie zwischen IQConnect und den Servern erfolgt ueber das TCP/IP-Protokoll.

Zum Abrufen der Daten verwendet die Clientanwendung vier Verbindungen ueber verschiedene Ports:

1. Level1 (Port 5009) - wird fuer Echtzeitdaten zu Instrumenten (Ticks, Eroeffnungskurs, Schlusskurs, Volatilitaet usw.) und Nachrichten verwendet.
2. Level2 (Port 9200) - wird verwendet, um erweiterte Quotes zu Instrumenten abzurufen; fuer jedes ECN kann das beste Quote-Paar abgerufen werden.
3. Lookup (Port 9100) - wird fuer die Suche nach Instrumenten, das Abrufen historischer Daten und das Erhalten erweiterter Informationen zu Nachrichten verwendet.
4. Admin (Port 9300) - wird verwendet, um allgemeine Informationen zur Verbindung abzurufen und Einstellungen zu aendern.

Die in Klammern angegebenen Portnummern werden standardmaessig fuer die Verbindung mit IQConnect verwendet. Fuer Clientverbindungen koennen Sie die Portnummern in der Registry aendern, zum Beispiel fuer Level1 unter folgendem Pfad: \[HKEY\_CURRENT\_USER\\SOFTWARE\\DTN\\IQFEED\\Startup\\Level1Port\]. Die Portnummern fuer die Verbindung zu IQ-Servern koennen nicht geaendert werden.
