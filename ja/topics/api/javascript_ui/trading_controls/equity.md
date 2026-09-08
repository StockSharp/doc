# エクイティカーブ

`EquityWidget` は、実行結果の累積 P&L を時系列のグラフとして描画します。パネルは識別子 `equity`（`ControlTypes.Equity`）で登録されており、カーブ自体は peer 依存関係として組み込まれる `@stocksharp/chart` のエンジンが描画します。

![実行結果に基づくエクイティカーブ](../../../../images/javascript_controls_equity.png)

## 作成と更新

```ts
import {
  EquityWidget,
  type PnlPoint,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const equity = EquityWidget.create(
  document.querySelector<HTMLElement>('#equity')!,
  {},
  { host },
);

const start = Date.now() - 3 * 60 * 60 * 1000;

const points: PnlPoint[] = [
  { time: start, value: 0 },
  { time: start + 45 * 60 * 1000, value: 320.5 },
  { time: start + 95 * 60 * 1000, value: -140.25 },
  { time: Date.now(), value: 1_180.75 },
];

equity.update(points);
```

`EquityDeps` の依存関係は `host` だけです。カーブ上で実行できるものは何もないため、パネルに操作ハンドラーはありません。`create` の第 2 引数であるインスタンスの保存状態は、このコントロールでは使用されません。パネルは独自の設定を、状態にも `host.preferences` にも保存しません。

`update` は実行結果を丸ごと置き換えます。カーブの値は累積であるため、それまでに渡したサンプルを含まないポイント集合は、以前の続きではなく別の実行結果と見なされます。

## データと描画

`PnlPoint` の `time` フィールドは unix ミリ秒です。エンジンは時刻を秒で扱うため、変換はコントロールが行います。ポイントは時刻順に並べ替えられ、`time` または `value` が数値でないサンプルは破棄されます。同じ秒に複数の値がある場合は最後のものが残ります。それが、その時点で実行結果が実際にどこにあったかを示すからです。

有効なポイントが 2 つに満たない間、パネルは `NoEquity` キーのメッセージを表示し、グラフは作成されず、`chart()` は `null` を返します。グラフ自体はコンストラクターではなく最初の描画時に構築されます。エンジンには、すでにページ上に配置されたコンテナーが必要だからです。領域のサイズ変更は `ResizeObserver` が監視し、エンジンの `resize` を呼び出します。

色、フォント、グリッド色は `host.presentation.canvasPalette()` から取得します。カーブは実行結果の最後の値によって着色され、値がゼロ以上なら `up`、負なら `down` になります。線の下の塗りつぶしは同じ色で、上端 28 %、下端 2 % まで抑えたものです。時間軸はブラウザーのタイムゾーンで時と秒を表示し、`Intl` が利用できない場合は UTC で表示します。

## ヘッダー、ツールチップ、ボタン

パネルのヘッダーには、実行結果の最後の値が `formatPnl` の書式で表示されます。符号付きで小数 2 桁です。色のクラスは `host.presentation.pnlClass` が返します。カーブの上には、ポインターの位置の値が表示されます。時点は `host.presentation.timeText` による文言で、その隣に値が並びます。ポインターがグラフから離れると、ツールチップは消去されます。

ヘッダーにある `ResetView` のツールチップが付いたボタンは `resetZoom` を呼び出し、閉じるボタンは `host.close()` を呼び出します。表示テキストは `Equity`、`ResetView`、`ClosePanel`、`PnLChart`、`NoEquity` の各キーで取得します。

カーブの形、色、スケールはコントロールが受け持ちます。ホストに残るのは、ラベルの言語、パレット、時点の書式、そしてポイントそのものの供給元です。コントロールは P&L の計算を行わず、データをどこからも要求しません。

## 公開メソッド

- `EquityWidget.create(hostEl, state, deps)` — パネルを作成し、そのルート要素をコンテナーに追加します。
- `EquityWidget.TYPE` — 識別子 `equity`。
- `update(points)` — 実行結果を丸ごと表示します。
- `resetZoom()` — 拡大後に実行結果全体の表示へ戻します。
- `chart()` — `IChartApi` のインスタンス。ホストがベンチマークのラインやドローダウンのマークなど独自の描画を追加するために使います。グラフが作成される前は `null` を返します。
- `dispose()` — サイズのオブザーバーを解除し、グラフを削除して、ホストへの登録を解除します。

`rootEl` プロパティはパネルのルート要素を返します。

## チャートエンジンの組み込み

`@stocksharp/chart` パッケージは個別にインストールします。

```bash
npm install @stocksharp/chart
```

ビルド済みバンドル `sstradingcontrols.js` にエンジンは含まれていません。そのため、バンドラーを使わないページでは、そのスクリプトを並べて読み込みます。

```html
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/indicators/dist/ssindicators.js"></script>
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/chart/dist/sschart.js"></script>
```

## 関連項目

- [JavaScript トレーディングコントロール](../trading_controls.md)
- [ポジション](positions.md)
- [約定履歴](trade_history.md)
- [JavaScript チャート](../charts.md)
