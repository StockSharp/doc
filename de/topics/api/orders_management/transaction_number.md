# Transaktionsnummer

Bei der Arbeit mit Orders ist der wichtigste Bezeichner [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId), nicht [Order.Id](xref:StockSharp.BusinessEntities.Order.Id). Das liegt daran, dass [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) von der Börse generiert wird. Deshalb kann [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) unmittelbar nach dem Ausführen der Methode [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** für einige Zeit noch nicht initialisiert sein. Daher erzeugt das Handelsprogramm unmittelbar nach dem Senden der Transaktion [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId).

[Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId) wird automatisch von der Klasse [IdGenerator](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs) generiert. Dies ist eine abstrakte Klasse mit zwei Standardimplementierungen:

- [IncrementalIdGenerator](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs#L28) - standardmäßig installiert. Er erhöht die Nummer um 1. Der Anfangswert wird über die Eigenschaft [IncrementalIdGenerator.Current](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs#L42) festgelegt; standardmäßig entspricht der Wert der Anzahl der Millisekunden seit Tagesbeginn.
- [MillisecondIdGenerator](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs#L93). Er generiert die Transaktionsnummer, die der Anzahl der Millisekunden seit dem Erstellen des Generators entspricht.

