# Órdenes activas

`ActiveOrdersWidget` muestra la lista completa de órdenes con sus estados actuales. Las filas ejecutadas, canceladas y rechazadas permanecen en la tabla, por lo que el usuario puede ver toda la secuencia de cambios de la sesión.

## Creación

```ts
import {
  ActiveOrdersWidget,
  OrderStates,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let orders!: ActiveOrdersWidget;

orders = ActiveOrdersWidget.create(
  document.querySelector<HTMLElement>('#orders')!,
  {},
  {
    host,
    cancelOrder: id => console.log('cancelar', id),
    dismissOrder: id => orders.removeOrder(id),
    editOrderField: (id, field) => {
      if (field === 'quantity' || field === 'limitPrice' || field === 'stopPrice')
        orders.startInlineEdit(id, field);
    },
    replaceOrder: (id, quantity, limitPrice, stopPrice) =>
      console.log('reemplazar', id, quantity, limitPrice, stopPrice),
    cancelAllOrders: () => console.log('cancelar todas'),
    refreshOrders: () => orders.update([]),
  },
);

orders.update([{
  id: 501,
  localId: 1,
  instrument: 'BTC@IMEX',
  side: 0,
  type: 0,
  quantity: 0.01,
  limitPrice: 68_400,
  status: OrderStates.Active,
}]);
```

`update` sustituye todo el conjunto de filas. Para los cambios en tiempo real, utilice `applyDelta(order)` y, para eliminar una sola fila, `removeOrder(orderId)`.

## Edición y acciones

La cantidad puede modificarse con un doble clic mientras la orden se encuentra en el estado `Sent` o `Active`. El precio límite solo puede editarse si `limitPrice` ya es mayor que `0`; del mismo modo, el precio de activación solo puede editarse si `stopPrice` ya es mayor que `0`. Después de confirmar, el control llama a `replaceOrder` y proporciona los tres valores `quantity`, `limitPrice` y `stopPrice`, no solo el campo modificado.

Para una orden activa, el botón de acción llama a `cancelOrder`. Para una fila en estado final, llama a `dismissOrder` y elimina el registro únicamente de la vista local. El motivo del rechazo `rejectReason` se muestra en una ayuda emergente.

El control también permite cancelar todas las órdenes, actualizar los datos, ordenar y seleccionar filas, abrir un menú contextual y exportar a XLSX.

## Métodos públicos

- `update(orders)`: reemplaza todas las filas.
- `applyDelta(order)`: añade o actualiza una orden.
- `removeOrder(orderId)`: elimina una fila.
- `getOrder(orderId)`: obtiene la fila actual.
- `startInlineEdit(orderId, field)`: inicia la edición de `quantity`, `limitPrice` o `stopPrice`.
- `dispose()`: libera los recursos del control.

El objeto `OrderStates` exporta los estados `PendingRisk`, `Sent`, `Active`, `PartiallyFilled`, `Filled`, `Rejected` y `Cancelled`.

## Véase también

- [Controles de negociación JavaScript](../trading_controls.md)
- [Posiciones](positions.md)
- [Historial de operaciones](trade_history.md)
