# Time and trading sessions

`TradingCalendar` expands an exchange schedule into concrete trading sessions on the UTC axis, and the rest of the layer builds on it: the bar clock works out when the current candle closes, and the axis formatter prepares the tick and crosshair labels. The layer itself draws nothing — it answers the questions "is trading on right now", "when does the next session open", and "how long is left until the bar closes".

## Wiring in

The entry point is `@stocksharp/chart/time`; the same names are available from the `@stocksharp/chart` root as well. All time in the API is Unix time in seconds, and the intervals are half-open: `[openTime, closeTime)`.

```ts
import {
  TradingCalendar,
  TradingSessionKind,
  type TradingSchedule,
} from '@stocksharp/chart/time';
```

## Creating a calendar

A schedule describes the exchange time zone (IANA), the weekly session rules and, where needed, the fully closed dates and the schedule replacements for a particular day. The time in the rules is local to the exchange; the calendar converts it into UTC itself, allowing for the daylight saving transitions.

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
      date: '2026-11-27',                                  // the short day after Thanksgiving
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

calendar.isTradingTime(now, [TradingSessionKind.Regular]);   // is the regular session on
calendar.sessionAt(now);                                     // the current session or null
calendar.nextSession(now, [TradingSessionKind.Regular]);     // the nearest opening
calendar.sessionsInRange({ from: now - 86_400, to: now });   // everything that fell within the day
```

The constructor validates and normalizes the schedule right away: the time zone must be recognized by `Intl`, the dates must be written as `YYYY-MM-DD`, the session identifiers must be unique, and the sessions themselves must not overlap, neither within the week nor after being expanded into UTC. The duration of a session must be positive and not exceed 24 hours, and one and the same date cannot be both a holiday and an override. Breaking any of these conditions leads to a `TypeError` or a `RangeError` while the calendar is still being created. Parsed local dates are cached, so repeated queries over the same range are not recomputed.

## Schedule and sessions

A `TradingSessionRule` is a `TradingSessionTemplate` plus the weekdays in ISO numbering (`1` is Monday, `7` is Sunday). The weekday refers to the date the session opens on; for a night session that closes on the next local day, `closeDayOffset: 1` is set.

`holidays` close a date completely — not a single rule is expanded onto it. `overrides` replace **all** the recurring sessions on the given date with the listed set, which is convenient for short days.

Every query returns materialized `TradingSession` objects: an `id` of the form `<date>/<rule id>`, `ruleId`, `kind`, the trading date `tradingDate`, the `openTime` and `closeTime` bounds in UTC, and the `isOverride` flag. The optional `kinds` parameter of every method filters the result by session kind (`pre-market`, `regular`, `post-market`).

Daylight saving transitions are handled explicitly: an opening in an ambiguous (repeated) hour is taken at the earlier offset and a closing at the later one, while a local time that falls into the skipped hour of the spring transition is moved to the first moment that really exists. The search in `nextSession` and `previousSession` is limited to ten years from the given point; when no session is found, `null` is returned.

## Bar countdown

`resolveTradingBarBounds` works out the bounds of a bar from its open time, and `calculateBarCountdown` builds a countdown snapshot from them. The current time is supplied by the caller — the layer starts no timer of its own, so the result is deterministic and suitable for tests.

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

A timeframe is written as a number with an `s`, `m`, `h`, `d`, or `w` suffix (`30s`, `5m`, `1h`, `1d`, `1w`); without a suffix the value counts as minutes, and calendar months are not supported. Without a calendar, a bar closes simply at its open plus the timeframe duration. With a calendar, an intraday bar is trimmed by the close of its session, daily bars step over trading dates, and weekly ones over ISO calendar weeks. If the open time falls into no session, both functions return `null`.

The `sessionKinds` parameter sets which sessions to count; by default only the regular ones (`regular`) are taken, and without `calendar` it raises an error. `BarCountdown` carries the `pending` / `open` / `closed` state, the `now` time, the `bounds`, and also `untilOpenSeconds`, `elapsedSeconds`, `remainingSeconds`, and the fraction of the bar elapsed, `progress`.

## Time axis labels

`TimeAxisFormatter` is a caching wrapper over `Intl.DateTimeFormat`, shared by the scale ticks and the crosshair label.

```ts
import { TimeAxisFormatter } from '@stocksharp/chart/time';

const formatter = new TimeAxisFormatter({
  locale: 'en-GB',
  timeZone: 'Europe/London',
  timeVisible: true,
  secondsVisible: false,
});

formatter.formatTick(now, 3_600);   // the tick label at an hourly step
formatter.formatCrosshair(now);     // the label under the crosshair
```

The grid step in seconds chooses the detail of the label: year, month, day, or time of day. Seconds appear only with `secondsVisible` and a step below a minute. By default the `en-GB` locale and the `UTC` time zone are used; both values are normalized through `Intl` and available as the `locale` and `timeZone` properties.

The optional `formatter` intercepts both labels and receives the time and a `TimeScaleFormatContext` context — the label kind (`tick` or `crosshair`), the locale, the time zone, the `timeVisible` and `secondsVisible` flags, and the `tickStep` step (which is `null` for the crosshair). If the function returns something other than a string or throws, the built-in format is used.

## Wiring into the chart

The chart takes a calendar directly: the `session-aware` scale mode squeezes the axis down to trading time, and the `SessionShading` primitive highlights the sessions with the pane background.

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

The `session-aware` mode is not created without a calendar — that is a configuration error. When `timeScale.timeZone` is not set, the calendar time zone is taken, and in its absence `UTC`. An omitted `sessionKinds` means that sessions of every kind stay on the axis.

## Public methods

`TradingCalendar` implements `ITradingCalendar`:

- `schedule()` — the normalized schedule.
- `sessionsInRange(range, kinds?)` — the sessions crossing the range, by ascending open time.
- `sessionAt(time, kinds?)` — the session containing the moment, or `null`.
- `isTradingTime(time, kinds?)` — whether trading is on at that moment.
- `nextSession(time, kinds?)` — the nearest session that opens no earlier than the given moment.
- `previousSession(time, kinds?)` — the last session that closed no later than the given moment.

The bar clock functions:

- `resolveTradingBarBounds(barOpenTime, resolution, options?)` — the bar bounds.
- `calculateBarCountdown(barOpenTime, resolution, now, options?)` — the countdown snapshot.

`TimeAxisFormatter`:

- `formatTick(time, step)` — the scale tick label for the given step.
- `formatCrosshair(time)` — the crosshair label.
- `locale` and `timeZone` — the normalized values, available read-only.

The other exports of the layer are types and constant sets: `TradingSessionKind`, `BarClockState`, `TimeScaleLabelKind`, `ITradingCalendar`, `TradingSchedule`, `TradingSessionRule`, `TradingSessionTemplate`, `TradingDayOverride`, `TradingSession`, `BarClockOptions`, `TradingBarBounds`, `BarCountdown`, `TimeAxisFormatterOptions`, `TimeScaleFormatter`, `TimeScaleFormatContext`, `IsoWeekday`, `LocalDate`, `LocalTimeOfDay`.

## See also

- [JavaScript charts](../charts.md)
- [History backfill](backfill.md)
- [Candlestick](candlestick.md)
- [Indicators](indicators.md)
