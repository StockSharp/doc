# TableExport

`TableExport` 在浏览器中创建真正的 OOXML `.xlsx` 工作簿，并立即开始下载。该实现不使用第三方库，也不会将 CSV 文件伪装成 Excel 扩展名。

## 直接导出

传入文件基础名称、工作表名称、表头和二维数据行数组：

```ts
import { TableExport } from '@stocksharp/grids/table-export';

TableExport.download(
  'portfolio-summary',
  '投资组合汇总',
  ['指标', '数值'],
  [
    ['现金', 125000.50],
    ['持仓数量', 7],
    ['未实现利润', 4380.25],
  ],
);
```

浏览器会下载带时间戳的 `<baseName>-YYYYMMDD-HHMMSS.xlsx` 格式文件，例如 `portfolio-summary-20260810-143025.xlsx`。

有限数值会写入数值单元格，其他非空值则写为内联字符串。`null`、`undefined` 和空字符串会创建空单元格。请按所需顺序传入数据行：`TableExport` 不会对数据进行排序或筛选。

工作表名称会自动去除 Excel 禁止使用的字符 `[]:*?/\`，截断为 31 个字符；若清理后为空，则替换为 `Sheet1`。

## 从 DataGrid 导出

[DataGrid](data_grid.md) 根据列声明准备数据，并调用同一个机制：

```ts
const data = grid.exportData();
console.log(data.headers, data.rows);

grid.download('orders', '订单');
```

文件中只会包含带有 `exportable: true` 的可见列。默认使用 `value(row)` 的结果；如果定义了 `exportValue(row)`，则使用后者的结果。例如，可以在单元格中显示格式化后的 DOM 元素，按数字代码排序，同时导出本地化文本。

数据行会在筛选、排序和分组后按照显示顺序导出。工作表中不会添加组标题，但已折叠组中的数据行仍会保留。`renderLimit` 不会截断导出内容，`pinnedRows()` 返回的固定行也不会写入工作簿。

`exportData()` 不会下载任何内容，因此适合预览和测试内容。`download()` 在客户端创建文件，临时向文档中添加带 `Blob` 的链接，并在开始下载后释放对象 URL。

## 限制

组件生成一个仅含单张工作表的最小工作簿。它不支持公式、单元格样式、列宽、多张工作表或 ZIP 压缩。如果应用程序需要这些功能，请使用专门的工具单独实现导出。

## 另请参阅

- [JavaScript 表格](../grids.md)
- [DataGrid](data_grid.md)
- [TableSort](table_sort.md)
- [@stocksharp/grids 包](https://www.npmjs.com/package/@stocksharp/grids)
