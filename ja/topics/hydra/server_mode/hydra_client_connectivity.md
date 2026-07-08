# Hydra クライアントの接続

サーバーモードでは、別の Hydra プログラムを接続できます。そのプログラムはクライアントとして動作し、自身にデータをダウンロードします。[FIX プロトコル経由の接続](fix_fast_connectivity.md)とは異なり、データは StockSharp 形式のファイルとして送信されます。そのため、このソースは大量の履歴データを転送する用途に適しています。

接続には専用のソースを使用します:

![hydra tasks server](../../../images/hydratasksserver_1.png)

**Settings**

![hydra tasks server](../../../images/hydratasksserver_2.png)

- **Address** - Hydra サーバーのアドレス。
- **Login** - ログイン (サーバーが認証を要求する場合に必要)。
- **Password** - パスワード (サーバーが認証を要求する場合に必要)。
- **Time Offset** - 現在日付からの日数で表す時間オフセット。現在の取引セッションの未完成データをダウンロードしないようにするために必要です。
- **Weekends** - 週末のデータをダウンロードするかどうか。

**Main**

- **Title** - タスクのタイトル。
- **Working Hours** - プラットフォームの動作設定。
- **Interval of Operation** - 動作間隔。
- **Data Directory** - [S#](../../api.md) 形式の最終ファイルが保存されるデータディレクトリ。
- **Format** - データ形式: BIN/CSV。
- **Max. Errors** - エラーの最大数。この数に達するとタスクは停止されます。既定では 0 で、エラー数は無視されます。
- **Dependency** - 現在のタスクを開始する前に完了している必要があるタスク。

**Logging**

- **Identifier** - 識別子。
- **Logging Level** - ログレベル。
