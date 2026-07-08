# クラウドテスト

クラウドでストラテジーをテストするには、まず関心のあるすべての銘柄を見つける必要があります。そのためには、**Designer** で **Cloud** タブにある、テストに利用可能な銘柄検索パネルを開く必要があります。

![Designer_Backtest_Cloud_01](../../../images/designer_backtest_cloud_01.png)

検索フィールドに銘柄名を入力して **Search** をクリックする (または **Enter** を押す) と、StockSharp サーバーが適切な検索結果を返します。履歴データの日付範囲も、銘柄名の右側に表示されます。

この手順は、新しい銘柄ごとに一度だけ行う必要があります。その後、見つかった銘柄はディスク上にローカル保存され、**Designer** を再起動したときにはローカルストレージから読み込まれます。このステップが必要なのは、ストラテジーを起動するとき (および [変数](../strategies/using_visual_designer/elements/data_sources/variable.md) ブロックで直接銘柄を指定するとき) に銘柄指定が必要になるためです。

その後、ストラテジーに戻り、**Backtest** タブでクラウドオプションを有効にする必要があります。

![Designer_Backtest_Cloud_00](../../../images/designer_backtest_cloud_00.png)

テストを開始すると、ストラテジーはローカルでテストされる代わりに StockSharp クラウドへ送信されます。

![Designer_Backtest_Cloud_02](../../../images/designer_backtest_cloud_02.png)

テストが完了すると、結果を含むレポートがタスク待機タブに表示されます。

![Designer_Backtest_Cloud_03](../../../images/designer_backtest_cloud_03.png)

クラウドでのテスト履歴を表示し、現在アクティブなタスクも確認したい場合は、**Cloud** タブの **Tasks** パネルを開きます。

![Designer_Backtest_Cloud_04](../../../images/designer_backtest_cloud_04.png)
