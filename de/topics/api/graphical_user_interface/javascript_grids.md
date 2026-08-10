# JavaScript-Tabellen

[StockSharp JS Grids](https://github.com/StockSharp/JS-Grids) ist eine Sammlung von Browserkomponenten zur Darstellung tabellarischer Daten. Das Paket ist als [@stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids) auf npm veröffentlicht, besitzt keine externen Laufzeitabhängigkeiten und kann sowohl mit TypeScript als auch direkt im Browser verwendet werden.

![StockSharp-Handelsjournal mit Filtern, ausgewählten Zeilen und angehefteter Ergebniszeile](../../../images/javascript_grids_blotter.jpg)

Die fertige Version können Sie in der [Online-Demo](https://stocksharp.github.io/JS-Grids/demo/) ausprobieren: Überschriften sortieren Zeilen, Spalten lassen sich verschieben und ausblenden, Filter und Gruppierungen ändern die Ansicht, und der Export erzeugt eine echte `.xlsx`-Datei.

## Paketinhalt

- [DataGrid](javascript_grids/data_grid.md) erstellt Überschrift und Zeilen anhand einer gemeinsamen Spaltendefinition. Die Komponente übernimmt Sortierung, Filterung, Gruppierung, Zeilenauswahl, angeheftete Ergebniszeilen, Kontextmenü, Zustandsspeicherung und Export.
- [ColumnSettings](javascript_grids/column_settings.md) bindet sich an eine bereits serverseitig gerenderte HTML-Tabelle und ermöglicht es Benutzern, Reihenfolge und Sichtbarkeit der Spalten zu ändern.
- [TableSort](javascript_grids/table_sort.md) stellt einen eigenständigen Sortiercontroller für eine vom Anwendungsprogramm verwaltete Tabelle bereit.
- [TableExport](javascript_grids/table_export.md) erzeugt eine OOXML-Arbeitsmappe im Format `.xlsx` ohne externe Bibliothek.

`DataGrid` und `ColumnSettings` lösen unterschiedliche Aufgaben. Die erste Komponente erstellt den Inhalt von `<thead>` und `<tbody>` selbst aus einer Spaltendeklaration. Die zweite erstellt keine Tabelle und ist ausschließlich für vorhandenes serverseitiges Markup mit `data-col`-Attributen bestimmt.

## Installation

Installieren Sie das Paket aus npm:

```bash
npm install @stocksharp/grids
```

Alle Hauptkomponenten sind über den gemeinsamen Einstiegspunkt verfügbar:

```ts
import {
  DataGrid,
  ColumnSettings,
  TableSort,
  TableExport,
} from '@stocksharp/grids';
```

Für kleinere Importe stehen separate Einstiegspunkte bereit:

```ts
import { DataGrid } from '@stocksharp/grids/data-grid';
import { ColumnSettings } from '@stocksharp/grids/column-settings';
import { TableSort } from '@stocksharp/grids/table-sort';
import { TableExport } from '@stocksharp/grids/table-export';
```

Ohne Bundler binden Sie das fertige Browserpaket ein. Seine öffentlichen Objekte stehen unter `window.SSGrid` bereit:

```html
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/grids@1.1.0/dist/ssgrid.js"></script>
<script>
  const { DataGrid, TableExport } = window.SSGrid;
</script>
```

## Schnellstart

Bereiten Sie eine gewöhnliche Tabelle mit Kopf- und Datenbereich vor:

```html
<table id="orders">
  <thead></thead>
  <tbody></tbody>
</table>
```

Definieren Sie die Spalten einmal und übergeben Sie die Zeilen an die Tabelle:

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
    { key: 'id', header: 'Nr.', value: order => order.id, filter: 'number', exportable: true },
    { key: 'symbol', header: 'Instrument', value: order => order.symbol, filter: 'text', exportable: true },
    { key: 'price', header: 'Preis', value: order => order.price, filter: 'number', exportable: true },
  ],
  defaultSort: { col: 'id', dir: SortDirections.Desc },
  rowKey: order => String(order.id),
  emptyText: 'Keine Aufträge',
  locale: 'de-DE',
});

grid.setRows([
  { id: 101, symbol: 'SBER', price: 312.45 },
  { id: 102, symbol: 'GAZP', price: 164.18 },
]);
```

Die Bibliothek erzeugt DOM-Elemente, schreibt jedoch keine Gestaltung vor. Farben, Abmessungen, Zeilenhervorhebung, Menü, Filterdialog und Hilfsklassen werden durch das Stylesheet der Anwendung festgelegt.

## Aus dem Quellcode erstellen

Das Repository und die lokale Demo verwenden die üblichen npm-Befehle:

```bash
git clone https://github.com/StockSharp/JS-Grids.git
cd JS-Grids
npm ci
npm test
npm run build
npm run serve
```

Nach dem Start ist die Demo unter `http://localhost:8793/demo/` erreichbar.

## Siehe auch

- [JS-Grids-Repository](https://github.com/StockSharp/JS-Grids)
- [Paket @stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids)
- [Online-Demo](https://stocksharp.github.io/JS-Grids/demo/)
- [JavaScript-Handelssteuerelemente](javascript_trading_controls.md)
- [JavaScript-Charts](charts/javascript_charts.md)
- [JavaScript-Diagramm](javascript_diagram.md)
