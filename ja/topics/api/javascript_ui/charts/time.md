# 時間と取引セッション

`TradingCalendar` は、取引所のスケジュールを UTC 軸上の具体的な取引セッションへ展開します。このレイヤーの他の部分はそれに依存しており、バーの時計は現在のローソク足がいつ確定するかを計算し、軸のフォーマッターは目盛りと十字カーソルのラベルを用意します。レイヤー自身は何も描画しません。「今は取引時間か」「次のセッションはいつ始まるか」「バーの確定まであとどれだけか」という問いに答えるだけです。

## 組み込み

エントリーポイントは `@stocksharp/chart/time` です。同じ名前はパッケージのルート `@stocksharp/chart` からも利用できます。API 内の時刻はすべて秒単位の Unix 時刻で、区間は半開区間 `[openTime, closeTime)` です。

```ts
import {
  TradingCalendar,
  TradingSessionKind,
  type TradingSchedule,
} from '@stocksharp/chart/time';
```

## カレンダーの作成

スケジュールには、取引所のタイムゾーン（IANA）、週単位のセッションのルール、そして必要に応じて完全な休業日と特定日のスケジュールの差し替えを記述します。ルール内の時刻は取引所のローカル時刻です。カレンダーが夏時間の切り替えを考慮して自分で UTC に変換します。

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
      date: '2026-11-27',                                  // 感謝祭の翌日の短縮取引日
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

calendar.isTradingTime(now, [TradingSessionKind.Regular]);   // 通常取引が行われているか
calendar.sessionAt(now);                                     // 現在のセッション、なければ null
calendar.nextSession(now, [TradingSessionKind.Regular]);     // 直近の開始
calendar.sessionsInRange({ from: now - 86_400, to: now });   // 24 時間に入るものすべて
```

コンストラクターはスケジュールをその場で検証して正規化します。タイムゾーンは `Intl` が認識できるものであること、日付は `YYYY-MM-DD` で書かれていること、セッションの識別子が一意であること、そしてセッションどうしが週の中でも UTC への展開後も重ならないことが求められます。セッションの長さは正の値で 24 時間を超えてはならず、同じ日付が休業日と差し替えの両方になることはできません。いずれかの条件に反すると、カレンダーの作成の時点で `TypeError` または `RangeError` になります。解析済みのローカル日付はキャッシュされるため、同じ範囲に対する再度の問い合わせが再計算されることはありません。

## スケジュールとセッション

ルール `TradingSessionRule` は、テンプレート `TradingSessionTemplate` に ISO 番号の曜日（`1` が月曜、`7` が日曜）を加えたものです。曜日はセッションの開始日を指します。翌ローカル日にまたがって終了するナイトセッションには、`closeDayOffset: 1` を指定します。

`holidays` はその日付を完全に閉じます。その日にはどのルールも展開されません。`overrides` は指定した日付の**すべて**の定期セッションを、列挙したセットで置き換えます。短縮取引日に便利です。

各問い合わせは、実体化された `TradingSession` を返します。`<日付>/<ルールの id>` 形式の `id`、`ruleId`、`kind`、取引日 `tradingDate`、UTC での境界 `openTime` と `closeTime`、そして `isOverride` のフラグです。すべてのメソッドにある任意の `kinds` パラメーターは、結果をセッションの種類（`pre-market`、`regular`、`post-market`）で絞り込みます。

夏時間の切り替えは明示的に扱われます。あいまいな（繰り返される）時刻での開始は早いほうのオフセットで、終了は遅いほうのオフセットで解釈され、春の切り替えで飛ばされた時刻に入るローカル時刻は、実在する最初の時点へずらされます。`nextSession` と `previousSession` の探索は、渡された時点から 10 年間に制限されます。セッションが見つからない場合は `null` を返します。

## バーのカウントダウン

`resolveTradingBarBounds` はバーの開始時刻からその境界を求め、`calculateBarCountdown` はそれをもとにカウントダウンのスナップショットを組み立てます。現在時刻は呼び出し側が渡します。このレイヤーは自前のタイマーを持たないため、結果は決定的でテストにも適しています。

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

時間足は、数値に `s`、`m`、`h`、`d`、`w` のいずれかの接尾辞を付けて記述します（`30s`、`5m`、`1h`、`1d`、`1w`）。接尾辞がない場合、値は分として扱われます。暦月はサポートされていません。カレンダーがない場合、バーの終了は単に開始に時間足の長さを足したものです。カレンダーがある場合、日中のバーは自分のセッションの終了で切り詰められ、日足は取引日に沿って進み、週足は ISO の暦週に沿って進みます。開始時刻がどのセッションにも入らない場合、どちらの関数も `null` を返します。

`sessionKinds` パラメーターは、どのセッションを考慮するかを指定します。既定では通常セッション（`regular`）のみが対象で、`calendar` がない状態で指定するとエラーになります。`BarCountdown` は、状態 `pending` / `open` / `closed`、時刻 `now`、境界 `bounds`、さらに `untilOpenSeconds`、`elapsedSeconds`、`remainingSeconds`、そしてバーの経過割合 `progress` を持ちます。

## 時間軸のラベル

`TimeAxisFormatter` は `Intl.DateTimeFormat` のキャッシュ付きラッパーで、スケールの目盛りと十字カーソルのラベルで共有されます。

```ts
import { TimeAxisFormatter } from '@stocksharp/chart/time';

const formatter = new TimeAxisFormatter({
  locale: 'ja-JP',
  timeZone: 'Asia/Tokyo',
  timeVisible: true,
  secondsVisible: false,
});

formatter.formatTick(now, 3_600);   // 刻みが 1 時間のときの目盛りラベル
formatter.formatCrosshair(now);     // 十字カーソルの下のラベル
```

秒単位のグリッドの刻みが、ラベルの粒度を決めます。年、月、日、または時刻です。秒が現れるのは `secondsVisible` が有効で、かつ刻みが 1 分未満の場合だけです。既定ではロケール `en-GB`、タイムゾーン `UTC` が使われます。どちらの値も `Intl` を通して正規化され、`locale` と `timeZone` のプロパティから参照できます。

任意の `formatter` は両方のラベルを横取りし、時刻とコンテキスト `TimeScaleFormatContext` を受け取ります。ラベルの種類（`tick` または `crosshair`）、ロケール、タイムゾーン、`timeVisible` と `secondsVisible` のフラグ、そして刻み `tickStep`（十字カーソルの場合は `null`）です。関数が文字列以外を返した場合や例外を投げた場合は、組み込みの書式が使われます。

## チャートへの接続

チャートはカレンダーを直接受け取ります。スケールのモード `session-aware` は軸を取引時間まで圧縮し、プリミティブ `SessionShading` はペインの背景でセッションを強調します。

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

カレンダーなしで `session-aware` モードを作ることはできません。設定の誤りとして扱われます。`timeScale.timeZone` が指定されていない場合はカレンダーのタイムゾーンが使われ、それもない場合は `UTC` になります。`sessionKinds` を省略すると、軸にはすべての種類のセッションが残ります。

## 公開メソッド

`TradingCalendar` は `ITradingCalendar` を実装します。

- `schedule()` — 正規化されたスケジュール。
- `sessionsInRange(range, kinds?)` — その範囲と交差するセッション。開始時刻の昇順です。
- `sessionAt(time, kinds?)` — その時点を含むセッション、なければ `null`。
- `isTradingTime(time, kinds?)` — その時点で取引が行われているかどうか。
- `nextSession(time, kinds?)` — 指定した時点以降に始まる最も近いセッション。
- `previousSession(time, kinds?)` — 指定した時点以前に終了した最後のセッション。

バーの時計の関数:

- `resolveTradingBarBounds(barOpenTime, resolution, options?)` — バーの境界。
- `calculateBarCountdown(barOpenTime, resolution, now, options?)` — カウントダウンのスナップショット。

`TimeAxisFormatter`:

- `formatTick(time, step)` — 指定した刻みに対するスケールの目盛りラベル。
- `formatCrosshair(time)` — 十字カーソルのラベル。
- `locale` と `timeZone` — 正規化された値。読み取り専用です。

このレイヤーの残りのエクスポートは型と定数の集合です。`TradingSessionKind`、`BarClockState`、`TimeScaleLabelKind`、`ITradingCalendar`、`TradingSchedule`、`TradingSessionRule`、`TradingSessionTemplate`、`TradingDayOverride`、`TradingSession`、`BarClockOptions`、`TradingBarBounds`、`BarCountdown`、`TimeAxisFormatterOptions`、`TimeScaleFormatter`、`TimeScaleFormatContext`、`IsoWeekday`、`LocalDate`、`LocalTimeOfDay` です。

## 関連項目

- [JavaScript チャート](../charts.md)
- [履歴のバックフィル](backfill.md)
- [ローソク足](candlestick.md)
- [インジケーター](indicators.md)
