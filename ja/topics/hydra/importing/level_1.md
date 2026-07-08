# Level 1

Level 1 データをインポートするには、アプリケーションのメイン メニューから **Import \=\> Level 1** を選択します。

![hydra import level1](../../../images/hydra_import_level1.png)

## インポート プロセス。

1. **Import settings.**。

   [Candles](candles.md) のインポートを参照してください。
2. [S#](../../api.md) フィールドのインポート パラメーターを設定します。

   [Candles](candles.md) のインポートを参照してください。

   **CSV ファイルから Level 1 をインポートする例を考えてみましょう。**
   - インポートするデータのファイルは、次のテンプレートを持っています。

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Changes:{BestBidPrice};{BestBidVolume};{BestAskPrice};{BestAskVolume};{LastTradeTime};{LastTradePrice};{LastTradeVolume}}
     	  				
     ```

     ここで、{SecurityId.SecurityCode} と {SecurityId.BoardCode} の値は、それぞれ **Security** と **Board** の値に対応します。したがって、**Field order** フィールドには、それぞれ値 0 と 1 を割り当てます。
   - {ServerTime:default:yyyyMMdd} と {ServerTime:default:HH:mm:ss.ffffff} フィールドについては、**S# field** ウィンドウからそれぞれ Date と **Time** フィールドを選択します。値 2 と 3 を割り当てます。
   - {BestBidPrice} フィールドについては、**S# field** ウィンドウから **Best buy price** フィールドを選択します。値 4 を割り当てます。
   - {BestBidVolume} フィールドについては、**S# field** ウィンドウから **Best buy volume** フィールドを選択します。値 5 を割り当てます。
   - {BestAskPrice} フィールドについては、**S# field** ウィンドウから **Best sale price** フィールドを選択します。値 6 を割り当てます。
   - {BestAskVolume} フィールドについては、**S# field** ウィンドウから **Best sale volume** フィールドを選択します。値 7 を割り当てます。
   - {LastTradeTime} フィールドについては、**S# field** ウィンドウから **Last trade time** フィールドを選択します。値 8 を割り当てます。
   - {LastTradePrice} フィールドについては、**S# field** ウィンドウから **Last trade price** フィールドを選択します。値 9 を割り当てます。
   - {LastTradeVolume} フィールドについては、**S# field** ウィンドウから **Last trade volume** フィールドを選択します。値 10 を割り当てます。
   - フィールド設定ウィンドウは次のようになります。![hydra import prop level 1](../../../images/hydra_import_prop_level1.png)

   ユーザーは、ダウンロードされたデータに対して多数のプロパティを設定できます。インポートするファイル テンプレートに基づいて、プロパティを指定し、シーケンス内で必要な番号を割り当てる必要があります。 
3. データをプレビューするには、**Preview** ボタンをクリックします。![hydra import preview level 1](../../../images/hydra_import_preview_level1.png)
4. **Import** ボタンをクリックします。
