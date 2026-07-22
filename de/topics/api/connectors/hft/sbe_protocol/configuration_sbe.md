# SBE-Konfiguration

Konfigurieren Sie die Clientverbindung mit den vom Administrator des SBE-Servers bereitgestellten Parametern:

- `Address` - TCP-Endpunkt des SBE-Servers.
- `SenderCompId` - Clientkennung für die Anmeldung.
- `TargetCompId` - Kennung des Zielservers.
- `Password` - Anmeldedaten für die Authentifizierung.
- `IsSupportNativeCandles` - aktiviert vom Server bereitgestellte Kerzen. Andernfalls werden Kerzen auf dem Client aus Marktdaten gebildet.

Der Server verwendet [SbeServerSettings](xref:StockSharp.Server.Sbe.SbeServerSettings):

- `IsEnabled` - aktiviert den SBE-Endpunkt.
- `Address` - Endpunkt für eingehende Verbindungen. Standardwert: `127.0.0.1:5002`.
- `HeartBeat` - Heartbeat-Intervall der Sitzung. Standardwert: 60 Sekunden.
- `TargetCompId` - Serverkennung. Standardwert: `StockSharp`.

Verwenden Sie auf Client und Server dieselbe SBE-Schemaversion.
