# 注文のキャンセル

発行済み注文にとって市場状況が不利に変化した場合、注文のキャンセルが必要になります。注文をキャンセルするには、[ITransactionProvider.CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** メソッドを [S#](../../api.md) で使用します。

```cs
// registeredOrder - 正常に登録された注文。
_connector.CancelOrder(registeredOrder);
```

## 推奨コンテンツ

[注文の一括キャンセル](orders_mass_cancel.md)

[注文の差し替え](orders_replacement.md)
