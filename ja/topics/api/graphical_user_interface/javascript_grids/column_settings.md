# ColumnSettings

`ColumnSettings` は、サーバーまたは別のコンポーネントがすでに生成した HTML テーブルに、列の選択、並べ替え、非表示化を追加します。[DataGrid](data_grid.md) と異なり、このアダプターはヘッダーや行を構築せず、テーブルのデータも管理しません。

## マークアップの要件

テーブルには実際の `<thead>` が必要です。管理対象の列には一意の `data-col` 属性を指定します。

```html
<table id="trades">
  <thead>
    <tr>
      <th data-col="time">時刻</th>
      <th data-col="symbol">銘柄</th>
      <th data-col="price">価格</th>
      <th data-col="volume">数量</th>
      <th>操作</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>10:15:02</td>
      <td>SBER</td>
      <td>312.45</td>
      <td>10</td>
      <td><button type="button">開く</button></td>
    </tr>
  </tbody>
</table>
```

`data-col` のない列は固定列と見なされ、ユーザーはその列を非表示にしたり、元の位置から移動したりできません。アダプターは作成時に、対応する本文セルへ同じキーを設定します。「データなし」を `colspan` で表示する行など、セル数が異なる行は変更されません。

## 接続

ホストアプリケーションは次の 3 つを提供します。

- サーバー側マークアップを持つテーブル。
- コンポーネントが切り替え項目と移動ボタンを追加するダイアログ。
- `read()` メソッドと `write()` メソッドを持つストア。

```ts
import { ColumnSettings } from '@stocksharp/grids/column-settings';

const dialogElement = document.querySelector<HTMLElement>('#column-dialog')!;
const list = dialogElement.querySelector<HTMLElement>('#column-list')!;

const settings = new ColumnSettings({
  table: document.querySelector<HTMLTableElement>('#trades')!,
  dialog: {
    list,
    moveUpTitle: '上へ移動',
    moveDownTitle: '下へ移動',
    classes: {
      item: 'column-picker-item',
      toggle: 'column-picker-toggle',
      label: 'column-picker-label',
      move: 'column-picker-move',
      moveUpIcon: 'icon-arrow-up',
      moveDownIcon: 'icon-arrow-down',
    },
    open: () => { dialogElement.hidden = false; },
    close: () => { dialogElement.hidden = true; },
  },
  store: {
    read: () => {
      const value = localStorage.getItem('trades-columns');
      return value ? JSON.parse(value) : null;
    },
    write: visible => {
      if (visible === null)
        localStorage.removeItem('trades-columns');
      else
        localStorage.setItem('trades-columns', JSON.stringify(visible));
    },
  },
});

document.querySelector('#open-columns')!
  .addEventListener('click', () => settings.openPicker());

document.querySelector('#apply-columns')!
  .addEventListener('click', () => settings.applyPicked());

document.querySelector('#reset-columns')!
  .addEventListener('click', () => settings.resetToDefault());
```

コンポーネントが内容を設定するのは `list` 要素だけです。タイトル、適用ボタンとリセットボタン、アニメーション、モーダルウィンドウの開閉はアプリケーション側が担当するため、`applyPicked()` と `resetToDefault()` のハンドラーもアプリケーションで割り当てる必要があります。

## レイアウトの保存

`ColumnLayoutStore.read()` は、表示するキーを必要な順序で並べた配列を返します。初期レイアウトを使用する場合は `null` を返します。コンストラクターは直ちに値を読み取り、ユーザーが最初に操作する前に適用します。

`write(visibleKeys)` が受け取るのは、表示されている管理対象列だけです。`null` は、初期順序が選択され、どの列も非表示でないことを意味します。これにより URL または `localStorage` のストアは、既定値全体を保存せず、不要なエントリーを削除できます。

アダプター内のキーは小文字に正規化されます。適用時には、不明なキーと重複したキーが破棄されます。

## メソッド

- `defaultKeys()` は、管理対象列の初期順序を返します。
- `apply(visibleKeys)` は列を直ちに並べ替えて非表示にしますが、レイアウトは保存しません。
- `isDefault(visibleKeys)` は、レイアウトが初期状態と一致するか確認します。
- `openPicker()` は現在の DOM を読み取り、一覧を構築してダイアログを開きます。
- `applyPicked()` は現在の選択を適用して保存し、ダイアログを閉じます。
- `resetToDefault()` は管理対象の全列を復元し、`write(null)` を呼び出してダイアログを閉じます。

## スタイル

`ColumnSettings` は CSS を同梱せず、特定のモーダルライブラリやアイコンセットにも依存しません。アプリケーションは `ColumnPickerClasses` を通じて、行、チェックボックス、ラベル、ボタン、2 つのアイコンのクラスを指定します。`moveUpTitle` と `moveDownTitle` は、コンポーネントへ渡す前にローカライズしてください。

## 関連項目

- [JavaScript テーブル](../javascript_grids.md)
- [DataGrid](data_grid.md)
- [JS-Grids リポジトリ](https://github.com/StockSharp/JS-Grids)
