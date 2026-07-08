# オーダーブックの価格レベルを取得する

オーダーブックから必要な買い行を取得するには、次のスキーマを使用できます:

![Designer Event model 00](../../../../../images/designer_event_model_00.png)

[変数](../elements/data_sources/variable.md) キューブでは、**Instrument** データ型が選択されています。インストゥルメントが指定されていないが、**Common** プロパティグループの **Parameters** フラグが設定されている場合、それはストラテジーから取得されます。[コンバーター](../elements/converters/converter.md) キューブでは、Bids 買いのために、データ型とキューブコレクションの対応するフィールドが選択されています。Indexer キューブは、買いの最良価格のコレクションから必要な要素を取得します。特定のレベルの価格または数量の値を取得するには、[コンバーター](../elements/converters/converter.md) キューブを使用できます。

## 推奨コンテンツ

[ギャラリー](../../../strategy_gallery.md)
