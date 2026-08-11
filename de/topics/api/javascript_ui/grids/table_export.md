# TableExport

`TableExport` erstellt im Browser eine echte OOXML-Arbeitsmappe im Format `.xlsx` und startet sofort deren Herunterladen. Die Implementierung verwendet keine externen Bibliotheken und tarnt keine CSV-Datei mit einer Excel-Erweiterung.

## Direkter Export

Übergeben Sie den Basisdateinamen, den Blattnamen, die Überschriften und ein zweidimensionales Zeilenarray:

```ts
import { TableExport } from '@stocksharp/grids/table-export';

TableExport.download(
  'portfolio-summary',
  'Portfolioübersicht',
  ['Kennzahl', 'Wert'],
  [
    ['Guthaben', 125000.50],
    ['Offene Positionen', 7],
    ['Nicht realisierter Gewinn', 4380.25],
  ],
);
```

Der Browser lädt eine Datei mit Zeitstempel im Format `<baseName>-YYYYMMDD-HHMMSS.xlsx` herunter, beispielsweise `portfolio-summary-20260810-143025.xlsx`.

Endliche Zahlen werden als numerische Zellen geschrieben, alle anderen nicht leeren Werte als Inline-Zeichenfolgen. `null`, `undefined` und eine leere Zeichenfolge erzeugen leere Zellen. Übergeben Sie die Zeilen in der gewünschten Reihenfolge: `TableExport` sortiert oder filtert die Daten nicht.

Der Blattname wird automatisch von den in Excel unzulässigen Zeichen `[]:*?/\` bereinigt, auf 31 Zeichen begrenzt und durch `Sheet1` ersetzt, falls nach der Bereinigung nichts übrig bleibt.

## Export aus DataGrid

[DataGrid](data_grid.md) bereitet die Daten aus den Spaltendeklarationen auf und verwendet denselben Mechanismus:

```ts
const data = grid.exportData();
console.log(data.headers, data.rows);

grid.download('orders', 'Aufträge');
```

In die Datei gelangen nur sichtbare Spalten mit `exportable: true`. Standardmäßig wird das Ergebnis von `value(row)` verwendet, falls vorhanden dagegen das Ergebnis von `exportValue(row)`. So kann beispielsweise in einer Zelle ein formatiertes DOM-Element angezeigt, nach einem numerischen Code sortiert und ein lokalisierter Text exportiert werden.

Die Zeilen werden nach Filterung, Sortierung und Gruppierung in der dargestellten Reihenfolge exportiert. Gruppenüberschriften werden dem Blatt nicht hinzugefügt, die Zeilen eingeklappter Gruppen bleiben jedoch erhalten. `renderLimit` beschneidet den Export nicht, und angeheftete Zeilen aus `pinnedRows()` werden nicht in die Arbeitsmappe aufgenommen.

`exportData()` lädt nichts herunter und eignet sich daher für die Vorschau und zum Testen des Inhalts. `download()` erzeugt die Datei auf dem Client, fügt dem Dokument vorübergehend einen Link mit einem `Blob` hinzu und gibt die Objekt-URL frei, nachdem das Herunterladen gestartet wurde.

## Einschränkungen

Die Komponente erzeugt eine minimale Arbeitsmappe mit einem Blatt. Sie unterstützt weder Formeln noch Zellstile, Spaltenbreiten, mehrere Blätter oder ZIP-Komprimierung. Wenn die Anwendung diese Funktionen benötigt, sollte der Export mit einem separaten Spezialwerkzeug erstellt werden.

## Siehe auch

- [JavaScript-Tabellen](../grids.md)
- [DataGrid](data_grid.md)
- [TableSort](table_sort.md)
- [Paket @stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids)
