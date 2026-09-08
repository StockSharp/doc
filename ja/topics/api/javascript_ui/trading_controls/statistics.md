# 統計

`StatisticsWidget` は、ストラテジーの統計パラメーターを表示するテーブルです。損益、ドローダウン、約定回数、レイテンシーなどを扱います。パネルは読み取り専用で、パラメーター 1 つにつき 1 行、行はそのパラメーターが属する領域ごとにグループ化されます。

![指標をグループ化して表示した実行結果の統計パネル](../../../../images/javascript_controls_statistics.png)

## 作成と更新

```ts
import {
  StatisticsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const statistics = StatisticsWidget.create(
  document.querySelector<HTMLElement>('#statistics')!,
  {},
  { host },
);

statistics.update([
  {
    key: 'NetProfit',
    category: 'pnl',
    categoryText: '損益',
    order: 1,
    name: '純利益',
    description: '実行結果',
    value: 11_055.75,
  },
  {
    key: 'MaxProfitDate',
    category: 'pnl',
    categoryText: '損益',
    order: 2,
    name: '最大値の日付',
    value: '2024-03-26T07:30:00Z',
  },
  {
    key: 'NetProfit',
    category: 'pnl',
    categoryText: '約定',
    order: 100,
    name: '約定回数',
    value: 1_340,
  },
]);
```

依存関係 `StatisticsDeps` は、必須フィールド `host` ただ 1 つだけで構成されます。コントロールに操作ハンドラーはありません。統計はストラテジーが生成するものであり、パネル内で取り消したり、再読み込みしたり、編集したりするものは何もないからです。`create` の第 2 引数はパネルの保存状態ですが、このコントロールは使用しません。

`update` は行のセット全体を置き換えます。ストラテジーは自身のパラメーターを 1 つのテーブルとして公開するため、セットから消えた行は「更新されなくなった」ではなく「存在しなくなった」と見なされます。行の同一性は `key` フィールドで判断されます。

## 行と並び順

行は `StatisticRow` 型で表されます。

| フィールド | 用途 |
|---|---|
| `key` | パラメーターの安定した識別子。 |
| `category` | 言語に依存しないグループのキー。 |
| `categoryText` | 表示用のグループ見出し。ない場合は `category` が使われます。 |
| `order` | レジストリ内でのパラメーターの位置。 |
| `name` | ローカライズされたパラメーター名。 |
| `description` | ローカライズされた説明。名前セルのツールチップとして表示されます。 |
| `value` | 数値、日付、または文字列。`null` はパラメーターがまだ計測されていないことを示します。 |

デスクトップ版のテーブルはストラテジーのパラメーターをリフレクションで取得して行を作りますが、ブラウザーにそうした仕組みはありません。そのため行は、名前と説明が翻訳済みの完成した形でホストから届きます。

グループ化は `categoryText` ではなく `category` で行われます。翻訳された見出しでグループ化すると、言語を切り替えるたびにテーブルが組み替わってしまうからです。グループの順序は、そのパラメーター群の中で最小の `order` によって決まります。そのため損益はドローダウンより上に、ドローダウンは約定回数のカウンターより上に並びます。名前や値で並べ替えると、一緒に読むべきパラメーターが離れてしまいます。

表示される列は `Name` と `Value` の 2 つです。`category` と `order` の列も宣言されていますが非表示です。これらはグループ化と並べ替えのために必要なだけで、読み手に伝える情報はありません。エクスポートに含まれるのも、表示されている 2 列だけです。

## 値の書式

値セルのテキストは、エクスポートされた関数 `formatStatistic(value)` が生成します。

```ts
import { formatStatistic } from '@stocksharp/trading-controls';

formatStatistic(11_055.756); // '11055.76'
formatStatistic(1_340);      // '1340'
formatStatistic('2024-03-26T07:30:00Z'); // '2024-03-26'
formatStatistic(null);       // ''
```

数値は小数第 2 位に丸められ、末尾のゼロは表示されません。日付は日付部分のみです。この種のパラメーターは実行全体を表すものであり、時刻はノイズにしかならないからです。文字列が日付として認識されるのは `YYYY-MM-DD` で始まる場合だけで、それ以外はテキストのまま扱われます。値がない場合は空のセルになります。ゼロにすると、計測された結果として読まれてしまうからです。

並べ替えは元の値に対して行われるため、数値は文字列ではなく数値として並べ替えられます。

## コントロールの役割とホストの役割

コントロールは、表示されるテキストをすべて `host.t` からホストに問い合わせます。列見出し、パネルの見出し、テーブルが空のときの表示、テーブルのコンテキストメニュー項目も含まれます。閉じるボタンは `host.close` を呼び出し、エクスポートはテーブルを XLSX として出力します。作成時にインスタンスは `host.register` で登録され、`dispose` 時に `host.unregister` で登録解除されます。

コントロールは `host.preferences` に独自の設定を保存しません。データの取得も行いません。行はホストが `update` の呼び出しで供給します。

パネル種別の識別子は `StatisticsWidget.TYPE` から取得でき、その値は `ControlTypes.Statistics` と等しくなります。

## 公開メソッド

- `StatisticsWidget.create(hostEl, state, deps)` — 指定したコンテナーにパネルを作成します。
- `update(rows)` — 統計行のセット全体を置き換えます。
- `dispose()` — リソースを解放します。

パネルは並べ替え、複数行の選択、コンテキストメニュー、XLSX へのエクスポートにも対応しています。

## 関連項目

- [JavaScript トレーディングコントロール](../trading_controls.md)
- [ポジション](positions.md)
- [約定履歴](trade_history.md)
- [有効注文](active_orders.md)
