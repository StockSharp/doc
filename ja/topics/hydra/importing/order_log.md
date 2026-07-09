# 注文ログ

注文ログをインポートするには、アプリケーションのメイン メニューから **インポート \=\> 注文ログ** 項目を選択します。

![hydra import orderlog](../../../images/hydra_import_orderlog.png)

## インポート プロセス。

1. **インポート設定**。

   [ローソク足](candles.md) のインポートを参照してください。
2. [S#](../../api.md) フィールドのインポート パラメーターを設定します。

   [ローソク足](candles.md) のインポートを参照してください。

   **CSV ファイルから注文ログをインポートする例を見てみましょう。**
   - インポートするデータのファイルは、次のテンプレートを持っています。

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{OrderId};{OrderPrice};{OrderVolume};{Side};{OrderState};{TimeInForce};{TradeId};{TradePrice}

     ```

     ここで、{SecurityId.SecurityCode} と {SecurityId.BoardCode} の値は、それぞれ **銘柄** と **ボード** の値に対応します。したがって、**フィールド順序** フィールドには、それぞれ値 0 と 1 を割り当てます。
   - {ServerTime:default:yyyyMMdd} と {ServerTime:default:HH:mm:ss.ffffff} フィールドについては、**S# フィールド** ウィンドウからそれぞれ **日付** と **時刻** フィールドを選択します。値 2 と 3 を割り当てます。
   - {OrderId} フィールドについては、**S# フィールド** ウィンドウから **ID** フィールド、つまり注文 ID を選択します。値 4 を割り当てます。
   - {OrderPrice} フィールドについては、**S# フィールド** ウィンドウから **価格** フィールド、つまり注文価格を選択します。値 5 を割り当てます
   - {OrderVolume} フィールドについては、**S# フィールド** ウィンドウから **数量** フィールド、つまり注文数量を選択します。値 6 を割り当てます。
   - {Side} フィールドについては、**S# フィールド** ウィンドウから **方向** フィールド、つまり注文方向（buy または sell）を選択します。値 7 を割り当てます。
   - {OrderState} フィールドについては、**S# フィールド** ウィンドウから **アクション** フィールド、つまり注文状態（アクティブ、非アクティブ、またはエラー）を選択します。値 8 を割り当てます。
   - {TimeInForce} フィールドについては、**S# フィールド** ウィンドウから **有効期間**、つまり指値注文の執行条件を選択します。値 9 を割り当てます。
   - {TradeId} フィールドについては、**S# フィールド** ウィンドウから **ID（約定）** フィールド、つまり取引識別子を選択します。値 10 を割り当てます。
   - {TradePrice} フィールドについては、**S# フィールド** ウィンドウから **価格（約定）** フィールド、つまり取引価格を選択します。値 11 を割り当てます。
   - フィールド設定ウィンドウは次のようになります。![hydra import prop orderlog](../../../images/hydra_import_prop_orderlog.png)

   ユーザーは、ダウンロードされたデータに対して多数のプロパティを設定できます。インポートするファイル テンプレートに基づいて、プロパティを指定し、シーケンス内で必要な番号を割り当てる必要があります。
3. データをプレビューするには、**プレビュー** ボタンをクリックします。![hydra import preview orderlog](../../../images/hydra_import_preview_orderlog.png)
4. **インポート** ボタンをクリックします。
