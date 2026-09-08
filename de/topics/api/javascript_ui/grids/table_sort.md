# TableSort

`TableSort<TRow>` ist ein eigenständiger Sortiercontroller, der intern von [DataGrid](data_grid.md) verwendet wird, aber auch an eine anwendungseigene Tabelle gebunden werden kann. Er speichert die gewählte Spalte und Richtung, verarbeitet Klicks auf Überschriften mit `data-sort` und gibt eine sortierte Kopie des Zeilenarrays zurück.

## Markup

Ordnen Sie jede sortierbare Überschrift einem Schlüssel aus dem Wörterbuch der Lesefunktionen zu:

```html
<table id="quotes">
  <thead>
    <tr>
      <th data-sort="symbol">Instrument</th>
      <th data-sort="bid">Kauf</th>
      <th data-sort="ask">Verkauf</th>
    </tr>
  </thead>
  <tbody></tbody>
</table>
```

## Controller erstellen

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
  new Intl.Collator('de-DE', { numeric: true, sensitivity: 'base' }),
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

Der Konstruktor akzeptiert:

1. das Überschriftselement oder `null`, wenn keine Klickverarbeitung benötigt wird;
2. ein Wörterbuch von Lesefunktionen für Werte nach Spaltenschlüssel;
3. die Funktion `onChange`, die die Zeilen neu zeichnet;
4. eine Standardsortierung oder `null` für die ursprüngliche Reihenfolge;
5. einen vorbereiteten `Intl.Collator` für Textvergleiche.

Wenn für den ausgewählten Schlüssel keine Lesefunktion definiert ist, versucht der Controller, die gleichnamige Eigenschaft der Zeile zu lesen.

## Sortierverhalten

Ein Klick auf eine neue Überschrift aktiviert die aufsteigende Sortierung. Der nächste Klick wechselt zur absteigenden Sortierung, der dritte stellt die Standardreihenfolge wieder her. Wurde für die Tabelle `defaultSort` übergeben, gibt es keinen separaten unsortierten Zustand.

`apply(rows)` gibt immer ein neues Array zurück und verändert das Array der Anwendung nicht. `null`, `undefined` und eine leere Zeichenfolge werden bei beiden Richtungen ans Ende gesetzt.

Der Vergleich wird **nach den Werten und nicht nach dem deklarierten Spaltentyp** gewählt: Lassen sich beide Werte zu einer endlichen Zahl umwandeln, werden sie numerisch verglichen, und nur in den übrigen Fällen kommt der übergebene `Intl.Collator` zum Einsatz. Die Zeichenfolge `"42"` steht deshalb zwischen 41 und 43 und nicht dort, wohin das Alphabet sie setzen würde; eine Zeichenfolge der Form `"1e3"` gilt ebenfalls als Zahl. Soll eine Spalte bei allen Werten als Text sortiert werden, geben Sie aus ihr einen Wert zurück, der nicht zu einer Zahl wird.

Der Controller weist der aktiven Überschrift die Klasse `sort-asc` oder `sort-desc` zu; Pfeile und die weitere visuelle Gestaltung dieser Klassen legt die Anwendung fest.

## Programmatische Steuerung

```ts
sort.set('bid', SortDirections.Desc);

const explicitSort = sort.current();
// { col: 'bid', dir: 'desc' }

sort.set(null, null);
// Rückkehr zu defaultSort.
```

`current()` gibt nur die ausdrückliche Benutzerauswahl zurück. Solange die Standardreihenfolge gilt, ist das Ergebnis `null`, auch wenn die Zeilen tatsächlich sortiert sind.

Wenn die Anwendung die `<th>`-Elemente innerhalb derselben Überschrift neu erstellt hat, rufen Sie `refreshHeader()` auf, um die Richtungsklassen erneut zu setzen. Der Klick-Handler ist auf dem übergebenen Überschriftselement selbst registriert und funktioniert weiterhin mit den neuen Kindelementen.

## Siehe auch

- [JavaScript-Tabellen](../grids.md)
- [DataGrid](data_grid.md)
- [TableExport](table_export.md)
