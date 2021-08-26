# Withdraw

To withdraw funds from the cryptocurrency exchange, it is necessary to create an order for withdrawal and register it in the connector as a regular order. For example, for the [Binance](Binance.md) exchange, the withdrawal request code will look like this:

```cs
Connector Connector \= new Connector();		
...   
public void Withdraw()
{         				
	var order \= new Order
	{
		Type \= OrderTypes.Conditional,
		Withdraw \= 1,
		Condition \= new BinanceOrderCondition
		{
			IsWithdraw \= true,
			WithdrawInfo \= new WithdrawInfo
			{
				\/\/ fill in the necessary details
				Comment \= "My profit",
				\/\/PaymentId \= "45467dyjyttR8WBiTJXptyuTx4wbSerGZ5t45", \/\/Riple
				CryptoAddress \= "16zK3M53JBGnjs9ajTBGBfkRqoHtm4E573",
			}
		},
		Security \= new Security() { Code \= "BTC" },
	};
	\_connector.RegisterOrder(order);
}
...
							
```
