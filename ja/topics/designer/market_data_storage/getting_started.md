# はじめに

履歴データストレージを作成するには、**Market data** タブの ![Designer Creating a repository of historical data 00](../../../images/designer_creating_repository_of_historical_data_00.png) ボタンをクリックします。現在のストレージパラメーターを変更するには、![Designer Creating a repository of historical data 01](../../../images/designer_creating_repository_of_historical_data_01.png) をクリックします。現在のストレージをストレージ一覧から削除するには、![Designer Creating a repository of historical data 02](../../../images/designer_creating_repository_of_historical_data_02.png) をクリックします。

![Designer Creating a repository of historical data 03](../../../images/designer_creating_repository_of_historical_data_03.png)

履歴データストレージは、ローカルまたはリモートにできます。

ローカルストレージ - すべてのデータがローカルコンピューターに保存される場合です。ローカルストレージを設定するには、保存データが入っているフォルダーへのパスを設定するだけで十分です。

リモートストレージは別のコンピューター上に配置できます。設定するには、リモートストレージのアドレスを指定し、必要に応じてログインとパスワードを指定します。

マーケットデータ（銘柄、ローソク足、ティック取引、板情報など）をさまざまなソースから自動的に読み込み、ローカルストレージに保存するために設計された [Hydra](../../hydra.md) ソフトウェア（コード名 Hydra）を使用して、リモートストレージをローカルマシン上で再現できます。そのためには、[Hydra](../../hydra.md) をサーバーモードに切り替えます。

![Designer Creating a repository of historical data 04](../../../images/designer_creating_repository_of_historical_data_04.png)

その後、[Designer](../../designer.md) で ![Designer Creating a repository of historical data 00](../../../images/designer_creating_repository_of_historical_data_00.png) ボタンをクリックして新しいストレージを作成します。ストレージ設定のアドレス欄に "net.tcp:\/\/localhost:8000" を指定します。OK をクリックします。[Hydra](../../hydra.md) をリモートストレージとして使用する場合は、[Hydra](../../hydra.md) が起動され、適切に設定されている必要があることを忘れないでください。

![Designer Creating a repository of historical data 05](../../../images/designer_creating_repository_of_historical_data_05.png)

新しいストレージを追加した後、**Storage** ドロップダウンリストで選択できます。

![Designer Creating a repository of historical data 06](../../../images/designer_creating_repository_of_historical_data_06.png)

また、ストレージファイルの形式（BIN または CSV）も選択する必要があります。データは 2 つの形式で保存できます。最大の圧縮率を提供する専用のバイナリ BIN 形式、または他のプログラムでデータを分析する際に便利なテキスト形式 CSV です。ディスク容量を節約する必要がある場合は BIN 形式が推奨されます。データを手動で調整する必要がある場合は CSV 形式が推奨されます。CSV は標準のメモ帳、MS Excel などで簡単に編集できます。

## 推奨コンテンツ

[銘柄のダウンロード](download_instruments.md)
