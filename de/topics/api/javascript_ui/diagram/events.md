# Ereignisse und API

Die Diagramm-Komponente ist bewusst zurückhaltend, was das UI-Drumherum angeht: Sie löst Ereignisse aus, die die Daten transportieren, und überlässt dem Host das Rendern von Menüs und Dialogen; anschließend stellt sie Methoden bereit, um das Modell zu steuern. Deshalb ist ein Eigenschaftenpanel oder ein Kontextmenü *Ihre* eigene UI, die an die Ereignisse der Komponente angebunden ist.

## Ereignisse

Abonnieren Sie mit `diagram.on(event, handler)`; es wird eine Funktion zum Abbestellen zurückgegeben.

- `nodeAdded` / `nodeRemoved` / `nodeMoved` — Lebenszyklus eines Knotens.
- `linkAdded` / `linkRemoved` / `linkRelinked` — Lebenszyklus einer Verbindung.
- `linkValidation` — `{ allowed, reason }` für jeden Verbindungsversuch.
- `selectionChanged` / `nodeSelected` / `linkSelected` — Auswahl.
- `contextMenuRequested` — `{ x, y, node, link, port, commands }` beim Rechtsklick.
- `fullscreenRequested` — `{ fullscreen }`; der Host wendet das Layout an.

```js
const off = diagram.on('linkAdded', ({ links }) => console.log('verbunden', links[0]));
// später:
off();
```

## Kontextmenü

Bei einem Rechtsklick zeichnet die Komponente ihr **eigenes** Menü und löst gleichzeitig `contextMenuRequested` aus. Das Ereignis kommt in beiden Fällen, deshalb muss das eingebaute Menü abgeschaltet werden, wenn Sie ein eigenes zeichnen — sonst liegen sie übereinander:

```js
const diagram = new StockSharpDiagram(container, { showContextMenu: false });
```

Das Ereignis meldet die Klickposition, das Objekt unter dem Cursor und die Liste der Einträge in der Reihenfolge, in der sie gezeichnet werden sollen. Es gibt zwei Arten von Einträgen, und unterschieden werden müssen sie am Vorhandensein des Feldes `group`: Ein **Befehl** trägt `command` und wird ausgeführt, ein **Untermenü** trägt `group` und eine eigene Liste `commands` — darin gibt es nichts auszuführen.

```js
diagram.on('contextMenuRequested', ({ x, y, node, link, commands }) => {
  const menu = renderMenu(commands.filter(c => c.enabled), x, y);

  menu.onPick = item => {
    if ('group' in item) return;                       // Untermenü: aufklappen, nicht ausführen
    diagram.executeContextCommand(item.command);       // gibt false zurück, wenn der Befehl gerade nicht verfügbar ist
  };
});
```

Die vollständige Liste der Befehle: `undo`, `redo`, `cut`, `copy`, `paste`, `open`, `delete`, `exportDocument`, `exportPng`, `exportSvg`, `overview`, `properties`, `help`. Die einzige Gruppe ist `export`, sie fasst die drei Exportbefehle zusammen.

> [!NOTE]
> Die Exportbefehle sind eine **Bitte an den Host** und keine Aktion der Komponente: Sie schreibt keine Dateien und öffnet keine Dialoge. Behandeln Sie `exportRequested` und führen Sie `saveDocument()`, `takeScreenshot()` oder `takeSvg()` mit Ihren eigenen Parametern aus.

## Verbindungsvalidierung

Ports sind typisiert, und die Komponente lehnt inkompatible oder überbelegte Verbindungen ab, indem sie `linkValidation` mit einem `reason` auslöst (`incompatible-type`, `duplicate-link`, `source-limit`, `target-limit`, `same-node`, …). Fügen Sie mit `setLinkValidator` Ihre eigene Regel hinzu:

```js
diagram.setLinkValidator(({ fromPort, toPort }) => fromPort.type === toPort.type);
```

## Speichern und Laden

```js
const scheme = diagram.save();              // { nodes, links }
diagram.load(scheme.nodes, scheme.links);

const document = diagram.saveDocument();     // versioniertes Dokument
diagram.loadDocument(document);
```

## Rückgängig, Wiederholen und Zwischenablage

`diagram.undo()` / `redo()` zusammen mit `canUndo()` / `canRedo()`, um die Schaltflächen zu steuern; `copySelection()` / `cutSelection()` / `pasteSelection()` und `deleteSelection()` für die Zwischenablage. `setReadOnly(true)` sperrt das Diagramm als reine Vorschau.

Die Verfügbarkeit von Rückgängig/Wiederholen liegt in der Hoheit des Steuerelements; verfolgen Sie daher dessen maßgebliches Ereignis `undoStackChanged`, um die Schaltflächen bei *jedem* Befehl synchron zu halten (Löschen, Ziehen, Neuverbinden, Einfügen) — nicht nur bei den oben genannten Ereignissen zur Modellmutation:

```js
diagram.on('undoStackChanged', ({ canUndo, canRedo }) => {
  undoButton.disabled = !canUndo;
  redoButton.disabled = !canRedo;
});
```

## Laufzeitzustand und Fehlerhervorhebung

Das Diagramm kann den Ausführungszustand über das Schema legen. `setNodeError` lässt den Rand eines Knotens mit einem animierten Puls (~1 Sekunde) aufblinken und markiert ihn mit einer roten Hervorhebung — verwenden Sie es, um einen Laufzeitfehler zu melden. Die Schaltfläche **Fehler** in [Interaktiver Editor](editor.md) tut genau das.

```js
diagram.setNodeError('sma', 'SMA fehlgeschlagen: Es ist keine Datenquelle konfiguriert.');
diagram.setNodeError('sma', 'Warnung', { animate: false }); // markieren, aber das erste Aufblinken überspringen
```

Fehler, die bereits zum Ladezeitpunkt bestehen, färben den Hintergrund rot, anstatt zu blinken — übergeben Sie sie an `load`:

```js
diagram.load(nodes, links, { nodeErrors: { sma: 'Der gespeicherte Periodenwert ist ungültig.' } });
```

Weitere Laufzeit-Hooks: `setActiveNode(id)` hebt den aktuell ausgeführten Knoten hervor (ein Debugger-Cursor), `setPortRuntimeState(id, direction, portId, patch)` annotiert einen einzelnen Port, und `setGlobalError(message)` lässt einen schemaweiten Fehler aufblinken. Setzen Sie alles mit `clearRuntimeState()` zurück.

## Siehe auch

- [JavaScript-Diagramm](../diagram.md)
- [Interaktiver Editor](editor.md)
