# Eventos e API

O componente de diagrama é deliberadamente independente da interface visual: emite eventos com os dados, deixa ao anfitrião a apresentação dos menus e diálogos e expõe métodos para controlar o modelo. Por isso, um painel de propriedades ou um menu de contexto constitui a interface criada pelo utilizador e ligada aos eventos do componente.

## Eventos

Subscreva com `diagram.on(event, handler)`; a chamada devolve uma função para cancelar a subscrição.

- `nodeAdded` / `nodeRemoved` / `nodeMoved` — ciclo de vida do nó.
- `linkAdded` / `linkRemoved` / `linkRelinked` — ciclo de vida da ligação.
- `linkValidation` — `{ allowed, reason }` para cada tentativa de ligação.
- `selectionChanged` / `nodeSelected` / `linkSelected` — seleção.
- `contextMenuRequested` — `{ x, y, node, link, port, commands }` ao clicar com o botão direito.
- `fullscreenRequested` — `{ fullscreen }`; o host aplica o layout.

```js
const off = diagram.on('linkAdded', ({ links }) => console.log('ligado', links[0]));
// depois:
off();
```

## Menu de contexto

O componente indica a posição do clique e a lista de comandos ativados; o utilizador apresenta o menu de contexto e executa o comando escolhido:

```js
diagram.on('contextMenuRequested', ({ x, y, commands }) => {
  // commands: { command, enabled }[] onde command é um de
  // undo | redo | cut | copy | paste | open | delete | properties | help
  const menu = renderMenu(commands.filter(c => c.enabled), x, y);
  menu.onPick = command => diagram.executeContextCommand(command);
});
```

## Validação de ligações

As portas são tipadas, e o componente rejeita ligações incompatíveis ou que excedam o limite, emitindo `linkValidation` com um `reason` (`incompatible-type`, `duplicate-link`, `source-limit`, `target-limit`, `same-node`, …). Adicione uma regra própria com `setLinkValidator`:

```js
diagram.setLinkValidator(({ fromPort, toPort }) => fromPort.type === toPort.type);
```

## Guardar e carregar

```js
const scheme = diagram.save();              // { nodes, links }
diagram.load(scheme.nodes, scheme.links);

const document = diagram.saveDocument();     // documento versionado
diagram.loadDocument(document);
```

## Anular, refazer e área de transferência

Utilize `diagram.undo()` / `redo()` com `canUndo()` / `canRedo()` para ativar ou desativar os botões; `copySelection()` / `cutSelection()` / `pasteSelection()` e `deleteSelection()` gerem a área de transferência. `setReadOnly(true)` bloqueia o diagrama no modo de visualização.

O controlo mantém o estado de disponibilidade das operações de anular/refazer. Acompanhe, por isso, o evento canónico `undoStackChanged` para manter os botões sincronizados a *cada* comando (eliminar, arrastar, voltar a ligar, colar), e não apenas nos eventos de alteração do modelo acima:

```js
diagram.on('undoStackChanged', ({ canUndo, canRedo }) => {
  undoButton.disabled = !canUndo;
  redoButton.disabled = !canRedo;
});
```

## Estado de execução e destaque de erros

O diagrama pode sobrepor o estado de execução ao esquema. `setNodeError` faz a borda de um nó piscar com uma pulsação animada (~1 segundo) e o marca com um destaque vermelho — use-o para reportar uma falha em tempo de execução. O botão **Error** no [Editor interativo](editor.md) faz exatamente isso.

```js
diagram.setNodeError('sma', 'SMA falhou: nenhuma fonte de dados está configurada.');
diagram.setNodeError('sma', 'Aviso', { animate: false }); // marca o nó, mas ignora o piscar inicial
```

Erros que já existem no momento do carregamento pintam um fundo vermelho em vez de piscar — passe-os para `load`:

```js
diagram.load(nodes, links, { nodeErrors: { sma: 'O valor de período guardado é inválido.' } });
```

Outros hooks de tempo de execução: `setActiveNode(id)` destaca o nó em execução no momento (um cursor de depurador), `setPortRuntimeState(id, direction, portId, patch)` anota uma única porta, e `setGlobalError(message)` faz piscar um erro que abrange todo o esquema. Limpe tudo com `clearRuntimeState()`.

## Veja também

- [Diagrama em JavaScript](../diagram.md)
- [Editor interativo](editor.md)
