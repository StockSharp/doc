# JavaScript-диаграмма

[StockSharp JS Diagram](https://github.com/StockSharp/JS-Diagram) — это автономный, не имеющий зависимостей браузерный компонент, который отрисовывает визуальную схему стратегии из [Designer](../../designer.md) — ту же блок-диаграмму связанных элементов — на HTML-элементе `canvas`. Он опубликован в npm как [@stocksharp/diagram](https://www.npmjs.com/package/@stocksharp/diagram) и обеспечивает работу диаграмм стратегий в режиме только для чтения, отображаемых на веб-сайтах StockSharp.

Стратегия описывается как **схема**: набор *узлов* (элементов, таких как источник свечей, индикатор, условие или заявка), соединённых между собой через типизированные *порты*. Компонент берёт эту схему вместе с *палитрой* (каталогом типов элементов, их портов и цветов) и отрисовывает её.

## Живое демо

Диаграмма ниже — это реальный движок, работающий на этой странице, — минимальный каркас стратегии «источник данных → индикатор → график». Перетаскивайте холст для панорамирования, используйте колёсико для масштабирования и нажмите кнопку разворачивания, чтобы открыть его на весь экран.

```diagram-demo sma
```

Три блока — это источник **Свечи**, питающий **Индикатор** (простую скользящую среднюю); и свечи, и выход индикатора отрисовываются на элементе **График**. Это самый маленький законченный паттерн в Designer: получить данные, преобразовать их, визуализировать.

## Установка

Установите пакет из npm:

```bash
npm install @stocksharp/diagram
```

Затем импортируйте ES-модули — `import { renderScheme } from '@stocksharp/diagram/embed'` для read-only-встраивания или `import { StockSharpDiagram } from '@stocksharp/diagram'` для [интерактивного редактора](diagram/editor.md).

## Встраивание диаграммы

Компонент предоставляет функцию `renderScheme(host, paletteUrl, scheme)` из точки входа `@stocksharp/diagram/embed`. Передайте ей элемент-хост, URL JSON-палитры и схему, построенную из `nodes` и `links`:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const scheme = {
  nodes: [
    { id: 'candles', typeId: 'CandleElement',    name: 'Candles', x: 60,  y: 130 },
    { id: 'sma',     typeId: 'IndicatorElement', name: 'SMA',     x: 340, y: 60  },
    { id: 'chart',   typeId: 'ChartElement',     name: 'Chart',   x: 620, y: 130 },
  ],
  links: [
    { from: 'candles', fromPort: 'Output', to: 'sma',   toPort: 'Input' },
    { from: 'sma',     fromPort: 'Output', to: 'chart', toPort: 'Input' },
    { from: 'candles', fromPort: 'Output', to: 'chart', toPort: 'Input' },
  ],
};

renderScheme(document.getElementById('diagram'), '/data/designer-palette.json', scheme);
```

`typeId` каждого узла должен существовать в палитре; неизвестные типы отрисовываются как блоки-заглушки. Порты указываются по их `key`, а связь корректна, когда тип исходного порта совместим с типом целевого порта. `renderScheme` работает только для чтения: движок раскладывает схему, применяет тему (следует за светлым/тёмным режимом страницы) и позволяет зрителю панорамировать, масштабировать и разворачивать её, но не редактирует схему.

Тот же компонент работает и как полноценный **редактор** — перетаскивание элементов из палитры, соединение портов, правка и удаление узлов, undo/redo. См. [Интерактивный редактор](diagram/editor.md) и [События и API](diagram/events.md).

## Смотрите также

- [Интерактивный редактор](diagram/editor.md)
- [События и API](diagram/events.md)
- [JavaScript-графики](charts.md)
- [Designer](../../designer.md) — настольный визуальный редактор стратегий
- [Репозиторий JS-Diagram](https://github.com/StockSharp/JS-Diagram)
