# TableExport

`TableExport` はブラウザー内で実際の OOXML `.xlsx` ワークブックを作成し、直ちにダウンロードを開始します。実装は外部ライブラリを使用せず、CSV ファイルを Excel 拡張子に見せかけることもありません。

## 直接エクスポート

ファイルのベース名、シート名、ヘッダー、行の 2 次元配列を渡します。

```ts
import { TableExport } from '@stocksharp/grids/table-export';

TableExport.download(
  'portfolio-summary',
  'ポートフォリオ集計',
  ['項目', '値'],
  [
    ['現金', 125000.50],
    ['未決済ポジション', 7],
    ['含み益', 4380.25],
  ],
);
```

ブラウザーは `<baseName>-YYYYMMDD-HHMMSS.xlsx` 形式のタイムスタンプ付きファイルをダウンロードします。たとえば `portfolio-summary-20260810-143025.xlsx` です。

有限数は数値セルとして書き込まれ、その他の空でない値はインライン文字列として書き込まれます。`null`、`undefined`、空文字列は空セルを作成します。行は必要な順序で渡してください。`TableExport` はデータの並べ替えやフィルタリングを行いません。

シート名からは Excel で禁止されている文字 `[]:*?/\` が自動的に除去され、31 文字に制限されます。除去後に何も残らない場合は `Sheet1` に置き換えられます。

## DataGrid からのエクスポート

[DataGrid](data_grid.md) は列定義からデータを準備し、同じ仕組みを呼び出します。

```ts
const data = grid.exportData();
console.log(data.headers, data.rows);

grid.download('orders', '注文');
```

ファイルに含まれるのは、`exportable: true` が設定された表示中の列だけです。既定では `value(row)` の結果が使用され、`exportValue(row)` が存在する場合はその結果が使用されます。たとえば、セルには整形した DOM 要素を表示し、並べ替えには数値コードを使いながら、ローカライズされたテキストをエクスポートできます。

行はフィルタリング、並べ替え、グループ化の後、表示順にエクスポートされます。グループヘッダーはシートに追加されませんが、折りたたまれたグループの行は保持されます。`renderLimit` はエクスポートを切り詰めず、`pinnedRows()` の固定行はワークブックに含まれません。

`exportData()` は何もダウンロードしないため、内容のプレビューやテストに便利です。`download()` はクライアント側でファイルを作成し、`Blob` を参照するリンクを一時的にドキュメントへ追加して、ダウンロード開始後にオブジェクト URL を解放します。

## 制限事項

コンポーネントが生成するのは、1 枚のシートを持つ最小限のワークブックです。数式、セルスタイル、列幅、複数シート、ZIP 圧縮は設定しません。アプリケーションでこれらの機能が必要な場合は、専用ツールでエクスポートを作成してください。

## 関連項目

- [JavaScript テーブル](../grids.md)
- [DataGrid](data_grid.md)
- [TableSort](table_sort.md)
- [@stocksharp/grids パッケージ](https://www.npmjs.com/package/@stocksharp/grids)
