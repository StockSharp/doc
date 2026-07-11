# データのエクスポート

[Hydra](../../hydra.md) では、受信したマーケットデータを [MetaStock データ形式](export_data/export_into_metastock.md) など、さまざまな形式でエクスポートできます。

データのエクスポートには、[Excel](https://en.wikipedia.org/wiki/Excel)、xml、bin、txt、Json 形式のファイル、または SQL テーブルが使用されます。

エクスポートするには、ドロップダウンリストから必要なファイル形式を選択します。

![Hydra エクスポート](../../../images/hydra_export.png)

次に、フォルダーを選択し、必要に応じてファイル名を変更します。

テキストファイル (txt) へエクスポートする場合、次の形式のエクスポートテンプレートを指定できるウィンドウが表示されます。 

**{OpenTime:default:yyyyMMdd};{OpenTime:default:HH:mm:ss};{OpenPrice};{HighPrice};{LowPrice};{ClosePrice};{TotalVolume}**

ここでは、中括弧内に、エクスポートするプロパティとその順序がセミコロン区切りで示されています。

**プレビュー** ボタンをクリックすると、ファイルに保存されるデータを確認できます。

![Hydra TSLab MetaStock エクスポート 1](../../../images/hydra_export_tslab_metastock_1.png)

ユーザーは、**{SecurityId.SecurityCode}** プロパティを使用して銘柄コードなどの追加プロパティを追加したり、時間枠の値を指定したりできます。

プロパティ名を示すヘッダーを追加できます。この場合、レコードは次のようになります。

![Hydra TSLab MetaStock エクスポート 2](../../../images/hydra_export_tslab_metastock_2.png)

コロンを使用する形式でエクスポートする必要がある場合は、上記の例 **{OpenTime:default:HH:mm:ss}** のように default キーワードを指定する必要があります。

**[動画チュートリアル](../videos/saving_format.md)を見る**
