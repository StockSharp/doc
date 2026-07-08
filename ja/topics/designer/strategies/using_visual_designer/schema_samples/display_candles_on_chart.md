# チャートにローソク足を表示する

インストゥルメントのローソク足をチャートに出力するには、次のスキーマを使用できます:

![Designer The conclusion of the candles on the chart 00](../../../../../images/designer_conclusion_of_candles_on_chart_00.png)

[Variable](../elements/data_sources/variable.md) キューブでは、**Instrument** データ型が選択されています。インストゥルメントが指定されていないが、**Common** プロパティグループの **Parameters** フラグが設定されている場合、インストゥルメントはストラテジーから取得され、[Candles](../elements/data_sources/candles.md) キューブに渡されます。[Candles](../elements/data_sources/candles.md) キューブでは、5 分足を構築し、完全に形成済みのローソク足のみを渡すための設定が指定されています。

[Chart](../elements/common/chart.md) キューブには、ローソク足型のグラフィック要素が 1 つ追加され、その入力パラメーターが自動的に追加されました。

必要なグラフィック要素をチャートパネルに追加した後、[Candles](../elements/data_sources/candles.md) 要素と [Chart](../elements/common/chart.md) 要素の接続を追加します。この接続を通じて、構築されたローソク足がチャートへの出力用に渡されます。

## 推奨コンテンツ

[インストゥルメントの最良価格を取得する](get_best_price_for_instrument.md)
