# JavaScript テーブル

[StockSharp JS Grids](https://github.com/StockSharp/JS-Grids) は、表形式データを表示するためのブラウザーコンポーネント集です。パッケージは [@stocksharp/grids](https://www.npmjs.com/package/@stocksharp/grids) として npm に公開されており、実行時の外部依存関係はありません。TypeScript からも、ブラウザーから直接でも利用できます。

![フィルター、選択行、固定集計行を備えた StockSharp 取引ジャーナル](../../../images/javascript_grids_blotter.jpg)

完成版は[オンラインデモ](https://stocksharp.github.io/JS-Grids/demo/)で確認できます。ヘッダーによる行の並べ替え、列のドラッグと非表示化、フィルターとグループ化による表示の変更、実際の `.xlsx` ファイルへのエクスポートを試せます。

## パッケージ構成

- [DataGrid](grids/data_grid.md) は、共通の列定義からヘッダーと行を構築します。並べ替え、フィルター、グループ化、行選択、固定集計行、コンテキストメニュー、状態の保存、エクスポートを担当します。
- [ColumnSettings](grids/column_settings.md) は、サーバーですでに描画された HTML テーブルに接続し、列の順序と表示状態をユーザーが変更できるようにします。
- [TableSort](grids/table_sort.md) は、アプリケーションが管理するテーブル向けの独立した並べ替えコントローラーです。
- [TableExport](grids/table_export.md) は、外部ライブラリを使わずに OOXML `.xlsx` ワークブックを生成します。

`DataGrid` と `ColumnSettings` は異なる用途を担います。前者は列定義から `<thead>` と `<tbody>` の内容を自ら作成します。後者はテーブルを作成せず、`data-col` 属性を持つ既存のサーバー側マークアップ専用です。

## インストール

npm からパッケージをインストールします。

```bash
npm install @stocksharp/grids
```

主要なコンポーネントはすべて共通のエントリーポイントから利用できます。

```ts
import {
  DataGrid,
  ColumnSettings,
  TableSort,
  TableExport,
} from '@stocksharp/grids';
```

インポートするコードを減らすため、個別のエントリーポイントも用意されています。

```ts
import { DataGrid } from '@stocksharp/grids/data-grid';
import { ColumnSettings } from '@stocksharp/grids/column-settings';
import { TableSort } from '@stocksharp/grids/table-sort';
import { TableExport } from '@stocksharp/grids/table-export';
```

バンドラーを使わない場合は、ビルド済みのブラウザーパッケージを読み込みます。公開オブジェクトは `window.SSGrid` に配置されます。

```html
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/grids@1/dist/ssgrid.js"></script>
<script>
  const { DataGrid, TableExport } = window.SSGrid;
</script>
```

`@1` の範囲指定は、最初のメジャーブランチの最新バージョンを取得します。正確な番号を固定するのは、ビルドを意図的に再現したい場合だけにしてください。古いバージョンを固定すると、その後追加された機能を含まないビルドが警告なく配信され、たとえば `groupOrder` が存在しないことは、このタグを書いた場所とは別のところで表面化します。

TypeScript の生ソースは、サブパス `@stocksharp/grids/source` と `@stocksharp/grids/source/<モジュール>` から利用できます。テーブルを他のコードと同じ 1 回のパスで自前のバンドラーによってビルドする場合に必要です。

## クイック例

ヘッダーとデータのセクションを持つ通常のテーブルを用意します。

```html
<table id="orders">
  <thead></thead>
  <tbody></tbody>
</table>
```

列を一度定義し、行をテーブルに渡します。

```ts
import { DataGrid, SortDirections } from '@stocksharp/grids';

interface Order {
  id: number;
  symbol: string;
  price: number;
}

const table = document.querySelector<HTMLTableElement>('#orders')!;
const grid = new DataGrid<Order>({
  head: table.tHead!,
  body: table.tBodies[0],
  columns: [
    { key: 'id', header: '番号', value: order => order.id, filter: 'number', exportable: true },
    { key: 'symbol', header: '銘柄', value: order => order.symbol, filter: 'text', exportable: true },
    { key: 'price', header: '価格', value: order => order.price, filter: 'number', exportable: true },
  ],
  defaultSort: { col: 'id', dir: SortDirections.Desc },
  rowKey: order => String(order.id),
  emptyText: '注文はありません',
  locale: 'ja-JP',
});

grid.setRows([
  { id: 101, symbol: 'SBER', price: 312.45 },
  { id: 102, symbol: 'GAZP', price: 164.18 },
]);
```

ライブラリは DOM 要素を作成しますが、外観を固定しません。色、サイズ、行の強調表示、メニュー、フィルターダイアログ、補助クラスは、アプリケーションのスタイルシートで定義します。

## ソースコードからビルドする

リポジトリとローカルデモでは、標準の npm コマンドを使用します。

```bash
git clone https://github.com/StockSharp/JS-Grids.git
cd JS-Grids
npm ci
npm test
npm run build
npm run serve
```

起動後、デモは `http://localhost:8793/demo/` で利用できます。

## 関連項目

- [JS-Grids リポジトリ](https://github.com/StockSharp/JS-Grids)
- [@stocksharp/grids パッケージ](https://www.npmjs.com/package/@stocksharp/grids)
- [オンラインデモ](https://stocksharp.github.io/JS-Grids/demo/)
- [JavaScript トレーディングコントロール](trading_controls.md)
- [JavaScript チャート](charts.md)
- [JavaScript ダイアグラム](diagram.md)
