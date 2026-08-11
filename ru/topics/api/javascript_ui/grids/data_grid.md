# DataGrid

`DataGrid<TRow>` строит браузерную таблицу по массиву деклараций `GridColumn<TRow>`. Одна декларация определяет заголовок, отображаемое значение, сортировку, фильтрацию, CSS-класс и значение для экспорта, поэтому эти представления не расходятся при изменении набора столбцов.

## Создание таблицы

`DataGrid` очищает переданные `head` и `body`, после чего управляет их содержимым. В разметке должны существовать обе секции:

```html
<table id="orders" class="orders-grid">
  <thead></thead>
  <tbody></tbody>
</table>
```

```ts
import {
  DataGrid,
  GridPinnedPlacements,
  SortDirections,
} from '@stocksharp/grids';

interface Order {
  id: number;
  symbol: string;
  side: 'buy' | 'sell';
  price: number;
  volume: number;
  board: string;
}

const table = document.querySelector<HTMLTableElement>('#orders')!;
const grid = new DataGrid<Order>({
  head: table.tHead!,
  body: table.tBodies[0],
  columns: [
    { key: 'id', header: '№', value: order => order.id, filter: 'number', exportable: true },
    { key: 'symbol', header: 'Инструмент', value: order => order.symbol, filter: 'text', exportable: true },
    {
      key: 'side',
      header: 'Направление',
      value: order => order.side,
      text: order => order.side === 'buy' ? 'Покупка' : 'Продажа',
      render: order => order.side === 'buy' ? 'Покупка' : 'Продажа',
      cellClass: order => order.side === 'buy' ? 'side-buy' : 'side-sell',
      exportValue: order => order.side === 'buy' ? 'Покупка' : 'Продажа',
      filter: 'set',
      exportable: true,
    },
    { key: 'price', header: 'Цена', value: order => order.price, filter: 'number', exportable: true },
    { key: 'volume', header: 'Объём', value: order => order.volume, filter: 'number', exportable: true },
    { key: 'board', header: 'Режим торгов', value: order => order.board, filter: 'set', exportable: true },
  ],
  defaultSort: { col: 'id', dir: SortDirections.Desc },
  rowKey: order => String(order.id),
  emptyText: 'Нет заявок',
  locale: 'ru-RU',
  reorderable: true,
  filtersVisible: true,
  selection: 'multi',
  selectedClass: 'is-selected',
  contextMenu: true,
  pinnedRows: () => [{
    key: 'total',
    className: 'grid-total',
    place: GridPinnedPlacements.Bottom,
    cells: [
      { content: '', className: '' },
      { content: 'Итого', className: 'grid-total-label' },
      { content: '', className: '' },
      { content: '', className: '' },
      { content: '150', className: 'grid-total-value' },
      { content: '', className: '' },
    ],
  }],
});

const orders: Order[] = [
  { id: 101, symbol: 'SBER', side: 'buy', price: 312.45, volume: 100, board: 'TQBR' },
  { id: 102, symbol: 'GAZP', side: 'sell', price: 164.18, volume: 50, board: 'TQBR' },
];

grid.setRows(orders);
```

`rowKey` должен возвращать уникальный и стабильный ключ. По нему таблица сохраняет выбор после перерисовки и находит элементы через `rowElement()` и `cellElement()`.

## Описание столбца

Основные поля `GridColumn<TRow>`:

- `key` — постоянный идентификатор столбца;
- `header` — уже локализованный заголовок;
- `value(row)` — значение для сортировки и, по умолчанию, отображения и экспорта;
- `render(row)` — строка или DOM-узел для ячейки;
- `text(row)` — текстовое представление значения для групп, фильтра-множества и контекстного меню;
- `cellClass(row)` и `bindCell(td, row)` — оформление и обработчики ячейки;
- `filter` — тип фильтра: `text`, `number` или `set`;
- `exportable` и `exportValue(row)` — включение столбца и отдельное значение для `.xlsx`.

Если `render()` возвращает `Node` или `DocumentFragment`, компонент добавляет его в ячейку как DOM, а не как HTML-строку. Это позволяет безопасно создавать кнопки и назначать им обработчики через `addEventListener`.

## Сортировка, фильтры и группировка

Щелчок по заголовку переключает сортировку по циклу «возрастание → убывание → порядок по умолчанию». Пустые значения остаются в конце при обоих направлениях. Сравнение текста выполняет `Intl.Collator` для языка из `locale`; при отсутствии параметра используется язык документа.

Быстрые фильтры выводятся строкой под заголовками, если задано `filtersVisible: true`. Программный интерфейс принимает сериализуемые объекты:

```ts
grid.setFilter('symbol', { op: 'startsWith', text: 'SB' });
grid.setFilter('price', { op: 'between', min: 300, max: 320 });
grid.setFilter('board', { op: 'anyOf', values: ['TQBR', 'TQTF'] });

const rowsAfterFilters = grid.filteredRows();
grid.clearFilters();
```

Доступны операции `contains`, `notContains`, `startsWith`, `endsWith`, `eq`, `ne`, `gt`, `ge`, `lt`, `le`, `between`, `anyOf`, `noneOf`, `empty` и `notEmpty`. Пустой объект фильтра эквивалентен отсутствию фильтра.

Группировка поддерживает один уровень. Строки внутри группы сохраняют выбранную сортировку, а группы можно сворачивать:

```ts
grid.groupBy('board');
grid.toggleGroup('TQBR');
grid.groupBy(null);
```

## Контекстное меню и диалог фильтра

![Контекстное меню DataGrid с сортировкой, фильтрацией, группировкой и действиями приложения](../../../../images/javascript_grids_context_menu.jpg)

При `contextMenu: true` встроенный `GridContextMenu` предлагает сортировку, группировку, фильтрацию по значению, скрытие и восстановление столбцов, копирование ячейки или строки и экспорт `.xlsx`. Объект `contextMenu` позволяет изменить CSS-классы, надписи и список действий:

```ts
contextMenu: {
  classes: { menu: 'orders-menu', item: 'orders-menu-item' },
  labels: {
    sortAsc: 'По возрастанию',
    sortDesc: 'По убыванию',
    filterRule: 'Настроить фильтр…',
    filterByValue: value => `Оставить значение: ${value}`,
  },
  items: (context, defaults) => context.row
    ? [
        {
          label: `Снять заявку №${context.row.id}`,
          run: () => cancelOrder(context.row!.id),
        },
        {},
        ...defaults,
      ]
    : defaults,
}
```

Поля `labels` частичные: для неуказанных надписей остаются английские значения по умолчанию. Для полностью русской оболочки приложение должно передать все видимые надписи `GridMenuLabels` и `GridFilterDialogLabels`.

![Диалог расширенного фильтра DataGrid со списком значений столбца](../../../../images/javascript_grids_filter_dialog.jpg)

`GridFilterDialog` открывается из меню или методом `openFilterDialog(key, x, y)`. В отличие от быстрой строки он позволяет выбрать оператор и его операнд. Для фильтра `set` диалог показывает список реально присутствующих значений. В каждый момент на странице открыт не более чем один встроенный диалог и одно контекстное меню.

Компоненты меню и диалога создают разметку и назначают имена классов, но не поставляют готовое оформление. Приложение должно описать стандартные классы `grid-menu*` и `grid-filter-dialog*` либо передать свои через `classes`.

Низкоуровневые классы также экспортируются из пакета. `GridContextMenu.open(items, x, y)` показывает массив `GridMenuItem`, где пустой объект служит разделителем, `disabled` отключает действие, а `checked` отмечает пункт. `GridFilterDialog.open(options, commit)` получает вид фильтра, текущее условие, варианты значений и координаты, а заголовок столбца берёт из `options.header`; функция `commit` получает новый `GridFilter` или `null` при очистке. У обоих классов есть свойство `isOpen` и метод `close()`. Обычно создавать их вручную не требуется: `DataGrid` управляет ими через параметр `contextMenu` и метод `openFilterDialog()`.

## Состояние и выбор строк

`getState()` возвращает обычный JSON-совместимый объект с порядком и скрытыми столбцами, сортировкой, фильтрами, группировкой, свёрнутыми группами и видимостью заголовка и строки фильтров:

```ts
const grid = new DataGrid<Order>({
  // остальные параметры
  onStateChange: state => {
    localStorage.setItem('orders-grid', JSON.stringify(state));
  },
});

const saved = localStorage.getItem('orders-grid');
if (saved)
  grid.setState(JSON.parse(saved));
```

Неизвестные ключи столбцов при восстановлении игнорируются, а отсутствующие в сохранённом порядке новые столбцы добавляются после перечисленных. `setState()` не вызывает `onStateChange`, поэтому загрузка не записывает состояние повторно.

Выбор строк не входит в `GridState`. Если его нужно восстанавливать, сохраните ключи из `onSelectionChange` отдельно и передайте их в `setSelection(keys)`.

## Живые данные и экспорт

`setRows(rows)` заменяет набор строк и перерисовывает таблицу. Компонент хранит переданный массив по ссылке, поэтому после изменения объектов можно вызвать `render()`. Для точечного обновления используйте `cellElement(rowKey, columnKey)`, а для поиска строки — `rowElement(rowKey)`.

`renderLimit` ограничивает только количество строк в DOM. Фильтрация, сортировка и экспорт по-прежнему работают со всем набором. `afterRender()` вызывается после каждой последующей перерисовки и подходит, например, для переподписки на инструменты, которые сейчас видны.

Закреплённые строки из `pinnedRows()` перечитываются при каждом рендеринге и не участвуют в сортировке, выборе и экспорте. Метод `exportData()` возвращает заголовки и строки без загрузки файла, а `download(baseName, sheetName)` создаёт `.xlsx`.

## Локализация и уничтожение экземпляра

![Та же таблица DataGrid с китайскими заголовками, меню и группами](../../../../images/javascript_grids_chinese.jpg)

Пакет не переводит заголовки и значения сам. Приложение передаёт локализованные `header`, `emptyText`, `text`, `groupHeader`, подписи меню и диалога. Состояние хранит ключи и исходные значения, поэтому его можно перенести в новый экземпляр после смены языка.

Перед заменой экземпляра вызовите `destroy()`. Метод снимает обработчик `Ctrl+C` с документа и закрывает открытые меню и диалоги:

```ts
const state = grid.getState();
grid.destroy();

const localizedGrid = createRussianGrid();
localizedGrid.setState(state);
```

## Смотрите также

- [JavaScript-таблицы](../grids.md)
- [TableSort](table_sort.md)
- [TableExport](table_export.md)
- [ColumnSettings](column_settings.md)
- [Онлайн-демонстрация JS-Grids](https://stocksharp.github.io/JS-Grids/demo/)
