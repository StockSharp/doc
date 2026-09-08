# Индикаторы

Каталог примерно из 160 технических индикаторов живёт в отдельном пакете [@stocksharp/indicators](https://www.npmjs.com/package/@stocksharp/indicators). Он приезжает вместе с графиком как зависимость, но импортировать определения нужно именно из него. Любое определение вычисляется публичным `IndicatorRuntime` по вашим свечам, а результат вы рисуете сами обычными сериями — наложением на ценовую панель или осциллятором в отдельной подпанели.

## Живая демонстрация

Полосы Боллинджера поверх свечей, а RSI и MACD — в отдельных панелях ниже.

```chart-demo indicators
```

## Настройка

Импортируйте среду выполнения и нужные определения, прогоните каждое по свечам и постройте выводы. На вход среде подаются пары `{ time, value }`, где `value` — это свеча:

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import {
  IndicatorRuntime,
  BollingerBandsIndicator,
  RelativeStrengthIndexIndicator,
  MacdHistogramIndicator,
} from '@stocksharp/indicators';

// Вычисляет один индикатор по свечам; возвращает его точки, сгруппированные по id вывода.
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of runtime.points()) {
    if (p.time == null || p.value == null) continue;   // прогревочные бары ничего не выдают
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// Полосы Боллинджера (20, 2) как огибающая-наложение на ценовой панели.
const bb = compute(BollingerBandsIndicator, { length: 20, width: 2 }, candles);   // выводы: upper, middle, lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) в собственной подпанели.
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);     // вывод: oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) во второй подпанели.
const macdPane = chart.addPane();
const macd = compute(
  MacdHistogramIndicator,
  { shortMaLength: 12, longMaLength: 26, signalMaLength: 9 },                    // выводы: macd, signal, histogram
  candles);
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

## Параметры и выводы

Каждое определение объявляет собственные параметры и идентификаторы выводов, и обращаться к ним нужно по этим идентификаторам, а не по описательным именам:

| Определение | Параметры | Выводы | Панель |
|---|---|---|---|
| `BollingerBandsIndicator` | `length` (20), `width` (2), `upBandWidth`, `lowBandWidth` | `upper`, `middle`, `lower` | наложение |
| `RelativeStrengthIndexIndicator` | `length` (15) | `oscillator` | отдельная |
| `MacdHistogramIndicator` | `shortMaLength` (12), `longMaLength` (26), `signalMaLength` (9) | `macd`, `signal`, `histogram` | отдельная |

> [!CAUTION]
> Незнакомый идентификатор параметра **молча отбрасывается**, и индикатор считается на значении по умолчанию. Ошибки не будет — будет неправильная линия. По этой же причине у полос Боллинджера отклонение задаётся ключом `width`, а не `stdDev`, а у RSI по умолчанию длина 15, а не 14: если нужен привычный период, его надо передать явно, как в примере выше.

Полный список определений с их параметрами отдаёт каталог:

```js
import { getClientCatalog } from '@stocksharp/indicators';

for (const item of getClientCatalog()) {
  console.log(item.id, item.name, item.parameters.map(p => p.id));
}
```

## Данные в реальном времени

Полный пересчёт на каждый бар не нужен. Сохраните `IndicatorRuntime` и вызывайте `update` — он возвращает патч с тем, что изменилось:

```js
const runtime = new IndicatorRuntime({ definition: RelativeStrengthIndexIndicator, parameters: { length: 14 } });
runtime.resetStreaming(history.map(c => ({ time: c.time, value: c })));

// Незакрытый бар: false означает предварительное значение, которое заменится следующим вызовом.
runtime.update({ time: bar.time, value: bar }, false);

// Бар закрылся — значение становится окончательным.
const patch = runtime.update({ time: bar.time, value: bar }, true);
```

Пока бар не закрыт, среда держит предварительную точку и заменяет её при каждом вызове, поэтому история не растёт от обновлений внутри одного бара. `discardPreview()` убирает предварительную точку, `correct(index, input)` пересчитывает исторический бар, приехавший с исправлением.

## Смотрите также

- [JavaScript-графики](../charts.md)
- [Полоса (Band)](band.md)
- [Догрузка истории](backfill.md)
