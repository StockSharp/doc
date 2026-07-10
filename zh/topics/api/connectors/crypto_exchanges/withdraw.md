# 提款

要从加密货币交易所提取资金，必须创建一个提款订单并将其在连接器中注册为常规订单。例如，对于 [Binance](binance.md) 交易所，提款请求代码将如下所示：

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
				// 填写必要详情
	Comment = "我的利润",
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
