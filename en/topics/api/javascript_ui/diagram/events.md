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

The component reports the click position and the list of enabled commands; you draw the popup and run the chosen command:

```js
diagram.on('contextMenuRequested', ({ x, y, commands }) => {
  // commands: { command, enabled }[] where command is one of
  // undo | redo | cut | copy | paste | open | delete | properties | help
  const menu = renderMenu(commands.filter(c => c.enabled), x, y);
  menu.onPick = command => diagram.executeContextCommand(command);
});
```

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
