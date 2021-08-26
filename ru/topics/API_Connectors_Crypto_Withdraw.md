# Вывод средств

Для вывода средств с биржи криптовалют необходимо сформировать заявку на вывод и зарегистрировать ее в коннекторе как обычную заявку. Например, для биржи [Binance](Binance.md) код заявки на вывод средств будет выглядеть следующим образом:

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
				// заполняются необходимые реквизиты
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
