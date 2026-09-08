# TableSort

`TableSort<TRow>` 是一个独立的排序控制器，在 [DataGrid](data_grid.md) 内部使用，也可以接入应用程序自己的表格。它保存选中的列和排序方向，处理对带有 `data-sort` 的表头的单击，并返回数据行数组的已排序副本。

## 标记

将每个可排序表头映射到取值函数字典中的一个键：

```html
<table id="quotes">
  <thead>
    <tr>
      <th data-sort="symbol">交易品种</th>
      <th data-sort="bid">买价</th>
      <th data-sort="ask">卖价</th>
    </tr>
  </thead>
  <tbody></tbody>
</table>
```

## 创建控制器

```ts
import { SortDirections, TableSort } from '@stocksharp/grids/table-sort';

interface Quote {
  symbol: string;
  bid: number | null;
  ask: number | null;
}

const table = document.querySelector<HTMLTableElement>('#quotes')!;
let quotes: Quote[] = [];

const sort = new TableSort<Quote>(
  table.tHead,
  {
    symbol: quote => quote.symbol,
    bid: quote => quote.bid,
    ask: quote => quote.ask,
  },
  render,
  { col: 'symbol', dir: SortDirections.Asc },
  new Intl.Collator('zh-CN', { numeric: true, sensitivity: 'base' }),
);

function render(): void {
  const body = table.tBodies[0];
  body.replaceChildren();

  for (const quote of sort.apply(quotes)) {
    const row = body.insertRow();
    row.insertCell().textContent = quote.symbol;
    row.insertCell().textContent = quote.bid?.toString() ?? '—';
    row.insertCell().textContent = quote.ask?.toString() ?? '—';
  }
}
```

构造函数接受：

1. 表头元素；若不需要处理单击，则传入 `null`；
2. 按列键索引的取值函数字典；
3. 用于重新渲染数据行的 `onChange` 函数；
4. 默认排序；若要使用初始顺序，则传入 `null`；
5. 预先创建的、用于比较文本的 `Intl.Collator`。

如果没有为选中的键定义取值函数，控制器会尝试读取数据行中同名的属性。

## 排序行为

单击新的表头会启用升序排序。再次单击会切换为降序，第三次单击则恢复默认顺序。如果为表格传入了 `defaultSort`，便不存在单独的未排序状态。

`apply(rows)` 始终返回新数组，不会修改应用程序的数组。无论排序方向如何，`null`、`undefined` 和空字符串都会排在末尾。

比较方式**取决于值本身，而不是列声明的类型**：如果两个值都能转换成有限的数字，就按数值比较，只有在其余情况下才会动用传入的 `Intl.Collator`。因此字符串 `"42"` 会落在 41 和 43 之间，而不是字母顺序会把它放到的位置；形如 `"1e3"` 的字符串同样被当作数字。如果某一列无论取什么值都应按文本排序，就让它给出一个不会变成数字的值。

控制器会为当前表头分配 `sort-asc` 或 `sort-desc` 类；箭头及这些类的其他视觉样式由应用程序定义。

## 通过代码控制

```ts
sort.set('bid', SortDirections.Desc);

const explicitSort = sort.current();
// { col: 'bid', dir: 'desc' }

sort.set(null, null);
// 恢复 defaultSort。
```

`current()` 只返回用户明确选择的排序。采用默认顺序时，即使数据行实际上经过了排序，返回值仍为 `null`。

如果应用程序在同一个表头内重新创建了 `<th>` 元素，请调用 `refreshHeader()` 以重新分配方向类。单击监听器绑定在传入的表头元素本身，因此会继续处理新的子元素。

## 另请参阅

- [JavaScript 表格](../grids.md)
- [DataGrid](data_grid.md)
- [TableExport](table_export.md)
