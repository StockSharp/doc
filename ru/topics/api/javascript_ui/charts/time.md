# Время и торговые сессии

`TradingCalendar` разворачивает расписание биржи в конкретные торговые сессии на оси UTC, а остальные части слоя опираются на него: часы бара считают, когда закроется текущая свеча, а форматтер оси готовит подписи меток и перекрестия. Сам слой ничего не рисует — он отвечает на вопросы «идут ли сейчас торги», «когда откроется следующая сессия» и «сколько осталось до закрытия бара».

## Подключение

Точка входа — `@stocksharp/chart/time`; те же имена доступны и из корня пакета `@stocksharp/chart`. Всё время в API — Unix-время в секундах, интервалы полуоткрытые: `[openTime, closeTime)`.

```ts
import {
  TradingCalendar,
  TradingSessionKind,
  type TradingSchedule,
} from '@stocksharp/chart/time';
```

## Создание календаря

Расписание описывает часовой пояс биржи (IANA), недельные правила сессий и, при необходимости, полностью выходные даты и замены расписания на конкретный день. Время в правилах — локальное для биржи; календарь сам переводит его в UTC с учётом переходов на летнее время.

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
      date: '2026-11-27',                                  // сокращённый день после Дня благодарения
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

calendar.isTradingTime(now, [TradingSessionKind.Regular]);   // идут ли основные торги
calendar.sessionAt(now);                                     // текущая сессия или null
calendar.nextSession(now, [TradingSessionKind.Regular]);     // ближайшее открытие
calendar.sessionsInRange({ from: now - 86_400, to: now });   // всё, что попало в сутки
```

Конструктор проверяет и нормализует расписание сразу: часовой пояс должен быть распознан `Intl`, даты — записаны как `YYYY-MM-DD`, идентификаторы сессий — быть уникальными, а сами сессии — не пересекаться ни внутри недели, ни после разворота в UTC. Продолжительность сессии должна быть положительной и не превышать 24 часов, а одна и та же дата не может быть одновременно выходным днём и заменой. Нарушение любого условия приводит к `TypeError` или `RangeError` ещё на этапе создания календаря. Разобранные локальные даты кэшируются, поэтому повторные запросы по одному диапазону не пересчитываются.

## Расписание и сессии

Правило `TradingSessionRule` — это шаблон `TradingSessionTemplate` плюс дни недели в нумерации ISO (`1` — понедельник, `7` — воскресенье). День недели относится к дате открытия сессии; для ночной сессии, закрывающейся уже на следующий локальный день, задаётся `closeDayOffset: 1`.

`holidays` полностью закрывают дату — на неё не разворачивается ни одно правило. `overrides` заменяют на указанную дату **все** повторяющиеся сессии перечисленным набором, что удобно для сокращённых дней.

Каждый запрос возвращает материализованные `TradingSession`: `id` вида `<дата>/<id правила>`, `ruleId`, `kind`, торговую дату `tradingDate`, границы `openTime` и `closeTime` в UTC и признак `isOverride`. Необязательный параметр `kinds` во всех методах фильтрует результат по типам сессий (`pre-market`, `regular`, `post-market`).

Переходы на летнее время учитываются явно: открытие в неоднозначный (повторяющийся) час берётся по раннему смещению, закрытие — по позднему, а локальное время, попавшее в пропущенный час весеннего перевода, сдвигается к первому реально существующему моменту. Поиск в `nextSession` и `previousSession` ограничен десятью годами от переданной точки; если сессия не найдена, возвращается `null`.

## Обратный отсчёт бара

`resolveTradingBarBounds` определяет границы бара по времени его открытия, а `calculateBarCountdown` строит по ним снимок обратного отсчёта. Текущее время передаёт вызывающая сторона — своего таймера слой не заводит, поэтому результат детерминирован и пригоден для тестов.

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

Таймфрейм записывается как число с суффиксом `s`, `m`, `h`, `d` или `w` (`30s`, `5m`, `1h`, `1d`, `1w`); без суффикса значение считается минутами, календарные месяцы не поддерживаются. Без календаря закрытие бара — это просто открытие плюс длительность таймфрейма. С календарём внутридневной бар обрезается закрытием своей сессии, дневные бары шагают по торговым датам, а недельные — по календарным неделям ISO. Если время открытия не попадает ни в одну сессию, обе функции возвращают `null`.

Параметр `sessionKinds` задаёт, какие сессии учитывать; по умолчанию берутся только основные (`regular`), а без `calendar` он вызывает ошибку. `BarCountdown` содержит состояние `pending` / `open` / `closed`, время `now`, границы `bounds`, а также `untilOpenSeconds`, `elapsedSeconds`, `remainingSeconds` и долю пройденного бара `progress`.

## Подписи оси времени

`TimeAxisFormatter` — кэширующая обёртка над `Intl.DateTimeFormat`, общая для меток шкалы и подписи перекрестия.

```ts
import { TimeAxisFormatter } from '@stocksharp/chart/time';

const formatter = new TimeAxisFormatter({
  locale: 'ru-RU',
  timeZone: 'Europe/Moscow',
  timeVisible: true,
  secondsVisible: false,
});

formatter.formatTick(now, 3_600);   // подпись метки при шаге в час
formatter.formatCrosshair(now);     // подпись под перекрестием
```

Шаг сетки в секундах выбирает детализацию подписи: год, месяц, день или время суток. Секунды появляются только при `secondsVisible` и шаге меньше минуты. По умолчанию используется локаль `en-GB` и часовой пояс `UTC`; оба значения нормализуются через `Intl` и доступны как свойства `locale` и `timeZone`.

Необязательный `formatter` перехватывает обе подписи и получает время и контекст `TimeScaleFormatContext` — вид метки (`tick` или `crosshair`), локаль, часовой пояс, флаги `timeVisible` и `secondsVisible`, а также шаг `tickStep` (для перекрестия он равен `null`). Если функция вернула не строку или выбросила исключение, применяется встроенный формат.

## Подключение к графику

График принимает календарь напрямую: режим шкалы `session-aware` сжимает ось до торгового времени, а примитив `SessionShading` подсвечивает сессии фоном панели.

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

Режим `session-aware` без календаря не создаётся — это ошибка конфигурации. Если `timeScale.timeZone` не задан, берётся часовой пояс календаря, а при его отсутствии — `UTC`. Опущенный `sessionKinds` означает, что на оси остаются сессии всех типов.

## Публичные методы

`TradingCalendar` реализует `ITradingCalendar`:

- `schedule()` — нормализованное расписание.
- `sessionsInRange(range, kinds?)` — сессии, пересекающие диапазон, по возрастанию времени открытия.
- `sessionAt(time, kinds?)` — сессия, содержащая момент времени, либо `null`.
- `isTradingTime(time, kinds?)` — идут ли торги в этот момент.
- `nextSession(time, kinds?)` — ближайшая сессия, открывающаяся не раньше указанного момента.
- `previousSession(time, kinds?)` — последняя сессия, закрывшаяся не позже указанного момента.

Функции часов бара:

- `resolveTradingBarBounds(barOpenTime, resolution, options?)` — границы бара.
- `calculateBarCountdown(barOpenTime, resolution, now, options?)` — снимок обратного отсчёта.

`TimeAxisFormatter`:

- `formatTick(time, step)` — подпись метки шкалы для заданного шага.
- `formatCrosshair(time)` — подпись перекрестия.
- `locale` и `timeZone` — нормализованные значения, доступные только на чтение.

Остальные экспорты слоя — типы и наборы констант: `TradingSessionKind`, `BarClockState`, `TimeScaleLabelKind`, `ITradingCalendar`, `TradingSchedule`, `TradingSessionRule`, `TradingSessionTemplate`, `TradingDayOverride`, `TradingSession`, `BarClockOptions`, `TradingBarBounds`, `BarCountdown`, `TimeAxisFormatterOptions`, `TimeScaleFormatter`, `TimeScaleFormatContext`, `IsoWeekday`, `LocalDate`, `LocalTimeOfDay`.

## Смотрите также

- [JavaScript-графики](../charts.md)
- [Догрузка истории](backfill.md)
- [Свечи](candlestick.md)
- [Индикаторы](indicators.md)
