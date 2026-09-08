# 期权 T 型报价表

`OptionDeskWidget` 显示期权链中的一个系列：左侧是看涨，右侧对称显示看跌，中间是行权价和内在价值。成交量、未平仓合约和波动率不仅以数字输出，还以色条呈现，因此这条链可以通过形状来阅读，而不只是数值。

![以行权价为中心、两侧为看涨和看跌的期权 T 型报价表](../../../../images/javascript_controls_option_desk.png)

## 创建和更新

```ts
import {
  OptionDeskWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const desk = OptionDeskWidget.create(
  document.querySelector<HTMLElement>('#option-desk')!,
  {},
  { host },
);

desk.update(
  [{
    strike: 68_000,
    call: {
      symbol: 'BTC-68000-C',
      bid: 1_240,
      ask: 1_265,
      last: 1_250,
      theoretical: 1_248,
      volume: 320,
      openInterest: 1_480,
      ivBid: 0.42,
      ivAsk: 0.44,
      ivLast: 0.43,
      historicalVolatility: 0.39,
    },
    put: {
      symbol: 'BTC-68000-P',
      bid: 820,
      ask: 845,
      volume: 210,
      openInterest: 960,
      ivLast: 0.47,
    },
  }],
  {
    assetPrice: 68_420,
    timeToExpiry: 0.08,
    riskFree: 0.05,
    dividend: 0,
  },
);
```

唯一的依赖是 `host`；该控件没有可选依赖。`create` 的第二个参数是面板状态，控件并不使用它。

`update(strikes, context)` 会替换整条链。两个参数一起传入：期权链和标的资产价格是同一次观测，分开更新会显示出按已经移动的价格计算出的希腊字母。`context` 是可选的，默认为空。

## 期权链上下文

`OptionChainContext` 描述该系列是相对于什么进行估值的：`assetPrice` 是标的资产价格，`timeToExpiry` 是以年为单位的到期时间，`riskFree` 和 `dividend` 是以小数表示的利率（`0.05` 即百分之五）。没有 `assetPrice` 时，报价表仍然显示行情，但内在价值为零，行也不再区分“价内”和“价外”。缺少 `assetPrice` 或 `timeToExpiry` 时不计算希腊字母，单元格保持为空，而不是为零。

## 希腊字母

希腊字母通过两种方式之一获得。如果宿主自己计算，它就在行权价的对应一侧传入现成的 `greeks` 对象，报价表显示收到的内容。如果宿主发送的是波动率，则按 Black–Scholes 就地计算，取值顺序为 `ivLast`、`ivBid`、`ivAsk`、`historicalVolatility` 中第一个可用的值。两种方式互不为对方的备选——它们是宿主的两种形态。

小数位数根据数据自动选择：以列中最小值的四位有效数字为准，但不少于两位、不多于八位。整列共用一个位数，并且两侧通用，因此 delta 不会变成 `0.0000`，而 gamma 及其镜像列的写法也保持一致。

## 列与展示

列的顺序自行权价向外排布：波动率和行情靠近中间，希腊字母在两端；看跌一侧则以相反顺序重复同样的内容。按行权价升序排序：这条链像阶梯一样被阅读。

默认隐藏的列有 `callRho`、`callTheta`、`callHv`、`callTheor` 及其镜像列 `putRho`、`putTheta`、`putHv`、`putTheor`——可通过表格上下文菜单让它们重新出现。

色条的缩放方式各不相同。成交量和未平仓合约按两侧分别缩放，因为看涨和看跌的成交规模不同。波动率则用同一把标尺覆盖两侧，否则两侧之间的偏斜就看不出来了。色条按元素宽度绘制，不使用 canvas。

行权价低于标的价格的行被赋予 `option-itm-call` 类，其余行为 `option-itm-put`；缺少 `assetPrice` 时只有 `option-row`。波动率以百分比输出并保留两位小数，价格采用本包通用的价格格式。

报价表不保存设置：它不访问 `host.preferences` 和 `host.cache`，列的组合与排序只存在于当前实例中。

## 宿主做什么

全部可见文本都取自 `host.t`——面板标题、列标题、空表提示、上下文菜单项。关闭按钮调用 `host.close()`：面板不会自行删除。创建时控件调用 `host.register(this)`，`dispose()` 时调用 `host.unregister(this)`。侧栏上的按钮把期权链导出为 XLSX。

控件不订阅数据，也不发送订单：期权链和上下文由宿主通过 `update` 方法传入。

## 公共方法

- `OptionDeskWidget.create(hostEl, state, deps)` — 构建面板并把它添加到容器中。
- `update(strikes, context)` — 替换期权链和估值上下文。
- `rows()` — 按报价表内部保存的形式返回数据行：包含已计算好的色条刻度和内在价值。
- `dispose()` — 释放资源并在宿主中注销。
- `OptionDeskWidget.TYPE` — 控件类型标识符，`ControlTypes.OptionDesk`。

## 导出的函数

计算部分可以脱离面板单独使用：

- `scaleChain(strikes, context)` — 一次遍历期权链，计算色条的最大值和每个行权价的内在价值。
- `sideGreeks(row, which, context)` — 行权价某一侧的希腊字母：宿主传入的，或由其波动率计算得到的；两者都不可行时返回 `null`。
- `greekPlaces(values)` — 某列数值的小数位数。
- `greekScales(rows, context)` — 每个希腊字母的位数，按期权链两侧一并测量。

## 另请参阅

- [JavaScript 交易控件](../trading_controls.md)
- [自选交易品种列表](watchlist.md)
- [订单簿](order_book.md)
- [持仓](positions.md)
