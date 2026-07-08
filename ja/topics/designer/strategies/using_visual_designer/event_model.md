# イベントモデル

[Designer](../../../designer.md) でスキーマを構築するアプローチは、イベントの生成とその後の処理に基づいています。ストラテジーを構築する際、マーケットデータ変更イベントがいつ発生するかは分かりませんが、このイベントを購読し、それに応じて処理できます。

出力パラメーターを持つ各 [Designer](../../../designer.md) キューブは、イベントの生成元です。また、入力パラメーターを持つキューブは、出力パラメーターによって生成されたイベントを購読できます。イベントを購読することは、2 つのキューブ間に接続線を作成することにほかなりません。

たとえば、[Order book](elements/market_depths/order_book.md) キューブはオーダーブック変更イベントを生成します。変更がいつ発生するかは事前には分かりません。[Order book](elements/market_depths/order_book.md) キューブと [Converter](elements/converters/converter.md) キューブの間に接続線を作成すると、[Converter](elements/converters/converter.md) キューブなどでさらに処理するために、オーダーブック変更に対する購読が行われます:

![Designer Event model 00](../../../../images/designer_event_model_00.png)

## 推奨コンテンツ

[最初のストラテジー](first_strategy.md)
