# 订单取消

如果市场情况对已发出的订单不利，则需要取消订单。要取消订单，请使用 [ITransactionProvider.CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** 方法在 [S#](../../api.md) 中执行。

```cs
// registeredOrder — 成功注册的订单。
_connector.CancelOrder(registeredOrder);
```

## 推荐内容

[批量取消订单](orders_mass_cancel.md)

[订单替换](orders_replacement.md)
