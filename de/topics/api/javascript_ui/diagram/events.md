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

Die Komponente meldet die Klickposition und die Liste der aktivierten Befehle; Sie zeichnen das Popup und führen den gewählten Befehl aus:

```js
diagram.on('contextMenuRequested', ({ x, y, commands }) => {
  // commands: { command, enabled }[], wobei command einen der folgenden Werte hat:
  // undo | redo | cut | copy | paste | open | delete | properties | help
  const menu = renderMenu(commands.filter(c => c.enabled), x, y);
  menu.onPick = command => diagram.executeContextCommand(command);
});
```

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
