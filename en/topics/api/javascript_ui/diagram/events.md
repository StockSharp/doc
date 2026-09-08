# Events and API

The diagram component is deliberately headless about chrome: it emits events carrying the data and lets the host render menus and dialogs, then exposes methods to drive the model. This is why a properties panel or a context menu is *your* UI wired to the component's events.

## Events

Subscribe with `diagram.on(event, handler)`; it returns an unsubscribe function.

- `nodeAdded` / `nodeRemoved` / `nodeMoved` — node lifecycle.
- `linkAdded` / `linkRemoved` / `linkRelinked` — link lifecycle.
- `linkValidation` — `{ allowed, reason }` for every attempted connection.
- `selectionChanged` / `nodeSelected` / `linkSelected` — selection.
- `contextMenuRequested` — `{ x, y, node, link, port, commands }` on right-click.
- `fullscreenRequested` — `{ fullscreen }`; the host applies the layout.

```js
const off = diagram.on('linkAdded', ({ links }) => console.log('connected', links[0]));
// later:
off();
```

## Context menu

On a right-click the component draws **its own** menu and at the same time emits `contextMenuRequested`. The event arrives in both cases, so if you draw a menu of your own, the built-in one has to be turned off — otherwise the two end up on top of each other:

```js
const diagram = new StockSharpDiagram(container, { showContextMenu: false });
```

The event reports the click position, the object under the cursor, and the list of entries in the order they should be drawn. Entries come in two kinds, and they are told apart by the presence of the `group` field: a **command** carries `command` and is executed, while a **submenu** carries `group` and a `commands` list of its own — there is nothing in it to execute.

```js
diagram.on('contextMenuRequested', ({ x, y, node, link, commands }) => {
  const menu = renderMenu(commands.filter(c => c.enabled), x, y);

  menu.onPick = item => {
    if ('group' in item) return;                       // a submenu: expand it, do not execute it
    diagram.executeContextCommand(item.command);       // returns false when the command is unavailable right now
  };
});
```

The full list of commands: `undo`, `redo`, `cut`, `copy`, `paste`, `open`, `delete`, `exportDocument`, `exportPng`, `exportSvg`, `overview`, `properties`, `help`. The only group is `export`, which brings the three export commands together.

> [!NOTE]
> The export commands are a **request to the host**, not an action of the component: it writes no files and opens no dialogs. Handle `exportRequested` and call `saveDocument()`, `takeScreenshot()`, or `takeSvg()` with your own parameters.

## Link validation

Ports are typed, and the component rejects incompatible or over-subscribed links, emitting `linkValidation` with a `reason` (`incompatible-type`, `duplicate-link`, `source-limit`, `target-limit`, `same-node`, …). Add your own rule with `setLinkValidator`:

```js
diagram.setLinkValidator(({ fromPort, toPort }) => fromPort.type === toPort.type);
```

## Save and load

```js
const scheme = diagram.save();              // { nodes, links }
diagram.load(scheme.nodes, scheme.links);

const document = diagram.saveDocument();     // versioned document
diagram.loadDocument(document);
```

## Undo, redo and clipboard

`diagram.undo()` / `redo()` with `canUndo()` / `canRedo()` to gate the buttons; `copySelection()` / `cutSelection()` / `pasteSelection()` and `deleteSelection()` for the clipboard. `setReadOnly(true)` locks the diagram to a preview.

Undo/redo availability is owned by the control, so track its canonical `undoStackChanged` event to keep the buttons in sync for *every* command (delete, drag, relink, paste), not only the model-mutation events above:

```js
diagram.on('undoStackChanged', ({ canUndo, canRedo }) => {
  undoButton.disabled = !canUndo;
  redoButton.disabled = !canRedo;
});
```

## Runtime state and error highlighting

The diagram can overlay execution state on top of the scheme. `setNodeError` flashes a node's border with an animated pulse (~1 second) and marks it with a red highlight — use it to report a runtime failure. The **Error** button in the [editor demo](editor.md) does exactly this.

```js
diagram.setNodeError('sma', 'SMA failed: no data source is configured.');
diagram.setNodeError('sma', 'Warning', { animate: false }); // mark it, but skip the initial flash
```

Errors that exist at load time paint a red background instead of flashing — pass them to `load`:

```js
diagram.load(nodes, links, { nodeErrors: { sma: 'The saved period value is invalid.' } });
```

Other runtime hooks: `setActiveNode(id)` highlights the node currently executing (a debugger cursor), `setPortRuntimeState(id, direction, portId, patch)` annotates a single port, and `setGlobalError(message)` flashes a scheme-wide error. Clear everything with `clearRuntimeState()`.

## See also

- [JavaScript diagram](../diagram.md)
- [Interactive editor](editor.md)
