# JavaScript-графики

[Торговые JavaScript-графики StockSharp](https://github.com/StockSharp/JS-Charts) — это самостоятельная браузерная библиотека графиков без внешних зависимостей. Она опубликована в npm как [@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart) и поставляет canvas-движок `sschart`, используемый в веб-терминале StockSharp. Рабочую версию можно посмотреть в [онлайн-демо](https://stocksharp.github.io/JS-Charts/demo/).

![Торговый JavaScript-график StockSharp](../../../images/javascript_charts.jpg)

В отличие от Windows-компонентов из `StockSharp.Xaml.Charting`, эта библиотека работает в браузере и рисует непосредственно на HTML-элементе `canvas`. Движок доступен через глобальный объект `SSChart` (из `dist/sschart.js`), а также может быть импортирован как ECMAScript-модули из npm-пакета (`import { createChart, CandlestickSeries } from '@stocksharp/chart'`).

## Онлайн-демо

График ниже — это реальный движок, работающий прямо на этой странице: свечи с гистограммой объёма и скользящей средней. Тяните мышью для прокрутки, используйте колесо для масштабирования и нажмите кнопку разворота (в правом верхнем углу), чтобы открыть его на весь экран.

```chart-demo overview
```

## Возможности

- Полный набор ценовых серий: свечи, OHLC-бары, линия, область, гистограмма, полоса (band), а также производные типы Heikin-Ashi, Renko и Point & Figure.
- Точные исследования потока ордеров: footprint, профиль объёма и TPO (рыночный профиль).
- Загрузка истории и обновление в реальном времени через `setData` и `update`.
- Маркеры сделок, ценовые линии, перекрестие, масштабирование, прокрутка и автоматический расчёт диапазона.
- Движок индикаторов примерно со 160 реализациями расчётов.
- Наложенные индикаторы, синхронизированные панели осцилляторов и легенда, управляемая перекрестием.
- Светлая и тёмная темы, контекстное меню, диалог индикаторов и переключение типа графика.

## Установка

Установите пакет из npm и импортируйте ES-модули:

```bash
npm install @stocksharp/chart
```

```js
import { createChart, CandlestickSeries } from '@stocksharp/chart';
```

Либо без сборщика подключите готовый глобальный объект `SSChart` тегом `<script>`, как показано ниже.

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
  upColor: '#26a69a',
  downColor: '#ef5350',
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

## Режимы графика

У каждого типа серии есть собственная тема с онлайн-демо и JavaScript-кодом, который его настраивает:

- [Свечи](charts/candlestick.md) — классические OHLC-свечи.
- [OHLC-бары](charts/bar.md) — тики открытия/закрытия на вертикальном баре диапазона.
- [Линия](charts/line.md) — одна ломаная линия по ценам закрытия.
- [Область](charts/area.md) — линия с градиентной заливкой.
- [Гистограмма](charts/histogram.md) — вертикальные бары, обычно объём.
- [Полоса (Band)](charts/band.md) — верхний/нижний канал (конверты, полосы Боллинджера).
- [Свечи Heikin-Ashi](charts/heikin_ashi.md) — сглаженные свечи, фильтрующие шум.
- [Renko](charts/renko.md) — кирпичи, управляемые ценой, независимые от времени.
- [График Point and Figure](charts/point_figure.md) — столбцы X/O ценового движения.
- [Footprint-график](charts/footprint.md) — объём bid × ask на каждой цене внутри каждого бара.
- [Профиль объёма](charts/volume_profile.md) — объём по ценам с POC и зоной стоимости.
- [TPO (Профиль рынка)](charts/tpo.md) — время, проведённое на каждой цене за сессию.

Помимо типов серий, у графика есть [движок индикаторов](charts/indicators.md) (около 160 исследований) и [ленивая догрузка истории](charts/backfill.md), которая подгружает старые бары по мере прокрутки.

О визуальном редакторе стратегий, отрисовываемом тем же веб-стеком, см. [JavaScript-диаграмма](diagram.md).

## Слои поверх движка

Всё, что описано выше, — это базовый движок, точка входа `@stocksharp/chart`. Остальное разложено по отдельным опубликованным точкам входа: импортируйте нужную, копировать исходники не требуется.

| Точка входа | Что даёт |
|---|---|
| [Обвязка графика](charts/ui.md) — `@stocksharp/chart/ui` | Готовая обвязка: легенда, контекстное меню, переключатель типа графика, диспетчер панелей, диалог индикаторов. Требует таблицу стилей `@stocksharp/chart/ui.css`. |
| `@stocksharp/chart/indicators` | `IndicatorEngine`, отрисовщики и настройки индикаторов. Сами расчёты — в пакете [@stocksharp/indicators](charts/indicators.md). |
| [Торговля с графика](charts/trading.md) — `@stocksharp/chart/trading` | Торговый слой: линии заявок, позиция с текущим результатом, защитные заявки и котировки. Брокера не знает — сообщает о намерении, исполняет хост. |
| [Инструменты разметки](charts/drawings.md) — `@stocksharp/chart/drawings` | Инструменты разметки, привязка к барам и регистрация собственных фигур. |
| [Несколько графиков](charts/workspace.md) — `@stocksharp/chart/workspace` | Несколько синхронизированных графиков, сравнение инструментов, навигатор диапазона, шаблоны индикаторов. |
| [Сохранение раскладки](charts/persistence.md) — `@stocksharp/chart/persistence` | Сохранение и восстановление раскладки с версионированием и миграциями. |
| [Время и торговые сессии](charts/time.md) — `@stocksharp/chart/time` | Торговый календарь, сессии и таймзоны, обратный отсчёт до закрытия бара. |
| `@stocksharp/chart/orderflow` | Кластерный анализ: футпринт и профиль объёма. |

Без сборщика те же возможности доступны браузерным пакетом: `dist/sschartui.js` публикует глобальный объект `SSChartUI`.

## Сборка из исходного кода

Клонируйте репозиторий и используйте включённые npm-скрипты:

```bash
git clone https://github.com/StockSharp/JS-Charts.git
cd JS-Charts
npm install
npm run build
npm test
npm run serve
```

Сервер разработки публикует демо по адресу `http://localhost:8791/demo/index.html`.

## См. также

- [JavaScript-диаграмма](diagram.md)
- [Репозиторий JS-Charts](https://github.com/StockSharp/JS-Charts)
- [Онлайн-демо](https://stocksharp.github.io/JS-Charts/demo/)
- [Компоненты графиков для Windows](../graphical_user_interface/charts.md)
