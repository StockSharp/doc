# 订单取消

如果市场情况对已发出的订单不利，则需要取消订单。要取消订单，请在 [S#](../../api.md) 中使用 [ITransactionProvider.CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order) **(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) 订单 **)** 方法。

```cs
// registeredOrder - successfully registered order.
_connector.CancelOrder(registeredOrder);
```

## 推荐内容

[批量取消订单](orders_mass_cancel.md)

[订单替换](orders_replacement.md)
