# JavaScript-графики

[Торговые JavaScript-графики StockSharp](https://github.com/StockSharp/Charts) — самостоятельная библиотека графиков для браузера. Она содержит canvas-движок `sschart` без внешних runtime-зависимостей и набор модулей графика, используемый веб-терминалом StockSharp. Работу компонента можно посмотреть в [онлайн-демо](https://stocksharp.github.io/Charts/demo/).

![Торговый JavaScript-график StockSharp](../../../../images/javascript_charts.jpg)

В отличие от Windows-компонентов из `StockSharp.Xaml.Charting`, эта библиотека работает в браузере и рисует непосредственно на HTML-элементе `canvas`. Движок доступен через глобальный объект `SSChart`; при сборке TypeScript его также можно импортировать из `src/sschart.ts`.

## Возможности

- Свечи, OHLC-бары, линии, области, гистограммы, Renko, крестики-нолики, профиль объёма, кластеры и Box-графики.
- Загрузка истории и обновление в реальном времени через `setData` и `update`.
- Маркеры сделок, ценовые линии, перекрестие, масштабирование, прокрутка и автоматический расчёт диапазона.
- Движок индикаторов примерно со 160 реализациями расчётов.
- Наложенные индикаторы, синхронизированные панели осцилляторов и легенда, управляемая перекрестием.
- Светлая и тёмная темы, контекстное меню, диалог индикаторов и переключение типа графика.

## Добавление графика на страницу

При сборке создаётся `dist/sschart.js`, который публикует `window.SSChart`. Значения времени передаются в API как Unix-время в секундах.

```html
<div id="chart" style="width: 800px; height: 400px"></div>
<script src="dist/sschart.js"></script>
```

Создайте график, добавьте серию и загрузите данные:

```js
const chart = SSChart.createChart(document.getElementById('chart'), {
  timeScale: { timeVisible: true },
  crosshair: { mode: SSChart.CrosshairMode.Normal },
});

const candles = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#00c853',
  downColor: '#ff3d57',
  borderVisible: false,
});

candles.setData([
  { time: 1704153600, open: 120, high: 122, low: 119, close: 121 },
  { time: 1704240000, open: 121, high: 124, low: 120, close: 123 },
]);

candles.update({
  time: 1704326400,
  open: 123,
  high: 123.5,
  low: 122.8,
  close: 123.2,
});

chart.timeScale().fitContent();
```

Вызов `update` с текущей меткой времени заменяет последнюю точку. Более новая метка добавляет точку.

## Полный набор модулей терминала

Модули в `src/chart` расширяют базовый движок возможностями терминала:

- `IndicatorEngine`, отрисовщики и настройки индикаторов, а также каталог расчётов.
- Переключатель свечей, баров, линий, областей, Heikin-Ashi, Renko, крестиков-ноликов, кластеров и Box-графиков.
- Легенда, синхронизированные дополнительные панели, контекстное меню и диалог выбора индикаторов.
- Пересчёт активных индикаторов при изменении данных в реальном времени.

В качестве примера интеграции полного набора модулей используйте `src/chart/app.ts`.

## Сборка из исходного кода

Клонируйте репозиторий и используйте включённые npm-скрипты:

```bash
git clone https://github.com/StockSharp/Charts.git
cd Charts
npm install
npm run build
npm test
npm run serve
```

Сервер разработки публикует демо по адресу `http://localhost:8791/demo/index.html`.

## См. также

- [Репозиторий Charts](https://github.com/StockSharp/Charts)
- [Онлайн-демо](https://stocksharp.github.io/Charts/demo/)
- [Компоненты графиков для Windows](../charts.md)
