# Tempo e sessões de negociação

`TradingCalendar` expande o horário de uma bolsa em sessões de negociação concretas no eixo UTC, e as restantes partes da camada apoiam-se nele: o relógio da barra calcula quando a vela atual fecha e o formatador do eixo prepara as etiquetas das marcas e do cursor em cruz. A camada não desenha nada por si — responde às perguntas «há negociação neste momento», «quando abre a próxima sessão» e «quanto falta para o fecho da barra».

## Ligação

O ponto de entrada é `@stocksharp/chart/time`; os mesmos nomes estão também disponíveis a partir da raiz do pacote `@stocksharp/chart`. Todos os tempos da API são tempo Unix em segundos e os intervalos são semiabertos: `[openTime, closeTime)`.

```ts
import {
  TradingCalendar,
  TradingSessionKind,
  type TradingSchedule,
} from '@stocksharp/chart/time';
```

## Criação do calendário

O horário descreve o fuso horário da bolsa (IANA), as regras semanais das sessões e, se necessário, as datas totalmente encerradas e as substituições de horário num dia concreto. As horas nas regras são locais da bolsa; o calendário converte-as em UTC tendo em conta as mudanças para a hora de verão.

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
      date: '2026-11-27',                                  // dia mais curto a seguir ao Dia de Ação de Graças
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

calendar.isTradingTime(now, [TradingSessionKind.Regular]);   // há negociação regular
calendar.sessionAt(now);                                     // a sessão atual ou null
calendar.nextSession(now, [TradingSessionKind.Regular]);     // a próxima abertura
calendar.sessionsInRange({ from: now - 86_400, to: now });   // tudo o que caiu nas últimas 24 horas
```

O construtor valida e normaliza o horário de imediato: o fuso horário tem de ser reconhecido pelo `Intl`, as datas têm de ser escritas como `YYYY-MM-DD`, os identificadores das sessões têm de ser únicos e as próprias sessões não podem sobrepor-se nem dentro da semana nem depois de expandidas para UTC. A duração de uma sessão tem de ser positiva e não pode exceder 24 horas, e a mesma data não pode ser simultaneamente feriado e substituição. A violação de qualquer destas condições origina um `TypeError` ou um `RangeError` logo na criação do calendário. As datas locais já analisadas ficam em cache, pelo que pedidos repetidos sobre o mesmo intervalo não são recalculados.

## Horário e sessões

Uma regra `TradingSessionRule` é um modelo `TradingSessionTemplate` mais os dias da semana na numeração ISO (`1` é segunda-feira, `7` é domingo). O dia da semana refere-se à data de abertura da sessão; para uma sessão noturna que fecha já no dia local seguinte, define-se `closeDayOffset: 1`.

`holidays` encerra a data por completo — nenhuma regra é expandida nela. `overrides` substitui na data indicada **todas** as sessões recorrentes pelo conjunto enumerado, o que é prático para dias mais curtos.

Cada pedido devolve `TradingSession` já materializadas: `id` na forma `<data>/<id da regra>`, `ruleId`, `kind`, a data de negociação `tradingDate`, os limites `openTime` e `closeTime` em UTC e o indicador `isOverride`. O parâmetro opcional `kinds`, presente em todos os métodos, filtra o resultado por tipo de sessão (`pre-market`, `regular`, `post-market`).

As mudanças para a hora de verão são tratadas explicitamente: uma abertura numa hora ambígua (repetida) é tomada pelo desvio anterior, o fecho pelo posterior, e uma hora local que caia na hora saltada da mudança da primavera é deslocada para o primeiro instante que realmente existe. A procura em `nextSession` e `previousSession` está limitada a dez anos a partir do ponto indicado; se a sessão não for encontrada, é devolvido `null`.

## Contagem decrescente da barra

`resolveTradingBarBounds` determina os limites de uma barra a partir da sua hora de abertura, e `calculateBarCountdown` constrói sobre eles um instantâneo da contagem decrescente. A hora atual é passada por quem chama — a camada não cria um temporizador próprio, pelo que o resultado é determinista e adequado a testes.

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

O período escreve-se como um número com o sufixo `s`, `m`, `h`, `d` ou `w` (`30s`, `5m`, `1h`, `1d`, `1w`); sem sufixo, o valor é lido como minutos e os meses de calendário não são suportados. Sem calendário, o fecho da barra é simplesmente a abertura mais a duração do período. Com calendário, uma barra intradiária é cortada pelo fecho da sua sessão, as barras diárias avançam por datas de negociação e as semanais por semanas de calendário ISO. Se a hora de abertura não cair em nenhuma sessão, ambas as funções devolvem `null`.

O parâmetro `sessionKinds` define que sessões considerar; por predefinição são tomadas apenas as regulares (`regular`) e, sem `calendar`, este parâmetro provoca um erro. `BarCountdown` contém o estado `pending` / `open` / `closed`, a hora `now`, os limites `bounds` e ainda `untilOpenSeconds`, `elapsedSeconds`, `remainingSeconds` e a fração já decorrida da barra, `progress`.

## Etiquetas do eixo do tempo

`TimeAxisFormatter` é um invólucro com cache sobre `Intl.DateTimeFormat`, comum às marcas da escala e à etiqueta do cursor em cruz.

```ts
import { TimeAxisFormatter } from '@stocksharp/chart/time';

const formatter = new TimeAxisFormatter({
  locale: 'pt-PT',
  timeZone: 'Europe/Lisbon',
  timeVisible: true,
  secondsVisible: false,
});

formatter.formatTick(now, 3_600);   // etiqueta da marca com um passo de uma hora
formatter.formatCrosshair(now);     // etiqueta sob o cursor em cruz
```

O passo da grelha em segundos determina o detalhe da etiqueta: ano, mês, dia ou hora do dia. Os segundos só aparecem com `secondsVisible` e um passo inferior a um minuto. Por predefinição são usados a localidade `en-GB` e o fuso horário `UTC`; ambos os valores são normalizados pelo `Intl` e estão acessíveis nas propriedades `locale` e `timeZone`.

O `formatter` opcional intercepta ambas as etiquetas e recebe o tempo e o contexto `TimeScaleFormatContext` — o tipo de etiqueta (`tick` ou `crosshair`), a localidade, o fuso horário, os sinalizadores `timeVisible` e `secondsVisible` e ainda o passo `tickStep` (que para o cursor em cruz é `null`). Se a função devolver algo que não seja uma cadeia de texto ou lançar uma exceção, é aplicado o formato interno.

## Ligação ao gráfico

O gráfico aceita o calendário diretamente: o modo de escala `session-aware` comprime o eixo ao tempo de negociação e o primitivo `SessionShading` realça as sessões no fundo do painel.

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

O modo `session-aware` não pode ser criado sem calendário — é um erro de configuração. Se `timeScale.timeZone` não estiver definido, é usado o fuso horário do calendário e, na sua ausência, `UTC`. Omitir `sessionKinds` significa que o eixo mantém as sessões de todos os tipos.

## Métodos públicos

`TradingCalendar` implementa `ITradingCalendar`:

- `schedule()` — o horário normalizado.
- `sessionsInRange(range, kinds?)` — as sessões que cruzam o intervalo, por hora de abertura ascendente.
- `sessionAt(time, kinds?)` — a sessão que contém o instante indicado, ou `null`.
- `isTradingTime(time, kinds?)` — se há negociação nesse instante.
- `nextSession(time, kinds?)` — a sessão mais próxima que abre não antes do instante indicado.
- `previousSession(time, kinds?)` — a última sessão que fechou não depois do instante indicado.

Funções do relógio da barra:

- `resolveTradingBarBounds(barOpenTime, resolution, options?)` — os limites da barra.
- `calculateBarCountdown(barOpenTime, resolution, now, options?)` — o instantâneo da contagem decrescente.

`TimeAxisFormatter`:

- `formatTick(time, step)` — a etiqueta da marca da escala para um dado passo.
- `formatCrosshair(time)` — a etiqueta do cursor em cruz.
- `locale` e `timeZone` — os valores normalizados, acessíveis apenas para leitura.

As restantes exportações da camada são tipos e conjuntos de constantes: `TradingSessionKind`, `BarClockState`, `TimeScaleLabelKind`, `ITradingCalendar`, `TradingSchedule`, `TradingSessionRule`, `TradingSessionTemplate`, `TradingDayOverride`, `TradingSession`, `BarClockOptions`, `TradingBarBounds`, `BarCountdown`, `TimeAxisFormatterOptions`, `TimeScaleFormatter`, `TimeScaleFormatContext`, `IsoWeekday`, `LocalDate`, `LocalTimeOfDay`.

## Veja também

- [Gráficos em JavaScript](../charts.md)
- [Preenchimento de histórico](backfill.md)
- [Candlestick](candlestick.md)
- [Indicadores](indicators.md)
