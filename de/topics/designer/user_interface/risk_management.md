# Risk Management

In den Panels [Testing Properties](components/backtesting_settings.md) und [Live Trading Properties](components/live_settings.md) konnen Sie die Einstellungen fur die Risikokontrolle festlegen.

Im Fenster Risks mussen Sie eine **Risk Rule** auswahlen, die Auslosebedingung fur die **Risk Rule** konfigurieren und die Aktion (Positionen schliessen, Handel stoppen, Orders stornieren) festlegen, die ausgefuhrt wird, wenn die Bedingung der **Risk Rule** eintritt.

Es ist moglich, mehrere Risikoregeln desselben Typs mit unterschiedlichen Aktionen zu verwenden. Im folgenden Screenshot werden beispielsweise bei einem Ordervolumen von 20 die Aktionen zum Stornieren von Orders und zum Stoppen des Handels ausgefuhrt.

![Designer Risk Rule](../../../images/designer_risk_rule.png)

### Liste der Risk Rules

Liste der Risk Rules

- **P/L** - eine Risikoregel, die die Hohe von Gewinn/Verlust uberwacht.
- **Position** - eine Risikoregel, die die Positionsgrosse uberwacht.
- **Position (Time)** - eine Risikoregel, die die Lebensdauer einer Position uberwacht.
- **Commission** - eine Risikoregel, die die Hohe der Kommission uberwacht.
- **Slippage** - eine Risikoregel, die die Hohe des Slippage uberwacht.
- **Order Price** - eine Risikoregel, die den Preis einer Order uberwacht.
- **Order Volume** - eine Risikoregel, die das Volumen einer Order uberwacht.
- **Order (Frequency)** - eine Risikoregel, die die Haufigkeit der Orderplatzierung uberwacht.
- **Error in Registration/Cancellation of Order** - eine Risikoregel, die die Anzahl der Fehler bei Registrierung/Stornierung von Orders uberwacht.
- **Trade Price** - eine Risikoregel, die den Preis eines Trades uberwacht.
- **Trade (Volume)** - eine Risikoregel, die das Volumen eines Trades uberwacht.
- **Trade (Frequency)** - eine Risikoregel, die die Haufigkeit der Ausfuhrung von Trades uberwacht.
- **Error** - eine Risikoregel, die die Anzahl beliebiger Fehler uberwacht.
