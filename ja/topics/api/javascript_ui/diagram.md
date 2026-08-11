# JavaScript ダイアグラム

[StockSharp JS Diagram](https://github.com/StockSharp/JS-Diagram) は、依存関係のないスタンドアロンのブラウザーコンポーネントで、[Designer](../../designer.md) のビジュアルなストラテジースキーム（接続された要素の同じブロックダイアグラム）を HTML の `canvas` 上に描画します。npm 上では [@stocksharp/diagram](https://www.npmjs.com/package/@stocksharp/diagram) として公開されており、StockSharp の各 Web サイトで表示される読み取り専用のストラテジーダイアグラムを支えています。

ストラテジーは **スキーム** として記述されます。すなわち、型付きの *ポート* を介して相互に配線された *ノード*（ローソク足ソース、インジケーター、条件、注文などの要素）の集合です。コンポーネントは、そのスキームと *パレット*（要素タイプ、そのポート、色のカタログ）を受け取って描画します。

## ライブデモ

以下のダイアグラムは、このページで実際に動作しているエンジンです。最小構成の「データソース → インジケーター → チャート」というストラテジーの骨格です。キャンバスをドラッグしてパンし、ホイールでズームし、展開ボタンを押すと全画面で開きます。

```diagram-demo sma
```

3 つのブロックは、**インジケーター**（単純移動平均）に供給する **ローソク足** ソースです。ローソク足とインジケーターの出力はどちらも **チャート** 要素上に描画されます。これは Designer における最小の完全なパターンです。データを生成し、変換し、可視化します。

## インストール

npm からパッケージをインストールします。

```bash
npm install @stocksharp/diagram
```

続いて ES モジュールをインポートします。読み取り専用の埋め込みには `import { renderScheme } from '@stocksharp/diagram/embed'` を、[インタラクティブエディタ](diagram/editor.md) には `import { StockSharpDiagram } from '@stocksharp/diagram'` を使用します。

## ダイアグラムの埋め込み

コンポーネントは `@stocksharp/diagram/embed` エントリーから `renderScheme(host, paletteUrl, scheme)` を公開します。ホスト要素、パレット JSON の URL、そして `nodes` と `links` から構築したスキームを渡します。

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const scheme = {
  nodes: [
    { id: 'candles', typeId: 'CandleElement',    name: 'Candles', x: 60,  y: 130 },
    { id: 'sma',     typeId: 'IndicatorElement', name: 'SMA',     x: 340, y: 60  },
    { id: 'chart',   typeId: 'ChartElement',     name: 'Chart',   x: 620, y: 130 },
  ],
  links: [
    { from: 'candles', fromPort: 'Output', to: 'sma',   toPort: 'Input' },
    { from: 'sma',     fromPort: 'Output', to: 'chart', toPort: 'Input' },
    { from: 'candles', fromPort: 'Output', to: 'chart', toPort: 'Input' },
  ],
};

renderScheme(document.getElementById('diagram'), '/data/designer-palette.json', scheme);
```

各ノードの `typeId` はパレット内に存在している必要があります。不明なタイプはプレースホルダーのブロックとして描画されます。ポートは `key` によって参照され、ソースポートの型がターゲットポートの型と互換性がある場合にリンクが有効になります。`renderScheme` は読み取り専用です。エンジンはレイアウトとテーマ適用（ページのライト/ダーク設定に従います）を行い、閲覧者にパン・ズーム・展開を許可しますが、スキームの編集は行いません。

同じコンポーネントは、完全な **エディター** としても動作します。パレットから要素をドラッグし、ポートを接続し、ノードの編集や削除、元に戻す／やり直すを行えます。[インタラクティブエディタ](diagram/editor.md) と [イベントと API](diagram/events.md) を参照してください。

## 関連項目

- [インタラクティブエディタ](diagram/editor.md)
- [イベントと API](diagram/events.md)
- [JavaScript チャート](charts.md)
- [Designer](../../designer.md) — デスクトップ版のビジュアルストラテジーエディター
- [JS-Diagram リポジトリ](https://github.com/StockSharp/JS-Diagram)
