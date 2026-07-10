# Retiro

Para retirar fondos del exchange de criptomonedas, debe crear una orden de retiro y registrarla en el conector como una orden normal. Por ejemplo, para el exchange [Binance](binance.md), el código de solicitud de retiro tendrá este aspecto:

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
				// rellenar los detalles necesarios
	Comment = "Mi beneficio",
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
