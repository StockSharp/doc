# TableExport

`TableExport` creates a real OOXML `.xlsx` workbook in the browser and immediately starts its download. The implementation uses no third-party libraries and does not disguise a CSV file with an Excel extension.

## Direct export

Pass a base file name, sheet name, headers, and a two-dimensional row array:

```ts
import { TableExport } from '@stocksharp/grids/table-export';

TableExport.download(
  'portfolio-summary',
  'Portfolio summary',
  ['Metric', 'Value'],
  [
    ['Cash', 125000.50],
    ['Open positions', 7],
    ['Unrealized profit', 4380.25],
  ],
);
```

The browser downloads a timestamped file in the `<baseName>-YYYYMMDD-HHMMSS.xlsx` format, for example `portfolio-summary-20260810-143025.xlsx`.

Finite numbers are written as numeric cells; other non-empty values are written as inline strings. `null`, `undefined`, and an empty string create blank cells. Pass rows in the required order: `TableExport` does not sort or filter data.

The sheet name is automatically stripped of the forbidden Excel characters `[]:*?/\`, limited to 31 characters, and replaced with `Sheet1` if nothing remains after sanitization.

## Exporting from DataGrid

[DataGrid](data_grid.md) prepares data from column declarations and invokes the same mechanism:

```ts
const data = grid.exportData();
console.log(data.headers, data.rows);

grid.download('orders', 'Orders');
```

Only visible columns with `exportable: true` are included in the file. By default, the result of `value(row)` is used; when `exportValue(row)` is present, its result is used instead. This makes it possible, for example, to display a formatted DOM element in a cell, sort by a numeric code, and export localized text.

Rows are exported after filtering, sorting, and grouping, in view order. Group headers are not added to the sheet, but rows from collapsed groups remain. `renderLimit` does not truncate the export, and pinned rows from `pinnedRows()` are not included in the workbook.

`exportData()` does not download anything, which makes it convenient for previewing and testing the contents. `download()` creates the file on the client, temporarily adds a link with a `Blob` to the document, and releases the object URL after starting the download.

## Limitations

The component creates a minimal workbook with one sheet. It does not provide formulas, cell styles, column widths, multiple sheets, or ZIP compression. If the application needs these capabilities, prepare the export with a separate specialized tool.

## See also

- [JavaScript Grids](../javascript_grids.md)
- [DataGrid](data_grid.md)
- [TableSort](table_sort.md)
- [@stocksharp/grids package](https://www.npmjs.com/package/@stocksharp/grids)
