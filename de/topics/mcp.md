# KI-Agenten anbinden (MCP)

**MCP** (Model Context Protocol) ist ein offenes Protokoll, über das ein KI-Agent (Claude Code, Claude Desktop oder ein beliebiger anderer MCP-Client) die Werkzeuge eines externen Programms aufruft — eines MCP-Servers. Der Client startet den Server selbst als Kindprozess und tauscht mit ihm Nachrichten über die Standardein- und -ausgabe aus. Der Server teilt mit, welche Werkzeuge er hat, was jedes davon tut und welche Parameter es annimmt, und der Agent entscheidet, welches Werkzeug er aufruft, und erhält die Antwort.

Für die StockSharp-Programme gibt es zwei MCP-Server. Beide arbeiten nur auf demselben Computer wie die Programme und kommunizieren nur mit den Programmen desselben Benutzerkontos.

| | [DesktopDriver](mcp/desktop_driver.md) | [Studio.Mcp](mcp/studio_mcp.md) |
|---|---|---|
| Was er tut | liest die Oberfläche des Programms, drückt Schaltflächen, gibt Text ein, scrollt, erstellt Aufnahmen | übergibt dem Programm Befehle: eine Order platzieren oder stornieren, eine Hydra-Quelle starten, einen Strategietest starten |
| Mit welchen Programmen | Terminal, Hydra, Designer und Installer (Avalonia- und WPF-Versionen), Shell und UDP Dumper — nur in einem Build mit aktivierter Automatisierung | die Avalonia-Versionen von Terminal, Hydra, Designer und Shell — im normalen Build |
| Was der Agent sieht | Fenster, Panels, Tabellen, Charts, Orderbücher, Eigenschafteneditoren, Strategieschemata | nur das, was die Befehle zurückgeben: Listen von Quellen, Strategien und Orders |
| Einschränkung der Aktionen | Klicks und Eingaben nimmt nur ein Programm an, das mit einem Testprofil gestartet wurde (außer UDP Dumper, der sie immer annimmt) | keine Einschränkung durch ein Profil: Die Order geht dorthin, wohin Terminal verbunden ist |
| Woher der Server kommt | das NuGet-Paket `StockSharp.DesktopDriver.Mcp`, eine Erweiterung für Claude Desktop | wird aus dem Quellcode gebaut |

## Welchen Server wählen

- Sie müssen prüfen, was ein Fenster anzeigt, ein Szenario mit Schaltflächen so durchlaufen, wie es ein Mensch tun würde, oder eine Aufnahme eines Panels erhalten — [DesktopDriver](mcp/desktop_driver.md).
- Das Programm soll auf Befehl eine Aktion ausführen, ohne Klicks: eine Order platzieren, das Herunterladen durch eine Hydra-Quelle starten, einen Strategietest starten — [Studio.Mcp](mcp/studio_mcp.md).
- Das Programm ist auf normale Weise gebaut, ohne Automatisierung — dann passt nur Studio.Mcp, und nur für die Avalonia-Versionen der Programme.
- Die WPF-Version eines Programms — nur DesktopDriver, und nur in einem Build mit Automatisierung.

Beide Server können gleichzeitig an den Agenten angebunden werden. Zum Beispiel startet Studio.Mcp einen Strategietest im Designer, gibt aber keine Ergebnisse zurück — der Agent kann sie über DesktopDriver aus den Panels des Designers lesen, wenn der Designer mit Automatisierung gebaut ist.

MCP ist nicht dasselbe wie die [Fernsteuerung](shell/remote_control.md) über die Website und über den [Telegram-Bot](telegram_services/control_panel.md). Die Fernsteuerung funktioniert über das StockSharp-Konto und benötigt weder einen Agenten noch einen MCP-Server.

## Siehe auch

[Steuerung der Oberfläche (DesktopDriver)](mcp/desktop_driver.md)

[Programmbefehle (Studio.Mcp)](mcp/studio_mcp.md)

[KI-unterstützte Entwicklung](api/ai_development.md)
