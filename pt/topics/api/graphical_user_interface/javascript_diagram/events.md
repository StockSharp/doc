# Eventos e API

O componente de diagrama é deliberadamente independente da interface visual: ele emite eventos contendo os dados e deixa que o host renderize menus e diálogos, expondo em seguida métodos para controlar o modelo. É por isso que um painel de propriedades ou um menu de contexto é a *sua* UI conectada aos eventos do componente.

## Eventos

Assine com `diagram.on(event, handler)`; a chamada retorna uma função de cancelamento da assinatura.

- `nodeAdded` / `nodeRemoved` / `nodeMoved` — ciclo de vida do nó.
- `linkAdded` / `linkRemoved` / `linkRelinked` — ciclo de vida da conexão.
- `linkValidation` — `{ allowed, reason }` para cada tentativa de conexão.
- `selectionChanged` / `nodeSelected` / `linkSelected` — seleção.
- `contextMenuRequested` — `{ x, y, node, link, port, commands }` ao clicar com o botão direito.
- `fullscreenRequested` — `{ fullscreen }`; o host aplica o layout.

```js
const off = diagram.on('linkAdded', ({ links }) => console.log('connected', links[0]));
// depois:
off();
```

## Menu de contexto

O componente informa a posição do clique e a lista de comandos habilitados; você desenha o popup e executa o comando escolhido:

```js
diagram.on('contextMenuRequested', ({ x, y, commands }) => {
  // commands: { command, enabled }[] onde command é um de
  // undo | redo | cut | copy | paste | open | delete | properties | help
  const menu = renderMenu(commands.filter(c => c.enabled), x, y);
  menu.onPick = command => diagram.executeContextCommand(command);
});
```

## Validação de conexões

As portas são tipadas, e o componente rejeita conexões incompatíveis ou que excedam o limite, emitindo `linkValidation` com um `reason` (`incompatible-type`, `duplicate-link`, `source-limit`, `target-limit`, `same-node`, …). Adicione sua própria regra com `setLinkValidator`:

```js
diagram.setLinkValidator(({ fromPort, toPort }) => fromPort.type === toPort.type);
```

## Salvar e carregar

```js
const scheme = diagram.save();              // { nodes, links }
diagram.load(scheme.nodes, scheme.links);

const document = diagram.saveDocument();     // documento versionado
diagram.loadDocument(document);
```

## Desfazer, refazer e área de transferência

`diagram.undo()` / `redo()` com `canUndo()` / `canRedo()` para habilitar ou desabilitar os botões; `copySelection()` / `cutSelection()` / `pasteSelection()` e `deleteSelection()` para a área de transferência. `setReadOnly(true)` bloqueia o diagrama em modo de visualização.

A disponibilidade de desfazer/refazer pertence ao controle, portanto acompanhe seu evento canônico `undoStackChanged` para manter os botões sincronizados a *cada* comando (excluir, arrastar, reconectar, colar), não apenas nos eventos de mutação do modelo acima:

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
diagram.load(nodes, links, { nodeErrors: { sma: 'O valor de período salvo é inválido.' } });
```

Outros hooks de tempo de execução: `setActiveNode(id)` destaca o nó em execução no momento (um cursor de depurador), `setPortRuntimeState(id, direction, portId, patch)` anota uma única porta, e `setGlobalError(message)` faz piscar um erro que abrange todo o esquema. Limpe tudo com `clearRuntimeState()`.

## Veja também

- [Diagrama em JavaScript](../javascript_diagram.md)
- [Editor interativo](editor.md)
