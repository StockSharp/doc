# Tiempo y sesiones de negociación

`TradingCalendar` despliega el horario de la bolsa en sesiones de negociación concretas sobre el eje UTC, y el resto de partes de la capa se apoyan en él: el reloj de la barra calcula cuándo se cerrará la vela actual, y el formateador del eje prepara las etiquetas de las marcas y del crosshair. La capa en sí no dibuja nada: responde a las preguntas «¿hay negociación ahora?», «¿cuándo abrirá la siguiente sesión?» y «¿cuánto queda para el cierre de la barra?».

## Conexión

El punto de entrada es `@stocksharp/chart/time`; esos mismos nombres están disponibles también desde la raíz del paquete `@stocksharp/chart`. Todo el tiempo de la API es tiempo Unix en segundos, y los intervalos son semiabiertos: `[openTime, closeTime)`.

```ts
import {
  TradingCalendar,
  TradingSessionKind,
  type TradingSchedule,
} from '@stocksharp/chart/time';
```

## Crear el calendario

El horario describe la zona horaria de la bolsa (IANA), las reglas semanales de sesiones y, si es necesario, las fechas totalmente no hábiles y las sustituciones del horario en un día concreto. La hora de las reglas es local de la bolsa; el calendario la convierte por su cuenta a UTC teniendo en cuenta los cambios de horario de verano.

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
      date: '2026-11-27',                                  // jornada reducida tras el Día de Acción de Gracias
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

calendar.isTradingTime(now, [TradingSessionKind.Regular]);   // si hay negociación principal
calendar.sessionAt(now);                                     // la sesión actual o null
calendar.nextSession(now, [TradingSessionKind.Regular]);     // la apertura más próxima
calendar.sessionsInRange({ from: now - 86_400, to: now });   // todo lo que cae en un día
```

El constructor comprueba y normaliza el horario de inmediato: la zona horaria debe ser reconocida por `Intl`, las fechas deben escribirse como `YYYY-MM-DD`, los identificadores de sesión deben ser únicos y las propias sesiones no deben solaparse ni dentro de la semana ni tras el despliegue a UTC. La duración de una sesión debe ser positiva y no superar las 24 horas, y una misma fecha no puede ser a la vez día no hábil y sustitución. Incumplir cualquiera de estas condiciones provoca un `TypeError` o un `RangeError` ya en la fase de creación del calendario. Las fechas locales analizadas se almacenan en caché, por lo que las consultas repetidas sobre un mismo rango no se recalculan.

## Horario y sesiones

Una regla `TradingSessionRule` es una plantilla `TradingSessionTemplate` más los días de la semana en numeración ISO (`1` es lunes y `7`, domingo). El día de la semana se refiere a la fecha de apertura de la sesión; para una sesión nocturna que cierra ya al día local siguiente se indica `closeDayOffset: 1`.

`holidays` cierra por completo una fecha: en ella no se despliega ninguna regla. `overrides` sustituye en la fecha indicada **todas** las sesiones recurrentes por el conjunto enumerado, lo que resulta cómodo para las jornadas reducidas.

Cada consulta devuelve `TradingSession` materializadas: un `id` con la forma `<fecha>/<id de la regla>`, `ruleId`, `kind`, la fecha de negociación `tradingDate`, los límites `openTime` y `closeTime` en UTC y el indicador `isOverride`. El parámetro opcional `kinds`, presente en todos los métodos, filtra el resultado por tipos de sesión (`pre-market`, `regular`, `post-market`).

Los cambios de horario de verano se tienen en cuenta de forma explícita: una apertura en una hora ambigua (repetida) se toma con el desplazamiento temprano, un cierre con el tardío, y una hora local que cae en la hora omitida del cambio de primavera se desplaza al primer instante que existe realmente. La búsqueda en `nextSession` y `previousSession` se limita a diez años desde el punto indicado; si no se encuentra la sesión, se devuelve `null`.

## Cuenta atrás de la barra

`resolveTradingBarBounds` determina los límites de una barra a partir de su hora de apertura, y `calculateBarCountdown` construye con ellos una instantánea de la cuenta atrás. La hora actual la transmite quien llama: la capa no crea un temporizador propio, por lo que el resultado es determinista y apto para pruebas.

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

El marco temporal se escribe como un número con el sufijo `s`, `m`, `h`, `d` o `w` (`30s`, `5m`, `1h`, `1d`, `1w`); sin sufijo, el valor se considera en minutos, y no se admiten los meses de calendario. Sin calendario, el cierre de una barra es simplemente su apertura más la duración del marco temporal. Con calendario, una barra intradía se recorta con el cierre de su sesión, las barras diarias avanzan por fechas de negociación y las semanales, por semanas ISO de calendario. Si la hora de apertura no cae en ninguna sesión, ambas funciones devuelven `null`.

El parámetro `sessionKinds` indica qué sesiones tener en cuenta; de forma predeterminada se toman solo las principales (`regular`), y sin `calendar` provoca un error. `BarCountdown` contiene el estado `pending` / `open` / `closed`, la hora `now`, los límites `bounds`, y también `untilOpenSeconds`, `elapsedSeconds`, `remainingSeconds` y la fracción de barra transcurrida `progress`.

## Etiquetas del eje de tiempo

`TimeAxisFormatter` es una envoltura con caché sobre `Intl.DateTimeFormat`, común a las marcas de la escala y a la etiqueta del crosshair.

```ts
import { TimeAxisFormatter } from '@stocksharp/chart/time';

const formatter = new TimeAxisFormatter({
  locale: 'es-ES',
  timeZone: 'Europe/Madrid',
  timeVisible: true,
  secondsVisible: false,
});

formatter.formatTick(now, 3_600);   // etiqueta de marca con un paso de una hora
formatter.formatCrosshair(now);     // etiqueta bajo el crosshair
```

El paso de la cuadrícula en segundos determina el detalle de la etiqueta: año, mes, día u hora del día. Los segundos aparecen solo con `secondsVisible` y un paso menor de un minuto. De forma predeterminada se usan la configuración regional `en-GB` y la zona horaria `UTC`; ambos valores se normalizan mediante `Intl` y están disponibles como las propiedades `locale` y `timeZone`.

El `formatter` opcional intercepta ambas etiquetas y recibe la hora y el contexto `TimeScaleFormatContext`: el tipo de etiqueta (`tick` o `crosshair`), la configuración regional, la zona horaria, los indicadores `timeVisible` y `secondsVisible`, y también el paso `tickStep` (para el crosshair es `null`). Si la función no devuelve una cadena o lanza una excepción, se aplica el formato integrado.

## Conexión con el gráfico

El gráfico acepta el calendario directamente: el modo de escala `session-aware` comprime el eje al tiempo de negociación, y el primitivo `SessionShading` resalta las sesiones con el fondo del panel.

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

El modo `session-aware` no se crea sin calendario: es un error de configuración. Si no se indica `timeScale.timeZone`, se toma la zona horaria del calendario y, si esta falta, `UTC`. Omitir `sessionKinds` significa que en el eje se quedan las sesiones de todos los tipos.

## Métodos públicos

`TradingCalendar` implementa `ITradingCalendar`:

- `schedule()`: el horario normalizado.
- `sessionsInRange(range, kinds?)`: las sesiones que cruzan el rango, en orden ascendente de hora de apertura.
- `sessionAt(time, kinds?)`: la sesión que contiene el instante indicado, o `null`.
- `isTradingTime(time, kinds?)`: si hay negociación en ese instante.
- `nextSession(time, kinds?)`: la sesión más próxima que abre no antes del instante indicado.
- `previousSession(time, kinds?)`: la última sesión que cerró no después del instante indicado.

Funciones del reloj de la barra:

- `resolveTradingBarBounds(barOpenTime, resolution, options?)`: los límites de la barra.
- `calculateBarCountdown(barOpenTime, resolution, now, options?)`: la instantánea de la cuenta atrás.

`TimeAxisFormatter`:

- `formatTick(time, step)`: la etiqueta de la marca de la escala para un paso dado.
- `formatCrosshair(time)`: la etiqueta del crosshair.
- `locale` y `timeZone`: los valores normalizados, disponibles solo para lectura.

El resto de exportaciones de la capa son tipos y conjuntos de constantes: `TradingSessionKind`, `BarClockState`, `TimeScaleLabelKind`, `ITradingCalendar`, `TradingSchedule`, `TradingSessionRule`, `TradingSessionTemplate`, `TradingDayOverride`, `TradingSession`, `BarClockOptions`, `TradingBarBounds`, `BarCountdown`, `TimeAxisFormatterOptions`, `TimeScaleFormatter`, `TimeScaleFormatContext`, `IsoWeekday`, `LocalDate`, `LocalTimeOfDay`.

## Véase también

- [Gráficos en JavaScript](../charts.md)
- [Relleno de historial](backfill.md)
- [Velas](candlestick.md)
- [Indicadores](indicators.md)
