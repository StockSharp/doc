# Hydra 設定

以下では、バックアップタスクの作成と設定方法について説明します。

1. タスクを作成するには、**Add tasks...** ボタンをクリックし、開いたウィンドウで **Backup** 項目を選択して **OK** ボタンをクリックします。![hydra tasks backup add](../../../../images/hydra_tasks_backup_add.png)
2. 次に、タスクを設定する必要があります。![hydra tasks backup](../../../../images/hydra_tasks_backup.png)

   **Backup**
   - **Service** - サービスアドレス。 
  - **Address** - リージョンアドレス。バケット設定で指定したリージョンのアドレスです。[Regions and Endpoints](https://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region) を参照してください。
   - **Storage** - バケット名。 
   - **Login** - ログイン。Access Key ID。 
   - **Password** - パスワード。**Secret Access Key**。 
   - **Start date** - バックアップを開始する日付。 
   - **Time offset** - 現在の日付からの日数オフセット。 

   **General**
   - **Header** - Converter。 
   - **Working hours** - ボードの稼働スケジュールの設定。 ![hydra tasks backup desk](../../../../images/hydra_tasks_backup_desk.png)
   - **Interval of operation** - 操作の間隔。 
  - **Data directory** - 変換対象のデータを取得するデータディレクトリ。
   - **Format** - 変換後のデータ形式: BIN\/CSV。 
   - **Max. errors** - エラーの最大数。この数に達するとタスクが停止されます。既定では 0 で、エラー数は無視されます。 
   - **Dependency** - 現在のタスクを実行する前に実行する必要があるタスク。 

   **Logging**
   - **Identifier** - 識別子。 
   - **Logging level** - ロギングレベル。 
3. タスクの設定後、バックアップストレージに保存する銘柄を追加し、**Start** ボタンをクリックします。

## 推奨コンテンツ

[アカウントの作成と設定](setup.md)

