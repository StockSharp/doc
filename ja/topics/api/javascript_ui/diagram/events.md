# イベントと API

ダイアグラムコンポーネントは、意図的に UI の装飾（クローム）を持ちません。データを運ぶイベントを発行し、メニューやダイアログの描画はホスト側に委ね、モデルを操作するためのメソッドを公開します。プロパティパネルやコンテキストメニューが、コンポーネントのイベントに接続した *あなた自身の* UI になるのはこのためです。

## イベント

`diagram.on(event, handler)` で購読します。これは購読解除用の関数を返します。

- `nodeAdded` / `nodeRemoved` / `nodeMoved` — ノードのライフサイクル。
- `linkAdded` / `linkRemoved` / `linkRelinked` — リンクのライフサイクル。
- `linkValidation` — 接続を試みるたびに発行される `{ allowed, reason }`。
- `selectionChanged` / `nodeSelected` / `linkSelected` — 選択。
- `contextMenuRequested` — 右クリック時の `{ x, y, node, link, port, commands }`。
- `fullscreenRequested` — `{ fullscreen }`。レイアウトの適用はホストが行います。

```js
const off = diagram.on('linkAdded', ({ links }) => console.log('接続しました', links[0]));
// 後で：
off();
```

## コンテキストメニュー

右クリックすると、コンポーネントは**独自の**メニューを描画すると同時に `contextMenuRequested` を発行します。イベントはどちらの場合も届くため、自分でメニューを描画するなら組み込みメニューを無効にする必要があります。そうしないと、両者が重なって表示されます。

```js
const diagram = new StockSharpDiagram(container, { showContextMenu: false });
```

イベントは、クリック位置、カーソル下のオブジェクト、そして描画すべき順序で並んだ項目のリストを通知します。項目には2種類あり、`group` フィールドの有無で区別します。**コマンド**は `command` を持ち実行できます。**サブメニュー**は `group` と独自の `commands` リストを持ち、実行するものはありません。

```js
diagram.on('contextMenuRequested', ({ x, y, node, link, commands }) => {
  const menu = renderMenu(commands.filter(c => c.enabled), x, y);

  menu.onPick = item => {
    if ('group' in item) return;                       // サブメニュー: 実行ではなく展開する
    diagram.executeContextCommand(item.command);       // コマンドが現在利用できない場合は false を返します
  };
});
```

コマンドの完全な一覧: `undo`、`redo`、`cut`、`copy`、`paste`、`open`、`delete`、`exportDocument`、`exportPng`、`exportSvg`、`overview`、`properties`、`help`。グループは `export` のみで、3つのエクスポートコマンドをまとめています。

> [!NOTE]
> エクスポートコマンドはコンポーネントの動作ではなく、**ホストへの依頼**です。コンポーネントはファイルを書き出さず、ダイアログも開きません。`exportRequested` を処理し、`saveDocument()`、`takeScreenshot()`、`takeSvg()` を自分のパラメーターで実行してください。

## リンクの検証

ポートには型があり、コンポーネントは互換性のないリンクや接続数の上限を超えるリンクを拒否し、`reason`（`incompatible-type`、`duplicate-link`、`source-limit`、`target-limit`、`same-node` など）を伴う `linkValidation` を発行します。`setLinkValidator` で独自のルールを追加できます。

```js
diagram.setLinkValidator(({ fromPort, toPort }) => fromPort.type === toPort.type);
```

## 保存と読み込み

```js
const scheme = diagram.save();              // { nodes, links }
diagram.load(scheme.nodes, scheme.links);

const document = diagram.saveDocument();     // バージョン管理されたドキュメント
diagram.loadDocument(document);
```

## 元に戻す、やり直し、クリップボード

`diagram.undo()` / `redo()` に加え、ボタンの有効・無効を制御するための `canUndo()` / `canRedo()`。クリップボード操作には `copySelection()` / `cutSelection()` / `pasteSelection()` と `deleteSelection()`。`setReadOnly(true)` はダイアグラムをプレビュー用にロックします。

元に戻す／やり直しの可否はコントロールが管理しているため、上記のモデル変更イベントだけでなく、コントロール正規の `undoStackChanged` イベントを追跡して、*あらゆる* コマンド（削除、ドラッグ、再リンク、貼り付け）でボタンの状態を同期させてください。

```js
diagram.on('undoStackChanged', ({ canUndo, canRedo }) => {
  undoButton.disabled = !canUndo;
  redoButton.disabled = !canRedo;
});
```

## 実行時の状態とエラーのハイライト

ダイアグラムはスキームの上に実行状態を重ねて表示できます。`setNodeError` はノードの枠線をアニメーション付きのパルス（約 1 秒）で点滅させ、赤いハイライトでマークします。実行時の失敗を報告するのに使います。[インタラクティブエディタ](editor.md) にある **Error** ボタンは、まさにこれを行っています。

```js
diagram.setNodeError('sma', 'SMA が失敗しました: データソースが設定されていません。');
diagram.setNodeError('sma', '警告', { animate: false }); // マークするが、最初のフラッシュはスキップする
```

読み込み時点で存在するエラーは、点滅する代わりに赤い背景で描画されます。それらは `load` に渡してください。

```js
diagram.load(nodes, links, { nodeErrors: { sma: '保存された期間の値が無効です。' } });
```

その他の実行時フック: `setActiveNode(id)` は現在実行中のノードをハイライトし（デバッガのカーソル）、`setPortRuntimeState(id, direction, portId, patch)` は単一のポートに注釈を付け、`setGlobalError(message)` はスキーム全体のエラーを点滅させます。`clearRuntimeState()` ですべてをクリアします。

## 関連項目

- [JavaScript ダイアグラム](../diagram.md)
- [インタラクティブエディタ](editor.md)
