# KI-unterstützte Entwicklung

Moderne KI-Tools können die Entwicklung von Handelsstrategien und Connectors mit StockSharp erheblich beschleunigen. Anstatt Code von Grund auf zu schreiben, beschreiben Sie die Aufgabe in natürlicher Sprache, und die KI generiert funktionierenden Code unter Verwendung der aktuellen API.

## Warum KI verwenden

- **Schneller Einstieg** — erstellen Sie einen funktionierenden Strategie-Prototyp in Minuten statt Stunden
- **Die API erlernen** — die KI zeigt Ihnen die korrekten StockSharp-Methoden und -Muster
- **Debugging** — beschreiben Sie einen Fehler in einfacher Sprache und erhalten Sie eine Lösung
- **Refactoring** — bestehenden Code verbessern und dabei die Logik beibehalten

## Geeignete Tools

| Tool | Typ | Merkmale |
|------|------|----------|
| [Claude Code](https://claude.com/claude-code) | CLI-Agent | Arbeitet mit dem gesamten Projekt, führt Befehle aus, führt Tests aus |
| [OpenAI Codex](https://openai.com/codex) | Cloud-Agent | Autonome Aufgabenausführung in einer Sandbox-Umgebung, GitHub-Integration |
| [Cursor](https://cursor.com) | IDE | Integrierter KI-Assistent, Auto-Vervollständigung, Bewusstsein für den Projektkontext |
| [GitHub Copilot](https://github.com/features/copilot) | IDE-Plugin | Code-Vervollständigung in Visual Studio, VS Code, Rider |
| [JetBrains AI](https://www.jetbrains.com/ai/) | In Rider integriert | Native Integration mit Rider und dem Debugger |

## Allgemeine Prinzipien

### 1. Kontext bereitstellen

Je mehr Kontext die KI hat, desto genauer ist das Ergebnis. Geben Sie an:
- Welche StockSharp-Pakete verwendet werden
- Welcher Connector (Börse/Broker)
- Strategietyp (trendfolgend, Arbitrage, Scalping)
- Einschränkungen (nur Long, maximale Positionsgröße usw.)

### 2. CLAUDE.md / .cursorrules verwenden

Erstellen Sie eine Projektregel-Datei im Repository-Root:

```markdown
# Projektregeln

- StockSharp 5.x API verwenden
- Zielframework: .NET 10
- Strategien erben von Strategy
- Connectors implementieren MessageAdapter
- Alle Abonnements über Connector.Subscribe()
- Protokollierung über this.AddInfoLog() / this.AddErrorLog()
```

### 3. Iterativer Ansatz

1. Beschreiben Sie die Aufgabe in groben Zügen
2. Erhalten Sie die erste Version des Codes
3. Verfeinern Sie die Anforderungen und bitten Sie um Verbesserungen
4. Überprüfen Sie den Code: Kompilierung, Logik, Fehlerbehandlung
5. Testen Sie mit historischen Daten

### 4. Das Ergebnis überprüfen

KI kann veraltete Methoden verwenden oder nicht existierende APIs erfinden. Überprüfen Sie immer:
- Ob der Code kompiliert
- Ob die verwendeten Klassen und Methoden existieren
- Ob die Signaturen von Subscriptions und Events korrekt sind
- Ob die Fehlerbehandlung korrekt ist

## Abschnitte

- [Eine Strategie mit KI schreiben](ai_development/strategy_with_ai.md) — Schritt-für-Schritt-Anleitung zur Erstellung einer Handelsstrategie
- [Einen Connector mit KI schreiben](ai_development/connector_with_ai.md) — Schritt-für-Schritt-Anleitung zur Erstellung eines Börsen-Connectors
