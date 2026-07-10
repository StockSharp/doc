# チャートにローソク足を表示する

インストゥルメントのローソク足をチャートに出力するには、次のスキーマを使用できます:

![Designer チャートへのローソク足出力 00](../../../../../images/designer_conclusion_of_candles_on_chart_00.png)

[変数](../elements/data_sources/variable.md) キューブでは、**銘柄** データ型が選択されています。インストゥルメントが指定されていないが、**共通** プロパティグループの **パラメーター** フラグが設定されている場合、インストゥルメントはストラテジーから取得され、[ローソク足](../elements/data_sources/candles.md) キューブに渡されます。[ローソク足](../elements/data_sources/candles.md) キューブでは、5 分足を構築し、完全に形成済みのローソク足のみを渡すための設定が指定されています。

[チャート](../elements/common/chart.md) キューブには、ローソク足型のグラフィック要素が 1 つ追加され、その入力パラメーターが自動的に追加されました。

必要なグラフィック要素をチャートパネルに追加した後、[ローソク足](../elements/data_sources/candles.md) 要素と [チャート](../elements/common/chart.md) 要素の接続を追加します。この接続を通じて、構築されたローソク足がチャートへの出力用に渡されます。

## 推奨コンテンツ

[インストゥルメントの最良価格を取得する](get_best_price_for_instrument.md)
