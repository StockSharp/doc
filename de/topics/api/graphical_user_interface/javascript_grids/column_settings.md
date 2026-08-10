# ColumnSettings

`ColumnSettings` ergänzt eine bereits vom Server oder einer anderen Komponente erzeugte HTML-Tabelle um Auswahl, Umordnung und Ausblenden von Spalten. Anders als [DataGrid](data_grid.md) erstellt der Adapter weder Überschrift noch Zeilen und verwaltet auch nicht die Tabellendaten.

## Anforderungen an das Markup

Die Tabelle muss ein echtes `<thead>` enthalten. Verwaltbare Spalten erhalten eindeutige `data-col`-Attribute:

```html
<table id="trades">
  <thead>
    <tr>
      <th data-col="time">Zeit</th>
      <th data-col="symbol">Instrument</th>
      <th data-col="price">Preis</th>
      <th data-col="volume">Volumen</th>
      <th>Aktionen</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>10:15:02</td>
      <td>SBER</td>
      <td>312,45</td>
      <td>10</td>
      <td><button type="button">Öffnen</button></td>
    </tr>
  </tbody>
</table>
```

Eine Spalte ohne `data-col` gilt als fixiert: Benutzer können sie weder ausblenden noch von ihrer ursprünglichen Position verschieben. Beim Erstellen versieht der Adapter die zugehörigen Zellen im Tabellenkörper mit denselben Schlüsseln. Zeilen mit einer abweichenden Anzahl von Zellen, beispielsweise eine Zeile „Keine Daten“ mit `colspan`, bleiben unverändert.

## Einbindung

Die Hostanwendung stellt drei Teile bereit:

- die Tabelle mit serverseitigem Markup;
- einen Dialog, dem die Komponente Umschalter und Verschiebeschaltflächen hinzufügt;
- einen Speicher mit den Methoden `read()` und `write()`.

```ts
import { ColumnSettings } from '@stocksharp/grids/column-settings';

const dialogElement = document.querySelector<HTMLElement>('#column-dialog')!;
const list = dialogElement.querySelector<HTMLElement>('#column-list')!;

const settings = new ColumnSettings({
  table: document.querySelector<HTMLTableElement>('#trades')!,
  dialog: {
    list,
    moveUpTitle: 'Nach oben verschieben',
    moveDownTitle: 'Nach unten verschieben',
    classes: {
      item: 'column-picker-item',
      toggle: 'column-picker-toggle',
      label: 'column-picker-label',
      move: 'column-picker-move',
      moveUpIcon: 'icon-arrow-up',
      moveDownIcon: 'icon-arrow-down',
    },
    open: () => { dialogElement.hidden = false; },
    close: () => { dialogElement.hidden = true; },
  },
  store: {
    read: () => {
      const value = localStorage.getItem('trades-columns');
      return value ? JSON.parse(value) : null;
    },
    write: visible => {
      if (visible === null)
        localStorage.removeItem('trades-columns');
      else
        localStorage.setItem('trades-columns', JSON.stringify(visible));
    },
  },
});

document.querySelector('#open-columns')!
  .addEventListener('click', () => settings.openPicker());

document.querySelector('#apply-columns')!
  .addEventListener('click', () => settings.applyPicked());

document.querySelector('#reset-columns')!
  .addEventListener('click', () => settings.resetToDefault());
```

Die Komponente befüllt nur das Element `list`. Überschrift, Bestätigungs- und Zurücksetzschaltflächen, Animation sowie Öffnen und Schließen des Modalfensters gehören zur Anwendung. Daher müssen auch die Handler `applyPicked()` und `resetToDefault()` in der Anwendung zugewiesen werden.

## Layout speichern

`ColumnLayoutStore.read()` gibt entweder ein Array sichtbarer Schlüssel in der gewünschten Reihenfolge oder `null` zurück, wenn das ursprüngliche Layout verwendet wird. Der Konstruktor liest den Wert sofort und wendet ihn vor der ersten Benutzerinteraktion an.

`write(visibleKeys)` erhält ausschließlich die sichtbaren, verwaltbaren Spalten. Der Wert `null` bedeutet, dass die ursprüngliche Reihenfolge ausgewählt und keine Spalte ausgeblendet ist. Dadurch kann ein Speicher in der URL oder in `localStorage` einen überflüssigen Eintrag löschen, statt den vollständigen Standardwert zu speichern.

Die Schlüssel werden innerhalb des Adapters in Kleinbuchstaben normalisiert. Unbekannte und doppelte Schlüssel werden beim Anwenden verworfen.

## Methoden

- `defaultKeys()` gibt die ursprüngliche Reihenfolge der verwaltbaren Spalten zurück;
- `apply(visibleKeys)` ordnet Spalten sofort neu an und blendet sie aus, speichert das Layout jedoch nicht;
- `isDefault(visibleKeys)` prüft, ob das Layout dem ursprünglichen entspricht;
- `openPicker()` liest das aktuelle DOM, erstellt die Liste und öffnet den Dialog;
- `applyPicked()` wendet die aktuelle Auswahl an, speichert sie und schließt den Dialog;
- `resetToDefault()` stellt alle verwaltbaren Spalten wieder her, ruft `write(null)` auf und schließt den Dialog.

## Gestaltung

`ColumnSettings` liefert kein CSS und ist weder von einer bestimmten Modalbibliothek noch von einem bestimmten Symbolsatz abhängig. Über `ColumnPickerClasses` legt die Anwendung die Klassen für Zeile, Kontrollkästchen, Beschriftung, Schaltflächen und die beiden Symbole fest. `moveUpTitle` und `moveDownTitle` müssen vor der Übergabe an die Komponente lokalisiert werden.

## Siehe auch

- [JavaScript-Tabellen](../javascript_grids.md)
- [DataGrid](data_grid.md)
- [JS-Grids-Repository](https://github.com/StockSharp/JS-Grids)
