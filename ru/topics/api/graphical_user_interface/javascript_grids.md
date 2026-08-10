# JavaScript-таблицы

[StockSharp JS Grids](https://github.com/StockSharp/JS-Grids) — это набор браузерных компонентов для отображения табличных данных. Пакет опубликован в npm как [@stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids), не имеет сторонних зависимостей времени выполнения и может использоваться как с TypeScript, так и напрямую в браузере.

![Торговый журнал StockSharp с фильтрами, выбранными строками и закреплёнными итогами](../../../images/javascript_grids_blotter.jpg)

Готовую версию можно проверить в [онлайн-демонстрации](https://stocksharp.github.io/JS-Grids/demo/): заголовки сортируют строки, столбцы перетаскиваются и скрываются, фильтры и группировка меняют представление, а экспорт создаёт настоящий файл `.xlsx`.

## Состав пакета

- [DataGrid](javascript_grids/data_grid.md) строит заголовок и строки по единому описанию столбцов. Он отвечает за сортировку, фильтры, группировку, выбор строк, закреплённые итоги, контекстное меню, сохранение состояния и экспорт.
- [ColumnSettings](javascript_grids/column_settings.md) подключается к уже отрисованной сервером HTML-таблице и позволяет пользователю менять порядок и видимость столбцов.
- [TableSort](javascript_grids/table_sort.md) предоставляет самостоятельный контроллер сортировки для таблицы, которой управляет приложение.
- [TableExport](javascript_grids/table_export.md) формирует книгу OOXML `.xlsx` без сторонней библиотеки.

`DataGrid` и `ColumnSettings` решают разные задачи. Первый сам создаёт содержимое `<thead>` и `<tbody>` из декларации столбцов. Второй не создаёт таблицу и предназначен только для существующей серверной разметки с атрибутами `data-col`.

## Установка

Установите пакет из npm:

```bash
npm install @stocksharp/grids
```

Все основные компоненты доступны из общей точки входа:

```ts
import {
  DataGrid,
  ColumnSettings,
  TableSort,
  TableExport,
} from '@stocksharp/grids';
```

Для уменьшения импортируемого кода предусмотрены отдельные точки входа:

```ts
import { DataGrid } from '@stocksharp/grids/data-grid';
import { ColumnSettings } from '@stocksharp/grids/column-settings';
import { TableSort } from '@stocksharp/grids/table-sort';
import { TableExport } from '@stocksharp/grids/table-export';
```

Без сборщика подключите готовый браузерный пакет. Его публичные объекты находятся в `window.SSGrid`:

```html
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/grids@1.1.0/dist/ssgrid.js"></script>
<script>
  const { DataGrid, TableExport } = window.SSGrid;
</script>
```

## Быстрый пример

Подготовьте обычную таблицу с секциями заголовка и данных:

```html
<table id="orders">
  <thead></thead>
  <tbody></tbody>
</table>
```

Опишите столбцы один раз и передайте строки в таблицу:

```ts
import { DataGrid, SortDirections } from '@stocksharp/grids';

interface Order {
  id: number;
  symbol: string;
  price: number;
}

const table = document.querySelector<HTMLTableElement>('#orders')!;
const grid = new DataGrid<Order>({
  head: table.tHead!,
  body: table.tBodies[0],
  columns: [
    { key: 'id', header: '№', value: order => order.id, filter: 'number', exportable: true },
    { key: 'symbol', header: 'Инструмент', value: order => order.symbol, filter: 'text', exportable: true },
    { key: 'price', header: 'Цена', value: order => order.price, filter: 'number', exportable: true },
  ],
  defaultSort: { col: 'id', dir: SortDirections.Desc },
  rowKey: order => String(order.id),
  emptyText: 'Нет заявок',
  locale: 'ru-RU',
});

grid.setRows([
  { id: 101, symbol: 'SBER', price: 312.45 },
  { id: 102, symbol: 'GAZP', price: 164.18 },
]);
```

Библиотека создаёт DOM-элементы, но не навязывает оформление. Цвета, размеры, подсветку строк, меню, диалог фильтра и служебные классы задаёт таблица стилей приложения.

## Сборка из исходного кода

Репозиторий и локальная демонстрация используют стандартные npm-команды:

```bash
git clone https://github.com/StockSharp/JS-Grids.git
cd JS-Grids
npm ci
npm test
npm run build
npm run serve
```

После запуска демонстрация доступна по адресу `http://localhost:8793/demo/`.

## Смотрите также

- [Репозиторий JS-Grids](https://github.com/StockSharp/JS-Grids)
- [Пакет @stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids)
- [Онлайн-демонстрация](https://stocksharp.github.io/JS-Grids/demo/)
- [Торговые JavaScript-контролы](javascript_trading_controls.md)
- [JavaScript-графики](charts/javascript_charts.md)
- [JavaScript-диаграмма](javascript_diagram.md)
