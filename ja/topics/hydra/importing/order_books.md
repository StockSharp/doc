# 板情報

板情報をインポートするには、アプリケーションのメイン メニューから **Import \=\> Order books** 項目を選択します。

![hydra import depths](../../../images/hydra_import_depths.png)

## インポート プロセス。

1. **Import settings.**。

   [ローソク足](candles.md) のインポートを参照してください。
2. [S#](../../api.md) フィールドのインポート パラメーターを設定します。

   [ローソク足](candles.md) のインポートを参照してください。

   **CSV ファイルから板情報をインポートする例を考えてみましょう。**
   - インポートするデータのファイルは、次のテンプレートを持っています。

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Quote.Price};{Quote.Volume};{Side}
     	  				
     ```

     ここで、{SecurityId.SecurityCode} と {SecurityId.BoardCode} の値は、それぞれ **Security** と **Board** の値に対応します。したがって、**Field order** フィールドには、それぞれ値 0 と 1 を割り当てます。
   - {ServerTime:default:yyyyMMdd} と {ServerTime:default:HH:mm:ss.ffffff} フィールドについては、**S# field** ウィンドウからそれぞれ **Date** と **Time** フィールドを選択します。値 2 と 3 を割り当てます。
   - {Quote.Price} フィールドについては、**S# field** ウィンドウから **Price** フィールド、つまりクォート価格を選択します。値 4 を割り当てます。
   - {Quote.Volume} フィールドについては、**S# field** ウィンドウから **Volume** フィールド、つまりクォート数量を選択します。値 5 を割り当てます
   - {Side} フィールドについては、**S# field** ウィンドウから **Direction** フィールド、つまり取引方向（Buy または Sell）を選択します。値 6 を割り当てます。
   - フィールド設定ウィンドウは次のようになります。![hydra import prop depth](../../../images/hydra_import_prop_depth.png)

   ユーザーは、ダウンロードされたデータに対して多数のプロパティを設定できます。インポートするファイル テンプレートに基づいて、プロパティを指定し、シーケンス内で必要な番号を割り当てる必要があります。 
3. データをプレビューするには、**プレビュー** ボタンをクリックします。![hydra import preview depth](../../../images/hydra_import_preview_depth.png)
4. **インポート** ボタンをクリックします。
