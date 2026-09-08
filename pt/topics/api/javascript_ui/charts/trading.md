# Negociação a partir do gráfico

`TradingLayer` é a camada de estado de negociação do gráfico, independente da corretora: guarda ordens, posições, negócios e a cotação já normalizados e entrega as ações do utilizador ao exterior sob a forma de intenções (`TradingIntent`). A camada não envia nada por si — não tem transporte, nem conta, nem repetições — e a ligação à corretora fica a cargo do anfitrião.

![Linhas de ordens, posição e ordens de proteção sobre o gráfico](../../../../images/javascript_charts_trading.png)

## Ligação

A camada é distribuída num ponto de entrada próprio do pacote [@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart):

```ts
import { TradingLayer, TradingLayerPrimitive } from '@stocksharp/chart/trading';
```

Sem um empacotador, as mesmas classes estão disponíveis no objeto global `SSChart` (`new SSChart.TradingLayer({ tickSize: 0.25 })`).

## Criação e atualização

A camada é criada com os parâmetros da grelha de preços, o primitivo `TradingLayerPrimitive` desenha o seu estado sobre a série e `TradingOrderPlacementAdapter` transforma um clique no gráfico numa intenção de colocar uma ordem:

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

// Estado canónico da corretora: o gráfico limita-se a apresentá-lo e nunca o altera.
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

// As intenções vão para o anfitrião e o anfitrião responde à camada com o resultado.
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

// Ctrl + botão esquerdo — compra limitada; Ctrl + botão direito — venda limitada.
new TradingOrderPlacementAdapter(chart, layer, { quantity: 1, orderType: 'limit', modifier: 'ctrl' });
```

`setOrders`, `setPositions`, `setExecutions` e `setQuote` substituem por inteiro a coleção correspondente. Cada chamada é normalizada e comparada com o estado atual: se nada mudou, os subscritores não são chamados; caso contrário é publicada uma alteração com os campos `added`, `updated` (pares `previous` / `current`), `removed` e `orderChanged`. A cotação é a exceção: a sua alteração só tem `previous` e `current`. O estado atual é devolvido por `state()` — é `{ version, orders, positions, executions }` mais a cotação atual.

A normalização é estrita: os preços têm de assentar na grelha de `tickSize` (com o desvio `priceOrigin`) e as quantidades em `quantityStep`, se este estiver definido; uma ordem limitada exige `price`, uma ordem stop exige `stopPrice` e uma stop-limit exige ambos os campos. Dados inconsistentes são rejeitados com uma exceção, não são corrigidos em silêncio.

## Intenções

A camada não executa as ações do utilizador, publica-as como intenção: um método `request*` devolve o objeto da intenção, coloca-o na fila das pendentes e entrega-o aos subscritores de `subscribeIntents`. O anfitrião executa a intenção junto da corretora e fecha-a chamando `resolveIntent({ intentId, status, reason })` com o estado `accepted` ou `rejected` — o desfecho chega a `subscribeIntentOutcomes` juntamente com a intenção original. As intenções pendentes são enumeradas por `pendingIntents()`.

As permissões são verificadas antes da publicação: alterar uma ordem exige `permissions.canModify`, cancelar exige `canCancel` e as ações sobre a posição exigem `canClose`, `canReverse` e `canProtect`. Uma ordem sem permissões é considerada apenas de leitura e a chamada lança uma exceção. Na intenção é colocado o `expectedRevision` da entidade canónica, para que a corretora possa rejeitar um pedido construído sobre dados desatualizados.

## Desenho e arrastamento

`TradingLayerPrimitive` é um primitivo do gráfico que subscreve a camada ao ser ligado e desenha as linhas das ordens com as respetivas etiquetas, a linha da posição com o P&L, os marcadores dos negócios, as linhas bid/ask/last e as ligações dos brackets. O que mostrar e com que cores é definido pelas opções do construtor e por `applyOptions`: `showOrders`, `showInactiveOrders`, `showPositions`, `showExecutions`, `showExecutionLabels`, `showQuote`, `showPnl`, `showBrackets`, `autoscale`, o conjunto de cores (`orderBuyColor`, `orderSellColor`, `inactiveOrderColor`, `longPositionColor`, `shortPositionColor`, `executionBuyColor`, `executionSellColor`, `bidColor`, `askColor`, `lastColor`, `bracketColor`), `lineWidth`, `fontSize`, `orderLabelSpacing`, `zOrder`, além dos formatadores `priceFormatter`, `quantityFormatter` e `pnlFormatter`. O identificador `id` é definido uma única vez na criação e depois não muda.

A linha de uma ordem pode ser arrastada com o rato se a ordem estiver ativa (`pending`, `working` ou `partially-filled`), não for ao mercado e tiver a permissão `canModify`. Durante o arrastamento, o primitivo mostra o preço provisório ajustado à grelha; ao largar o botão é publicada a intenção `requestModifyOrder` e a pré-visualização mantém-se até o anfitrião fechar a intenção — uma rejeição devolve a linha ao preço canónico.

O que está sob o cursor é descrito por `TradingOrderHitData`, `TradingPositionHitData`, `TradingExecutionHitData` e `TradingQuoteHitData`; para os reconhecer no processador existe `isTradingPrimitiveHitData(value)`.

## Colocação de ordens com o rato

`TradingOrderPlacementAdapter` liga o sinal de colocação nativo do gráfico à camada: ativa o modo de colocação, escuta os cliques e chama `requestPlaceOrder`. Por si só, não cria linhas de preço nem comunica com a corretora.

Opções: `quantity` (obrigatória), `orderType` — `'limit'` ou `'stop'` (por predefinição `'limit'`), `timeInForce` (por predefinição `'good-till-cancelled'`), `modifier` — `'ctrl'`, `'shift'` ou `'alt'` (por predefinição `'ctrl'`), `color`, `title`, `enabled` e `sideResolver`. O resolvedor predefinido devolve compra no botão esquerdo e venda no direito, e `null` cancela a colocação. O adaptador é comandado pelos métodos `options()`, `applyOptions(patch)`, `setEnabled(enabled)` e `dispose()`.

## Métodos públicos da camada

- `setOrders(orders)`, `setPositions(positions)`, `setExecutions(executions)`, `setQuote(...)` — substituir o estado canónico; `setQuote(null)` retira a cotação.
- `state()` — o estado atual com o número de versão.
- `normalizationOptions()` — os parâmetros da grelha de preços com que a camada foi criada.
- `subscribeChanges(handler)`, `subscribeIntents(handler)`, `subscribeIntentOutcomes(handler)` — subscrições; cada uma devolve a função para cancelar.
- `pendingIntents()`, `resolveIntent(resolution)` — a fila de intenções pendentes e o seu fecho.
- `requestPlaceOrder(order)`, `requestModifyOrder(orderId, changes)`, `requestCancelOrder(orderId)` — trabalho com ordens.
- `requestClosePosition(positionId, quantity)`, `requestReversePosition(positionId, quantity)` — trabalho com a posição; sem quantidade é tomada a posição inteira.
- `requestCreateStopLoss`, `requestEditStopLoss`, `requestRemoveStopLoss`, `requestCreateTakeProfit`, `requestEditTakeProfit`, `requestRemoveTakeProfit` — ordens de proteção do bracket.
- `dispose()` — libertar os recursos.

## Restantes exportações

- Enumerações do modelo: `TradingSide`, `ChartOrderType`, `ChartOrderStatus`, `ChartOrderTimeInForce`, `ChartPositionSide`, `ChartBracketRole`, `ChartExecutionLiquidity`, `TradingIntentKind`.
- Enumerações da camada e do primitivo: `TradingLayerChangeKind`, `TradingIntentOutcomeStatus`, `TradingPrimitiveEntityKind`, `TradingQuoteKind`.
- Validação e normalização de dados: `normalizeChartOrder`, `normalizeChartOrders`, `normalizeChartPosition`, `normalizeChartPositions`, `normalizeChartExecution`, `normalizeChartExecutions`, `normalizeChartQuote`, `normalizeChartOrderRequest`, `normalizeTradingIntent`, `normalizeTradingModelOptions`.
- Cálculos auxiliares: `quantizeTradingPrice` — ajuste de um preço arbitrário à grelha, `chartOrderRemainingQuantity` — a quantidade por executar de uma ordem, `chartPnlTotal` — a soma do P&L realizado e não realizado.
- Os tipos `ChartOrder`, `ChartPosition`, `ChartExecution`, `ChartQuote`, `ChartOrderRequest`, `ChartOrderModification`, `TradingIntent` e as interfaces de intenção que deles derivam.

## Veja também

- [Gráficos em JavaScript](../charts.md)
- [Candlestick](candlestick.md)
- [Preenchimento de histórico](backfill.md)
