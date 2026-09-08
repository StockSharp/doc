# 描画ツール

`DrawingController` は、チャートに手作業で描画を加えるレイヤーです。ライン、図形、フィボナッチのレベル、ポジションの下書きを扱います。コントローラーは図形を純粋な JSON オブジェクトとして保持し、それをキャンバスのプリミティブに結び付け、あらゆる変更をチャートの取り消しスタックを通し、マウスによる段階的な作図も引き受けます。

## 組み込み

このレイヤーは `@stocksharp/chart` パッケージの独立したエントリーポイントとして提供されます。

```ts
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';
```

このエントリーポイントをインポートすると、組み込みのすべての描画タイプが共通のカタログ `drawingDefinitionRegistry` に登録されます。

## 作成と更新

コントローラーが必要とするのはチャートだけです。コマンドスタックも既定では同じチャートから取得するため（`chart.commandStack()`）、取り消しとやり直しはチャート上の他の操作と一緒に機能します。

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';

const chart = createChart(document.getElementById('chart')!, { timeScale: { timeVisible: true } });
chart.addSeries(CandlestickSeries, {}).setData(candles);

const drawings = new DrawingController({ chart });

// 水平ライン: 点は 1 つ、時刻は秒単位の Unix 時刻です。
const level = drawings.create(
  BuiltInDrawingType.HorizontalLine,
  [{ time: 1_712_000_000, price: 68_000 }],
  { options: { color: '#f5c542', lineWidth: 2 } },
);

// 2 点によるトレンドライン。サブペインを指定する場合は paneId を使います。
drawings.create(
  BuiltInDrawingType.TrendLine,
  [
    { time: 1_712_000_000, price: 67_400 },
    { time: 1_712_600_000, price: 69_150 },
  ],
  { paneId: 'main' },
);

drawings.updateOptions(level.id, { lineWidth: 3 });
drawings.setLocked(level.id, true);

// セットのスナップショットは zOrder 順に並んで届きます。
drawings.subscribe(items => console.log(items.length));

chart.commandStack().undo();
```

`create` は不足しているフィールドを補います。`paneId` は既定で `main`、`visible` は `true`、`locked` は `false`、`zOrder` は現在の最大値より 1 大きい値になり、オプションはそのタイプの `defaultOptions` の上に重ねられます。`add` は完成したインスタンスをそのまま挿入し、`duplicate` は既存のものを複製し、`remove` と `clear` は削除します。これらの呼び出しはいずれも、履歴にちょうど 1 つの取り消し可能なコマンドを積みます。

`update` は任意のフィールドの組み合わせ（`points`、`options`、`paneId`、`visible`、`locked`、`zOrder`）を変更します。`updateOptions`、`setVisible`、`setLocked`、`moveToPane` は、よくある場合のための短縮形です。書き込みの前にインスタンスは正規化されます。点とオプションは JSON 互換かどうかを検証されて凍結され、点の数はそのタイプのスキーマと、ペインはチャートに存在するペインと照合されます。

## 組み込みのタイプ

識別子は `BuiltInDrawingType` にまとめられており、その文字列値がそのまま、保存された図形の `type` フィールドになります。

| 定数 | 値 | 点の数 | オプション |
|---|---|---|---|
| `HorizontalLine` | `horizontal-line` | 1 | `LineDrawingOptions` |
| `VerticalLine` | `vertical-line` | 1 | `LineDrawingOptions` |
| `TrendLine` | `trend-line` | 2 | `LineDrawingOptions` |
| `Ray` | `ray` | 2 | `LineDrawingOptions` |
| `Rectangle` | `rectangle` | 2 | `RectangleDrawingOptions` |
| `Text` | `text` | 1 | `TextDrawingOptions` |
| `Note` | `note` | 1 | `TextDrawingOptions` |
| `FibonacciRetracement` | `fibonacci-retracement` | 2 | `FibonacciDrawingOptions` |
| `Measure` | `measure` | 2 | `MeasureDrawingOptions` |
| `LongPosition` | `long-position` | 3 | `PositionDrawingOptions` |
| `ShortPosition` | `short-position` | 3 | `PositionDrawingOptions` |

オプションのセットは用途によって異なります。

- `LineDrawingOptions` — `color`、`lineWidth`（範囲は (0, 20]）、`lineStyle`（0〜4）。
- `RectangleDrawingOptions` — 上記に加えて、塗りつぶし用の `fillColor`。
- `TextDrawingOptions` — `text`（最大 10,000 文字、改行も反映されます）、`color`、`backgroundColor`、`borderColor`、`borderWidth`、`fontSize`、`fontFamily`、`padding`。`Note` が `Text` と違うのは既定値だけで、背景、枠線、広めの余白が付きます。
- `FibonacciDrawingOptions` — `levels`（範囲 [-5, 5] の値を 2 個から 32 個。重複は取り除かれ、リストは並べ替えられます）、`labelsVisible`、`extendRight`、および `color`、`lineWidth`、`lineStyle`、`fillColor`、`fontSize`。
- `MeasureDrawingOptions` — `color`、`lineWidth`、`fillColor`、`labelColor`、`labelBackgroundColor`、`fontSize`。ラベルには、選択した区間の価格の変化、パーセント、経過時間が表示されます。
- `PositionDrawingOptions` — `entryColor`、`targetColor`、`stopColor`、`targetFillColor`、`stopFillColor`、`textColor`、`lineWidth`、`fontSize`、`quantity`。3 つの点は順に、エントリー、ターゲット、ストップを表し、これらからラベルの利益、リスク、R:R 比が計算されます。

型の検証を通らないオプション値は例外になります。不正な線幅や空の色を持つ図形を保存することはできません。

## マウスによる作図

段階的な入力はコントローラー自身が受け持ちます。チャートを描画モードに切り替え、クリックと十字カーソルを購読します。

```ts
drawings.subscribeCreation(state => {
  if (state === null) return;                    // 作図が完了または中止されました
  console.log(state.name, state.points.length, state.minimumPoints, state.maximumPoints);
});

drawings.beginCreation(BuiltInDrawingType.Rectangle, { options: { color: '#26a69a' } });

// 図形が必要な点の数に達するまでは、Esc で中止できます。
document.addEventListener('keydown', event => {
  if (event.key === 'Escape') drawings.cancelCreation();
});
```

クリックのたびに、マグネットを通した点が追加されます。カーソルの移動は下書きを更新し、下書きは完成した図形と同じプリミティブで描画されますが、履歴には入りません。ペインは最初のクリックで固定され、他のペインでのクリックは無視されます。そのタイプが許す最大数まで点が集まると、作図は自動的に完了し、通常の図形が作成されます。`finishCreation` は作図を早めに終了し、点が最小数に満たない場合は `null` を返します。`cancelCreation` は下書きを破棄し、`creation` は現在のスナップショット `DrawingCreationSnapshot` を返します。

## バーへのスナップ

マグネットは、現在のペインのシリーズの値に点を引き寄せます。計算は画面座標で、候補までの垂直距離に基づいて行われます。

```ts
import { DrawingMagnetMode } from '@stocksharp/chart/drawings';

const drawings = new DrawingController({
  chart,
  magnet: { mode: DrawingMagnetMode.Weak, maxDistance: 12 },
});

drawings.applyMagnetOptions({ mode: DrawingMagnetMode.Strong });
console.log(drawings.magnetOptions());
```

`DrawingMagnetMode.None` はスナップを無効にし、`Weak`（既定のモード）は `maxDistance`（既定は 10 CSS ピクセル）の範囲内でだけ引き寄せ、`Strong` は常に最も近い値へ引き寄せます。作図中に設定を変更すると、プレビューの点はすぐに再計算されます。

## 保存と復元

`DrawingInstance` は意図的に実行時のオブジェクトを含まないため、描画のセットはそのままシリアライズできます。

```ts
import type { DrawingInstance } from '@stocksharp/chart/drawings';

const saved = JSON.stringify(drawings.drawings());

const result = drawings.replaceAll(JSON.parse(saved) as DrawingInstance[], { unknownType: 'skip' });
console.log(result.restored.length, result.skipped);
```

`replaceAll` はセット全体を置き換えます。まず入力されたすべてのインスタンスが検証され（識別子の重複はエラーです）、次に古い図形がチャートから外され、新しいものが追加されます。1 つでも配置できなかった場合は、直前の状態が復元されます。未知の `type` は、ポリシーが `skip`（既定）なら理由 `unknown-type` とともに `skipped` に入り、`error` なら復元を中断します。復元はコマンド履歴を消去するため、トランザクションの内側で呼び出すことはできません。

## 独自の描画タイプ

タイプのカタログは拡張できます。定義を記述して、プリミティブへのバインディングを返すだけで済みます。選択、ハンドル、ドラッグを備えた既製の外枠は `createInteractiveDrawingBinding` が提供します。

```ts
import { createInteractiveDrawingBinding, registerDrawing } from '@stocksharp/chart/drawings';

registerDrawing({
  type: 'price-band',
  name: 'Price Band',
  points: { min: 2, max: 2 },
  defaultOptions: { color: '#4a9eff' },
  normalizeOptions: options => Object.freeze({ color: String(options.color).trim() }),
  create: (instance, events) => createInteractiveDrawingBinding(instance, events, {
    draw(context) {
      const [first, second] = context.points;
      if (second === undefined) return;
      context.context.strokeStyle = String(context.instance.options.color);
      context.context.strokeRect(
        context.plot.x, Math.min(first.y, second.y),
        context.plot.width, Math.abs(second.y - first.y),
      );
    },
    hitTest(point, context) {
      const [first, second] = context.points;
      if (second === undefined) return null;
      return point.y >= Math.min(first.y, second.y) && point.y <= Math.max(first.y, second.y)
        ? { cursor: 'move' }
        : null;
    },
  }),
});

drawings.create('price-band', [
  { time: 1_712_000_000, price: 67_800 },
  { time: 1_712_600_000, price: 68_900 },
]);
```

`draw` は、画面上の点、描画領域の矩形、テーマ、スケール係数、選択状態を受け取ります。`hitTest` は、カーソルが図形の本体に入っているかどうかを返します。任意の `autoscaleInfo` と `handleColor` は、自動スケーリングへの参加とハンドルの色を指定します。`normalizeOptions` はモデルへの書き込みのたびに呼ばれます。オプション値の検証を行うべき唯一の場所です。

本体や個々の点のドラッグは、`preview`（中間状態。履歴には書き込まれません）、`commit`（「Edit drawing」という 1 つのコマンド）、`cancel`（ジェスチャー前の状態への復帰）のイベントを通じて行われます。ロックされた図形（`locked`）はドラッグできず、ハンドルも表示されません。

カタログは直接操作することもできます。`unregisterDrawing(type)`、`getDrawingDefinition(type)`、`getDrawingTypes()` があり、`DrawingDefinitionRegistry` を使えば独自のカタログを用意して、`registry` パラメーターでコントローラーに渡せます。

## 公開メソッド

`DrawingController`:

- `drawings()`、`get(id)`、`has(id)` — 現在のセットの読み取り。
- `create(type, points, options?)`、`add(instance)`、`duplicate(id, duplicateId?)` — 図形の追加。
- `update(id, patch)`、`updateOptions(id, patch)`、`setVisible(id, visible)`、`setLocked(id, locked)`、`moveToPane(id, paneId)` — 変更。
- `remove(id)`、`clear()` — 削除。
- `beginCreation(type, options?)`、`finishCreation()`、`cancelCreation()`、`creation()` — マウスによる作図。
- `magnetOptions()`、`applyMagnetOptions(patch)` — バーへのスナップ。
- `replaceAll(instances, options?)` — 保存したセットの復元。
- `subscribe(listener)` / `unsubscribe(listener)`、`subscribeCreation(listener)` / `unsubscribeCreation(listener)` — 購読。
- `dispose()` — リソースを解放します。

コンストラクターは `chart`（必須）のほか、`registry`、`commandStack`、`idFactory`、`magnet` を受け取ります。

エントリーポイントは、このレイヤーの残りの部分もエクスポートしています。スナップを自前で計算するための `DrawingMagnet`、`createInteractiveDrawingBinding` とセットの `InteractiveDrawingPrimitive`、検証用の関数 `normalizeDrawingInstance` と `normalizeDrawingOptions`、定義の既製セット `builtInLineDrawingDefinitions`、`builtInShapeDrawingDefinitions`、`builtInAnalysisDrawingDefinitions`、`builtInPositionDrawingDefinitions`、そしてそれらを独自のカタログに登録するための対になる関数 `registerBuiltInLineDrawings`、`registerBuiltInShapeDrawings`、`registerBuiltInAnalysisDrawings`、`registerBuiltInPositionDrawings` です。

## 関連項目

- [JavaScript チャート](../charts.md)
- [ローソク足](candlestick.md)
- [インジケーター](indicators.md)
- [履歴のバックフィル](backfill.md)
