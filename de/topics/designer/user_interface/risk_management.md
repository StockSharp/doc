# Risikomanagement

In den Panels [Rücktest-Einstellungen](components/backtesting_settings.md) und [Live-Einstellungen](components/live_settings.md) können Sie die Einstellungen für die Risikokontrolle festlegen.

Im Fenster Risiken müssen Sie eine **Risikoregel** auswählen, die Auslösebedingung für die **Risikoregel** konfigurieren und die Aktion (Positionen schließen, Handel stoppen, Aufträge stornieren) festlegen, die ausgeführt wird, wenn die Bedingung der **Risikoregel** eintritt.

Es ist möglich, mehrere Risikoregeln desselben Typs mit unterschiedlichen Aktionen zu verwenden. Im folgenden Screenshot werden beispielsweise bei einem Ordervolumen von 20 die Aktionen zum Stornieren von Orders und zum Stoppen des Handels ausgeführt.

![Designer Risikoregel](../../../images/designer_risk_rule.png)

### Liste der Risikoregeln

Liste der Risikoregeln

- **P/L** - eine Risikoregel, die die Höhe von Gewinn/Verlust überwacht.
- **Position** - eine Risikoregel, die die Positionsgröße überwacht.
- **Position (Zeit)** - eine Risikoregel, die die Lebensdauer einer Position überwacht.
- **Provision** - eine Risikoregel, die die Höhe der Kommission überwacht.
- **Kursabweichung** - eine Risikoregel, die die Höhe des Slippage überwacht.
- **Auftragspreis** - eine Risikoregel, die den Preis einer Order überwacht.
- **Auftragsvolumen** - eine Risikoregel, die das Volumen einer Order überwacht.
- **Auftrag (Frequenz)** - eine Risikoregel, die die Häufigkeit der Orderplatzierung überwacht.
- **Fehler bei Registrierung/Stornierung der Order** - eine Risikoregel, die die Anzahl der Fehler bei Registrierung/Stornierung von Orders überwacht.
- **Ausführungspreis** - eine Risikoregel, die den Preis eines Trades überwacht.
- **Ausführung (Volumen)** - eine Risikoregel, die das Volumen eines Trades überwacht.
- **Ausführung (Frequenz)** - eine Risikoregel, die die Häufigkeit der Ausführung von Trades überwacht.
- **Fehler** - eine Risikoregel, die die Anzahl beliebiger Fehler überwacht.
