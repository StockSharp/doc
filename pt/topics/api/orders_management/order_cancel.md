# Cancelamento de ordem

O cancelamento de ordem é necessário se a situação de mercado se alterar de forma desfavorável à ordem emitida. Para cancelar ordens, use o método [ITransactionProvider.CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** no [S#](../../api.md).

```cs
// registeredOrder - successfully registered order.
_connector.CancelOrder(registeredOrder);
```

## Conteúdo recomendado

[Cancelamento em massa de ordens](orders_mass_cancel.md)

[Substituição de ordens](orders_replacement.md)
