# 出金

暗号資産取引所から資金を出金するには、出金用の注文を作成し、通常の注文としてコネクタに登録する必要があります。たとえば、[Binance](binance.md) 取引所の場合、出金リクエストのコードは次のようになります。

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
				// 必要な詳細を入力します
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

