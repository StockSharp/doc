# TableSort

`TableSort<TRow>` は、[DataGrid](data_grid.md) の内部で使用される独立した並べ替えコントローラーです。アプリケーション独自のテーブルにも接続できます。選択された列と方向を保持し、`data-sort` を持つヘッダーのクリックを処理して、並べ替え済みの行配列のコピーを返します。

## マークアップ

並べ替え可能な各ヘッダーを、値取得関数の辞書にあるキーと対応付けます。

```html
<table id="quotes">
  <thead>
    <tr>
      <th data-sort="symbol">銘柄</th>
      <th data-sort="bid">買気配</th>
      <th data-sort="ask">売気配</th>
    </tr>
  </thead>
  <tbody></tbody>
</table>
```

## コントローラーの作成

```ts
import { SortDirections, TableSort } from '@stocksharp/grids/table-sort';

interface Quote {
  symbol: string;
  bid: number | null;
  ask: number | null;
}

const table = document.querySelector<HTMLTableElement>('#quotes')!;
let quotes: Quote[] = [];

const sort = new TableSort<Quote>(
  table.tHead,
  {
    symbol: quote => quote.symbol,
    bid: quote => quote.bid,
    ask: quote => quote.ask,
  },
  render,
  { col: 'symbol', dir: SortDirections.Asc },
  new Intl.Collator('ja-JP', { numeric: true, sensitivity: 'base' }),
);

function render(): void {
  const body = table.tBodies[0];
  body.replaceChildren();

  for (const quote of sort.apply(quotes)) {
    const row = body.insertRow();
    row.insertCell().textContent = quote.symbol;
    row.insertCell().textContent = quote.bid?.toString() ?? '—';
    row.insertCell().textContent = quote.ask?.toString() ?? '—';
  }
}
```

コンストラクターは次の引数を受け取ります。

1. クリック処理が不要な場合は `null`、必要な場合はヘッダー要素。
2. 列キーごとの値取得関数の辞書。
3. 行を再描画する `onChange` 関数。
4. 既定の並べ替え。初期順序を使用する場合は `null`。
5. テキスト比較用にあらかじめ作成した `Intl.Collator`。

選択したキーの値取得関数が定義されていない場合、コントローラーは行から同名のプロパティを読み取ろうとします。

## 並べ替えの動作

新しいヘッダーをクリックすると昇順で並べ替えられます。次のクリックで降順になり、3 回目のクリックで既定の順序に戻ります。テーブルに `defaultSort` が渡されている場合、並べ替えなしの状態はありません。

`apply(rows)` は常に新しい配列を返し、アプリケーションの配列を変更しません。`null`、`undefined`、空文字列は、どちらの方向でも末尾に配置されます。

比較方法は、**宣言された列の型ではなく値によって**選択されます。両方の値が有限の数値に変換できる場合は数値として比較され、それ以外の場合にのみ、渡された `Intl.Collator` が使われます。そのため文字列 `"42"` は、アルファベット順の位置ではなく 41 と 43 の間に並びます。`"1e3"` のような文字列も数値と見なされます。値にかかわらず列を常にテキストとして並べ替えたい場合は、数値にならない値をその列から返してください。

コントローラーは、アクティブなヘッダーへ `sort-asc` または `sort-desc` クラスを割り当てます。矢印など、これらのクラスの外観はアプリケーション側で定義します。

## コードからの制御

```ts
sort.set('bid', SortDirections.Desc);

const explicitSort = sort.current();
// { col: 'bid', dir: 'desc' }

sort.set(null, null);
// defaultSort に戻します。
```

`current()` が返すのは、ユーザーが明示的に選択した内容だけです。既定の順序が有効な間は、実際に行が並べ替えられていても結果は `null` になります。

アプリケーションが同じヘッダー内の `<th>` 要素を作り直した場合は、`refreshHeader()` を呼び出して方向クラスを再設定してください。クリックリスナーは渡されたヘッダー要素自体に割り当てられるため、新しい子要素でも引き続き機能します。

## 関連項目

- [JavaScript テーブル](../grids.md)
- [DataGrid](data_grid.md)
- [TableExport](table_export.md)
