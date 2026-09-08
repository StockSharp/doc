# ボラティリティスマイル

`OptionSmileWidget` は、オプション 1 シリーズのボラティリティスマイルを描画します。権利行使価格ごとのコールとプットのインプライドボラティリティを、同じスケール上に 2 本の線として表示します。グラフの描画には、peer 依存関係として宣言されている `@stocksharp/chart` パッケージのエンジンを使います。

![オプションチェーンの権利行使価格ごとのボラティリティスマイル](../../../../images/javascript_controls_option_smile.png)

## 作成と更新

```ts
import {
  OptionSmileWidget,
  type OptionStrike,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const smile = OptionSmileWidget.create(
  document.querySelector<HTMLElement>('#smile')!,
  {},
  { host },
);

const chain: OptionStrike[] = [
  { strike: 67_000, call: { ivLast: 0.52 }, put: { ivBid: 0.55, ivAsk: 0.57 } },
  { strike: 67_500, call: { ivLast: 0.48 }, put: { ivLast: 0.50 } },
  { strike: 68_000, call: { ivBid: 0.45, ivAsk: 0.47 }, put: { ivLast: 0.47 } },
  { strike: 69_000, call: { ivLast: 0.49 }, put: {} },
];

smile.update(chain, { assetPrice: 68_120 });
```

依存関係は `host` だけで、`OptionSmileDeps` インターフェイスに他のフィールドはありません。`create` の第 2 引数はインスタンスの保存状態ですが、スマイルはこれを読み取らず、書き込みもしません。

`update` はチェーン全体を置き換えます。第 2 引数の `context` は任意で、既定では空のオブジェクトです。オプションデスクと同じ `OptionChainContext` 型で渡されますが、スマイルがそこから必要とするのは原資産価格 `assetPrice` だけです。

静的プロパティ `OptionSmileWidget.TYPE` は `ControlTypes.OptionSmile`、つまり識別子 `optionSmile` と等しくなります。

## データと表示

権利行使価格は昇順に並べ替えられ、`strike` が数値でないエントリーは破棄されます。各サイドのボラティリティは `ivLast` から取得し、約定がなかった場合は `ivBid` と `ivAsk` の平均を使います。有限の正の値だけが対象です。いずれも得られない場合、ポイントは描画されません。権利行使価格は軸上に残り、線は途切れます。これにより、片側からしか気配が出ていない権利行使価格が見て取れます。グラフ上の値はパーセントで表示されます。

権利行使価格の軸は `ordinal` モードで動作します。刻みは数値どうしの間隔ではなく、上場されている一覧に沿って等間隔になるため、67_500 と 68_000 の間の抜けが空白になることはありません。軸のラベルと十字カーソルのラベルは、同じ価格フォーマッターが生成します。

原資産価格は線としては描画されません。オーディナル軸にはそのための刻みがないからです。代わりに、`Call` と `Put` のキーの隣にある凡例にテキストとして表示されます。グラフ上にカーソルを合わせると、権利行使価格とその位置における両サイドの値を示す行が表示されます。スマイルは 2 本の曲線の間隔で読むものなので、両方が表示されます。

気配が付いた権利行使価格が 1 つもない間は、グラフの代わりに `NoOptions` キーのテキストを表示するプレースホルダーが出ます。

## コントロールの役割とホストに残る役割

コントロールは、最初のデータが届いた時点で自らグラフを作成し、色、フォント、グリッド色を `host.presentation.canvasPalette()` から取得し（`up` がコール、`down` がプット）、`ResizeObserver` でコンテナーのサイズを監視してキャンバスを合わせ、表示リセットのボタンと、`host.close()` を呼び出すパネルの閉じるボタンを処理します。表示されるテキストはすべて `host.t` から取得します。`OptionSmile`、`ResetView`、`ClosePanel`、`ImpliedVolatility`、`Call`、`Put`、`OptionChain`、`NoOptions`、`Underlying` です。

データを供給するのはホストです。スマイルは市場データを購読せず、ボラティリティも計算せず、シリーズどうしを区別もしません。`update` に渡されたものがそのまま描画されます。コントロールは `host.preferences` に独自の設定を保存せず、キーも持ちません。

## 公開メソッド

- `update(strikes, context)` — チェーンと、それが取得されたコンテキストを表示します。
- `resetZoom()` — 拡大後にチェーン全体の表示へ戻します。
- `chart()` — グラフオブジェクト（`IChartApi`）を返します。まだ作成されていない場合は `null` です。同じキャンバスに 2 本目のシリーズやマーカーを追加するホストが必要とします。
- `dispose()` — サイズのオブザーバーを解除し、グラフを削除し、`host.unregister` を呼び出してルート要素を取り除きます。

## 補助関数

パッケージは、描画のもとになっている関数もエクスポートしています。単独で利用できます。

- `sideVolatility(side)` — そのサイドのボラティリティ。不明な場合は `null`。
- `sortedChain(strikes)` — 描画順に並べたチェーン。権利行使価格の昇順で、不正なエントリーは含まれません。
- `toSmileSeries(chain, put)` — 片側をグラフ用のポイント列にしたもの。

## 関連項目

- [JavaScript トレーディングコントロール](../trading_controls.md)
- [オプションデスク](option_desk.md)
- [エクイティカーブ](equity.md)
- [板情報](order_book.md)
