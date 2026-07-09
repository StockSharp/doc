# ティック

取引をインポートするには、**Import \=\> Ticks** タブを選択します。

![hydra import trades](../../../images/hydra_import_trades.png)

## インポート プロセス。

1. **インポート設定**。

   [ローソク足](candles.md) のインポートを参照してください。
2. [S#](../../api.md) フィールドのインポート パラメーターを設定します。

   [ローソク足](candles.md) のインポートを参照してください。

   **CSV ファイルから取引（ティック）をインポートする例を考えてみましょう。**
   - インポートするデータのファイルは、次のテンプレートを持っています。

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{TradeId};{TradePrice};{TradeVolume};{OriginSide}

     ```

     ここで、{SecurityId.SecurityCode} と {SecurityId.BoardCode} の値は、それぞれ **Security** と **Board** の値に対応します。したがって、**Field order** フィールドには、それぞれ値 0 と 1 を割り当てます。
   - {ServerTime:default:yyyyMMdd} と {ServerTime:default:HH:mm:ss.ffffff} フィールドについては、**S# field** ウィンドウからそれぞれ **Date** と **Time** フィールドを選択します。値 2 と 3 を割り当てます。
   - {TradeId} フィールドについては、**S# field** ウィンドウから **識別子** フィールド、つまり取引識別子または取引番号を選択します。値 4 を割り当てます。
   - {TradePrice} フィールドについては、**S# field** ウィンドウから **Price** フィールド、つまり取引価格を選択します。値 5 を割り当てます。
   - {TradeVolume} フィールドについては、**S# field** ウィンドウから **Volume** フィールド、つまり取引数量を選択します。値 6 を割り当てます。
   - {OriginSide} フィールドについては、**S# field** ウィンドウから **Initiator** フィールド、つまり取引のイニシエーター（Seller または Buyer）を選択します。値 7 を割り当てます。
   - フィールド設定ウィンドウは次のようになります。![hydra import prop trade](../../../images/hydra_import_prop_trade.png)

   ユーザーは、ダウンロードされたデータに対して多数のプロパティを設定できます。インポートするファイル テンプレートに基づいて、プロパティを指定し、シーケンス内で必要な番号を割り当てる必要があります。
3. データをプレビューするには、**プレビュー** ボタンをクリックします。![hydra import preview trade](../../../images/hydra_import_preview_trade.png)
4. **インポート** ボタンをクリックします。
