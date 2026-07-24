# インタラクティブエディタ

読み取り専用の埋め込み（`renderScheme`）は、完全なエディタの薄いラッパーにすぎません。`StockSharpDiagram` クラス（すべて `@stocksharp/diagram` からエクスポートされる `StockSharpPalette` および `StockSharpCatalog` とともに）は、完全なビジュアルエディタです。パレットから要素をドラッグして追加し、ポートを接続し、ノードを移動・削除し、元に戻す/やり直し、型付きリンクの検証を行えます。編集はデフォルトで有効です。

## ライブデモ

パレットから要素をキャンバスにドラッグし、ポート間をドラッグして接続し、右クリックでメニューを開き、Undo/Redo を使用してください。互換性のない接続は拒否されます（ステータス行に注目してください）。**Error** を押すと、ノード上でアニメーション付きのランタイムエラーが点滅します（[イベントと API](events.md) を参照）。

```diagram-editor sma
```

## セットアップ

まずポート型と要素型の**カタログ**を構築し、それをもとにダイアグラムとパレットを作成します:

```js
import {
  StockSharpDiagram, StockSharpCatalog, StockSharpPalette,
  Node, PortType, DiagramNode, Link, PALETTE_DRAG_MIME,
} from '@stocksharp/diagram';

// 1) カタログ: ソケット（ポート）型と要素（ノード）型。
const catalog = new StockSharpCatalog();
catalog.addPortType(new PortType({ name: 'Candle', color: '#4aa3ff' }));
catalog.addPortType(new PortType({ name: 'Indicator', color: '#a779e9' }));
catalog.addNodeType(new Node({
  id: 'candles', name: 'Candles', groupName: 'Sources',
  outPorts: [{ id: 'Output', name: 'Output', type: 'Candle' }],
}));
catalog.addNodeType(new Node({
  id: 'sma', name: 'SMA', groupName: 'Indicators',
  inPorts: [{ id: 'Input', name: 'Input', type: 'Candle', maxLinks: 1 }],
  outPorts: [{ id: 'Output', name: 'Output', type: 'Indicator' }],
}));

// 2) 編集可能なダイアグラム + パレットツールボックス（それぞれが自身の要素に描画されます）。
const diagram = new StockSharpDiagram({ div: canvasHost, catalog, showFullscreenButton: true });
const palette = new StockSharpPalette({ div: paletteHost, catalog });

// 3) パレットからノードを追加: ダブルクリック、またはキャンバスへのネイティブなドラッグ&ドロップ。
palette.on('nodeActivated', ({ node }) => diagram.dropNodeFromPalette(node.id, centerX, centerY));
canvasHost.addEventListener('drop', event => {
  const { typeId } = JSON.parse(event.dataTransfer.getData(PALETTE_DRAG_MIME) || '{}');
  if (typeId) diagram.dropNodeFromPalette(typeId, event.clientX, event.clientY);
});

// 4) 初期スキームを読み込み、編集に反応します。
diagram.load(
  [new DiagramNode({ id: 'c', typeId: 'candles', name: 'Candles', x: 60, y: 120 })],
  [],
);
diagram.on('linkValidation', ({ allowed, reason }) => { if (!allowed) console.log('拒否:', reason); });
diagram.zoomToFit();
```

カタログを手作業で構築する代わりに、パレット JSON（読み取り専用の埋め込みが取得するものと同じ `designer-palette.json`）を渡すこともできます。その際、`socketTypes` は `PortType` に、`elements` は `Node` に変換されます。

読み取り専用の埋め込みを再構築せずにエディタへ変えるには、それが返すハンドルを利用します。`.diagram` が完全なインスタンスです:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const handle = await renderScheme(host, '/data/designer-palette.json', scheme);
handle.diagram.setReadOnly(false);   // 編集を有効化
```

## 関連項目

- [JavaScript ダイアグラム](../javascript_diagram.md)
- [イベントと API](events.md)
- [JavaScript チャート](../charts/javascript_charts.md)
