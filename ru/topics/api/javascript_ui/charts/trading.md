# Торговля с графика

`TradingLayer` — брокеро-независимый слой торгового состояния графика: он хранит нормализованные заявки, позиции, сделки и котировку, а действия пользователя отдаёт наружу намерениями (`TradingIntent`). Слой ничего не отправляет сам — ни транспорта, ни счёта, ни повторов в нём нет, связь с брокером остаётся за хостом.

![Линии заявок, позиция и защитные заявки поверх графика](../../../../images/javascript_charts_trading.png)

## Подключение

Слой поставляется отдельной точкой входа пакета [@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart):

```ts
import { TradingLayer, TradingLayerPrimitive } from '@stocksharp/chart/trading';
```

Без сборщика те же классы доступны в глобальном объекте `SSChart` (`new SSChart.TradingLayer({ tickSize: 0.25 })`).

## Создание и обновление

Слой создаётся с параметрами ценовой сетки, примитив `TradingLayerPrimitive` рисует его состояние поверх серии, а `TradingOrderPlacementAdapter` превращает клик по графику в намерение выставить заявку:

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

// Каноническое состояние брокера: график только отображает его и никогда не меняет сам.
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

// Намерения уходят хосту, хост отвечает слою результатом.
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

// Ctrl + левая кнопка — лимитная покупка, Ctrl + правая — лимитная продажа.
new TradingOrderPlacementAdapter(chart, layer, { quantity: 1, orderType: 'limit', modifier: 'ctrl' });
```

`setOrders`, `setPositions`, `setExecutions` и `setQuote` заменяют соответствующую коллекцию целиком. Каждый вызов нормализуется и сравнивается с текущим состоянием: если ничего не поменялось, подписчики не вызываются, а иначе публикуется изменение с полями `added`, `updated` (пары `previous` / `current`), `removed` и `orderChanged`. Котировка — исключение: у её изменения есть только `previous` и `current`. Актуальный срез отдаёт `state()` — это `{ version, orders, positions, executions, quote }`.

Нормализация строгая: цены обязаны ложиться на сетку `tickSize` (со смещением `priceOrigin`), количества — на `quantityStep`, если он задан; лимитная заявка требует `price`, стоповая — `stopPrice`, а стоп-лимитная — оба поля. Несогласованные данные отклоняются исключением, а не молча исправляются.

## Намерения

Пользовательские действия слой не выполняет, а публикует как намерение: метод `request*` возвращает объект намерения, кладёт его в очередь ожидающих и передаёт подписчикам `subscribeIntents`. Хост исполняет намерение у брокера и закрывает его вызовом `resolveIntent({ intentId, status, reason })` со статусом `accepted` или `rejected` — итог приходит в `subscribeIntentOutcomes` вместе с исходным намерением. Ожидающие намерения перечисляет `pendingIntents()`.

Права проверяются до публикации: изменение заявки требует `permissions.canModify`, отмена — `canCancel`, действия с позицией — `canClose`, `canReverse` и `canProtect`. Заявка без прав считается доступной только для чтения, и вызов бросает исключение. В намерение подставляется `expectedRevision` из канонической сущности, чтобы брокер мог отвергнуть запрос, построенный на устаревших данных.

## Отрисовка и перетаскивание

`TradingLayerPrimitive` — примитив графика, который подписывается на слой при подключении и рисует линии заявок с подписями, линию позиции с P&L, маркеры сделок, линии bid/ask/last и связи брекетов. Что показывать и какими цветами, задают опции конструктора и `applyOptions`: `showOrders`, `showInactiveOrders`, `showPositions`, `showExecutions`, `showExecutionLabels`, `showQuote`, `showPnl`, `showBrackets`, `autoscale`, набор цветов (`orderBuyColor`, `orderSellColor`, `inactiveOrderColor`, `longPositionColor`, `shortPositionColor`, `executionBuyColor`, `executionSellColor`, `bidColor`, `askColor`, `lastColor`, `bracketColor`), `lineWidth`, `fontSize`, `orderLabelSpacing`, `zOrder`, а также форматтеры `priceFormatter`, `quantityFormatter` и `pnlFormatter`. Идентификатор `id` задаётся один раз при создании и позже не меняется.

Линию заявки можно тянуть мышью, если заявка активна (`pending`, `working` или `partially-filled`), не рыночная и имеет право `canModify`. Пока идёт перетаскивание, примитив показывает предварительную цену, привязанную к сетке; по отпусканию кнопки публикуется намерение `requestModifyOrder`, а предпросмотр держится, пока хост не закроет намерение — отклонённое возвращает линию на каноническую цену.

Попадание курсора описывают `TradingOrderHitData`, `TradingPositionHitData`, `TradingExecutionHitData` и `TradingQuoteHitData`; распознать их в обработчике помогает `isTradingPrimitiveHitData(value)`.

## Размещение заявок мышью

`TradingOrderPlacementAdapter` связывает штатный сигнал размещения графика со слоем: он включает режим размещения, слушает клики и вызывает `requestPlaceOrder`. Сам он ни ценовых линий не создаёт, ни с брокером не общается.

Опции: `quantity` (обязательна), `orderType` — `'limit'` или `'stop'` (по умолчанию `'limit'`), `timeInForce` (по умолчанию `'good-till-cancelled'`), `modifier` — `'ctrl'`, `'shift'` или `'alt'` (по умолчанию `'ctrl'`), `color`, `title`, `enabled` и `sideResolver`. Резолвер по умолчанию отдаёт покупку по левой кнопке и продажу по правой, а `null` отменяет размещение. Управляют адаптером методы `options()`, `applyOptions(patch)`, `setEnabled(enabled)` и `dispose()`.

## Публичные методы слоя

- `setOrders(orders)`, `setPositions(positions)`, `setExecutions(executions)`, `setQuote(quote)` — заменить каноническое состояние; `setQuote(null)` убирает котировку.
- `state()` — текущий срез состояния с номером версии.
- `normalizationOptions()` — параметры ценовой сетки, с которыми создан слой.
- `subscribeChanges(handler)`, `subscribeIntents(handler)`, `subscribeIntentOutcomes(handler)` — подписки; каждая возвращает функцию отписки.
- `pendingIntents()`, `resolveIntent(resolution)` — очередь ожидающих намерений и её закрытие.
- `requestPlaceOrder(order)`, `requestModifyOrder(orderId, changes)`, `requestCancelOrder(orderId)` — работа с заявками.
- `requestClosePosition(positionId, quantity)`, `requestReversePosition(positionId, quantity)` — работа с позицией; без количества берётся вся позиция.
- `requestCreateStopLoss`, `requestEditStopLoss`, `requestRemoveStopLoss`, `requestCreateTakeProfit`, `requestEditTakeProfit`, `requestRemoveTakeProfit` — защитные заявки брекета.
- `dispose()` — освободить ресурсы.

## Остальные экспорты

- Перечисления модели: `TradingSide`, `ChartOrderType`, `ChartOrderStatus`, `ChartOrderTimeInForce`, `ChartPositionSide`, `ChartBracketRole`, `ChartExecutionLiquidity`, `TradingIntentKind`.
- Перечисления слоя и примитива: `TradingLayerChangeKind`, `TradingIntentOutcomeStatus`, `TradingPrimitiveEntityKind`, `TradingQuoteKind`.
- Проверка и нормализация данных: `normalizeChartOrder`, `normalizeChartOrders`, `normalizeChartPosition`, `normalizeChartPositions`, `normalizeChartExecution`, `normalizeChartExecutions`, `normalizeChartQuote`, `normalizeChartOrderRequest`, `normalizeTradingIntent`, `normalizeTradingModelOptions`.
- Вспомогательные вычисления: `quantizeTradingPrice` — привязка произвольной цены к сетке, `chartOrderRemainingQuantity` — неисполненный остаток заявки, `chartPnlTotal` — сумма реализованного и нереализованного P&L.
- Типы `ChartOrder`, `ChartPosition`, `ChartExecution`, `ChartQuote`, `ChartOrderRequest`, `ChartOrderModification`, `TradingIntent` и производные от них интерфейсы намерений.

## Смотрите также

- [JavaScript-графики](../charts.md)
- [Свечи](candlestick.md)
- [Догрузка истории](backfill.md)
