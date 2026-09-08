# 时间与交易时段

`TradingCalendar` 把交易所的日程展开成 UTC 时间轴上一个个具体的交易时段，该层的其余部分都以它为基础：柱体时钟据此计算当前 K 线何时收盘，坐标轴格式化器据此准备刻度和十字线的标签。该层本身不绘制任何东西——它回答的是“现在是否在交易”“下一个时段何时开始”“距离柱体收盘还有多久”这类问题。

## 引入

入口点是 `@stocksharp/chart/time`；同样的名称也可以从包的根 `@stocksharp/chart` 获取。API 中的所有时间都是以秒为单位的 Unix 时间，区间是半开的：`[openTime, closeTime)`。

```ts
import {
  TradingCalendar,
  TradingSessionKind,
  type TradingSchedule,
} from '@stocksharp/chart/time';
```

## 创建日历

日程描述交易所的时区（IANA）、每周的时段规则，以及必要时的完整休市日期和特定日期的日程替换。规则中的时间是交易所的本地时间；日历会自行把它换算为 UTC，并考虑夏令时切换。

```ts
import {
  TradingCalendar,
  TradingSessionKind,
  type TradingSchedule,
} from '@stocksharp/chart/time';

const schedule: TradingSchedule = {
  id: 'xnys',
  timeZone: 'America/New_York',
  sessions: [
    {
      id: 'pre',
      kind: TradingSessionKind.PreMarket,
      weekdays: [1, 2, 3, 4, 5],
      open: { hour: 4, minute: 0 },
      close: { hour: 9, minute: 30 },
    },
    {
      id: 'regular',
      kind: TradingSessionKind.Regular,
      weekdays: [1, 2, 3, 4, 5],
      open: { hour: 9, minute: 30 },
      close: { hour: 16, minute: 0 },
    },
    {
      id: 'post',
      kind: TradingSessionKind.PostMarket,
      weekdays: [1, 2, 3, 4, 5],
      open: { hour: 16, minute: 0 },
      close: { hour: 20, minute: 0 },
    },
  ],
  holidays: ['2026-01-01', '2026-07-03'],
  overrides: [
    {
      date: '2026-11-27',                                  // 感恩节之后的半日市
      sessions: [{
        id: 'regular',
        kind: TradingSessionKind.Regular,
        open: { hour: 9, minute: 30 },
        close: { hour: 13, minute: 0 },
      }],
    },
  ],
};

const calendar = new TradingCalendar(schedule);
const now = Math.floor(Date.now() / 1000);

calendar.isTradingTime(now, [TradingSessionKind.Regular]);   // 主交易时段是否正在进行
calendar.sessionAt(now);                                     // 当前时段，没有则为 null
calendar.nextSession(now, [TradingSessionKind.Regular]);     // 最近一次开盘
calendar.sessionsInRange({ from: now - 86_400, to: now });   // 这一天内的所有时段
```

构造函数会立即校验并规范化日程：时区必须能被 `Intl` 识别，日期必须写成 `YYYY-MM-DD`，时段标识符必须唯一，而时段本身无论在一周之内还是展开到 UTC 之后都不得重叠。时段的时长必须为正且不超过 24 小时，同一个日期不能既是休市日又是替换日。违反任何一条都会在创建日历时就抛出 `TypeError` 或 `RangeError`。解析过的本地日期会被缓存，因此对同一区间的重复查询不会重新计算。

## 日程与时段

规则 `TradingSessionRule` 就是模板 `TradingSessionTemplate` 加上按 ISO 编号的星期几（`1` 为星期一，`7` 为星期日）。星期几针对的是时段的开始日期；对于在下一个本地日才收盘的夜盘时段，需要设置 `closeDayOffset: 1`。

`holidays` 会完全封闭某个日期——任何规则都不会展开到这一天。`overrides` 会在指定日期用所列的一组时段替换**全部**重复性时段，这对半日市很方便。

每次查询返回的都是已实例化的 `TradingSession`：形如 `<日期>/<规则 id>` 的 `id`、`ruleId`、`kind`、交易日期 `tradingDate`、以 UTC 表示的边界 `openTime` 和 `closeTime`，以及标志 `isOverride`。所有方法中的可选参数 `kinds` 都会按时段类型（`pre-market`、`regular`、`post-market`）过滤结果。

夏令时切换被显式处理：落在含糊（重复出现）小时的开盘取较早的偏移，收盘取较晚的偏移，而落在春季调时被跳过的那一小时里的本地时间，会被移到第一个真实存在的时刻。`nextSession` 和 `previousSession` 的搜索被限制在所给时刻起十年之内；找不到时段时返回 `null`。

## 柱体倒计时

`resolveTradingBarBounds` 按柱体的开始时间确定它的边界，而 `calculateBarCountdown` 据此构建倒计时快照。当前时间由调用方传入——该层不自建计时器，因此结果是确定性的，适合用于测试。

```ts
import {
  BarClockState,
  calculateBarCountdown,
  resolveTradingBarBounds,
} from '@stocksharp/chart/time';

const bounds = resolveTradingBarBounds(barOpenTime, '15m', { calendar });
// bounds: { resolution, intervalSeconds, openTime, closeTime, durationSeconds, session }

const countdown = calculateBarCountdown(
  barOpenTime,
  '15m',
  Math.floor(Date.now() / 1000),
  { calendar },
);

if (countdown !== null && countdown.state === BarClockState.Open) {
  console.log(countdown.remainingSeconds, countdown.progress);
}
```

时间框架写成一个数字加上后缀 `s`、`m`、`h`、`d` 或 `w`（`30s`、`5m`、`1h`、`1d`、`1w`）；没有后缀时数值按分钟计算，不支持自然月。没有日历时，柱体的收盘时间就是开始时间加上时间框架的时长。有日历时，日内柱体会被自己所属时段的收盘时间截断，日线柱体按交易日推进，而周线柱体按 ISO 日历周推进。如果开始时间不落在任何时段内，这两个函数都返回 `null`。

参数 `sessionKinds` 指定要考虑哪些时段；默认只取主时段（`regular`），而在没有 `calendar` 时使用它会报错。`BarCountdown` 包含状态 `pending` / `open` / `closed`、时间 `now`、边界 `bounds`，以及 `untilOpenSeconds`、`elapsedSeconds`、`remainingSeconds` 和柱体已完成的比例 `progress`。

## 时间轴标签

`TimeAxisFormatter` 是对 `Intl.DateTimeFormat` 的带缓存封装，刻度标签和十字线标签共用它。

```ts
import { TimeAxisFormatter } from '@stocksharp/chart/time';

const formatter = new TimeAxisFormatter({
  locale: 'zh-CN',
  timeZone: 'Asia/Shanghai',
  timeVisible: true,
  secondsVisible: false,
});

formatter.formatTick(now, 3_600);   // 步长为一小时时的刻度标签
formatter.formatCrosshair(now);     // 十字线下方的标签
```

以秒为单位的网格步长决定标签的详细程度：年、月、日或一天中的时间。只有在 `secondsVisible` 为真且步长小于一分钟时才会出现秒。默认使用 `en-GB` 区域设置和 `UTC` 时区；这两个值都会通过 `Intl` 规范化，并可通过属性 `locale` 和 `timeZone` 获取。

可选的 `formatter` 会接管这两种标签，并收到时间和上下文 `TimeScaleFormatContext`——标签的种类（`tick` 或 `crosshair`）、区域设置、时区、标志 `timeVisible` 和 `secondsVisible`，以及步长 `tickStep`（对十字线而言它等于 `null`）。如果该函数返回的不是字符串或抛出了异常，就使用内置格式。

## 接入图表

图表可以直接接受日历：刻度模式 `session-aware` 会把坐标轴压缩到交易时间，而原语 `SessionShading` 会用面板背景高亮各时段。

```ts
import { createChart, SessionShading, TimeScaleMode } from '@stocksharp/chart';
import { TradingSessionKind } from '@stocksharp/chart/time';

const chart = createChart(document.getElementById('chart')!, {
  timeScale: {
    mode: TimeScaleMode.SessionAware,
    calendar,
    sessionKinds: [TradingSessionKind.Regular],
    timeVisible: true,
  },
});

chart.attachPrimitive(new SessionShading({ calendar }));
```

没有日历就无法创建 `session-aware` 模式——这属于配置错误。如果没有指定 `timeScale.timeZone`，就取日历的时区，日历也没有时则取 `UTC`。省略 `sessionKinds` 表示坐标轴上保留所有类型的时段。

## 公共方法

`TradingCalendar` 实现 `ITradingCalendar`：

- `schedule()` — 规范化之后的日程。
- `sessionsInRange(range, kinds?)` — 与该区间相交的时段，按开始时间升序。
- `sessionAt(time, kinds?)` — 包含该时刻的时段，否则为 `null`。
- `isTradingTime(time, kinds?)` — 该时刻是否正在交易。
- `nextSession(time, kinds?)` — 不早于指定时刻开始的最近一个时段。
- `previousSession(time, kinds?)` — 不晚于指定时刻结束的最后一个时段。

柱体时钟的函数：

- `resolveTradingBarBounds(barOpenTime, resolution, options?)` — 柱体的边界。
- `calculateBarCountdown(barOpenTime, resolution, now, options?)` — 倒计时快照。

`TimeAxisFormatter`：

- `formatTick(time, step)` — 给定步长下的刻度标签。
- `formatCrosshair(time)` — 十字线标签。
- `locale` 和 `timeZone` — 规范化之后的值，只读。

该层的其余导出是类型和常量集合：`TradingSessionKind`、`BarClockState`、`TimeScaleLabelKind`、`ITradingCalendar`、`TradingSchedule`、`TradingSessionRule`、`TradingSessionTemplate`、`TradingDayOverride`、`TradingSession`、`BarClockOptions`、`TradingBarBounds`、`BarCountdown`、`TimeAxisFormatterOptions`、`TimeScaleFormatter`、`TimeScaleFormatContext`、`IsoWeekday`、`LocalDate`、`LocalTimeOfDay`。

## 另请参阅

- [JavaScript 图表](../charts.md)
- [历史数据回填](backfill.md)
- [K线图](candlestick.md)
- [指标](indicators.md)
