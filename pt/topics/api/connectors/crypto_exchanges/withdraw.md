# Levantamento

Para levantar fundos da bolsa de criptomoedas, é necessário criar uma ordem de levantamento e registá-la no conector como uma ordem normal. Por exemplo, para a bolsa [Binance](binance.md), o código do pedido de levantamento terá o seguinte aspeto:

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
				// preencha os detalhes necessários
				Comment = "My profit",
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
