# Handelsoperationen

Implementieren Sie beim Erstellen eines eigenen Adapters für eine Börse die Methoden, die Handelsoperationen ausführen: Registrieren, Ersetzen und Stornieren von Aufträgen. Diese Methoden werden aufgerufen, wenn der Adapter die entsprechenden Nachrichten vom StockSharp-Kern erhält.

## Auftragsregistrierung

Zur Registrierung eines neuen Auftrags wird die Methode **RegisterOrderAsync** implementiert. Diese Methode wird beim Empfang der Nachricht [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) aufgerufen.

Die wichtigsten Schritte bei der Registrierung eines Auftrags:

1. Überprüfung des Auftragstyps und zusätzlicher Bedingungen.
2. Umwandlung der Auftragsparameter in ein Format, das von der Börse verstanden wird.
3. Senden einer Anfrage zur Registrierung des Auftrags über die Börsen-API.
4. Verarbeitung der Antwort der Börse und Senden der entsprechenden [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)-Nachricht.

```cs
public override async ValueTask RegisterOrderAsync(OrderRegisterMessage regMsg, CancellationToken cancellationToken)
{
	var condition = (CoinbaseOrderCondition)regMsg.Condition;

	switch (regMsg.OrderType)
	{
		case null:
		case OrderTypes.Limit:
		case OrderTypes.Market:
			break;
		case OrderTypes.Conditional:
		{
			// Bedingte Orders verarbeiten, z. B. Auszahlung von Mitteln
			if (!condition.IsWithdraw)
				break;

			var withdrawId = await _restClient.Withdraw(regMsg.SecurityId.SecurityCode, regMsg.Volume, condition.WithdrawInfo, cancellationToken);

			await SendOutMessageAsync(new ExecutionMessage
			{
				DataTypeEx = DataType.Transactions,
				OrderStringId = withdrawId,
				ServerTime = CurrentTime.ConvertToUtc(),
				OriginalTransactionId = regMsg.TransactionId,
				OrderState = OrderStates.Done,
				HasOrderInfo = true,
			}, cancellationToken);

			await PortfolioLookupAsync(null, cancellationToken);
			return;
		}
		default:
			throw new NotSupportedException(LocalizedStrings.OrderUnsupportedType.Put(regMsg.OrderType, regMsg.TransactionId));
	}

	// Ordertyp bestimmen (Markt- oder Limitorder)
	var isMarket = regMsg.OrderType == OrderTypes.Market;
	var price = isMarket ? (decimal?)null : regMsg.Price;

	// Order an die Börse senden
	var result = await _restClient.RegisterOrder(
		regMsg.TransactionId.To<string>(), regMsg.SecurityId.ToSymbol(),
		regMsg.OrderType.ToNative(), regMsg.Side.ToNative(), price,
		condition?.StopPrice, regMsg.Volume, regMsg.TimeInForce,
		regMsg.TillDate.EnsureToday(), regMsg.Leverage, cancellationToken);

	var orderState = result.Status.ToOrderState();

	// Ergebnis der Orderregistrierung verarbeiten
	if (orderState == OrderStates.Failed)
	{
		await SendOutMessageAsync(new ExecutionMessage
		{
			DataTypeEx = DataType.Transactions,
			ServerTime = result.CreationTime,
			OriginalTransactionId = regMsg.TransactionId,
			OrderState = OrderStates.Failed,
			Error = new InvalidOperationException(),
			HasOrderInfo = true,
		}, cancellationToken);
	}
}
```

## Auftragsersetzung

Zum Ersetzen eines bestehenden Auftrags wird die Methode **ReplaceOrderAsync** implementiert. Diese Methode wird beim Empfang der Nachricht [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage) aufgerufen.

Die wichtigsten Schritte beim Ersetzen eines Auftrags:

1. Überprüfung der Möglichkeit, den Auftrag an der Börse zu ersetzen.
2. Senden einer Anfrage zum Ersetzen des Auftrags über die Börsen-API.
3. Verarbeitung der Antwort der Börse und Senden der entsprechenden [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)-Nachricht.

```cs
public override async ValueTask ReplaceOrderAsync(OrderReplaceMessage replaceMsg, CancellationToken cancellationToken)
{
	// Anfrage zum Ersetzen der Order senden
	await _restClient.EditOrder(
		replaceMsg.OldOrderId.To<string>(),
		replaceMsg.Price,
		replaceMsg.Volume,
		cancellationToken);

	// Hinweis: Die Verarbeitung des Ergebnisses der Orderersetzung erfolgt normalerweise
	// in einer separaten Methode, die beim Empfang eines Updates von der Börse aufgerufen wird
}
```

### Besonderheiten der Auftragsersetzung

Berücksichtigen Sie bei der Implementierung der Auftragsersetzung das Börsenprotokoll. StockSharp stellt dafür die Eigenschaft [MessageAdapter.IsReplaceCommandEditCurrent](xref:StockSharp.Messages.MessageAdapter.IsReplaceCommandEditCurrent) bereit.

Wenn das Börsenprotokoll einen Auftrag ändert, während die alte Kennung beibehalten wird, überschreiben Sie diese Eigenschaft und geben Sie `true` zurück. Dies teilt StockSharp mit, dass beim Ersetzen eines Auftrags nicht auf eine neue Kennung von der Börse gewartet werden muss.

```cs
public override bool IsReplaceCommandEditCurrent => true;
```

Wenn beim Ändern eines Auftrags der alte storniert und ein neuer mit einer neuen Börsenkennung registriert wird, muss diese Eigenschaft nicht überschrieben werden. Standardmäßig gibt sie `false` zurück, was dem Verhalten der meisten Börsen entspricht.

## Auftragsstornierung

Zur Stornierung eines bestehenden Auftrags wird die Methode **CancelOrderAsync** implementiert. Diese Methode wird beim Empfang der Nachricht [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage) aufgerufen.

Die wichtigsten Schritte bei der Stornierung eines Auftrags:

1. Überprüfung des Vorhandenseins der Auftragskennung.
2. Senden einer Anfrage zur Stornierung des Auftrags über die Börsen-API.
3. Verarbeitung der Antwort der Börse und Senden der entsprechenden [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)-Nachricht.

```cs
public override async ValueTask CancelOrderAsync(OrderCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	// Vorhandensein der Orderkennung prüfen
	if (cancelMsg.OrderStringId.IsEmpty())
		throw new InvalidOperationException(LocalizedStrings.OrderNoExchangeId.Put(cancelMsg.OriginalTransactionId));

	// Anfrage zum Stornieren der Order senden
	await _restClient.CancelOrder(cancelMsg.OrderStringId, cancellationToken);

	// Hinweis: Die Verarbeitung des Ergebnisses der Orderstornierung erfolgt normalerweise
	// in einer separaten Methode, die beim Empfang eines Updates von der Börse aufgerufen wird
}
```

## Massenstornierung von Aufträgen

Einige Börsen unterstützen die Funktion der Massenstornierung von Aufträgen, mit der mehrere oder alle aktiven Aufträge mit einer einzigen Anfrage storniert werden können. Dies kann nützlich sein, um Positionen schnell zu schließen oder das Orderbuch unter bestimmten Marktbedingungen zu bereinigen.

Zur Implementierung der Massenstornierung von Aufträgen im Adapter wird in der Regel die Methode **CancelOrderGroupAsync** verwendet. Diese Methode wird beim Empfang der Nachricht [OrderGroupCancelMessage](xref:StockSharp.Messages.OrderGroupCancelMessage) aufgerufen.

Nicht alle Börsen unterstützen diese Funktion. Coinbase bietet beispielsweise keine API für die Massenstornierung von Aufträgen. Implementieren Sie in solchen Fällen bei Bedarf die sequenzielle Stornierung einzelner Aufträge.

Nachfolgend finden Sie ein Beispiel für die Implementierung der Methode zur Massenstornierung von Aufträgen, entnommen aus dem [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp)-Connector, der diese Funktion unterstützt:

```cs
public override async ValueTask CancelOrderGroupAsync(OrderGroupCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	await _httpClient.CancelAllOrders(cancellationToken);
}
```

Entfernen Sie die Unterstützung für diesen Befehlstyp nicht im Adapter-Konstruktor:

```cs
//this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);
```

## Verfolgung des Auftragszustands

Im Falle von Coinbase, wie auch bei einigen anderen modernen Börsen, werden Aktualisierungen des Auftragszustands über eine WebSocket-Verbindung übertragen. Das bedeutet, dass nach der Durchführung von Handelsoperationen (Registrierung, Ersetzung oder Stornierung eines Auftrags) nicht sofort der neue Auftragszustand über die REST-API abgefragt werden muss. Stattdessen erhält der Adapter Aktualisierungen automatisch über die bestehende WebSocket-Verbindung.

Die Verarbeitung dieser Aktualisierungen erfolgt in einer Methode ähnlich `SessionOnOrderReceived`, die im Abschnitt über [die Abfrage des aktuellen Zustands des Portfolios und der Aufträge](portfolio_and_orders_state.md) besprochen wurde. Diese Methode wird jedes Mal aufgerufen, wenn die Börse eine Aktualisierung des Auftragszustands sendet, unabhängig davon, ob diese Aktualisierung durch Benutzeraktionen oder Änderungen an der Börse selbst ausgelöst wurde.

Dieser Ansatz verfolgt Auftragszustände effizienter, reduziert die Last auf der Börsen-API und liefert Echtzeit-Aktualisierungen. Studieren Sie bei der Implementierung eines eigenen Adapters die Dokumentation der Börsen-API sorgfältig, damit diese WebSocket-Aktualisierungen korrekt konfiguriert und verarbeitet werden.

## Fehlerbehandlung

Behandeln Sie bei der Durchführung von Handelsoperationen mögliche Fehler und Ausnahmen korrekt. Wenn ein Fehler auftritt, senden Sie eine [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)-Nachricht mit gesetzter [Error](xref:StockSharp.Messages.ExecutionMessage.Error)-Eigenschaft.

## Besonderheiten der Implementierung

Berücksichtigen Sie bei der Implementierung der Handelsoperationsmethoden die Besonderheiten einer bestimmten Börse:

- Unterstützte Auftragstypen (Market, Limit, Stop-Aufträge usw.).
- Format der Auftragskennungen.
- Besonderheiten der Börsen-API für die Arbeit mit Aufträgen.
- Mögliche Einschränkungen bei der Häufigkeit des Sendens von Anfragen.
