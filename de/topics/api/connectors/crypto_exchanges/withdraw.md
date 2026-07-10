# Auszahlung

Um Mittel von der Kryptobörse abzuheben, müssen Sie einen Auszahlungsauftrag erstellen und ihn im Connector wie einen normalen Auftrag registrieren. Für die Börse [Binance](binance.md) sieht der Code für eine Auszahlungsanfrage beispielsweise so aus:

```cs
Connector Connector = new Connector();
...
public void Withdraw()
{
	var order = new Order
	{
		Type = OrderTypes.Conditional,
		Withdraw = 1,
		Condition = new BinanceOrderCondition
		{
			IsWithdraw = true,
			WithdrawInfo = new WithdrawInfo
			{
				// notwendige Details ausfüllen
	Comment = "Mein Gewinn",
				//PaymentId = "45467dyjyttR8WBiTJXptyuTx4wbSerGZ5t45", //Riple
				CryptoAddress = "16zK3M53JBGnjs9ajTBGBfkRqoHtm4E573",
			}
		},
		Security = new Security() { Code = "BTC" },
	};
	_connector.RegisterOrder(order);
}
...

```

