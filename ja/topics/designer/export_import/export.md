# エクスポート

Designer では、ストラテジー、ブロック、インジケーターなど、任意の種類のデータをエクスポートできます。エクスポートする方法はいくつかあります。

- **スキーマ** パネルで、ストラテジー、ブロック、またはインジケーターを右クリックします。表示されるメニューで **エクスポート** を選択します。
- **共通** タブで **エクスポート** ボタンを押します。

![Designer Export strategies 00](../../../images/designer_export_strategies_00.png)

**エクスポート** を押すと、コンテンツの種類に応じてウィンドウが表示されます。

- [スキーマ](../strategies/using_visual_designer.md) の場合:

  ![Designer Export strategies 01](../../../images/designer_export_strategies_01.png)

  - スキーマ - スキーマをそのままエクスポートします。独自の要素またはインジケーターを使用するスキーマには、**スタンドアロン** モードが必要です。この場合、すべての内部要素はストラテジーダイアグラム内にエクスポートされます。
  - コード - スキーマを C# コードに変換します。
  - DLL - スキーマを DLL にコンパイルします。コードの機密性を保つ必要がある場合に適しています。

- [コード](../strategies/using_code.md) の場合:

  ![Designer Export strategies 02](../../../images/designer_export_strategies_02.png)

  - スキーマ - コードを JSON ファイルとしてエクスポートします。このファイルには、コード自体と、このコードのコンパイルに必要な参照の両方が含まれます。
  - コード - コードをそのままエクスポートします。
  - DLL - コードを DLL にコンパイルします。コードの機密性を保つ必要がある場合に適しています。

- [dll](../strategies/using_dll.md) の場合は、ファイル選択ウィンドウが表示されます。

## 関連項目

[Designer の外部でストラテジーを実行する](../live_execution/running_strategies_outside_of_designer.md)
