# Posiciones

`PositionsWidget` muestra las posiciones abiertas y una fila de saldo de efectivo fijada en la parte superior. De forma predeterminada, las posiciones se disponen en orden alfabético, mientras que el saldo no participa en la ordenación, la selección ni la exportación.

## Creación y actualización

```ts
import {
  PositionsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let positions!: PositionsWidget;

positions = PositionsWidget.create(
  document.querySelector<HTMLElement>('#positions')!,
  {},
  {
    host,
    closePosition: (portfolioId, instrumentId, symbol) =>
      console.log('cerrar', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('invertir', portfolioId, instrumentId, symbol),
    refreshPositions: () => positions.update([]),
  },
);

positions.update([{
  portfolioId: 10,
  instrumentId: 42,
  instrument: 'BTC@IMEX',
  quantity: 0.25,
  avgPrice: 68_000,
  currentPrice: 68_420,
  unrealizedPnl: 105,
  realizedPnl: 20,
}]);

positions.updateBalance({
  available: 48_251,
  locked: 1_749,
  total: 50_000,
});
```

`update` sustituye la lista de posiciones. `applyDelta` actualiza una posición según la combinación de cartera e instrumento; una fila con cantidad cero se elimina. `updateBalance(null)` retira el saldo fijado.

## Datos y acciones

La fila muestra la cantidad, los precios medio y actual, y un único PnL total calculado como la suma de `realizedPnl` y `unrealizedPnl`. `host.presentation.pnlClass` devuelve la clase que determina su color.

Los botones de la fila llaman a las funciones `closePosition` y `reversePosition` proporcionadas por el anfitrión. El control no genera ni envía órdenes de negociación por sí mismo.

## Métodos públicos

- `update(positions)`: reemplaza todas las posiciones.
- `updateBalance(balance)`: establece o elimina el saldo de efectivo.
- `applyDelta(position)`: aplica un cambio en tiempo real a una posición.
- `dispose()`: libera los recursos.

El panel también permite actualizar y ordenar los datos, abrir un menú contextual y exportar las posiciones a XLSX.

## Véase también

- [Controles de negociación JavaScript](../javascript_trading_controls.md)
- [Órdenes activas](active_orders.md)
- [Entrada de órdenes](order_entry.md)
