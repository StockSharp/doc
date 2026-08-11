# Historial de operaciones

`TradeHistoryWidget` es una tabla de ejecuciones de la cartera actual. Las operaciones más recientes aparecen en la parte superior; cada fila muestra la hora, el instrumento, el lado, la cantidad, el precio, el identificador de la operación y el identificador de la orden.

## Creación y carga

```ts
import {
  TradeHistoryWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const history = TradeHistoryWidget.create(
  document.querySelector<HTMLElement>('#history')!,
  {},
  { host },
);

await history.refresh();
```

La creación del control no inicia la carga automáticamente. El método `refresh()` obtiene la cartera actual mediante `host.trading.portfolioId()`, comprueba el permiso y carga las ejecuciones:

```ts
const portfolioId = host.trading.portfolioId();
if (portfolioId && host.allow('load trade history'))
  host.trading.api.getExecutions(portfolioId, null, 200);
```

Este enfoque es importante al cambiar de cartera: el identificador se lee justo antes de cada actualización y no se guarda al crear el panel.

## Comportamiento

El panel es de solo lectura. El usuario puede ordenar y seleccionar filas, abrir el menú contextual, actualizar los datos y exportar las columnas visibles a XLSX. Los errores de carga se envían a `host.log`.

La API pública del control consta de `refresh(): Promise<void>` y `dispose(): void`.

## Véase también

- [Controles de negociación JavaScript](../trading_controls.md)
- [Órdenes activas](active_orders.md)
- [Flujo de operaciones](trade_feed.md)
