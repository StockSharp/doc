# 成交明细

`TradeFeedWidget` 显示公开市场成交以及当前投资组合的成交记录。市场成交明细可以在表格和气泡图之间切换。

## 创建和数据流

```ts
import {
  TradeFeedWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const feed = TradeFeedWidget.create(
  document.querySelector<HTMLElement>('#trade-feed')!,
  {},
  { host },
);

feed.setActiveSymbol('BTC@IMEX');

feed.setTrades([{
  symbol: 'BTC@IMEX',
  side: 0,
  price: 68_420.5,
  quantity: 0.25,
  time: new Date().toISOString(),
}]);

feed.addTrade({
  symbol: 'BTC@IMEX',
  side: 1,
  price: 68_419.9,
  quantity: 0.4,
  time: new Date().toISOString(),
});
```

`setTrades` 替换原始数据集，`addTrade` 则添加一笔新的实时成交。该控件最多保存 50 个表格行和用于图表的最近 500 个成交点。

## 气泡图

图表的横轴表示时间，纵轴表示价格，气泡半径表示成交量，颜色表示交易方向。合并相邻成交点时，工具提示会显示成交量加权平均价、总成交量和成交笔数。当一笔成交的数量超过移动平均值两倍时，它会被视为大额成交。

canvas 颜色来自 `host.presentation.canvasPalette()`。所选模式以页面通用键保存在 `host.preferences` 中。

## 其他交易品种

除了当前交易品种外，面板还可以固定其他交易品种：

```ts
await feed.addExtraSymbol('ETH@IMEX');
console.log(feed.getExtraSymbols());
await feed.removeExtraSymbol('ETH@IMEX');
```

控件会为这些交易品种创建 `MarketDataLevels.Tape` 级别的订阅，并在气泡图中为其创建具有独立价格刻度的单独区域。其他交易品种列表保存在具体实例的状态中，创建时可按 `{ extras: ['ETH@IMEX'] }` 的形式传入。

## 自有成交

第二个选项卡显示投资组合的成交记录。可以显式触发加载：

```ts
await feed.loadMyTrades(host.trading.portfolioId(), 'BTC@IMEX');
```

该方法会调用 `host.trading.api.getExecutions`。其他市场成交始终从外部传给控件，以便多个面板共享同一个连接。

## 公共方法

- `setActiveSymbol(symbol)` — 设置主要交易品种。
- `setTrades(...)`、`addTrade(...)` — 替换或补充市场成交明细。
- `loadMyTrades(portfolioId, symbol)` — 加载自有成交。
- `addExtraSymbol`、`removeExtraSymbol`、`getExtraSymbols` — 管理固定交易品种。
- `dispose()` — 移除订阅并释放资源。

## 另请参阅

- [JavaScript 交易控件](../javascript_trading_controls.md)
- [成交历史](trade_history.md)
- [订单簿](order_book.md)
