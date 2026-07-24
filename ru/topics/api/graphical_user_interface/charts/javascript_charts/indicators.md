# Индикаторы

График поставляется с каталогом примерно из 160 технических индикаторов. Любой из них вычисляется публичным `IndicatorRuntime` по вашим свечам, а результат вы рисуете сами обычными сериями — наложением на ценовую панель или осциллятором в отдельной подпанели.

## Живая демонстрация

Полосы Боллинджера поверх свечей, а RSI и MACD — в отдельных панелях ниже.

```chart-demo indicators
```

## Настройка

Импортируйте среду выполнения и нужные определения индикаторов, прогоните каждый по свечам и постройте выводы. На вход среде подаются пары `{ time, value }`, где `value` — это свеча:

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import { IndicatorRuntime, BollingerBandsIndicator, RelativeStrengthIndexIndicator, MacdIndicator } from '@stocksharp/chart/indicators';

// Вычисляет один индикатор по свечам; возвращает его точки, сгруппированные по id вывода.
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  const points = runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of points) {
    if (p.time == null || p.value == null) continue;   // прогревочные бары ничего не выдают
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// Полосы Боллинджера (20, 2) как огибающая-наложение на ценовой панели.
const bb = compute(BollingerBandsIndicator, { length: 20, stdDev: 2 }, candles);   // выводы: upper, middle, lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) в собственной подпанели.
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);       // вывод: oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) во второй подпанели.
const macdPane = chart.addPane();
const macd = compute(MacdIndicator, { fastLength: 12, slowLength: 26, signalLength: 9 }, candles);   // выводы: macd, signal, histogram
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

Каждое определение объявляет свои параметры и идентификаторы выводов (Боллинджер отдаёт `upper`/`middle`/`lower`, RSI — единственный `oscillator`, MACD — `macd`/`signal`/`histogram`). Для данных в реальном времени сохраните `IndicatorRuntime` и вызывайте `runtime.update({ time, value }, isFinal)` на каждый бар вместо полного пересчёта.

## Смотрите также

- [JavaScript-графики](../javascript_charts.md)
- [Полоса (Band)](band.md)
- [Догрузка истории](backfill.md)
