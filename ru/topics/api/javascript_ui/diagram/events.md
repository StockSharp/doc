# События и API

Компонент диаграммы намеренно ничего не навязывает по части UI-обвязки: он испускает события с данными и предоставляет хосту самому рисовать меню и диалоги, а затем даёт методы для управления моделью. Именно поэтому панель свойств или контекстное меню — это *ваш* UI, привязанный к событиям компонента.

## События

Подписка через `diagram.on(event, handler)`; возвращается функция отписки.

- `nodeAdded` / `nodeRemoved` / `nodeMoved` — жизненный цикл узлов.
- `linkAdded` / `linkRemoved` / `linkRelinked` — жизненный цикл связей.
- `linkValidation` — `{ allowed, reason }` для каждой попытки соединения.
- `selectionChanged` / `nodeSelected` / `linkSelected` — выделение.
- `contextMenuRequested` — `{ x, y, node, link, port, commands }` по правому клику.
- `fullscreenRequested` — `{ fullscreen }`; макет применяет хост.

```js
const off = diagram.on('linkAdded', ({ links }) => console.log('connected', links[0]));
// позже:
off();
```

## Контекстное меню

По правому клику компонент рисует **собственное** меню и одновременно испускает `contextMenuRequested`. Событие приходит в обоих случаях, поэтому, если вы рисуете своё меню, встроенное надо выключить — иначе они окажутся друг на друге:

```js
const diagram = new StockSharpDiagram(container, { showContextMenu: false });
```

Событие сообщает позицию клика, объект под курсором и список пунктов в том порядке, в котором их следует рисовать. Пункты бывают двух видов, и различать их нужно по наличию поля `group`: **команда** несёт `command` и выполняется, **подменю** несёт `group` и собственный список `commands` — выполнять в нём нечего.

```js
diagram.on('contextMenuRequested', ({ x, y, node, link, commands }) => {
  const menu = renderMenu(commands.filter(c => c.enabled), x, y);

  menu.onPick = item => {
    if ('group' in item) return;                       // подменю: раскрыть, а не выполнять
    diagram.executeContextCommand(item.command);       // вернёт false, если команда сейчас недоступна
  };
});
```

Полный список команд: `undo`, `redo`, `cut`, `copy`, `paste`, `open`, `delete`, `exportDocument`, `exportPng`, `exportSvg`, `overview`, `properties`, `help`. Единственная группа — `export`, она объединяет три команды экспорта.

> [!NOTE]
> Команды экспорта — это **просьба к хосту**, а не действие компонента: он не пишет файлов и не открывает диалогов. Обработайте `exportRequested` и выполните `saveDocument()`, `takeScreenshot()` или `takeSvg()` со своими параметрами.

## Валидация связей

Порты типизированы, и компонент отклоняет несовместимые или переполненные связи, испуская `linkValidation` с причиной `reason` (`incompatible-type`, `duplicate-link`, `source-limit`, `target-limit`, `same-node`, …). Добавьте своё правило через `setLinkValidator`:

```js
diagram.setLinkValidator(({ fromPort, toPort }) => fromPort.type === toPort.type);
```

## Сохранение и загрузка

```js
const scheme = diagram.save();              // { nodes, links }
diagram.load(scheme.nodes, scheme.links);

const document = diagram.saveDocument();     // версионированный документ
diagram.loadDocument(document);
```

## Undo, redo и буфер обмена

`diagram.undo()` / `redo()` с `canUndo()` / `canRedo()` для блокировки кнопок; `copySelection()` / `cutSelection()` / `pasteSelection()` и `deleteSelection()` для буфера обмена. `setReadOnly(true)` переводит диаграмму в режим предпросмотра.

Доступность undo/redo принадлежит контролу, поэтому отслеживайте его канонический сигнал `undoStackChanged`, чтобы кнопки оставались в синхроне для *каждой* команды (delete, drag, relink, paste), а не только для событий мутации модели выше:

```js
diagram.on('undoStackChanged', ({ canUndo, canRedo }) => {
  undoButton.disabled = !canUndo;
  redoButton.disabled = !canRedo;
});
```

## Состояние выполнения и подсветка ошибок

Диаграмма может накладывать состояние выполнения поверх схемы. `setNodeError` мигает рамкой узла анимацией (~1 секунда) и помечает его красной подсветкой — используйте это, чтобы сообщить о сбое во время выполнения. Кнопка **Error** в [Интерактивный редактор](editor.md) делает ровно это.

```js
diagram.setNodeError('sma', 'Сбой SMA: источник данных не настроен.');
diagram.setNodeError('sma', 'Предупреждение', { animate: false }); // пометить его, но пропустить начальную вспышку
```

Ошибки, существующие на момент загрузки, красят фон красным вместо мигания — передайте их в `load`:

```js
diagram.load(nodes, links, { nodeErrors: { sma: 'Сохранённое значение периода недопустимо.' } });
```

Другие runtime-хуки: `setActiveNode(id)` подсвечивает узел, выполняющийся сейчас (курсор отладчика), `setPortRuntimeState(id, direction, portId, patch)` аннотирует отдельный порт, а `setGlobalError(message)` мигает ошибкой уровня всей схемы. Очистить всё — `clearRuntimeState()`.

## Смотрите также

- [JavaScript-диаграмма](../diagram.md)
- [Интерактивный редактор](editor.md)
