# Export (auto)

このタスクは、取引所データを Excel、xml、sql、bin、Json、txt などのさまざまな形式にエクスポートします。

![hydra tasks export](../../../images/hydra_tasks_export.png)

**Database**

- **Connection** - データベースへの接続。SQL 経由でエクスポートする場合に使用されます。 
- **Packet** - 送信されるデータパケットのサイズ。既定ではサイズは50要素です。SQL 経由でエクスポートする場合に使用されます。 
- **Uniqueness** - データベース内のデータ一意性をチェックします。パフォーマンスに影響します。既定で有効です。SQL 経由でエクスポートする場合に使用されます。 

> [!TIP]
> SQL 経由でエクスポートする場合は、接続文字列のパラメーターを設定する必要があります

**New connection string**

![hydra tasks connstring](../../../images/hydra_tasks_connstring.png)

- **Provider** - プロバイダー設定。 
- **Server** - サーバーアドレスまたはデータベースへのパス。 
- **Database** - データベース名。SQLite では使用されません。 
- **Login** - データベースへアクセスするためのログイン。匿名アクセスでは使用されません。 
- **Password** - データベースへアクセスするためのパスワード。匿名アクセスでは使用されません。 
- **Windows** - データベースへ接続するために現在の Windows アカウントを使用します。 
- **Connection** - 既成の接続文字列。 

> [!TIP]
> **Check** ボタンを使用して、データベースへの接続を確認できます。

**General**

- **Header** - Converter。 
- **Working hours** - ボード稼働スケジュールの設定。 ![hydra tasks backup desk](../../../images/hydra_tasks_backup_desk.png)
- **Interval of operation** - 動作間隔。 
- **Data directory** - 変換用のデータを受け取るデータディレクトリ。 
- **Format** - 変換後のデータ形式: BIN\/CSV。 
- **Max. errors** - エラーの最大数。この数に達するとタスクは停止されます。既定では 0 で、エラー数は無視されます。 
- **Dependency** - 現在のタスクを実行する前に実行されている必要があるタスク。 

**CSV**

- **Templates** - エクスポートされる各データ型のテンプレート。 
- **Header** - 先頭行のヘッダー。空文字列が渡された場合、ヘッダーはファイルに追加されません。
- **Name format** - エクスポートされるファイル名を記録するための形式。 

**Export (auto)**

- **Type** - エクスポートの種類 (形式)。 
- **Start date** - どの日付からデータのエクスポートを開始するか。 
- **Time offset** - 日数で表す時間オフセット。 
- **Export directory** - データをエクスポートするディレクトリ。 
- **Format** - データ形式。 
- **Split** - 分割タイプ。 

**Logging**

- **Identifier** - 識別子。 
- **Logging level** - ログレベル。 

自動エクスポートの例を見てみましょう:

1. 証券を選択します。
2. エクスポートする必要があるマーケットデータを設定します。![hydra tasks export 00](../../../images/hydra_tasks_export_00.png)
3. エクスポート期間を設定します。マーケットデータのリアルタイムダウンロードが設定されている場合、期間の終了日を省略できます。この場合、データは作業間隔 (データ更新) に従ってリアルタイムでエクスポートされます。 ![hydra tasks export 01](../../../images/hydra_tasks_export_01.png)
4. ディレクトリ、動作間隔、データ型、データ形式を設定します。
5. エクスポートを開始します。![hydra tasks export 02](../../../images/hydra_tasks_export_02.png)

エクスポートされたデータを表示してみましょう

![hydra tasks export 03](../../../images/hydra_tasks_export_03.png)

**[ビデオチュートリアルを見る](../videos/export_task.md)**
