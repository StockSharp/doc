# Konfiguration des Webull-Konnektors

Erstellen Sie eine Anwendung in der [Webull OpenAPI](https://developer.webull.com/apis/docs/) und beziehen Sie deren Zugangsdaten.

- **Schlüssel** (`Key`) — der Anwendungsschlüssel der Webull OpenAPI.
- **Geheimnis** (`Secret`) — das Anwendungsgeheimnis.
- **Zugriffstoken** (`Token`) — ein optionales Zugriffstoken.
- **Konto** (`Account`) — die Kennung des Handelskontos. Ist kein Wert angegeben, ruft der Konnektor die Kontoliste automatisch ab.
- **Demomodus** (`IsDemo`) — Verwendung der Webull-Testumgebung.

Die Einstellungen `Token` und `Account` können entfallen, wenn sie für Ihre Webull-Anwendung und Ihr Konto nicht erforderlich sind.
