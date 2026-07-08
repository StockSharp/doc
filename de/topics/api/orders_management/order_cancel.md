# Order stornieren

Eine Orderstornierung ist erforderlich, wenn sich die Marktsituation gegen die aufgegebene Order entwickelt. Zum Stornieren von Orders verwenden Sie in [S#](../../api.md) die Methode [ITransactionProvider.CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)**.

```cs
// registeredOrder - erfolgreich registrierte Order.
_connector.CancelOrder(registeredOrder);
```

## Empfohlene Inhalte

[Massenstornierung von Orders](orders_mass_cancel.md)

[Orders ersetzen](orders_replacement.md)

