# レイアウトの保存

`ChartStatePersistence` は、チャートのレイアウト（オプション、ペイン、価格スケール、シリーズ、インジケーター、描画オブジェクト）を 1 つの検証済み JSON スナップショットにまとめ、そこから復元します。バーのデータはスナップショットに含まれません。保存されるのは構成であり、相場データはあなたのデータソースから届きます。

このレイヤーは、ストレージもキーの命名規則も所有しません。どこへ書き込むか（ファイル、バックエンド、`localStorage`、IndexedDB）、スナップショットをどう分けるか（レイアウト単位、銘柄単位、ユーザー単位）は、`ChartStateStorage` の実装と `key` 関数を通じてアプリケーションが決めます。

## 作成と更新

インポート元はエントリーポイント `@stocksharp/chart/persistence` です。

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { DrawingController } from '@stocksharp/chart/drawings';
import {
  ChartStatePersistence,
  NativeChartLayoutAdapter,
  IndicatorEngineStateAdapter,
  type ChartStateStorage,
  type IndicatorEnginePersistenceApi,
} from '@stocksharp/chart/persistence';

declare const indicatorEngine: IndicatorEnginePersistenceApi;

const chart = createChart(document.querySelector<HTMLElement>('#chart')!, {});
chart.addSeries(CandlestickSeries, { id: 'price', upColor: '#26a69a', downColor: '#ef5350' });

const storage: ChartStateStorage = {
  load: key => localStorage.getItem(key),
  save: (key, value) => { localStorage.setItem(key, value); },
  remove: key => { localStorage.removeItem(key); },
};

const persistence = new ChartStatePersistence<{ layoutId: string; symbol: string }>({
  layout: new NativeChartLayoutAdapter({ chart, mainPaneId: 'main' }),
  indicators: new IndicatorEngineStateAdapter({ engine: indicatorEngine }),
  drawings: new DrawingController({ chart }),
  storage,
  key: ({ layoutId, symbol }) => `chart:${layoutId}:${symbol}`,
  pretty: true,
});

const context = { layoutId: 'desk', symbol: 'BTC@IMEX' };

await persistence.save(context);

const restored = await persistence.load(context);
if (restored !== null) {
  console.log(restored.state.panes.length);
  console.log(restored.drawings.skipped);   // 未知のタイプのオブジェクトはスキップされ、復元全体は失われません
}
```

型パラメーター `TContext` は、`save`、`load`、`remove` に渡すものです。`key` 関数はコンテキストをキーの文字列に変換するもので、空でない文字列を返さなければなりません。`pretty: true` はインデント付きの JSON を書き出します。

`DrawingController` は、チャート上の描画を担当しているのと同じインスタンスである必要があります。そうでなければ、空のオブジェクト集合が保存されます。

## アダプター

`ChartStatePersistence` は、チャートのネイティブ API についてもインジケーターエンジンについても知りません。2 つのアダプターを介して動作します。既製の実装は同じモジュールに含まれていますが、独自のアーキテクチャであれば自前のものを渡せます。インターフェイス `ChartStateLayoutAdapter`（`capture`、`restore`）と `ChartStateIndicatorAdapter`（`capture`、`clear`、`restore`）は公開されています。

**`NativeChartLayoutAdapter`** は、チャート自体のレイアウトを取得して戻します。チャートのオプション、ペインとその順序、高さ、最小の高さ、状態（`normal`、`minimized`、`maximized`）、価格スケールの設定、さらにシリーズとそのタイプ、ペイン、スケール、スタイルオプションです。コンストラクターのオプション:

- `chart` — チャートのインスタンス（必須）。
- `mainPaneId` — 復元を通じて残るルートペインの識別子。既定は `main` で、それがなければ最初のペインです。
- `createSeries(series, pane)` — タイプのレジストリの代わりに独自の方法でシリーズを作成します。シリーズをデータソースに接続する必要がある場合に使います。
- `includeSeries(series)` — フィルター。`false` を返したシリーズは保存されず、復元時に削除もされません。
- `onRemoveSeries(series)` — 読み込むレイアウトにそのペインが含まれないために、復元がやむを得ず外すことになった外部のシリーズについて呼び出されます。
- `onUnknownSeries(series)` — シリーズのタイプがレジストリに存在しない場合。

`persist: false` オプションを持つシリーズは、`includeSeries` フィルターで除外されたものと同様にスナップショットから除かれます。

**`IndicatorEngineStateAdapter`** は、インジケーターの構成（タイプ、パラメーター、描画スタイル、ペインとスケールへの割り当て、表示状態、ソース）を保存しますが、計算済みの値は保存しません。復元後に改めて計算されます。コンストラクターのオプション:

- `engine` — `IndicatorEnginePersistenceApi`（`getIndicators`、`removeAll`、`add`、`setVisible`）を実装したインジケーターエンジン。
- `resolveTargetPaneId(indicator)` — 識別子が異なる場合に、保存されたペインをホストのペインへ対応付けます。
- `onUnknownIndicator(indicator)` — エンジンがそのタイプのインジケーターを作成できなかった場合。
- `onUnknownStyle(indicator, styleId)` — スタイルの中に、そのインジケーターに存在しない識別子があった場合。

別のインジケーターの出力を入力とするインジケーターは、その後に復元されます。アダプターがソースの連鎖を自分で並べ替え、参照先のインジケーターが存在しない場合やグラフが循環している場合はエラーを報告します。

## 状態の形式とマイグレーション

スナップショットは `ChartStateV1` 型で表され、`schemaVersion`、`chartOptions`、`panes`、`series`、`indicators`、`drawings` のフィールドを持ちます。現在のスキーマバージョンは定数 `CHART_STATE_SCHEMA_VERSION`（値は 1）です。

- `serializeChartState(state, { pretty })` — 状態を検証して JSON 文字列に変換します。
- `deserializeChartState(value, { migrations })` — 文字列を解析し（すでにオブジェクトであればそのまま受け取り）、現在のバージョンまでマイグレーションを実行して、結果を検証します。
- `normalizeChartStateV1(value)` — 状態の検証と凍結。余分なキー、識別子の重複、存在しないペインへの参照、ペインが 1 つもないレイアウトは拒否されます。
- `normalizePersistedObject(value, path, { omitUndefined })` — 任意の JSON を不変オブジェクトへ深くコピーします。循環、数値でない値、過度な入れ子、`__proto__`、`prototype`、`constructor` のキーは禁止されています。

古いスナップショットは、段階的なマイグレーションで引き上げられます。共通のレジストリ `chartStateMigrations` にはバージョン 0 から 1 への移行がすでに含まれており、独自の手順は次のように登録します。

```ts
import {
  ChartStateMigrationRegistry,
  deserializeChartState,
} from '@stocksharp/chart/persistence';

const migrations = new ChartStateMigrationRegistry();
migrations.register(1, state => ({ ...state, schemaVersion: 2 }));

const state = deserializeChartState(json, { migrations });
```

各マイグレーションは状態をちょうど 1 バージョンだけ進めるもので、新しい `schemaVersion` の値を設定しなければなりません。サポートされているバージョンより新しいスナップショットは、読み込みを受け付けません。

## 公開メソッド

- `snapshot()` — ストレージにアクセスせず、現在のチャートの状態を `ChartStateV1` にまとめます。
- `restore(state)` — 状態をチャートに適用します。`{ state, drawings }` を返し、`drawings` には `restored` と `skipped` のリストが含まれます。
- `save(context)` — スナップショットを取得し、シリアライズして、算出したキーでストレージに書き込みます。保存された状態を返します。
- `load(context)` — キーでレコードを読み取り、マイグレーションを実行して復元します。レコードがない場合は `null` です。
- `remove(context)` — ストレージからレコードを削除します。

復元の順序は固定です。まずインジケーターが解放され、次にペインとシリーズのレイアウトが復元され、続いてインジケーター、最後に描画オブジェクトが復元されます。

このモジュールは、スナップショットとアダプターを表す型もエクスポートします。`ChartStateLayoutSnapshot`、`ChartStateRestoreResult`、`ChartStatePersistenceOptions`、`PersistedPane`、`PersistedPriceScale`、`PersistedSeries`、`PersistedIndicator`、`PersistedDrawing`、`PersistedChartOptions`、`PersistedSeriesOptions`、`PersistedIndicatorParameters`、`PersistedIndicatorStyles`、`PersistedObject`、`PersistedJsonValue`、`PersistableIndicatorEntry`、`RawChartState`、`ChartStateMigration`、`MaybePromise` です。

## 関連項目

- [JavaScript チャート](../charts.md)
- [インジケーター](indicators.md)
- [履歴のバックフィル](backfill.md)
