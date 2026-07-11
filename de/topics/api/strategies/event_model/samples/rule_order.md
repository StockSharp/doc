# Regel für Orders

## Überblick

`SimpleOrderRulesStrategy` ist eine Strategie, die die Verwendung von Regeln zur Verarbeitung von Ereignissen rund um Orders in StockSharp demonstriert. Sie abonniert Trades und erstellt Regeln zur Verarbeitung von Orderregistrierungsereignissen.

## Hauptkomponenten

```cs
// Hauptkomponenten
public class SimpleOrderRulesStrategy : Strategy
{
}
```

## OnStarted-Methode

Wird aufgerufen, wenn die Strategie startet:

- Erstellt ein Abonnement für Ticks
- Erstellt zwei Regelsätze zur Verarbeitung von Orderregistrierungsereignissen

```cs
// OnStarted-Methode
protected override void OnStarted2(DateTime time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 1);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("Order №1 registriert"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("Order №1 Registrierung fehlgeschlagen"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	sub.WhenTickTradeReceived(this).Do(() =>
	{
		var order = CreateOrder(Sides.Buy, default, 10000000);

		var ruleReg = order.WhenRegistered(this);
		var ruleRegFailed = order.WhenRegisterFailed(this);

		ruleReg
			.Do(() => LogInfo("Order №2 registriert"))
			.Once()
			.Apply(this)
			.Exclusive(ruleRegFailed);

		ruleRegFailed
			.Do(() => LogInfo("Order №2 Registrierung fehlgeschlagen"))
			.Once()
			.Apply(this)
			.Exclusive(ruleReg);

		RegisterOrder(order);
	}).Once().Apply(this);

	// Anfrage zum Abonnieren von Marktdaten senden.
	Subscribe(sub);

	base.OnStarted2(time);
}
```

## Logik

### Erster Regelsatz

- Wenn ein Tick empfangen wird, erstellt die Strategie eine Kauforder über 1 Einheit.
- Die Order wird mit der Methode `CreateOrder` erstellt; dabei werden Richtung, Preis (default = Market) und Volumen angegeben.
- Es werden Regeln für erfolgreiche Registrierung und Registrierungsfehler eingerichtet.
- Die Regeln schließen sich gegenseitig aus und lösen nur einmal aus.

### Zweiter Regelsatz

- Beim nächsten empfangenen Tick erstellt die Strategie eine Kauforder über 10.000.000 Einheiten.
- Ebenso werden Regeln für erfolgreiche Registrierung und Registrierungsfehler eingerichtet.
- Auch diese Regeln schließen sich gegenseitig aus und lösen nur einmal aus.

## Funktionen

- Demonstriert das Erstellen von Regeln zur Verarbeitung von Orderregistrierungsereignissen.
- Verwendet den Mechanismus gegenseitig ausschließender Regeln (`Exclusive`).
- Zeigt ein Beispiel für das Protokollieren von Informationen zu Orderereignissen mit der Methode `LogInfo`.
- Veranschaulicht die Verwendung von `Once()`, um das Auslösen von Regeln zu begrenzen.
- Erstellt Orders mit unterschiedlichen Volumina, um verschiedene Szenarien zu demonstrieren (erfolgreiche Registrierung und Registrierungsfehler).
