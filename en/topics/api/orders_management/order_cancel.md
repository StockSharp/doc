# Order cancel

Order cancel is needed in case of the market situation has changed in disfavour of the issued order. To cancel orders the [ITransactionProvider.CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** method used in the [S\#](../../api.md). 

```cs
// registeredOrder - successfully registered order.
_connector.CancelOrder(registeredOrder);
```

## Recommended content

[Orders mass cancel](orders_mass_cancel.md)

[Orders replacement](orders_replacement.md)
