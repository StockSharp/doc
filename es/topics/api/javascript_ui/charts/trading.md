# Negociación desde el gráfico

`TradingLayer` es la capa de estado de negociación del gráfico, independiente del bróker: almacena de forma normalizada las órdenes, las posiciones, las operaciones y la cotización, y entrega hacia fuera las acciones del usuario como intenciones (`TradingIntent`). La capa no envía nada por sí misma —no tiene transporte, ni cuenta, ni reintentos—, y la comunicación con el bróker queda a cargo del anfitrión.

![Líneas de órdenes, posición y órdenes de protección sobre el gráfico](../../../../images/javascript_charts_trading.png)

## Conexión

La capa se distribuye como punto de entrada aparte del paquete [@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart):

```ts
import { TradingLayer, TradingLayerPrimitive } from '@stocksharp/chart/trading';
```

Sin empaquetador, esas mismas clases están disponibles en el objeto global `SSChart` (`new SSChart.TradingLayer({ tickSize: 0.25 })`).

## Creación y actualización

La capa se crea con los parámetros de la retícula de precios, el primitivo `TradingLayerPrimitive` dibuja su estado sobre la serie y `TradingOrderPlacementAdapter` convierte un clic en el gráfico en la intención de colocar una orden:

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import {
  TradingLayer,
  TradingLayerPrimitive,
  TradingOrderPlacementAdapter,
  type TradingIntent,
} from '@stocksharp/chart/trading';

declare const broker: { send(intent: TradingIntent): Promise<void> };

const chart = createChart(document.querySelector<HTMLElement>('#chart')!, {
  timeScale: { timeVisible: true },
});
const candles = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });

const layer = new TradingLayer({ tickSize: 0.25 });

chart.attachPrimitive(new TradingLayerPrimitive(layer, { showInactiveOrders: false }), {
  series: candles,
});

// Estado canónico del bróker: el gráfico solo lo muestra y nunca lo modifica por su cuenta.
layer.setOrders([{
  id: 'ob1',
  side: 'buy',
  type: 'limit',
  status: 'working',
  timeInForce: 'good-till-cancelled',
  quantity: 2,
  filledQuantity: 0,
  price: 96,
  revision: 1,
  permissions: { canModify: true, canCancel: true },
  label: 'BID',
}]);

layer.setPositions([{
  id: 'pos1',
  side: 'long',
  quantity: 3,
  averagePrice: 100,
  revision: 1,
  pnl: { realized: 0, unrealized: 45, currency: 'USD', markPrice: 101.5 },
  permissions: { canClose: true, canReverse: true, canProtect: true },
}]);

layer.setQuote({ time: 1704326400, bidPrice: 100.75, bidSize: 5, askPrice: 101, askSize: 4 });

// Las intenciones van al anfitrión, y el anfitrión responde a la capa con el resultado.
layer.subscribeIntents(intent => {
  broker.send(intent).then(
    () => layer.resolveIntent({ intentId: intent.intentId, status: 'accepted' }),
    (error: Error) => layer.resolveIntent({
      intentId: intent.intentId,
      status: 'rejected',
      reason: error.message,
    }),
  );
});

// Ctrl + botón izquierdo: compra limitada; Ctrl + botón derecho: venta limitada.
new TradingOrderPlacementAdapter(chart, layer, { quantity: 1, orderType: 'limit', modifier: 'ctrl' });
```

`setOrders`, `setPositions`, `setExecutions` y `setQuote` sustituyen por completo la colección correspondiente. Cada llamada se normaliza y se compara con el estado actual: si no ha cambiado nada, no se llama a los suscriptores; en caso contrario se publica un cambio con los campos `added`, `updated` (pares `previous` / `current`), `removed` y `orderChanged`. La cotización es la excepción: su cambio solo tiene `previous` y `current`. La instantánea actual la entrega `state()`, que es `{ version, orders, positions, executions }` más la cotización actual.

La normalización es estricta: los precios deben caer sobre la retícula `tickSize` (con el desplazamiento `priceOrigin`) y las cantidades sobre `quantityStep`, si se ha indicado; una orden limitada exige `price`, una orden stop exige `stopPrice` y una stop-limit exige ambos campos. Los datos incoherentes se rechazan con una excepción, no se corrigen en silencio.

## Intenciones

La capa no ejecuta las acciones del usuario, sino que las publica como intención: el método `request*` devuelve el objeto de la intención, lo coloca en la cola de pendientes y lo transmite a los suscriptores de `subscribeIntents`. El anfitrión ejecuta la intención en el bróker y la cierra llamando a `resolveIntent({ intentId, status, reason })` con el estado `accepted` o `rejected`; el resultado llega a `subscribeIntentOutcomes` junto con la intención original. Las intenciones pendientes las enumera `pendingIntents()`.

Los permisos se comprueban antes de publicar: modificar una orden exige `permissions.canModify`, cancelarla exige `canCancel`, y las acciones sobre una posición exigen `canClose`, `canReverse` y `canProtect`. Una orden sin permisos se considera de solo lectura y la llamada lanza una excepción. En la intención se inserta el `expectedRevision` de la entidad canónica, para que el bróker pueda rechazar una petición construida sobre datos obsoletos.

## Dibujado y arrastre

`TradingLayerPrimitive` es un primitivo del gráfico que se suscribe a la capa al conectarse y dibuja las líneas de las órdenes con sus rótulos, la línea de la posición con su P&L, los marcadores de operaciones, las líneas de bid/ask/last y los vínculos de los brackets. Qué mostrar y con qué colores lo determinan las opciones del constructor y `applyOptions`: `showOrders`, `showInactiveOrders`, `showPositions`, `showExecutions`, `showExecutionLabels`, `showQuote`, `showPnl`, `showBrackets`, `autoscale`, el conjunto de colores (`orderBuyColor`, `orderSellColor`, `inactiveOrderColor`, `longPositionColor`, `shortPositionColor`, `executionBuyColor`, `executionSellColor`, `bidColor`, `askColor`, `lastColor`, `bracketColor`), `lineWidth`, `fontSize`, `orderLabelSpacing`, `zOrder`, y también los formateadores `priceFormatter`, `quantityFormatter` y `pnlFormatter`. El identificador `id` se fija una sola vez, al crear el primitivo, y después no cambia.

La línea de una orden puede arrastrarse con el ratón si la orden está activa (`pending`, `working` o `partially-filled`), no es de mercado y tiene el permiso `canModify`. Mientras dura el arrastre, el primitivo muestra el precio previsto ajustado a la retícula; al soltar el botón se publica la intención `requestModifyOrder`, y la vista previa se mantiene hasta que el anfitrión cierra la intención: si se rechaza, la línea vuelve al precio canónico.

Los impactos del cursor los describen `TradingOrderHitData`, `TradingPositionHitData`, `TradingExecutionHitData` y `TradingQuoteHitData`; para reconocerlos en un manejador ayuda `isTradingPrimitiveHitData(value)`.

## Colocar órdenes con el ratón

`TradingOrderPlacementAdapter` enlaza la señal de colocación estándar del gráfico con la capa: activa el modo de colocación, escucha los clics y llama a `requestPlaceOrder`. Por sí mismo no crea líneas de precio ni se comunica con el bróker.

Opciones: `quantity` (obligatoria), `orderType`, que es `'limit'` o `'stop'` (de forma predeterminada `'limit'`), `timeInForce` (de forma predeterminada `'good-till-cancelled'`), `modifier`, que es `'ctrl'`, `'shift'` o `'alt'` (de forma predeterminada `'ctrl'`), `color`, `title`, `enabled` y `sideResolver`. El resolvedor predeterminado devuelve compra con el botón izquierdo y venta con el derecho, y `null` cancela la colocación. El adaptador se gobierna con los métodos `options()`, `applyOptions(patch)`, `setEnabled(enabled)` y `dispose()`.

## Métodos públicos de la capa

- `setOrders(orders)`, `setPositions(positions)`, `setExecutions(executions)`, `setQuote(...)`: sustituyen el estado canónico; `setQuote(null)` retira la cotización.
- `state()`: la instantánea actual del estado con su número de versión.
- `normalizationOptions()`: los parámetros de la retícula de precios con los que se creó la capa.
- `subscribeChanges(handler)`, `subscribeIntents(handler)`, `subscribeIntentOutcomes(handler)`: suscripciones; cada una devuelve una función para cancelarla.
- `pendingIntents()`, `resolveIntent(resolution)`: la cola de intenciones pendientes y su cierre.
- `requestPlaceOrder(order)`, `requestModifyOrder(orderId, changes)`, `requestCancelOrder(orderId)`: trabajo con las órdenes.
- `requestClosePosition(positionId, quantity)`, `requestReversePosition(positionId, quantity)`: trabajo con la posición; sin cantidad se toma la posición completa.
- `requestCreateStopLoss`, `requestEditStopLoss`, `requestRemoveStopLoss`, `requestCreateTakeProfit`, `requestEditTakeProfit`, `requestRemoveTakeProfit`: órdenes de protección del bracket.
- `dispose()`: libera los recursos.

## Otras exportaciones

- Enumeraciones del modelo: `TradingSide`, `ChartOrderType`, `ChartOrderStatus`, `ChartOrderTimeInForce`, `ChartPositionSide`, `ChartBracketRole`, `ChartExecutionLiquidity`, `TradingIntentKind`.
- Enumeraciones de la capa y del primitivo: `TradingLayerChangeKind`, `TradingIntentOutcomeStatus`, `TradingPrimitiveEntityKind`, `TradingQuoteKind`.
- Comprobación y normalización de datos: `normalizeChartOrder`, `normalizeChartOrders`, `normalizeChartPosition`, `normalizeChartPositions`, `normalizeChartExecution`, `normalizeChartExecutions`, `normalizeChartQuote`, `normalizeChartOrderRequest`, `normalizeTradingIntent`, `normalizeTradingModelOptions`.
- Cálculos auxiliares: `quantizeTradingPrice`, que ajusta un precio arbitrario a la retícula; `chartOrderRemainingQuantity`, el resto no ejecutado de una orden; `chartPnlTotal`, la suma del P&L realizado y no realizado.
- Los tipos `ChartOrder`, `ChartPosition`, `ChartExecution`, `ChartQuote`, `ChartOrderRequest`, `ChartOrderModification`, `TradingIntent` y las interfaces de intención derivadas de ellos.

## Véase también

- [Gráficos en JavaScript](../charts.md)
- [Velas](candlestick.md)
- [Relleno de historial](backfill.md)
