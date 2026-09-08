# Zeit und Handelssessions

`TradingCalendar` entfaltet den Zeitplan einer Börse zu konkreten Handelssessions auf der UTC-Achse, und die übrigen Teile der Schicht stützen sich darauf: Die Balkenuhr berechnet, wann die aktuelle Kerze schließt, und der Achsenformatierer bereitet die Beschriftungen der Marken und des Fadenkreuzes auf. Die Schicht selbst zeichnet nichts — sie beantwortet die Fragen „wird gerade gehandelt“, „wann öffnet die nächste Session“ und „wie lange dauert es bis zum Schluss des Balkens“.

## Einbinden

Der Einstiegspunkt ist `@stocksharp/chart/time`; dieselben Namen stehen auch aus der Wurzel des Pakets `@stocksharp/chart` bereit. Alle Zeiten in der API sind Unix-Zeit in Sekunden, die Intervalle sind halboffen: `[openTime, closeTime)`.

```ts
import {
  TradingCalendar,
  TradingSessionKind,
  type TradingSchedule,
} from '@stocksharp/chart/time';
```

## Kalender erstellen

Der Zeitplan beschreibt die Zeitzone der Börse (IANA), die wöchentlichen Session-Regeln und bei Bedarf vollständig freie Tage sowie Ersetzungen des Zeitplans für einen bestimmten Tag. Die Zeiten in den Regeln sind lokal für die Börse; der Kalender rechnet sie unter Berücksichtigung der Sommerzeitumstellungen selbst in UTC um.

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
      date: '2026-11-27',                                  // verkürzter Tag nach Thanksgiving
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

calendar.isTradingTime(now, [TradingSessionKind.Regular]);   // läuft der Haupthandel
calendar.sessionAt(now);                                     // die aktuelle Session oder null
calendar.nextSession(now, [TradingSessionKind.Regular]);     // die nächste Eröffnung
calendar.sessionsInRange({ from: now - 86_400, to: now });   // alles, was in den Tag fällt
```

Der Konstruktor prüft und normalisiert den Zeitplan sofort: Die Zeitzone muss von `Intl` erkannt werden, die Daten müssen als `YYYY-MM-DD` geschrieben sein, die Session-Kennungen eindeutig sein, und die Sessions selbst dürfen sich weder innerhalb der Woche noch nach der Entfaltung in UTC überschneiden. Die Dauer einer Session muss positiv sein und darf 24 Stunden nicht überschreiten, und dasselbe Datum kann nicht gleichzeitig freier Tag und Ersetzung sein. Die Verletzung einer dieser Bedingungen führt bereits beim Erstellen des Kalenders zu einem `TypeError` oder `RangeError`. Die zerlegten lokalen Daten werden zwischengespeichert, deshalb werden wiederholte Abfragen zu demselben Bereich nicht neu berechnet.

## Zeitplan und Sessions

Die Regel `TradingSessionRule` ist die Vorlage `TradingSessionTemplate` zuzüglich der Wochentage in ISO-Nummerierung (`1` — Montag, `7` — Sonntag). Der Wochentag bezieht sich auf das Eröffnungsdatum der Session; für eine Nachtsession, die erst am nächsten lokalen Tag schließt, wird `closeDayOffset: 1` gesetzt.

`holidays` schließen ein Datum vollständig — auf ihm wird keine einzige Regel entfaltet. `overrides` ersetzen an dem angegebenen Datum **alle** wiederkehrenden Sessions durch den aufgeführten Satz, was für verkürzte Tage praktisch ist.

Jede Abfrage gibt materialisierte `TradingSession` zurück: eine `id` der Form `<Datum>/<id der Regel>`, `ruleId`, `kind`, das Handelsdatum `tradingDate`, die Grenzen `openTime` und `closeTime` in UTC und das Kennzeichen `isOverride`. Der optionale Parameter `kinds` filtert in allen Methoden das Ergebnis nach Session-Typen (`pre-market`, `regular`, `post-market`).

Die Sommerzeitumstellungen werden ausdrücklich berücksichtigt: Eine Eröffnung in einer mehrdeutigen (wiederholten) Stunde wird nach dem frühen Versatz genommen, ein Schluss nach dem späten, und eine lokale Zeit, die in die übersprungene Stunde der Frühjahrsumstellung fällt, wird auf den ersten tatsächlich existierenden Moment verschoben. Die Suche in `nextSession` und `previousSession` ist auf zehn Jahre ab dem übergebenen Zeitpunkt begrenzt; wird keine Session gefunden, wird `null` zurückgegeben.

## Countdown des Balkens

`resolveTradingBarBounds` bestimmt die Grenzen eines Balkens anhand seiner Eröffnungszeit, und `calculateBarCountdown` baut daraus eine Momentaufnahme des Countdowns. Die aktuelle Zeit übergibt der Aufrufer — einen eigenen Timer legt die Schicht nicht an, deshalb ist das Ergebnis deterministisch und für Tests geeignet.

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

Der Zeitrahmen wird als Zahl mit dem Suffix `s`, `m`, `h`, `d` oder `w` geschrieben (`30s`, `5m`, `1h`, `1d`, `1w`); ohne Suffix gilt der Wert als Minuten, Kalendermonate werden nicht unterstützt. Ohne Kalender ist der Schluss eines Balkens einfach die Eröffnung plus die Dauer des Zeitrahmens. Mit Kalender wird ein Intraday-Balken durch den Schluss seiner Session beschnitten, Tagesbalken schreiten über Handelsdaten und Wochenbalken über ISO-Kalenderwochen. Fällt die Eröffnungszeit in keine Session, geben beide Funktionen `null` zurück.

Der Parameter `sessionKinds` legt fest, welche Sessions berücksichtigt werden; standardmäßig werden nur die Haupt-Sessions (`regular`) genommen, und ohne `calendar` löst er einen Fehler aus. `BarCountdown` enthält den Zustand `pending` / `open` / `closed`, die Zeit `now`, die Grenzen `bounds` sowie `untilOpenSeconds`, `elapsedSeconds`, `remainingSeconds` und den durchlaufenen Anteil des Balkens `progress`.

## Beschriftungen der Zeitachse

`TimeAxisFormatter` ist eine zwischenspeichernde Hülle um `Intl.DateTimeFormat`, gemeinsam für die Marken der Skala und die Beschriftung des Fadenkreuzes.

```ts
import { TimeAxisFormatter } from '@stocksharp/chart/time';

const formatter = new TimeAxisFormatter({
  locale: 'de-DE',
  timeZone: 'Europe/Berlin',
  timeVisible: true,
  secondsVisible: false,
});

formatter.formatTick(now, 3_600);   // Beschriftung einer Marke bei einem Schritt von einer Stunde
formatter.formatCrosshair(now);     // Beschriftung unter dem Fadenkreuz
```

Der Gitterschritt in Sekunden bestimmt die Detailtiefe der Beschriftung: Jahr, Monat, Tag oder Tageszeit. Sekunden erscheinen nur bei `secondsVisible` und einem Schritt unter einer Minute. Standardmäßig werden die Locale `en-GB` und die Zeitzone `UTC` verwendet; beide Werte werden über `Intl` normalisiert und sind als die Eigenschaften `locale` und `timeZone` verfügbar.

Ein optionaler `formatter` fängt beide Beschriftungen ab und erhält die Zeit und den Kontext `TimeScaleFormatContext` — die Art der Marke (`tick` oder `crosshair`), die Locale, die Zeitzone, die Kennzeichen `timeVisible` und `secondsVisible` sowie den Schritt `tickStep` (für das Fadenkreuz ist er `null`). Gibt die Funktion keine Zeichenfolge zurück oder löst sie eine Ausnahme aus, wird das eingebaute Format angewandt.

## An das Chart anbinden

Das Chart nimmt den Kalender direkt entgegen: Der Skalenmodus `session-aware` staucht die Achse auf die Handelszeit, und das Primitiv `SessionShading` hebt die Sessions als Hintergrund des Panels hervor.

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

Der Modus `session-aware` wird ohne Kalender nicht erstellt — das ist ein Konfigurationsfehler. Ist `timeScale.timeZone` nicht gesetzt, wird die Zeitzone des Kalenders genommen und in deren Abwesenheit `UTC`. Ein weggelassenes `sessionKinds` bedeutet, dass auf der Achse Sessions aller Typen verbleiben.

## Öffentliche Methoden

`TradingCalendar` implementiert `ITradingCalendar`:

- `schedule()` — der normalisierte Zeitplan.
- `sessionsInRange(range, kinds?)` — die Sessions, die den Bereich schneiden, aufsteigend nach Eröffnungszeit.
- `sessionAt(time, kinds?)` — die Session, die einen Zeitpunkt enthält, oder `null`.
- `isTradingTime(time, kinds?)` — ob zu diesem Zeitpunkt gehandelt wird.
- `nextSession(time, kinds?)` — die nächste Session, die nicht früher als der angegebene Zeitpunkt öffnet.
- `previousSession(time, kinds?)` — die letzte Session, die nicht später als der angegebene Zeitpunkt geschlossen hat.

Die Funktionen der Balkenuhr:

- `resolveTradingBarBounds(barOpenTime, resolution, options?)` — die Grenzen eines Balkens.
- `calculateBarCountdown(barOpenTime, resolution, now, options?)` — eine Momentaufnahme des Countdowns.

`TimeAxisFormatter`:

- `formatTick(time, step)` — die Beschriftung einer Skalenmarke für den angegebenen Schritt.
- `formatCrosshair(time)` — die Beschriftung des Fadenkreuzes.
- `locale` und `timeZone` — die normalisierten Werte, nur lesbar.

Die übrigen Exporte der Schicht sind Typen und Konstantensätze: `TradingSessionKind`, `BarClockState`, `TimeScaleLabelKind`, `ITradingCalendar`, `TradingSchedule`, `TradingSessionRule`, `TradingSessionTemplate`, `TradingDayOverride`, `TradingSession`, `BarClockOptions`, `TradingBarBounds`, `BarCountdown`, `TimeAxisFormatterOptions`, `TimeScaleFormatter`, `TimeScaleFormatContext`, `IsoWeekday`, `LocalDate`, `LocalTimeOfDay`.

## Siehe auch

- [JavaScript-Charts](../charts.md)
- [Nachladen der Historie](backfill.md)
- [Kerzenchart](candlestick.md)
- [Indikatoren](indicators.md)
