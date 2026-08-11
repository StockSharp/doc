# Догрузка истории

График не может держать всю историю инструмента сразу, поэтому он загружает окно последних баров и подтягивает более старые по мере необходимости. `ChartDataController` следит за видимым диапазоном и, когда вы прокручиваете или масштабируете к левому краю, запрашивает следующую страницу истории из вашего источника данных и добавляет её слева.

## Живая демонстрация

Прокрутите или приблизьте к левому краю — старые бары подгружаются страницами (следите за плашкой статуса). У источника здесь искусственная задержка, чтобы состояние загрузки было видно.

```chart-demo backfill
```

## Настройка

Передайте контроллеру график, серию, которой он управляет, и источник данных. Источник разрешает символ и возвращает страницы баров; контроллер запрашивает старые страницы автоматически:

```js
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { ChartDataController } from '@stocksharp/chart/data';

// Источник данных: разрешает символ, затем отдаёт страницы баров, заканчивающиеся до `to`.
const dataSource = {
  resolveSymbol(request) {
    return Promise.resolve({ id: request.symbol, priceFormat: { type: 'price', precision: 2, minMove: 0.01 } });
  },
  getBars(request) {
    // request: { symbol, resolution, to?, countBack }. Верните { bars, hasMoreBefore, hasMoreAfter }.
    return fetchBars(request).then(bars => ({ bars, hasMoreBefore: bars.length > 0, hasMoreAfter: false }));
  },
  subscribeBars(request, listener) {
    // Отправляйте бары в реальном времени через слушатель ({ bar, isFinal }); верните функцию отписки.
    return () => {};
  },
};

const chart = createChart(document.getElementById('chart'), { timeScale: { timeVisible: true } });
const series = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });

const controller = new ChartDataController({
  chart,
  series,
  dataSource,
  initialCount: 300,               // баров загружается первыми
  historyCount: 250,               // баров на каждую страницу более старой истории
  historyPrefetchThreshold: 40,    // предзагрузка, когда до левого края остаётся 40 баров
  autoPrefetch: true,              // автоматически подгружать старые бары при прокрутке / масштабировании
});

// Необязательно: отслеживать состояние загрузки и прогресс.
controller.subscribe(snap => {
  console.log(snap.loadedBars, snap.hasMoreBefore, snap.loadingHistory);
});

// Загрузите первую страницу, затем разместите видимую область у правого края, чтобы было куда прокручивать влево.
controller.setSelection({ symbol: 'DEMO', resolution: '1h' }).then(() => {
  const loaded = controller.rawData().length;
  chart.timeScale().setVisibleLogicalRange({ from: Math.max(0, loaded - 90), to: loaded + 3 });
});
```

`historyPrefetchThreshold` — насколько близко к самому старому загруженному бару должен подойти видимый диапазон, прежде чем будет запрошена следующая страница; `autoPrefetch` включает эту автоматическую загрузку. Пока страница в пути, `snapshot.loadingHistory` равно `true`, так что можно показать индикатор загрузки. Вызовите `controller.loadMoreBefore()`, чтобы подгрузить страницу вручную, и `controller.dispose()` для очистки.

## Смотрите также

- [JavaScript-графики](../charts.md)
- [Свечи](candlestick.md)
- [Индикаторы](indicators.md)
