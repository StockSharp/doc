# DataGrid

`DataGrid<TRow>` erstellt eine Browsertabelle aus einem Array von `GridColumn<TRow>`-Deklarationen. Eine Deklaration bestimmt Überschrift, angezeigten Wert, Sortierung, Filterung, CSS-Klasse und Exportwert. Dadurch bleiben diese Darstellungen auch bei Änderungen am Spaltensatz konsistent.

## Tabelle erstellen

`DataGrid` leert die übergebenen Bereiche `head` und `body` und verwaltet anschließend deren Inhalt. Im Markup müssen beide Bereiche vorhanden sein:

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
    { key: 'id', header: 'Nr.', value: order => order.id, filter: 'number', exportable: true },
    { key: 'symbol', header: 'Instrument', value: order => order.symbol, filter: 'text', exportable: true },
    {
      key: 'side',
      header: 'Richtung',
      value: order => order.side,
      text: order => order.side === 'buy' ? 'Kauf' : 'Verkauf',
      render: order => order.side === 'buy' ? 'Kauf' : 'Verkauf',
      cellClass: order => order.side === 'buy' ? 'side-buy' : 'side-sell',
      exportValue: order => order.side === 'buy' ? 'Kauf' : 'Verkauf',
      filter: 'set',
      exportable: true,
    },
    { key: 'price', header: 'Preis', value: order => order.price, filter: 'number', exportable: true },
    { key: 'volume', header: 'Volumen', value: order => order.volume, filter: 'number', exportable: true },
    { key: 'board', header: 'Handelsmodus', value: order => order.board, filter: 'set', exportable: true },
  ],
  defaultSort: { col: 'id', dir: SortDirections.Desc },
  rowKey: order => String(order.id),
  emptyText: 'Keine Aufträge',
  locale: 'de-DE',
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
      { content: 'Gesamt', className: 'grid-total-label' },
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

`rowKey` muss einen eindeutigen und stabilen Schlüssel zurückgeben. Über diesen Schlüssel behält die Tabelle die Auswahl nach dem Neuzeichnen bei und findet Elemente mit `rowElement()` und `cellElement()`.

## Spaltenbeschreibung

Die wichtigsten Felder von `GridColumn<TRow>`:

- `key` — dauerhafte Spaltenkennung;
- `header` — bereits lokalisierte Überschrift;
- `value(row)` — Wert für die Sortierung und standardmäßig auch für Anzeige und Export;
- `render(row)` — Zeichenfolge oder DOM-Knoten für die Zelle;
- `text(row)` — Textdarstellung des Werts für Gruppen, Mengenfilter und Kontextmenü;
- `cellClass(row)` und `bindCell(td, row)` — Gestaltung und Ereignishandler der Zelle;
- `filter` — Filtertyp: `text`, `number` oder `set`;
- `exportable` und `exportValue(row)` — Einbeziehung der Spalte und separater Wert für `.xlsx`.

Wenn `render()` einen `Node` oder ein `DocumentFragment` zurückgibt, fügt die Komponente diesen Wert als DOM und nicht als HTML-Zeichenfolge in die Zelle ein. So lassen sich Schaltflächen sicher erstellen und ihre Ereignishandler mit `addEventListener` zuweisen.

## Sortierung, Filter und Gruppierung

Ein Klick auf eine Überschrift wechselt die Sortierung im Zyklus „aufsteigend → absteigend → Standardsortierung“. Leere Werte bleiben in beiden Richtungen am Ende. Text wird mit `Intl.Collator` für die in `locale` angegebene Sprache verglichen; fehlt der Parameter, wird die Dokumentsprache verwendet.

Schnellfilter erscheinen als Zeile unter den Überschriften, wenn `filtersVisible: true` gesetzt ist. Die Programmierschnittstelle akzeptiert serialisierbare Objekte:

```ts
grid.setFilter('symbol', { op: 'startsWith', text: 'SB' });
grid.setFilter('price', { op: 'between', min: 300, max: 320 });
grid.setFilter('board', { op: 'anyOf', values: ['TQBR', 'TQTF'] });

const rowsAfterFilters = grid.filteredRows();
grid.clearFilters();
```

Verfügbar sind die Operationen `contains`, `notContains`, `startsWith`, `endsWith`, `eq`, `ne`, `gt`, `ge`, `lt`, `le`, `between`, `anyOf`, `noneOf`, `empty` und `notEmpty`. Ein leeres Filterobjekt entspricht einem nicht gesetzten Filter.

Die Gruppierung unterstützt eine Ebene. Zeilen innerhalb einer Gruppe behalten die gewählte Sortierung bei, und Gruppen lassen sich ein- und ausklappen:

```ts
grid.groupBy('board');
grid.toggleGroup('TQBR');
grid.groupBy(null);
```

## Kontextmenü und Filterdialog

![DataGrid-Kontextmenü mit Sortierung, Filterung, Gruppierung und Anwendungsaktionen](../../../../images/javascript_grids_context_menu.jpg)

Bei `contextMenu: true` bietet das integrierte `GridContextMenu` Sortierung, Gruppierung, Filterung nach Wert, Ausblenden und Wiederherstellen von Spalten, Kopieren einer Zelle oder Zeile sowie den Export nach `.xlsx`. Mit einem `contextMenu`-Objekt können CSS-Klassen, Beschriftungen und die Aktionsliste geändert werden:

```ts
contextMenu: {
  classes: { menu: 'orders-menu', item: 'orders-menu-item' },
  labels: {
    sortAsc: 'Aufsteigend',
    sortDesc: 'Absteigend',
    filterRule: 'Filter einrichten…',
    filterByValue: value => `Nur diesen Wert anzeigen: ${value}`,
  },
  items: (context, defaults) => context.row
    ? [
        {
          label: `Auftrag Nr. ${context.row.id} stornieren`,
          run: () => cancelOrder(context.row!.id),
        },
        {},
        ...defaults,
      ]
    : defaults,
}
```

Die `labels`-Felder sind optional: Für nicht angegebene Beschriftungen bleiben die englischen Standardwerte erhalten. Für eine vollständig deutsche Oberfläche muss die Anwendung alle sichtbaren Beschriftungen von `GridMenuLabels` und `GridFilterDialogLabels` übergeben.

![Dialog für erweiterte DataGrid-Filter mit einer Liste der Spaltenwerte](../../../../images/javascript_grids_filter_dialog.jpg)

`GridFilterDialog` wird über das Menü oder mit der Methode `openFilterDialog(key, x, y)` geöffnet. Anders als die Schnellfilterzeile ermöglicht er die Auswahl eines Operators und seines Operanden. Für den Filter `set` zeigt der Dialog die tatsächlich vorhandenen Werte an. Auf einer Seite ist zu jedem Zeitpunkt höchstens ein integrierter Dialog und ein Kontextmenü geöffnet.

Die Menü- und Dialogkomponenten erzeugen Markup und vergeben Klassennamen, liefern jedoch keine fertige Gestaltung. Die Anwendung muss die Standardklassen `grid-menu*` und `grid-filter-dialog*` definieren oder eigene Klassen über `classes` übergeben.

Die Basisklassen werden ebenfalls aus dem Paket exportiert. `GridContextMenu.open(items, x, y)` zeigt ein Array von `GridMenuItem` an. Ein leeres Objekt dient dabei als Trennlinie, `disabled` deaktiviert eine Aktion und `checked` markiert einen Eintrag. Bei `GridFilterDialog.open(options, commit)` legt `options.header` die Spaltenüberschrift fest; die weiteren Optionen bestimmen Filterart, aktuelle Bedingung, Wertvarianten und Koordinaten. Die Funktion `commit` erhält beim Anwenden einen neuen `GridFilter` oder beim Löschen `null`. Beide Klassen besitzen die Eigenschaft `isOpen` und die Methode `close()`. Normalerweise müssen sie nicht manuell erstellt werden: `DataGrid` verwaltet sie über den Parameter `contextMenu` und die Methode `openFilterDialog()`.

## Zustand und Zeilenauswahl

`getState()` gibt ein gewöhnliches JSON-kompatibles Objekt mit Reihenfolge und ausgeblendeten Spalten, Sortierung, Filtern, Gruppierung, eingeklappten Gruppen sowie der Sichtbarkeit von Überschrift und Filterzeile zurück:

```ts
const grid = new DataGrid<Order>({
  // weitere Parameter
  onStateChange: state => {
    localStorage.setItem('orders-grid', JSON.stringify(state));
  },
});

const saved = localStorage.getItem('orders-grid');
if (saved)
  grid.setState(JSON.parse(saved));
```

Unbekannte Spaltenschlüssel werden beim Wiederherstellen ignoriert; neue Spalten, die in der gespeicherten Reihenfolge fehlen, werden hinter den aufgeführten Spalten ergänzt. `setState()` ruft `onStateChange` nicht auf, daher wird der Zustand beim Laden nicht erneut gespeichert.

Die Zeilenauswahl gehört nicht zu `GridState`. Wenn sie wiederhergestellt werden soll, speichern Sie die Schlüssel aus `onSelectionChange` separat und übergeben Sie sie an `setSelection(keys)`.

## Echtzeitdaten und Export

`setRows(rows)` ersetzt den Zeilensatz und zeichnet die Tabelle neu. Die Komponente hält das übergebene Array als Referenz. Nach Änderungen an den Objekten kann daher `render()` aufgerufen werden. Verwenden Sie für eine gezielte Aktualisierung `cellElement(rowKey, columnKey)` und zum Suchen einer Zeile `rowElement(rowKey)`.

`renderLimit` begrenzt nur die Anzahl der Zeilen im DOM. Filterung, Sortierung und Export arbeiten weiterhin mit dem gesamten Datensatz. `afterRender()` wird nach jedem weiteren Neuzeichnen aufgerufen und eignet sich beispielsweise dazu, Abonnements für die derzeit sichtbaren Instrumente neu zu setzen.

Die angehefteten Zeilen aus `pinnedRows()` werden bei jedem Rendern neu gelesen und nehmen nicht an Sortierung, Auswahl oder Export teil. Die Methode `exportData()` gibt Überschriften und Zeilen zurück, ohne eine Datei herunterzuladen; `download(baseName, sheetName)` erzeugt die `.xlsx`-Datei.

## Lokalisierung und Instanzbereinigung

![Dieselbe DataGrid-Tabelle mit chinesischen Überschriften, Menüs und Gruppen](../../../../images/javascript_grids_chinese.jpg)

Das Paket übersetzt Überschriften und Werte nicht selbst. Die Anwendung übergibt lokalisierte Werte für `header`, `emptyText`, `text`, `groupHeader` sowie die Menü- und Dialogbeschriftungen. Der Zustand speichert Schlüssel und Rohwerte und kann deshalb nach einem Sprachwechsel in eine neue Instanz übernommen werden.

Rufen Sie vor dem Ersetzen einer Instanz `destroy()` auf. Die Methode entfernt den `Ctrl+C`-Handler vom Dokument und schließt geöffnete Menüs und Dialoge:

```ts
const state = grid.getState();
grid.destroy();

const localizedGrid = createGermanGrid();
localizedGrid.setState(state);
```

## Siehe auch

- [JavaScript-Tabellen](../javascript_grids.md)
- [TableSort](table_sort.md)
- [TableExport](table_export.md)
- [ColumnSettings](column_settings.md)
- [Online-Demo von JS-Grids](https://stocksharp.github.io/JS-Grids/demo/)
