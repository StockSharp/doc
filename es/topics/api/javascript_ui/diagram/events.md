# Eventos y API

El componente de diagrama es deliberadamente neutral respecto a su interfaz visual: emite eventos que transportan los datos y deja que el host renderice los menús y los diálogos, para luego exponer métodos que controlan el modelo. Por eso un panel de propiedades o un menú contextual es *su* interfaz de usuario conectada a los eventos del componente.

## Eventos

Suscríbase con `diagram.on(event, handler)`; devuelve una función para cancelar la suscripción.

- `nodeAdded` / `nodeRemoved` / `nodeMoved` — ciclo de vida del nodo.
- `linkAdded` / `linkRemoved` / `linkRelinked` — ciclo de vida del enlace.
- `linkValidation` — `{ allowed, reason }` para cada intento de conexión.
- `selectionChanged` / `nodeSelected` / `linkSelected` — selección.
- `contextMenuRequested` — `{ x, y, node, link, port, commands }` al hacer clic derecho.
- `fullscreenRequested` — `{ fullscreen }`; el host aplica el diseño.

```js
const off = diagram.on('linkAdded', ({ links }) => console.log('conectado', links[0]));
// más tarde:
off();
```

## Menú contextual

Al hacer clic derecho, el componente dibuja su **propio** menú y, al mismo tiempo, emite `contextMenuRequested`. El evento llega en ambos casos, así que si usted dibuja su propio menú hay que desactivar el integrado; de lo contrario, quedarán uno encima del otro:

```js
const diagram = new StockSharpDiagram(container, { showContextMenu: false });
```

El evento informa de la posición del clic, del objeto bajo el cursor y de la lista de entradas en el orden en que deben dibujarse. Las entradas son de dos clases y hay que distinguirlas por la presencia del campo `group`: un **comando** lleva `command` y se ejecuta, mientras que un **submenú** lleva `group` y su propia lista `commands`, en la que no hay nada que ejecutar.

```js
diagram.on('contextMenuRequested', ({ x, y, node, link, commands }) => {
  const menu = renderMenu(commands.filter(c => c.enabled), x, y);

  menu.onPick = item => {
    if ('group' in item) return;                       // submenú: desplegarlo, no ejecutarlo
    diagram.executeContextCommand(item.command);       // devuelve false si el comando no está disponible ahora
  };
});
```

Lista completa de comandos: `undo`, `redo`, `cut`, `copy`, `paste`, `open`, `delete`, `exportDocument`, `exportPng`, `exportSvg`, `overview`, `properties`, `help`. El único grupo es `export`, que reúne los tres comandos de exportación.

> [!NOTE]
> Los comandos de exportación son una **petición al anfitrión**, no una acción del componente: este no escribe archivos ni abre diálogos. Procese `exportRequested` y ejecute `saveDocument()`, `takeScreenshot()` o `takeSvg()` con sus propios parámetros.

## Validación de enlaces

Los puertos tienen tipo, y el componente rechaza los enlaces incompatibles o sobresuscritos, emitiendo `linkValidation` con una `reason` (`incompatible-type`, `duplicate-link`, `source-limit`, `target-limit`, `same-node`, …). Agregue su propia regla con `setLinkValidator`:

```js
diagram.setLinkValidator(({ fromPort, toPort }) => fromPort.type === toPort.type);
```

## Guardar y cargar

```js
const scheme = diagram.save();              // { nodes, links }
diagram.load(scheme.nodes, scheme.links);

const document = diagram.saveDocument();     // documento versionado
diagram.loadDocument(document);
```

## Deshacer, rehacer y portapapeles

`diagram.undo()` / `redo()` con `canUndo()` / `canRedo()` para habilitar o deshabilitar los botones; `copySelection()` / `cutSelection()` / `pasteSelection()` y `deleteSelection()` para el portapapeles. `setReadOnly(true)` bloquea el diagrama en modo de vista previa.

La disponibilidad de deshacer/rehacer la gestiona el control, así que siga su evento canónico `undoStackChanged` para mantener los botones sincronizados con *cada* comando (eliminar, arrastrar, reconectar, pegar), no solo con los eventos de mutación del modelo mencionados arriba:

```js
diagram.on('undoStackChanged', ({ canUndo, canRedo }) => {
  undoButton.disabled = !canUndo;
  redoButton.disabled = !canRedo;
});
```

## Estado de ejecución y resaltado de errores

El diagrama puede superponer el estado de ejecución sobre el esquema. `setNodeError` hace parpadear el borde de un nodo con un pulso animado (~1 segundo) y lo marca con un resaltado rojo — úselo para informar de un fallo en tiempo de ejecución. El botón **Error** del [Editor interactivo](editor.md) hace exactamente esto.

```js
diagram.setNodeError('sma', 'SMA falló: no hay ninguna fuente de datos configurada.');
diagram.setNodeError('sma', 'Advertencia', { animate: false }); // márquelo, pero omita el parpadeo inicial
```

Los errores que existen en el momento de la carga pintan un fondo rojo en lugar de parpadear — páselos a `load`:

```js
diagram.load(nodes, links, { nodeErrors: { sma: 'El valor de período guardado no es válido.' } });
```

Otros hooks de tiempo de ejecución: `setActiveNode(id)` resalta el nodo que se está ejecutando actualmente (un cursor de depuración), `setPortRuntimeState(id, direction, portId, patch)` anota un único puerto, y `setGlobalError(message)` hace parpadear un error a nivel de todo el esquema. Límpielo todo con `clearRuntimeState()`.

## Consulta también

- [Diagrama en JavaScript](../diagram.md)
- [Editor interactivo](editor.md)
