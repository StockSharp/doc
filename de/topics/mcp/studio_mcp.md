# Programmbefehle (Studio.Mcp)

**Studio.Mcp** ist ein MCP-Server, über den ein Agent laufenden StockSharp-Programmen Befehle gibt: Er platziert und storniert Orders im Terminal, verwaltet die Datenquellen von Hydra und startet Strategietests im Designer und in Shell. Der Server drückt keine Schaltflächen und liest keine Fenster — den Befehl führt das Programm selbst aus.

Der Server startet nichts und speichert nichts. Ein Befehl erreicht ein Programm nur dann, wenn das Programm geöffnet ist; läuft das Programm nicht, antwortet der Server genau das.

## Welche Programme antworten

| Programm | Welche Befehle es annimmt |
|---|---|
| [Terminal](../terminal.md) | Liste der Orders, Platzieren und Stornieren einer Order |
| [Hydra](../hydra.md) | Arten von Quellen, Liste der Quellen, Hinzufügen, Aktivieren, Starten, Stoppen und Löschen einer Quelle |
| [Designer](../designer.md) | Liste der Strategien, Starten und Stoppen des Tests auf historischen Daten |
| [Shell](../shell.md) | Liste der Strategien des Bereichs [Emulation](../shell/user_interface/emulation.md), Starten und Stoppen ihrer Durchläufe |

Die Befehle nehmen nur die Avalonia-Versionen dieser Programme an, und zwar im normalen Build: Ein eigener Build ist nicht nötig. Ein Programm öffnet den Kanal für Befehle zusammen mit seinem Hauptfenster und schließt ihn, wenn das Fenster geschlossen wird. Die WPF-Versionen von Terminal, Hydra und Designer nehmen diese Befehle nicht an.

Der Kanal ist eine lokale benannte Pipe, die nur den Prozessen des aktuellen Betriebssystembenutzers zugänglich ist. Über das Netzwerk werden keine Befehle übertragen.

## Installieren

Der Server wird aus dem Quellcode von StockSharp gebaut:

```sh
dotnet build Studio/Studio.Mcp/Studio.Mcp.csproj -c Release
```

Die fertige Datei ist `Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe`. Eigene Einstellungen hat der Server nicht: Er braucht weder Argumente noch Umgebungsvariablen. Das Protokoll schreibt er in die Standardausgabe, sein Log in den Standardfehlerstrom; manuell ohne Client gestartet, wartet er einfach auf Eingaben.

## Anbindung des Agenten

Claude Code — die Datei `.mcp.json` des Projekts:

```json
{
  "mcpServers": {
    "stocksharp": {
      "command": "Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe"
    }
  }
}
```

In anderen MCP-Clients, auch in Claude Desktop, wird der Server genauso angebunden: Der Befehl ist der Pfad zu `StockSharp.Studio.Mcp.exe`, ohne Argumente und Umgebungsvariablen.

## Was der Agent kann

Die Kennungen von Quellen, Strategien und Orders werden aus den Antworten der Listenwerkzeuge entnommen: zuerst die Liste, dann die Aktion.

### Allgemein

| Werkzeug | Was es tut |
|---|---|
| `list_products` | welche Programme laufen und welche Befehle jedes von ihnen annimmt |
| `describe_product` | die Befehle eines Programms; `product` — `Terminal`, `Hydra`, `Designer` oder `Shell` |
| `close_product` | bittet das Programm, sich zu schließen |

### Hydra

| Werkzeug | Was es tut |
|---|---|
| `hydra_list_source_types` | die Arten von Quellen, die Hydra erstellen kann, mit ihren Kennungen |
| `hydra_list_sources` | die Quellen von Hydra: ob jede aktiviert ist und ob sie gerade herunterlädt |
| `hydra_add_source` (`typeId`) | fügt eine Quelle der gewählten Art hinzu und gibt sie zurück; die neue Quelle ist deaktiviert |
| `hydra_enable_source` (`sourceId`, `isEnabled`) | nimmt die Quelle in das Herunterladen auf oder schließt sie davon aus |
| `hydra_start_source` (`sourceId`) | startet das Herunterladen durch die Quelle; eine deaktivierte Quelle startet nicht |
| `hydra_stop_source` (`sourceId`) | stoppt das Herunterladen |
| `hydra_remove_source` (`sourceId`) | löscht die Quelle zusammen mit ihren Einstellungen; eine laufende Quelle muss zuerst gestoppt werden |

Um Daten mit einer neuen Quelle herunterzuladen, sind drei Aufrufe nötig: `hydra_add_source`, `hydra_enable_source`, `hydra_start_source`. Die Einstellungen der Quelle — Instrumente, Datentypen, Verbindungsparameter — ändern diese Befehle nicht: Sie werden im Fenster von Hydra festgelegt (siehe [Datenquellen](../hydra/data_sources.md)).

### Designer

| Werkzeug | Was es tut |
|---|---|
| `designer_list_strategies` | die Strategien des Designers: die Art (`Backtest`, `Optimization`, `Live`, `Remote`, `Other`) und der aktuelle Zustand |
| `designer_start_backtest` (`strategyId`, `startTime`, `stopTime`) | startet eine Strategie der Art `Backtest` oder `Optimization` auf historischen Daten; der Zeitraum wird in UTC im Format ISO 8601 angegeben und ersetzt den Zeitraum in den Einstellungen der Strategie, ohne ihn wird der Zeitraum aus den Einstellungen genommen |
| `designer_stop_backtest` (`strategyId`) | stoppt eine laufende Strategie |

### Terminal

| Werkzeug | Was es tut |
|---|---|
| `terminal_list_orders` | die Orders des Terminals mit der Transaktionsnummer jeder Order |
| `terminal_register_order` (`securityId`, `portfolioName`, `side`, `volume`, `price`, `orderType`) | platziert eine Order und gibt ihre Transaktionsnummer zurück |
| `terminal_cancel_order` (`orderTransactionId`) | storniert eine Order anhand der Transaktionsnummer |

Das Instrument wird so angegeben, wie Terminal es kennt, zum Beispiel `SBER@TQBR`; das Portfolio — mit dem Namen, der Terminal bekannt ist; `side` — `Buy` oder `Sell`. Eine Limit-Order braucht einen Preis; bei Orders anderer Typen wird der übergebene Preis nicht verwendet. Eine Order wird durch die Transaktionsnummer bezeichnet, die `terminal_register_order` oder `terminal_list_orders` zurückgegeben hat, und nicht durch die Nummer, die die Börse später vergibt.

### Shell

Die Werkzeuge für Shell baut der Server aus der Beschreibung der Befehle, die das Programm selbst sendet. Deshalb gibt es sie nur, wenn Shell beim Start des Servers oder beim letzten Aufruf von `list_products` oder `describe_product` lief.

| Werkzeug | Was es tut |
|---|---|
| `shell_list_strategies` | die Strategien des Bereichs Emulation und ihr Zustand |
| `shell_start_backtest` (`StrategyId`, `StartTime`, `StopTime`) | startet einen Durchlauf der Strategie auf historischen Daten |
| `shell_stop_backtest` (`StrategyId`) | stoppt den Durchlauf |

Shell testet eine Strategie immer auf dem Zeitraum aus ihren Einstellungen: Wenn `StartTime` oder `StopTime` übergeben wird, wird der Befehl abgelehnt, übergeben Sie darin also `null`.

## Einschränkungen und Sicherheit

> [!CAUTION]
> `terminal_register_order` platziert eine echte Order dort, wohin Terminal verbunden ist. Der Server fragt nicht nach: Die Bestätigung ist Sache des MCP-Clients, in dem ein Mensch den Aufruf sieht, bevor er ausgeführt wird.

- **Strategien, die echt handeln, werden nicht gestartet.** Der Designer startet nur Strategien der Arten `Backtest` und `Optimization`; für die übrigen wird der Befehl abgelehnt. Shell öffnet für Befehle nur die Strategien des Bereichs Emulation.
- **Löschen ist unumkehrbar.** `hydra_remove_source` löscht die Quelle zusammen mit ihren Einstellungen.
- **Schließen.** `close_product` schließt das Programm.
- **Das Programm muss laufen.** Andernfalls gibt der Befehl den Fehler `<Programm> is not running.` zurück. Das Warten auf eine Antwort von einem nicht laufenden Programm dauert etwa 5 Sekunden, deshalb antwortet `list_products` bei geschlossenen Programmen nicht sofort. Die Ablehnung durch ein laufendes Programm erreicht den Agenten als Text: `<Programm> refused the command: <Grund>`.
- **Ohne Passwort.** Der Kanal ist für jedes Programm offen, das unter demselben Benutzerkonto gestartet wurde: Eine eigene Prüfung, wer den Befehl gesendet hat, gibt es nicht.
- **Kopien von Programmen werden nicht unterschieden.** Wenn mehrere Kopien von Terminal oder Designer laufen oder neben dem normalen Hydra ein Hydra mit Testprofil arbeitet, gelangt der Befehl in eine von ihnen, und in welche, ist vorher nicht bekannt.
- **Keine Testergebnisse.** Der Server gibt nur den Zustand der Strategie zurück, aber weder Statistik noch Trades. Lesen kann sie [DesktopDriver](desktop_driver.md) aus den Panels des Programms.

## Beispielsitzung

Der Benutzer bittet: „Starte im Designer den Test der Strategie für Januar und prüfe, dass er läuft“.

1. `list_products` — der Designer läuft; zu den übrigen Programmen — `… is not running.`
2. `designer_list_strategies` — unter den Strategien ist die benötigte, von der Art `Backtest`, im Zustand `Stopped`.
3. `designer_start_backtest`, `strategyId` aus Schritt 2, `startTime` = `2026-01-01T00:00:00Z`, `stopTime` = `2026-02-01T00:00:00Z` — die Antwort ist `Strategy <Kennung> was started.`
4. `designer_list_strategies` — der Zustand der Strategie hat sich geändert.
5. `designer_stop_backtest` mit derselben `strategyId`, wenn der Test abgebrochen werden soll.

## Siehe auch

[KI-Agenten anbinden (MCP)](../mcp.md)

[Steuerung der Oberfläche (DesktopDriver)](desktop_driver.md)

[Fernsteuerung](../shell/remote_control.md)
