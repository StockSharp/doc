# DataGrid

`DataGrid<TRow>` 根据 `GridColumn<TRow>` 声明数组构建浏览器表格。每个声明同时定义表头、显示值、排序、筛选、CSS 类和导出值，因此在更改列集合时，这些表示不会彼此脱节。

## 创建表格

`DataGrid` 会清空传入的 `head` 和 `body`，之后接管其中的内容。标记中必须同时存在这两个区域：

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
    { key: 'id', header: '编号', value: order => order.id, filter: 'number', exportable: true },
    { key: 'symbol', header: '交易品种', value: order => order.symbol, filter: 'text', exportable: true },
    {
      key: 'side',
      header: '方向',
      value: order => order.side,
      text: order => order.side === 'buy' ? '买入' : '卖出',
      render: order => order.side === 'buy' ? '买入' : '卖出',
      cellClass: order => order.side === 'buy' ? 'side-buy' : 'side-sell',
      exportValue: order => order.side === 'buy' ? '买入' : '卖出',
      filter: 'set',
      exportable: true,
    },
    { key: 'price', header: '价格', value: order => order.price, filter: 'number', exportable: true },
    { key: 'volume', header: '数量', value: order => order.volume, filter: 'number', exportable: true },
    { key: 'board', header: '交易板', value: order => order.board, filter: 'set', exportable: true },
  ],
  defaultSort: { col: 'id', dir: SortDirections.Desc },
  rowKey: order => String(order.id),
  emptyText: '暂无订单',
  locale: 'zh-CN',
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
      { content: '合计', className: 'grid-total-label' },
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

`rowKey` 必须返回唯一且稳定的键。表格依靠该键在重新渲染后保留选择，并通过 `rowElement()` 和 `cellElement()` 查找元素。

## 列定义

`GridColumn<TRow>` 的主要字段：

- `key` — 固定的列标识符；
- `header` — 已完成本地化的表头；
- `value(row)` — 用于排序的值，默认也用于显示和导出；
- `render(row)` — 单元格所用的字符串或 DOM 节点；
- `text(row)` — 用于分组、集合筛选和上下文菜单的文本表示；
- `cellClass(row)` 和 `bindCell(td, row)` — 单元格样式与事件处理程序；
- `filter` — 筛选器类型：`text`、`number` 或 `set`；
- `exportable` 和 `exportValue(row)` — 是否包含该列以及单独的 `.xlsx` 导出值。

如果 `render()` 返回 `Node` 或 `DocumentFragment`，组件会将其作为 DOM 添加到单元格，而不是作为 HTML 字符串。这使得应用程序能够安全地创建按钮，并通过 `addEventListener` 为其分配处理程序。

## 排序、筛选和分组

单击表头会按照“升序 → 降序 → 默认顺序”的循环切换排序。在两个方向上，空值始终排在末尾。文本比较使用根据 `locale` 语言创建的 `Intl.Collator`；若未指定该参数，则使用文档语言。

设置 `filtersVisible: true` 后，快速筛选器会显示在表头下方的一行中。编程接口接受可序列化的对象：

```ts
grid.setFilter('symbol', { op: 'startsWith', text: 'SB' });
grid.setFilter('price', { op: 'between', min: 300, max: 320 });
grid.setFilter('board', { op: 'anyOf', values: ['TQBR', 'TQTF'] });

const rowsAfterFilters = grid.filteredRows();
grid.clearFilters();
```

可用操作包括 `contains`、`notContains`、`startsWith`、`endsWith`、`eq`、`ne`、`gt`、`ge`、`lt`、`le`、`between`、`anyOf`、`noneOf`、`empty` 和 `notEmpty`。空筛选对象等同于未设置筛选器。

分组支持一个层级。组内各行保留当前排序，组本身可以折叠：

```ts
grid.groupBy('board');
grid.toggleGroup('TQBR');
grid.groupBy(null);
```

## 上下文菜单和筛选对话框

![包含排序、筛选、分组和应用程序操作的 DataGrid 上下文菜单](../../../../images/javascript_grids_context_menu.jpg)

设置 `contextMenu: true` 后，内置 `GridContextMenu` 会提供排序、分组、按值筛选、隐藏及恢复列、复制单元格或行以及导出 `.xlsx` 等功能。使用 `contextMenu` 对象可以修改 CSS 类、标签和操作列表：

```ts
contextMenu: {
  classes: { menu: 'orders-menu', item: 'orders-menu-item' },
  labels: {
    sortAsc: '升序',
    sortDesc: '降序',
    filterRule: '设置筛选条件…',
    filterByValue: value => `仅保留此值：${value}`,
  },
  items: (context, defaults) => context.row
    ? [
        {
          label: `撤销订单 №${context.row.id}`,
          run: () => cancelOrder(context.row!.id),
        },
        {},
        ...defaults,
      ]
    : defaults,
}
```

`labels` 字段是可选字段集合：未指定的标签会保留默认英文值。若要获得完全中文化的界面，应用程序必须传入 `GridMenuLabels` 和 `GridFilterDialogLabels` 中的所有可见标签。

![列值列表所在的 DataGrid 高级筛选对话框](../../../../images/javascript_grids_filter_dialog.jpg)

`GridFilterDialog` 可从菜单打开，也可通过 `openFilterDialog(key, x, y)` 方法打开。与快速筛选行不同，它允许选择运算符及其操作数。对于 `set` 筛选器，对话框会显示实际存在的值列表。页面在任意时刻最多只会打开一个内置对话框和一个上下文菜单。

菜单和对话框组件负责创建标记并指定类名，但不提供现成样式。应用程序必须定义标准的 `grid-menu*` 和 `grid-filter-dialog*` 类，或者通过 `classes` 传入自定义类。

该包也会导出底层类。`GridContextMenu.open(items, x, y)` 显示 `GridMenuItem` 数组，其中空对象作为分隔符，`disabled` 禁用操作，`checked` 标记菜单项。`GridFilterDialog.open(options, commit)` 从 `options.header` 获取列标题，并接收筛选器类型、当前条件、可选值和坐标；`commit` 函数会收到新的 `GridFilter`，清除条件时则收到 `null`。两个类均有 `isOpen` 属性和 `close()` 方法。通常无需手动创建它们：`DataGrid` 会通过 `contextMenu` 参数和 `openFilterDialog()` 方法进行管理。

## 状态和行选择

`getState()` 返回一个普通的 JSON 兼容对象，其中包含列顺序及隐藏列、排序、筛选器、分组、已折叠组，以及表头和筛选行的可见性：

```ts
const grid = new DataGrid<Order>({
  // 其他参数
  onStateChange: state => {
    localStorage.setItem('orders-grid', JSON.stringify(state));
  },
});

const saved = localStorage.getItem('orders-grid');
if (saved)
  grid.setState(JSON.parse(saved));
```

恢复状态时，未知列键会被忽略；未出现在已保存顺序中的新列会追加到所列列的后面。`setState()` 不会触发 `onStateChange`，因此加载操作不会再次写入状态。

行选择不属于 `GridState`。如果需要恢复选择，请另外保存 `onSelectionChange` 提供的键，并将其传给 `setSelection(keys)`。

## 实时数据和导出

`setRows(rows)` 替换数据行集合并重新渲染表格。组件按引用保存传入的数组，因此修改其中的对象后可以调用 `render()`。如需局部更新，请使用 `cellElement(rowKey, columnKey)`；如需查找行，请使用 `rowElement(rowKey)`。

`renderLimit` 只限制 DOM 中的行数。筛选、排序和导出仍会处理整个数据集。`afterRender()` 会在之后的每次重新渲染完成后调用，例如可用于重新订阅当前可见的交易品种。

每次渲染都会重新读取 `pinnedRows()` 提供的固定行，它们不参与排序、选择和导出。`exportData()` 方法返回表头和数据行而不下载文件，`download(baseName, sheetName)` 则创建 `.xlsx` 文件。

## 本地化和销毁实例

![具有中文表头、菜单和分组的同一个 DataGrid 表格](../../../../images/javascript_grids_chinese.jpg)

该包不会自行翻译表头和值。应用程序应传入本地化后的 `header`、`emptyText`、`text`、`groupHeader` 以及菜单和对话框标签。状态中保存的是键和原始值，因此切换语言后可以将其迁移到新实例。

替换实例前请调用 `destroy()`。该方法会移除文档上的 `Ctrl+C` 处理程序，并关闭已经打开的菜单和对话框：

```ts
const state = grid.getState();
grid.destroy();

const localizedGrid = createChineseGrid();
localizedGrid.setState(state);
```

## 另请参阅

- [JavaScript 表格](../javascript_grids.md)
- [TableSort](table_sort.md)
- [TableExport](table_export.md)
- [ColumnSettings](column_settings.md)
- [JS-Grids 在线演示](https://stocksharp.github.io/JS-Grids/demo/)
