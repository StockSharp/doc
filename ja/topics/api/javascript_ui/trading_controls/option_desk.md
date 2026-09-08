# オプションデスク

`OptionDeskWidget` は、オプションチェーンの 1 シリーズを表示します。左にコール、右にその鏡像としてプット、その間に権利行使価格と本質的価値が並びます。出来高、建玉、ボラティリティは数値だけでなくバーでも表示されるため、チェーンは値だけでなく形からも読み取れます。

![権利行使価格を挟んでコール側とプット側を並べたオプションデスク](../../../../images/javascript_controls_option_desk.png)

## 作成と更新

```ts
import {
  OptionDeskWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const desk = OptionDeskWidget.create(
  document.querySelector<HTMLElement>('#option-desk')!,
  {},
  { host },
);

desk.update(
  [{
    strike: 68_000,
    call: {
      symbol: 'BTC-68000-C',
      bid: 1_240,
      ask: 1_265,
      last: 1_250,
      theoretical: 1_248,
      volume: 320,
      openInterest: 1_480,
      ivBid: 0.42,
      ivAsk: 0.44,
      ivLast: 0.43,
      historicalVolatility: 0.39,
    },
    put: {
      symbol: 'BTC-68000-P',
      bid: 820,
      ask: 845,
      volume: 210,
      openInterest: 960,
      ivLast: 0.47,
    },
  }],
  {
    assetPrice: 68_420,
    timeToExpiry: 0.08,
    riskFree: 0.05,
    dividend: 0,
  },
);
```

依存関係は `host` だけで、任意の依存関係はありません。`create` の第 2 引数はパネルの状態ですが、このコントロールは使用しません。

`update(strikes, context)` はチェーン全体を置き換えます。2 つの引数は必ず一緒に渡します。チェーンと原資産価格は 1 つの観測結果であり、別々に更新すると、すでに動いた価格を基に計算されたグリークスが表示されてしまうからです。`context` は任意で、既定では空です。

## チェーンのコンテキスト

`OptionChainContext` は、シリーズを何を基準に評価するかを表します。`assetPrice` は原資産価格、`timeToExpiry` は満期までの残存期間（年単位）、`riskFree` と `dividend` は小数で表した金利です（`0.05` は 5 パーセントを意味します）。`assetPrice` がなくてもデスクは気配値を表示し続けますが、本質的価値はゼロになり、行は「イン・ザ・マネー」と「アウト・オブ・ザ・マネー」に分かれません。`assetPrice` または `timeToExpiry` がない場合、グリークスは計算されず、セルはゼロではなく空のままになります。

## グリークス

グリークスは 2 つの経路のいずれかで届きます。ホストが自分で計算する場合は、権利行使価格の各サイドに完成した `greeks` オブジェクトを渡し、デスクは受け取ったものをそのまま表示します。ホストがボラティリティを送る場合は、`ivLast`、`ivBid`、`ivAsk`、`historicalVolatility` の順で最初に利用できる値を使い、ブラック・ショールズでその場で計算します。どちらか一方が他方のフォールバックというわけではなく、これらはホスト側の 2 つの形式です。

小数点以下の桁数はデータに合わせて選ばれます。その列の最小値に対して有効数字 4 桁になるようにし、下限は 2 桁、上限は 8 桁です。桁数は列ごとに 1 つ、しかも両サイドで共通なので、デルタが `0.0000` になることはなく、ガンマとその鏡像の列は同じ表記になります。

## 列と表示

列の順序は権利行使価格から外側に向かって決まっています。ボラティリティと気配値が中央寄り、グリークスが端です。プット側はまったく同じものを逆順に並べます。並べ替えは権利行使価格の昇順で、チェーンははしごのように読めます。

既定では `callRho`、`callTheta`、`callHv`、`callTheor` と、その鏡像である `putRho`、`putTheta`、`putHv`、`putTheor` の列が非表示です。これらはテーブルのコンテキストメニューから戻せます。

バーのスケールは種類ごとに異なります。出来高と建玉はサイドごとに別々に計算されます。コールとプットは取引される規模が異なるためです。ボラティリティは両サイドをまとめて 1 つのスケールで表示します。そうしないと、サイド間の偏りが見えなくなるからです。バーは canvas を使わず、要素の幅で描画されます。

行には、原資産価格より低い権利行使価格には `option-itm-call`、それ以外には `option-itm-put` のクラスが割り当てられます。`assetPrice` がない場合は `option-row` だけです。ボラティリティは小数 2 桁のパーセントで、価格はパッケージ共通の価格書式で表示されます。

デスクは設定を保存しません。`host.preferences` と `host.cache` へのアクセスはなく、列の構成と並べ替えは現在のインスタンス内にのみ存在します。

## ホストの役割

表示されるテキストはすべて `host.t` から取得されます。パネルの見出し、列見出し、テーブルが空のときの表示、コンテキストメニューの項目が該当します。閉じるボタンは `host.close()` を呼び出します。パネルが自分自身を削除することはありません。作成時にコントロールは `host.register(this)` を、`dispose()` 時には `host.unregister(this)` を呼び出します。サイドパネルのボタンはチェーンを XLSX に出力します。

コントロールはデータを購読せず、注文も送りません。チェーンとコンテキストはホストが `update` メソッドで渡します。

## 公開メソッド

- `OptionDeskWidget.create(hostEl, state, deps)` — パネルを構築してコンテナーに追加します。
- `update(strikes, context)` — チェーンと評価コンテキストを置き換えます。
- `rows()` — デスクが保持している形のまま行を返します。バーのスケールと本質的価値が計算済みの状態です。
- `dispose()` — リソースを解放し、ホストへの登録を解除します。
- `OptionDeskWidget.TYPE` — コントロール種別の識別子、`ControlTypes.OptionDesk`。

## エクスポートされる関数

計算部分はパネルとは別に利用できます。

- `scaleChain(strikes, context)` — チェーンを 1 度走査して、バー用の最大値と各権利行使価格の本質的価値を計算します。
- `sideGreeks(row, which, context)` — 権利行使価格の片側のグリークス。ホストから渡されたもの、またはそのボラティリティから計算したものを返します。どちらも不可能な場合は `null` です。
- `greekPlaces(values)` — 値の列に対する小数点以下の桁数。
- `greekScales(rows, context)` — 各グリークの桁数。チェーンの両サイドをまとめて測ります。

## 関連項目

- [JavaScript トレーディングコントロール](../trading_controls.md)
- [ウォッチリスト](watchlist.md)
- [板情報](order_book.md)
- [ポジション](positions.md)
