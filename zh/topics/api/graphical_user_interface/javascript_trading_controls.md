# JavaScript 交易控件

[StockSharp JavaScript 交易控件](https://github.com/StockSharp/JS-TradingControls) 是一套面向交易终端的浏览器面板。该包以 [`@stocksharp/trading-controls`](https://www.npmjs.com/package/@stocksharp/trading-controls) 的名称发布在 npm 上，可在[在线演示](https://stocksharp.github.io/JS-TradingControls/demo/)中查看所有控件。

![包含成交明细、订单簿、自选交易品种列表、订单输入和表格的交易界面](../../../images/javascript_trading_controls.jpg)

截图中还显示了来自独立 `@stocksharp/chart` 包的K线图。`@stocksharp/trading-controls` 包含七个独立控件：

| 控件 | 类 | 标识符 |
|---|---|---|
| [活动订单](javascript_trading_controls/active_orders.md) | `ActiveOrdersWidget` | `activeOrders` |
| [持仓](javascript_trading_controls/positions.md) | `PositionsWidget` | `positions` |
| [成交历史](javascript_trading_controls/trade_history.md) | `TradeHistoryWidget` | `tradeHistory` |
| [自选交易品种列表](javascript_trading_controls/watchlist.md) | `WatchlistWidget` | `watchlist` |
| [订单输入](javascript_trading_controls/order_entry.md) | `OrderEntryWidget` | `orderEntry` |
| [订单簿](javascript_trading_controls/order_book.md) | `OrderBookWidget` | `orderbook` |
| [成交明细](javascript_trading_controls/trade_feed.md) | `TradeFeedWidget` | `tradefeed` |

这些标识符的值可通过导出的 `ControlTypes` 对象获取。

## 安装

```bash
npm install @stocksharp/trading-controls
```

必须引入基础样式。也可以额外引入现成的浅色和深色配色方案，或者用自己的 `--t-*` CSS 变量进行替换：

```ts
import '@stocksharp/trading-controls/styles.css';
import '@stocksharp/trading-controls/theme.css'; // 可选：现成主题。
```

这些控件使用 [Bootstrap Icons](https://icons.getbootstrap.com/) 类，但不自带字体和 SVG。宿主页面必须单独引入图标。

不使用打包工具的页面可使用 `dist/sstradingcontrols.js` 文件，它会创建全局对象 `window.SSTradingControls`。

## 通用创建方式

每个控件都通过静态 `create` 方法创建。该方法会检查宿主、构建自己的 DOM，并将根元素添加到传入的容器中：

```ts
import {
  PositionsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const positions = PositionsWidget.create(
  document.querySelector<HTMLElement>('#positions')!,
  {},
  {
    host,
    closePosition: (portfolioId, instrumentId, symbol) =>
      console.log('平仓', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('反向开仓', portfolioId, instrumentId, symbol),
    refreshPositions: () => console.log('刷新'),
  },
);

positions.update([]);
```

第二个参数是实例保存的状态。第三个参数中的依赖项因控件而异：例如，持仓面板接收平仓和反向开仓处理程序，订单簿则接收价格选择和执行处理程序。

## TradingHost 契约

控件不会直接访问全局翻译器、设置存储、交易连接或窗口管理器。所有外部交互都通过一个 `TradingHost` 对象完成。

| 宿主成员 | 用途 |
|---|---|
| `isPrimary` | 指示页面上的主要控件实例。 |
| `t(key, ...args)` | 翻译可见文本并代入参数。 |
| `presentation` | 格式化方向、订单类型和状态、盈亏类以及 canvas 配色。 |
| `preferences`, `cache` | 保存长期设置和临时数据。 |
| `trading.api` | 搜索交易品种并加载成交记录。 |
| `trading.marketData` | 管理订阅并提供活动订单。 |
| `trading.portfolioId()` | 返回当前投资组合。 |
| `trading.pickInstrument(...)` | 打开交易品种选择器。 |
| `ticker` | 接收可见交易品种及其行情。 |
| `allow(action)` | 检查某项操作的权限。 |
| `close`, `spawn`, `persistState`, `saveLayout` | 管理面板的生命周期和状态。 |
| `register`, `unregister`, `broadcast` | 注册实例，并在实例之间广播变更。 |
| `log(message)` | 接收诊断消息。 |

所有成员均为必需项。`assertHost` 会在渲染控件前检查嵌套函数，并准确指出缺失的路径。如果应用程序不需要某些功能，可以为必需命令传入有意义的占位实现，例如 `log: console.warn` 或空的 `saveLayout`。

## 本地化和外观

控件的所有可见文本都通过 `host.t` 获取。`@stocksharp/trading-controls/translation-keys.json` 中包含当前完整的 153 个键。未知键会原样显示给用户，因此宿主应为整个列表提供翻译。

`styles.css` 文件包含样式规则，但颜色、字体和尺寸取自 `--t-*` CSS 变量。如果不使用现成的 `theme.css`，应用程序需要自行定义这些变量。订单簿和气泡成交明细的 canvas 颜色由 `host.presentation.canvasPalette()` 返回。

## 释放资源

移除面板时请调用 `dispose()`。该方法会移除事件处理程序，在适用时断开具体控件的观察器和订阅，最后调用 `host.unregister`。

```ts
positions.dispose();
```

## 从源代码构建

```bash
git clone https://github.com/StockSharp/JS-TradingControls.git
cd JS-TradingControls
npm install
npm test
npm run build
```

## 另请参阅

- [JavaScript 表格](javascript_grids.md)
- [JavaScript 图表](charts/javascript_charts.md)
- [JS-TradingControls 仓库](https://github.com/StockSharp/JS-TradingControls)
- [在线演示](https://stocksharp.github.io/JS-TradingControls/demo/)
