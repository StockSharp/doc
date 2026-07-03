# Cancelación de orden

La cancelación de órdenes es necesaria si la situación de mercado cambia en contra de la orden emitida. Para cancelar órdenes, use el método [ITransactionProvider.CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** en [S#](../../api.md).

```cs
// registeredOrder - orden registrada correctamente.
_connector.CancelOrder(registeredOrder);
```

## Contenido recomendado

[Cancelación masiva de órdenes](orders_mass_cancel.md)

[Reemplazo de órdenes](orders_replacement.md)
