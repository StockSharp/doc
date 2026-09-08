# 複数チャート

`MultiChartWorkspace` は、独立した複数のチャートを 1 つのコンテナー内にグリッドで配置し、銘柄と時間足で連動させ、表示範囲と十字カーソルを同期させます。このクラスは、ワークスペースの他のコントローラー（ペイン、インジケーター、テンプレート、銘柄の比較、履歴のナビゲーション）とともに、エントリーポイント `@stocksharp/chart/workspace` から提供されます。

ワークスペースが所有するのは最上位のチャートだけです。インジケーターのペインは各チャートの内部構造にとどまり、セルとは見なされず、レイアウトにも同期にも参加しません。

## 作成と更新

コンテナーとチャートのファクトリーは必須です。ファクトリーは `{ id, index, host }` を受け取り、セルを返します。セルはチャート本体、任意のデータコントローラー、任意の解放関数（既定では `chart.remove()` が呼ばれます）で構成されます。

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { ChartDataController, type IChartDataSource } from '@stocksharp/chart/data';
import { MultiChartWorkspace } from '@stocksharp/chart/workspace';

declare const dataSource: IChartDataSource;

const workspace = new MultiChartWorkspace({
  container: document.querySelector<HTMLElement>('#workspace')!,
  count: 4,
  columns: 2,                                   // null なら正方形に近い自動グリッド
  links: { symbol: true, resolution: false },   // 銘柄は共通、時間足はセルごと
  sync: { range: true, crosshair: true },
  createChart: ({ host }) => {
    const chart = createChart(host, { timeScale: { timeVisible: true } });
    const series = chart.addSeries(CandlestickSeries, {
      upColor: '#26a69a',
      downColor: '#ef5350',
    });
    const data = new ChartDataController({ chart, series, dataSource, initialCount: 300 });

    return {
      chart,
      data,
      dispose: () => {
        data.dispose();
        chart.remove();
      },
    };
  },
});

const [first] = workspace.cells();
await workspace.setSelection(first.id, { symbol: 'BTC@IMEX', resolution: '1h' });

workspace.subscribe(snapshot => {
  console.log(snapshot.activeId, snapshot.columns, snapshot.rows, snapshot.errors.length);
});
```

`setCount` と `setColumns` はグリッドのサイズを個別に変更し、`setLayout({ count, columns })` は 1 回の操作でまとめて変更します。コンテナーは CSS グリッドとして構成されます。元のスタイルは記憶され、`dispose` で復元されます。セルは 1 個から 64 個までで、最後の 1 個を削除することはできません。

## 連動と同期

`links` は、銘柄が変わったときに他のセルへ何を伝播させるかを表します。`symbol` と `resolution` は個別に有効化できます。ファクトリーが `data` を返さなかったセルは、連動に参加しません。

`sync` は、表示範囲（`range`）と十字カーソルの位置（`crosshair`）の伝播を有効にします。範囲は適用後に受け側のチャートから読み直されます。履歴が短いセルは要求されたウィンドウを切り詰めるため、スナップショットには実際に表示されているものが公開されます。

アクティブなセルは `activate` で指定できるほか、セル内での `pointerdown` と `focusin` によって自動的に切り替わります。`links` や `sync` を変更すると、アクティブなセルの現在の状態がただちに他のセルへ配られます。

同期のエラーは他のセルの動作を止めることなく `snapshot.errors` に蓄積されます。直近 32 件までです。各レコードは `cellId`、`kind`（`WorkspaceSyncErrorKind`: `selection`、`range`、`crosshair`、`lifecycle`）、`error` を持ちます。リストは `clearErrors` の呼び出しで消去されます。

## 公開メソッド

- `snapshot()` — 完全な状態。セルの数、列と行、アクティブなセル、`links`、`sync`、各セル、エラー。
- `cells()` — セルのスナップショット。`id`、`index`、`active`、`selection`、`visibleRange`、`crosshairTime`。
- `chart(id)` / `host(id)` — セルのチャートと DOM 要素。
- `add(id?)` — セルを追加します。引数がない場合、識別子は生成されます。
- `remove(id)` — セルを削除します。
- `setCount(count)`、`setColumns(columns)`、`setLayout(layout)` — グリッドを変更します。
- `activate(id)` — セルをアクティブにします。
- `setLinks(options)`、`setSync(options)` — 連動と同期を切り替えます。
- `setSelection(id, selection)` — セルの銘柄と時間足を設定し、連動に従って配ります。
- `clearErrors()` — 蓄積されたエラーを消去します。
- `subscribe(listener)` / `unsubscribe(listener)` — 状態スナップショットの購読。
- `dispose()` — セルを解放し、コンテナーのスタイルを復元します。

## このレイヤーのその他のコントローラー

- `PaneController` — チャートのペインを取り消し可能（undo/redo）に操作します。`resizePair`、`reorder`、`moveSeries`、`setState`、`toggleMinimized`、`toggleMaximized`。チャート共通のコマンドスタックを通して動作し、ペインの内容を作り直すことはありません。
- `IndicatorController` — 計算エンジンの上でインジケーターを検証付きで編集します。`update`、`setParameters`、`setSource`、`moveToPane`、`setPriceScale`、`setVisible`、`setOutputStyle`。すべての変更はコマンドスタックに入り、スナップショットにはパラメーターの定義、ソースの状態、出力のスタイルが含まれます。
- `IndicatorCatalogController` — インジケーターカタログの検索（テキスト、カテゴリー、お気に入りかどうかによる `search`）と、ホストが保存するお気に入り。`loadFavorites`、`setFavorite`、`toggleFavorite`。
- `IndicatorTemplateController` — 持ち運べるインジケーター設定のテンプレート。`create`、`replace`、`rename`、`remove`、`apply`、`load`。`apply` メソッドはパラメーター、ソース、表示状態、出力のスタイルを移しますが、適用先のペインと価格スケールは意図的に変更しません。
- `serializeIndicatorTemplates`、`deserializeIndicatorTemplates`、`normalizeIndicatorTemplateDocument`、`INDICATOR_TEMPLATE_SCHEMA_VERSION` — バージョン管理されたテンプレートドキュメントのシリアライズと検証。
- `CompareController` — 複数の銘柄を 1 つのチャートに重ねます。`add`、`remove`、`setPrimary`、`setColor`、`setVisible`、`reload`、`loadMoreBefore`、`legend`。正規化のモードは `setMode`（`CompareMode.Percentage` または `CompareMode.IndexedTo100`）で、時刻の揃え方は `setAlignment`（`CompareAlignment.Chart` または `CompareAlignment.PrimarySession`）で指定します。銘柄ごとに専用の `ChartDataController` と購読を持ちます。
- `ChartNavigator` — DOM に依存しない履歴のナビゲーション。`setRange`、`selectPreset`、`goToDate`、`cancel`。このコントローラーは足りない履歴のページを自分で読み込み（既定では 1 操作あたり最大 100 ページ）、限られた数のサンプル（既定は 600）から概観モデルを公開します。既製のプリセット `1D`、`5D`、`1M`、`3M`、`6M`、`YTD`、`1Y`、`5Y`、`All` は `defaultNavigatorPresets` が返します。操作の結果は `NavigatorNavigationOutcome`（`applied`、`clamped`、`page-limit`、`empty`、`cancelled`）で、日付の揃え方は `NavigatorDateAlignment` で表されます。

## 関連項目

- [JavaScript チャート](../charts.md)
- [履歴のバックフィル](backfill.md)
- [インジケーター](indicators.md)
