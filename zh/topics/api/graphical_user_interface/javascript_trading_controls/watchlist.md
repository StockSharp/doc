# 自选交易品种列表

`WatchlistWidget` 显示交易品种及其实时行情。该控件支持搜索、收藏、分类、选择当前交易品种，并且只订阅实际可见的代码。

## 创建

```ts
import {
  WatchlistWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let watchlist!: WatchlistWidget;

watchlist = WatchlistWidget.create(
  document.querySelector<HTMLElement>('#watchlist')!,
  {},
  {
    host,
    onSelect: symbol => {
      watchlist.setCurrentSymbol(symbol);
      console.log('已选择', symbol);
    },
  },
);
```

创建时会自动启动 `init()`，它通过 `host.trading.api.searchInstruments('')` 加载列表。每条记录使用 `symbol`、`name`、`exchange` 和 `category` 字段。

价格流从外部传入：

```ts
watchlist.onPriceUpdate('BTC@IMEX', 68_420.5);
```

该方法只更新所需的价格和百分比单元格，同时保留变动动画。`setCurrentSymbol(symbol)` 会突出显示当前交易品种。

## 搜索、分类和订阅

搜索会检查代码、名称和交易所。系统会为全部交易品种、收藏项和检测到的分类创建选项卡。收藏的交易品种代码保存在 `host.preferences` 中。

控件会以 `MarketDataLevels.Quotes` 级别订阅前 30 个可见交易品种。屏幕上最多渲染 300 行，但筛选和导出仍会处理搜索到的完整数据集。只有 `host.isPrimary === true` 的实例才会通过 `host.ticker` 发布可见行情。

涨跌幅以当日 UTC 收到的第一笔价格为基准计算。这些基准价格属于缓存数据，因此保存在 `host.cache` 而非用户设置中。

## 公共方法

- `init()` — 加载交易品种并准备订阅；`create` 会自动调用它。
- `setCurrentSymbol(symbol)` — 标记当前交易品种。
- `onPriceUpdate(symbol, price)` — 应用新价格。
- `dispose()` — 取消订阅并释放资源。

## 另请参阅

- [JavaScript 交易控件](../javascript_trading_controls.md)
- [订单簿](order_book.md)
- [订单输入](order_entry.md)
