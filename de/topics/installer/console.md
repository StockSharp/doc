# Konsolen-Installer

Die Anwendung `Installer.Console` ist eine plattformübergreifende Version des StockSharp Installer. Sie ermöglicht das Herunterladen, Aktualisieren und Entfernen von Produkten ohne grafische Oberfläche. Das Tool läuft auf jedem Betriebssystem, auf dem die [.NET 6](https://dotnet.microsoft.com/)-Runtime verfügbar ist.

## Ausführen

1. Installieren Sie das .NET 6 SDK oder die Runtime für Ihre Plattform.
2. Laden Sie `StockSharp.Installer.Console.zip` von der [Seite zum Herunterladen](https://stocksharp.com/de/products/download/) herunter.
3. Entpacken Sie das Archiv und starten Sie das Dienstprogramm über die Befehlszeile:

   ```bash
   dotnet StockSharp.Installer.Console.dll <Command> [product] [dir] [options]
   ```

`<Command>` ist einer der folgenden Werte:

- `Install` - ein Produkt installieren.
- `Update` - ein installiertes Produkt aktualisieren.
- `Repair` - eine vorhandene Installation reparieren.
- `Remove` - ein Produkt deinstallieren.
- `License` - die Lizenz für ein Produkt anzeigen.
- `Licenses` - verfügbare Lizenzen auflisten.
- `HddId` - die Festplattenkennung ausgeben.
- `Products` - verfügbare Produkte auflisten.
- `Updates` - verfügbare Updates anzeigen.
- `Installed` - installierte Programme auflisten.
- `Sign` - eine DLL-Datei signieren.

Der optionale Parameter `[product]` ist die Produkt-ID aus dem [Shop](https://stocksharp.com/de/store/). Sie finden diese ID auf der Produktseite, zum Beispiel auf der [Hydra-Server-Seite](https://stocksharp.com/de/store/hydra-server/), oder indem Sie `StockSharp.Installer.Console.exe Products -s hydra` ausführen. `[dir]` gibt das Installationsverzeichnis an.

## Optionen

Das Dienstprogramm akzeptiert die folgenden Optionen:

- `-s`, `--search` - Produkte nach Namen filtern.
- `-r`, `--run` - eine Anwendung (zum Beispiel `StockSharp.Hydra.Server.exe`) nach der Installation automatisch starten.
- `-c`, `--cache` - den NuGet-Cache verwenden.
- `-f`, `--force` - die Update-Prüfung unabhängig vom konfigurierten Intervall erzwingen.
- `-p`, `--pre` - die Installation von Vorabversionen erlauben.
- `-e`, `--noerror` - alle Fehler unterdrücken.
- `-b`, `--backup` - vorherige Einstellungen vor Reparatur oder Update sichern.
- `-l`, `--clear` - das Zielverzeichnis vor der Installation leeren.
- `-t`, `--fw` - das Ziel-.NET-Framework angeben.
- `-d`, `--data` - den Anwendungsdatenordner entfernen.
- `-i`, `--in` - zu signierende DLL.
- `-o`, `--out` - Ergebnis-DLL nach der Signierung.

Beispielbefehl:

```bash
dotnet StockSharp.Installer.Console.dll Install 1269 /home/user/stocksharp -p -r StockSharp.Hydra.Server.exe
```

Dies installiert das Produkt **1269** (hier nur als Beispiel verwendet) in das angegebene Verzeichnis, erlaubt Vorabversionen und startet nach Abschluss `StockSharp.Hydra.Server.exe`.

## Signieren

Der Befehl `Sign` signiert eine Roboter-DLL digital. Verwenden Sie ihn, wenn Sie einen eigenen API-basierten Roboter verteilen, damit alle Empfänger ihn ausführen können, auch im [kostenlosen Tarif](https://stocksharp.com/de/pricing/).

Kompilieren Sie den Roboter vorher und fügen Sie das Attribut `[assembly: ProductId(9)]` aus dem Namespace `StockSharp.Configuration` hinzu. Signieren Sie die DLL (zum Beispiel `MyRobot.dll`) und nicht die `.exe`-Datei.

Beispiel:

```bash
StockSharp.Installer.Console.exe Sign -i "MyRobot.dll"
```

Ein erneutes Erstellen des Projekts entfernt die Signatur. Signieren Sie die Assembly daher am Ende der Entwicklung, wenn keine weitere Kompilierung mehr erwartet wird.
