# Entrada de órdenes

`OrderEntryWidget` es un panel bilateral de compra y venta. Admite órdenes de mercado, limitadas, con activación y limitadas con activación, muestra solo los campos correspondientes al tipo seleccionado y valida los valores antes de pasarlos al anfitrión.

## Creación y configuración del instrumento

```ts
import {
  OrderEntrySides,
  OrderEntryTypes,
  OrderEntryWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const pad = OrderEntryWidget.create(
  document.querySelector<HTMLElement>('#order-entry')!,
  {},
  {
    host,
    submitOrder: (side, values) =>
      console.log('enviar', side, values),
  },
);

pad.setInstrument({
  symbol: 'BTC@IMEX',
  lotSize: 0.001,
  tickSize: 0.1,
  minVolume: 0.001,
  maxVolume: 5,
});

pad.setOrderType(OrderEntryTypes.Limit);
pad.setBbo(68_420.4, 68_420.5);
pad.setAvailable(OrderEntrySides.Buy, 48_251);
pad.setMaxQuantity(OrderEntrySides.Buy, 0.7);
pad.setQuantity(0.01);
```

`OrderEntryTypes` contiene `Market`, `Limit`, `Stop` y `StopLimit`, mientras que `OrderEntrySides` contiene `Buy` y `Sell`.

## Validación y envío

El control valida los valores positivos, el tamaño del lote, el incremento de precio y el volumen mínimo y máximo. La toma de beneficios y el límite de pérdidas opcionales se activan mediante el método `toggleTpSl`.

`submit(side)` llama primero a `validate(side)`. Cuando los datos son correctos, el controlador `submitOrder` recibe el lado y un objeto:

```ts
interface OrderEntryValues {
  type: 'market' | 'limit' | 'stop' | 'stoplimit';
  quantity: number;
  limitPrice: number | null;
  stopPrice: number | null;
  takeProfit: number | null;
  stopLoss: number | null;
}
```

El control no selecciona la cartera, no comprueba la conexión ni envía la orden al servidor. Estas acciones corresponden al controlador del anfitrión.

## Precios, volumen y estado

- `setBbo(bid, ask)` actualiza las mejores ofertas de compra y venta.
- `applyBbo(side)` inserta en la columna seleccionada el lado opuesto de la BBO para una ejecución inmediata.
- `setAvailable(side, value)` establece el saldo disponible mostrado.
- `setMaxQuantity(side, value)` establece el máximo a partir del cual los botones de porcentaje calculan la cantidad.
- `applyPercent(side, pct)` aplica el 25, 50, 75 o 100 por ciento del máximo establecido mediante `setMaxQuantity`.
- `preselect(side)` resalta el lado después de un gesto de negociación con un clic.
- `setEnabled(false)` impide el envío sin eliminar los valores introducidos.
- `getValues(side)` y `validate(side)` permiten comprobar el formulario desde el exterior.

El método estático `toApiType` convierte `Limit` en `0`, `Market` en `1`, y `Stop` y `StopLimit` en `2`.

## Véase también

- [Controles de negociación JavaScript](../javascript_trading_controls.md)
- [Libro de órdenes](order_book.md)
- [Posiciones](positions.md)
