# Steuerung der Oberfläche (DesktopDriver)

**DesktopDriver** ist ein MCP-Server, über den ein Agent mit den Fenstern eines laufenden Programms so arbeitet, wie ein Mensch mit ihnen arbeitet: Er findet Panels und Schaltflächen, liest Tabellen, Charts, Orderbücher und Eigenschafteneditoren, klickt, gibt Text ein, scrollt und erstellt Aufnahmen.

Der Server liest die Oberfläche selbst und nicht ein Bild des Bildschirms. Eine Tabelle antwortet mit ihren Spalten und Zeilen, ein Chart mit den Punkten, die es gezeichnet hat, ein Orderbuch mit seinen Preisstufen. Die Antwort hängt weder vom Design noch von der Schrift noch von der Sprache des Programms ab. Aufnahmen sind für den Menschen gedacht, den Inhalt prüft man durch Lesen.

Der Server wird als NuGet-Paket `StockSharp.DesktopDriver.Mcp` verteilt und nennt sich `StockSharp.DesktopDriver`; im MCP Registry ist er als `io.github.StockSharp/desktop-driver` eingetragen. Der Quellcode liegt im Repository [DesktopDriver.Mcp](https://github.com/StockSharp/DesktopDriver.Mcp).

## Wie es funktioniert

1. Der Treiber wird beim Bauen des Programms in das Programm aufgenommen. Im normalen Build ist er nicht enthalten.
2. Ein Programm mit Treiber, das mit dem Schalter `--ui-automation` gestartet wird, öffnet einen lokalen Kanal — eine benannte Pipe, die nur dem aktuellen Betriebssystembenutzer zugänglich ist — und schreibt dessen Adresse in die Datei, die mit dem Schalter `--ui-automation-endpoint=<Pfad>` angegeben ist. Die Datei erscheint, wenn das Fenster bereits aufgebaut und gezeichnet ist. Ohne den Schalter `--ui-automation` öffnet das Programm nichts.
3. Der MCP-Server startet das Programm aus seinem Katalog mit diesen Schaltern oder verbindet sich mit einem Programm, das jemand anderes gestartet hat. Beim Verbinden prüft er, dass das richtige Programm, die richtige Kopie davon und eine kompatible Protokollversion geantwortet haben, und erst danach führt er die Aufrufe des Agenten aus.

Dieselben Operationen sind über die Befehlszeile verfügbar — mit dem Programm `desktop-driver` aus dem Paket `StockSharp.DesktopDriver.Cli`.

## Welche Builds angebunden werden können

Verbinden kann man sich nur mit einem Programm, das mit aktivierter Automatisierung gebaut wurde — mit der Eigenschaft `EnableUiAutomation=true`. Standardmäßig ist die Eigenschaft ausgeschaltet, und im normalen Build gibt es weder Treiber noch Kanal. Ein normaler Build, der mit `--ui-automation` gestartet wird, schreibt in den Standardfehlerstrom die Meldung `<Kennung> was built without automation, so --ui-automation does nothing. Build it with EnableUiAutomation=true.` und arbeitet wie gewohnt. Der MCP-Server wartet 90 Sekunden auf den Kanal eines solchen Programms, meldet dann, dass das Programm nicht hochgefahren ist, und schließt es.

Ein Build mit Automatisierung wird aus dem Quellcode des Programms erstellt, neben dem das Repository DesktopDriver.Mcp liegen muss. Er wird in einen eigenen Ordner `uiauto\bin` gelegt, um sich nicht mit dem normalen zu vermischen:

```sh
dotnet build Terminal/Avalonia/Terminal.Avalonia/Terminal.Avalonia.csproj -c Release -p:EnableUiAutomation=true
```

Die ausführbare Datei eines solchen Builds ist `Terminal/Avalonia/Terminal.Avalonia/uiauto/bin/Release/net10.0/StockSharp.Terminal.Avalonia.exe`.

| Programm | Kennung der Avalonia-Version | Kennung der WPF-Version | Klicks und Eingaben ohne Testprofil |
|---|---|---|---|
| [Terminal](../terminal.md) | `stocksharp.terminal` | `stocksharp.terminal.wpf` | werden abgelehnt |
| [Hydra](../hydra.md) | `stocksharp.hydra` | `stocksharp.hydra.wpf` | werden abgelehnt |
| [Designer](../designer.md) | `stocksharp.designer` | `stocksharp.designer.wpf` | werden abgelehnt |
| [Installer](../installer.md) | `stocksharp.installer` | `stocksharp.installer.wpf` | werden abgelehnt |
| [Shell](../shell.md) | `stocksharp.shell` | — | werden abgelehnt |
| [UDP Dumper](../udp_dumper.md) | `tool.udpdumper` | — | werden angenommen |

Die Kennung teilt das Programm selbst mit, und dieselbe Kennung wird im Katalog des Servers angegeben.

In den Avalonia-Versionen liest der Treiber Tabellen, den Arbeitsbereich mit Panels, Charts, das Orderbuch, den Eigenschafteneditor, das Strategieschema, den Code-Editor und die Markdown-Ansicht. In den WPF-Versionen — Tabellen, Dock-Panels, Charts, das Orderbuch, den Eigenschafteneditor, das Strategieschema, Eingabefelder, das Menüband, Menüs und Symbolleisten. In den Avalonia-Versionen von Terminal, Hydra und Designer wird alles, was auf einem Panel liegt, relativ zu diesem Panel adressiert: `panel:<Paneltyp>/<Kennung>`.

## Installation des Servers

### .NET-Tool

Benötigt wird das [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0):

```sh
dotnet tool install StockSharp.DesktopDriver.Mcp --tool-path tools
```

Der Befehl des Servers ist `tools/desktop-driver-mcp`. Das Protokoll schreibt der Server in die Standardausgabe, sein Log in den Standardfehlerstrom. Manuell ohne Client gestartet, wartet er einfach auf Eingaben.

### Aus dem Quellcode

```sh
dotnet build DesktopDriver.Mcp/DesktopDriver.Mcp.csproj -c Release
```

### Erweiterung für Claude Desktop

Für Claude Desktop wird der Server in eine Erweiterung gepackt — die Datei `stocksharp-desktop-driver-<Version>.mcpb`, die das Skript `scripts/Pack-McpBundle.ps1` des Repositorys DesktopDriver.Mcp erstellt. Die Erweiterung startet den Server über das auf dem Computer installierte .NET, deshalb wird das .NET 10 SDK benötigt, das über `PATH` erreichbar ist.

1. Öffnen Sie in Claude Desktop **Settings → Extensions → Advanced settings → Install Extension** und wählen Sie die Datei der Erweiterung.
2. Geben Sie den Programmkatalog (eine JSON-Datei, siehe unten) und den Ordner für Aufnahmen an.

## Programmkatalog

Der Agent kann keine beliebige Datei starten. Der Server startet nur die Programme aus dem Katalog — einer JSON-Datei, die derjenige schreibt, der den Server einrichtet. Der Agent nennt ein Programm mit seiner Kennung, nicht mit einem Pfad.

Seine Einstellungen nimmt der Server nur aus Umgebungsvariablen:

| Variable | Was sie festlegt | Wenn nicht gesetzt |
|---|---|---|
| `STOCKSHARP_UI_CATALOGUE` | den Pfad zum Programmkatalog | die Datei `applications.json` neben dem Server |
| `STOCKSHARP_UI_ARTIFACTS` | den Ordner für Aufnahmen | der Ordner `artifacts` neben dem Server |

Bei einem Server, der als .NET-Tool installiert ist, ist „neben dem Server“ der Ordner des Tool-Speichers, deshalb müssen beide Variablen ausdrücklich gesetzt werden. Ohne Katalog kann sich der Server nur mit Programmen verbinden, die jemand anderes gestartet hat.

Beispiel eines Katalogs mit einem Programm:

```json
{
  "applications": [
    {
      "appId": "stocksharp.terminal",
      "executable": "..\\Terminal\\Avalonia\\Terminal.Avalonia\\uiauto\\bin\\Release\\net10.0\\StockSharp.Terminal.Avalonia.exe",
      "arguments": "--ui-test",
      "description": "Das Handelsterminal."
    }
  ]
}
```

- `appId` — die Kennung des Programms aus der Tabelle oben. Wenn das gestartete Programm eine andere Kennung von sich meldet, wird die Verbindung abgelehnt.
- `executable` — der Pfad zum Build mit Automatisierung. Ein relativer Pfad wird von der Katalogdatei aus gerechnet.
- `arguments` — zusätzliche Startschalter; der Server fügt ihnen `--ui-automation` und `--ui-automation-endpoint` hinzu. Die Zeichenfolge wird an Leerzeichen in Schalter aufgeteilt, deshalb kann hier kein Pfad mit Leerzeichen übergeben werden.
- `description` — die Beschreibung, die der Agent sieht.

In der Katalogdatei können Kommentare `//` geschrieben werden.

## Testprofil

StockSharp-Programme verbinden sich mit Brokern, Börsen und Datenquellen, deshalb nehmen sie Klicks und Eingaben vom Agenten nur an, wenn sie mit einem Testprofil gestartet wurden. Ohne Testprofil antwortet ein Programm auf Lesen und Aufnahmen, lehnt aber Klicks, Texteingabe, Tasten, Scrollen und Ziehen mit dem Fehler `inputUnavailable` ab: `Input is delivered only to a run started on a test profile.` Die Ausnahme ist UDP Dumper: Beim Start verbindet er sich mit nichts, und Eingaben sind bei ihm immer erlaubt.

Das Testprofil wird mit Startschaltern eingeschaltet; im Katalog werden sie in `arguments` geschrieben.

| Schalter | Was er tut |
|---|---|
| `--ui-test` | startet das Programm mit einem eigenen Einstellungsordner |
| `--ui-test-dir=<Pfad>` | legt diesen Ordner fest; ohne den Schalter wird neben der ausführbaren Datei ein neuer Ordner `ui-test\<Kennung des Starts>` angelegt |
| `--ui-test-lang=<Sprachcode>` | die Sprache der Oberfläche, zum Beispiel `ru` |
| `--ui-test-view=<Kennung>` | der Bereich, in dem sich das Programm öffnet; was die Kennung bedeutet, entscheidet das Programm selbst |

Mit einem Testprofil speichert das Programm alles im Ordner des Profils, liest und ändert die Einstellungen des Computerbenutzers nicht und beginnt mit der Standardanordnung. Terminal, Hydra, Designer und Installer verlegen auch den gemeinsamen StockSharp-Ordner, in dem die Anmeldung beim Konto gespeichert ist, in das Profil, deshalb melden sie sich mit einem Testprofil nicht beim Konto an. In den Avalonia-Versionen der Programme antworten Dateidialoge aus dem Unterordner `Files` des Profils: Legen Sie dort eine Datei ab, die das Programm öffnen soll, und suchen Sie dort, was es gespeichert hat.

> [!WARNING]
> Ein Testprofil ist ein eigener Einstellungsordner und keine Sandbox. Wenn Sie Verbindungseinstellungen in das Profil legen oder der Agent sie selbst eingibt, verbindet sich das Programm wirklich, und Klicks vom Agenten werden weiterhin angenommen. Ein Klick auf die Kaufschaltfläche im Terminal platziert mit einem solchen Profil eine Order.

## Anbindung des Agenten

Claude Code — die Datei `.mcp.json` des Projekts. Ein Beispiel aus der Beschreibung des Servers für einen Build aus dem Quellcode:

```json
{
  "mcpServers": {
    "desktop-driver": {
      "command": "DesktopDriver.Mcp/bin/Release/net10.0/StockSharp.DesktopDriver.Mcp.exe",
      "env": {
        "STOCKSHARP_UI_CATALOGUE": "DesktopDriver.Mcp/applications.json"
      }
    }
  }
}
```

Für einen Server, der als .NET-Tool installiert ist, geben Sie in `command` den Pfad zu `desktop-driver-mcp` an und setzen beide Variablen:

```json
{
  "mcpServers": {
    "desktop-driver": {
      "command": "tools/desktop-driver-mcp",
      "env": {
        "STOCKSHARP_UI_CATALOGUE": "<Pfad zum Programmkatalog>",
        "STOCKSHARP_UI_ARTIFACTS": "<Ordner für Aufnahmen>"
      }
    }
  }
}
```

Codex — die Datei `~/.codex/config.toml`:

```toml
[mcp_servers.desktop-driver]
command = "DesktopDriver.Mcp/bin/Release/net10.0/StockSharp.DesktopDriver.Mcp.exe"
env = { STOCKSHARP_UI_CATALOGUE = "DesktopDriver.Mcp/applications.json" }
```

Claude Desktop — installieren Sie die Erweiterung wie oben beschrieben; der Katalog und der Ordner für Aufnahmen werden in ihren Einstellungen festgelegt.

## Was der Agent kann

Alle Werkzeuge außer `ui_list_applications`, `ui_start_application` und `ui_attach_application` bezeichnen das Programm mit dem Parameter `instance` — dem Namen, den `ui_start_application` oder `ui_attach_application` zurückgegeben hat. Eine zweite laufende Kopie desselben Programms erhält einen Namen mit dem Suffix `#2`.

Ein Element der Oberfläche wird mit der Adresse `Bereich/Kennung` bezeichnet, zum Beispiel `panel:OrdersPanel/OrdersGrid` — die Tabelle im Orders-Panel des Terminals. Adressen geben `ui_windows`, `ui_find` und `ui_tree` zurück. Eine Adresse ändert sich nicht, wenn das Panel in eine andere Gruppe verschoben, in ein eigenes Fenster abgekoppelt oder neu aufgebaut wird.

### Programme

| Werkzeug | Was es tut |
|---|---|
| `ui_list_applications` | die Programme des Katalogs: ob jedes gebaut ist und welche seiner Kopien bereits laufen |
| `ui_start_application` | startet ein Programm aus dem Katalog und wartet, bis sein Fenster gezeichnet ist (bis zu 90 Sekunden) |
| `ui_attach_application` | verbindet sich mit einem Programm, das jemand mit `--ui-automation` gestartet hat; benötigt wird die Adressdatei, die es geschrieben hat |
| `ui_release_application` | gibt das Programm frei: Ein von diesem Server gestartetes wird geschlossen, ein fremdes läuft weiter |
| `ui_session` | welches Programm geantwortet hat und was es kann: Version, Testprofil, Art der Eingabe, Grenze der Antwortgröße |

### Was auf dem Bildschirm ist

| Werkzeug | Was es tut |
|---|---|
| `ui_windows` | geöffnete Fenster und Popups mit den Adressen ihrer Wurzelelemente |
| `ui_find` | sucht Elemente nach Name, Art (`grid`, `chart`, `orderBook`, `propertyEditor`, `workspace`, `button`, `textBox` und andere), Automatisierungskennung oder Text |
| `ui_tree` | der Elementbaum, begrenzt nach Tiefe und Anzahl |
| `ui_snapshot` | der gesamte Zustand eines Elements: ob es existiert, verfügbar, sichtbar und fokussiert ist, sowie der Zustand seiner Art — Zeilenzahl und Sortierung einer Tabelle, Bereiche eines Charts, beste Preise eines Orderbuchs |

### Lesen von Elementen

| Werkzeug | Was es tut |
|---|---|
| `ui_grid_columns`, `ui_grid_rows`, `ui_grid_groups` | Spalten, Zeilen und Gruppen einer Tabelle in der Reihenfolge, in der die Tabelle selbst sie nach Sortierung, Filter und Gruppierung zeigt |
| `ui_chart_series`, `ui_chart_points` | die Serien eines Charts und die Punkte, die es tatsächlich gezeichnet hat; Zeit in UTC |
| `ui_order_book_levels` | die Stufen eines Orderbuchs nach Seite und Preis |
| `ui_property_items` | die Eigenschaften, die der Eigenschafteneditor zeigt: Wert, Text auf dem Bildschirm, Art des Editors, Fehler |
| `ui_tree_items` | die Elemente eines Baums nach ihrem Pfad |
| `ui_dock_layout` | die Anordnung des Arbeitsbereichs: Panels, Gruppen von Registerkarten, schwebende Fenster |
| `ui_document_content` | die Zeilen des Code-Editors oder der Markdown-Ansicht, nummeriert ab eins |
| `ui_diagram_nodes`, `ui_diagram_connections` | die Würfel des Strategieschemas und die Verbindungen zwischen ihnen, so wie sie gezeichnet sind |

### Aktionen

| Werkzeug | Was es tut |
|---|---|
| `ui_click` | ein Klick auf ein Element oder einen Teil davon: eine Spaltenüberschrift, eine Zelle, eine Registerkarte eines Panels und deren Schließen-Schaltfläche, ein Listen- oder Baumelement, eine Eigenschaft und auf einem Chart — auf einen Preis oder auf eine Order; dabei können `Shift`, `Control`, `Alt`, `Meta` gehalten werden |
| `ui_drag` | Ziehen innerhalb eines Elements, zum Beispiel der Linie einer Order auf einem Chart zu einem anderen Preis |
| `ui_type_text` | zeichenweise Texteingabe: an den vorhandenen Text anhängen (`Append`) oder ihn ersetzen (`Replace`) |
| `ui_press_key` | das Drücken einer Taste, zum Beispiel `Enter`, `Escape`, `Tab`, `Delete`, auch zusammen mit `Shift`, `Control`, `Alt`, `Meta` |
| `ui_scroll` | Scrollen mit dem Rad um eine angegebene Anzahl von Rasten |
| `ui_hover` | führt den Zeiger auf ein Element oder einen Teil davon und klickt nichts: Es erscheint, was nur unter dem Zeiger sichtbar ist — Schaltflächen einer Zeile, ein Tooltip, das Fadenkreuz eines Charts |
| `ui_leave` | führt den Zeiger aus dem Fenster, in dem das Element liegt: Im Fenster ist nichts mehr als überfahren hervorgehoben. Wird vor einer Aufnahme des Fensters „in Ruhe“ benötigt |
| `ui_bring_into_view` | bringt einen Teil eines Elements ins Bild, zum Beispiel eine Zeile weit unten in einer Tabelle, ohne eine Eingabe zu senden |

### Prüfung des Ergebnisses

| Werkzeug | Was es tut |
|---|---|
| `ui_wait` | wartet, bis ein Element erscheint oder verschwindet oder bis ein Feld aus der Antwort von `ui_snapshot` den gewünschten Wert annimmt |
| `ui_action_status` | was mit einer bereits gesendeten Eingabe geschehen ist, anhand der Kennung aus der Quittung |

### Aufnahmen und Diagnose

| Werkzeug | Was es tut |
|---|---|
| `ui_screenshot` | eine Aufnahme eines Elements in eine Datei; die Antwort sind der Pfad zur Datei und die Parameter der Aufnahme |
| `ui_screenshot_image` | eine Aufnahme eines Elements, die dem Agenten als Bild zurückgegeben wird |
| `ui_diagnostics` | die Einträge des Automatisierungsmoduls selbst: Ablehnungen, was nicht gelesen werden konnte, und Eingaben, die nicht zugestellt werden konnten |

> [!NOTE]
> `ui_drag`, `ui_hover`, `ui_leave`, der Klick mit gehaltenen Tasten, der Klick auf einen Preis, auf eine Order und auf das Kreuz eines Panels auf dem Chart sowie der Rahmen `highlight` auf Aufnahmen sind im Quellcode des Servers vorhanden, aber nicht im veröffentlichten Paket der Version 1.0.1.

Lesen ändert nichts: Der Server öffnet kein Panel, das nie angezeigt wurde, klappt keine Eigenschaft auf und scrollt keine Tabelle. Wenn sich deshalb keine Antwort erhalten lässt, steht das in der Antwort selbst. Die Größe einer Antwort ist begrenzt (standardmäßig 256 KB), deshalb werden große Tabellen in Teilen gelesen, mit den Parametern `limit`, `startIndex`, `columns` und `rowKeys`.

## Einschränkungen und Sicherheit

- **Nur Programme aus dem Katalog.** Der Agent wählt ein Programm aus dem Katalog und kann dem Server keinen eigenen Dateipfad übergeben.
- **Nur dieser Computer und dieser Benutzer.** Der Kanal ist nur den Prozessen des aktuellen Betriebssystembenutzers zugänglich. Beim Verbinden prüft der Server das Programm, seine Kopie und die Protokollversion.
- **Klicks nur mit Testprofil.** Ohne Testprofil antwortet das Programm auf Lesen und Aufnahmen, lehnt aber Eingaben ab (siehe „Testprofil“). Ob Eingaben erlaubt sind, zeigt `ui_session`: `safeTestProfile` ist `true`, und `inputBackend` ist nicht `none`.
- **Die Eingabe berührt Maus und Tastatur des Computers nicht.** Die Ereignisse gelangen in die interne Eingabekette des Programms, deshalb funktionieren Fokus und Handler genauso wie bei einem Klick durch einen Menschen. Ein gewöhnlicher Linksklick auf ein ganzes Element wird als Automatisierungsaktion dieses Elements ausgeführt. Vor dem Senden prüft der Treiber, dass das Element sichtbar, verfügbar und durch nichts verdeckt ist.
- **„Zugestellt“ heißt nicht „erledigt“.** Die Quittung `dispatched` bedeutet nur, dass die Eingabe das Programm erreicht hat. Ob das Programm getan hat, was erwartet wurde, prüfen `ui_snapshot` und `ui_wait`.
- **Ein Zeiger wie bei einem Menschen.** Jede Zeigeraktion versetzt ihn an eine neue Stelle, und die Elemente, die er verlassen und erreicht hat, erfahren davon genauso wie von einer echten Maus: Eine hervorgehobene Schaltfläche erlischt, das Fadenkreuz eines Charts verschwindet, wenn der Zeiger das Chart verlässt.
- **Die WPF-Versionen** lehnen den Klick mit gehaltenen Tasten, das Ziehen, `ui_hover` und `ui_leave` ab, statt sie durch einen einfachen Klick zu ersetzen.
- **Orders aus dem Chart.** Ein Klick mit `Control` auf einen Preis im Chart platziert eine Order. Mit einem Testprofil und eingerichteter Verbindung ist das eine echte Order.
- **Schließen ohne Speichern.** `ui_release_application` schließt ein von diesem Server gestartetes Programm, ohne etwas zum Speichern anzubieten. Ein Programm, mit dem sich der Server nur verbunden hat, schließt er nicht.
- **Aufnahmen** werden in den Ordner `STOCKSHARP_UI_ARTIFACTS` geschrieben, eine Datei `<Kennung der Aufnahme>.png` pro Aufnahme. Wenn die Grenze `maxPixelWidth` oder `maxPixelHeight` gesetzt ist, wird eine zu große Aufnahme abgelehnt und nicht verkleinert.

## Beispielsitzung

Terminal ist mit Automatisierung gebaut, der Katalog ist wie im Beispiel oben. Der Benutzer bittet: „Öffne im Terminal die Einstellungen des Order-Panels und zeige, welche Felder es dort gibt“.

1. `ui_list_applications` — im Katalog gibt es `stocksharp.terminal`, es ist gebaut und läuft nicht.
2. `ui_start_application`, `appId` = `stocksharp.terminal` — der Server startet Terminal mit einem Testprofil und gibt `instance` = `stocksharp.terminal` zurück.
3. `ui_session` — `safeTestProfile` = `true`, Eingaben sind erlaubt.
4. `ui_wait`, `node` = `panel:BuySellPanel/SettingsButton`, `timeoutSeconds` = `180` — warten, bis Terminal den Arbeitsbereich aufgebaut hat: Das Fenster erscheint früher als die Panels.
5. `ui_click`, `node` = `panel:BuySellPanel/SettingsButton` — die Einstellungen des Order-Panels öffnen. Die Quittung ist `dispatched`.
6. `ui_property_items`, `node` = `panel:BuySellPanel/SettingsPropertyGrid` — die Eigenschaften mit ihren aktuellen Werten, darunter `Security` (das Instrument) und `Depth`.
7. `ui_screenshot` mit demselben `node` — der Pfad zur Datei mit der Aufnahme des Editors, um sie dem Benutzer zu zeigen.
8. `ui_release_application` — Terminal wird geschlossen.

## Siehe auch

[KI-Agenten anbinden (MCP)](../mcp.md)

[Programmbefehle (Studio.Mcp)](studio_mcp.md)
