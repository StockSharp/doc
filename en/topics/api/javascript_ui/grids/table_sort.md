# TableSort

`TableSort<TRow>` is a standalone sorting controller used internally by [DataGrid](data_grid.md), but it can also be attached to an application-managed table. It stores the selected column and direction, handles clicks on headers with `data-sort`, and returns a sorted copy of the row array.

## Markup

Map each sortable header to a key in the value-reader dictionary:

```html
<table id="quotes">
  <thead>
    <tr>
      <th data-sort="symbol">Instrument</th>
      <th data-sort="bid">Bid</th>
      <th data-sort="ask">Ask</th>
    </tr>
  </thead>
  <tbody></tbody>
</table>
```

## Creating the controller

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
  new Intl.Collator('en-US', { numeric: true, sensitivity: 'base' }),
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

The constructor accepts:

1. the header element, or `null` if click handling is not required;
2. a dictionary of value-reader functions keyed by column;
3. an `onChange` function that redraws the rows;
4. the default sort, or `null` for the original order;
5. a pre-created `Intl.Collator` for comparing text.

If no reader function is defined for the selected key, the controller attempts to read the row property with the same name.

## Sorting behavior

Clicking a new header enables ascending sorting. The next click changes it to descending, and the third restores the default order. There is no separate unsorted state if the table has a `defaultSort`.

`apply(rows)` always returns a new array and does not modify the application array. `null`, `undefined`, and an empty string are placed at the end in both directions.

The comparison is chosen **by the values, not by the declared column type**: when both values convert to a finite number, they are compared numerically, and only in every other case does the supplied `Intl.Collator` come into play. The string `"42"` therefore lands between 41 and 43 rather than where the alphabet would put it; a string such as `"1e3"` also counts as a number. If a column has to be sorted as text whatever the values, give out a value from it that does not become a number.

The controller assigns the `sort-asc` or `sort-desc` class to the active header; arrows and other styling for these classes are defined by the application.

## Programmatic control

```ts
sort.set('bid', SortDirections.Desc);

const explicitSort = sort.current();
// { col: 'bid', dir: 'desc' }

sort.set(null, null);
// Return to defaultSort.
```

`current()` returns only the user's explicit selection. While the default order is active, the result is `null` even when the rows are actually sorted.

If the application recreates the `<th>` elements inside the same header, call `refreshHeader()` to reapply the direction classes. The click listener is attached to the supplied header element itself and continues to work with new child elements.

## See also

- [JavaScript Grids](../grids.md)
- [DataGrid](data_grid.md)
- [TableExport](table_export.md)
