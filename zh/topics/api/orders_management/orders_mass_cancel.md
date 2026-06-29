# 批量取消订单

要取消多个订单，请使用 [Connector.CancelOrders](xref:StockSharp.Algo.Connector.CancelOrders(System.Nullable{System.Boolean},StockSharp.BusinessEntities.Portfolio,System.Nullable{StockSharp.Messages.Sides},StockSharp.BusinessEntities.ExchangeBoard,StockSharp.BusinessEntities.Security,System.Nullable{StockSharp.Messages.SecurityTypes},System.Nullable{System.Int64}))**(**[System.Nullable\<System.Boolean\>](xref:System.Nullable`1) isStopOrder, [StockSharp.BusinessEntities.Portfolio](xref:StockSharp.BusinessEntities.Portfolio) portfolio, [System.Nullable\<StockSharp.Messages.Sides\>](xref:System.Nullable`1) direction, [StockSharp.BusinessEntities.ExchangeBoard](xref:StockSharp.BusinessEntities.ExchangeBoard) board, [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security, [System.Nullable\<StockSharp.Messages.SecurityTypes\>](xref:System.Nullable`1) securityType, [System.Nullable\<System.Int64\>](xref:System.Nullable`1) transactionId **)** 方法，该方法根据传入的参数掩码取消活动订单。

## 订单批量取消示例

要取消指定投资组合和工具的所有普通订单（[OrderTypes.Limit](xref:StockSharp.Messages.OrderTypes.Limit)）:

```cs
_connector.CancelOrders(false, MainWindow.Instance.Portfolio, null, null, security);
```

要取消指定工具的所有订单：

```cs
_connector.CancelOrders(null, null, null, null, security);
```

取消所有长期止损订单：

```cs
_connector.CancelOrders(true, null, Sides.Buy, null, null);
```
