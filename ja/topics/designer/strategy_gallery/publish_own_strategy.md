# ストラテジーの公開

[Schemes](../user_interface/schemas.md) パネルでストラテジーをマウスの左ボタンでクリックし、**Publish** を選択することで、ストラテジーを公開できます:

![Designer_publish_00](../../../images/designer_publish_00.png)

**Publish** ボタンをクリックすると、エクスポート種類を選択するウィンドウが開きます。詳細については、[ストラテジーのエクスポート](../export_import/export.md) セクションを参照してください。

エクスポート種類を選択すると、公開パラメーターを使用して [Installer](../../installer.md) プログラムが起動されます（[Installer](../../installer.md) は事前に起動しておく必要があります）:

![Designer_publish_01](../../../images/designer_publish_01.png)

入力が必須のフィールド:

- Name
- Description
- Nuget package identifier. このパラメーターは、ストア内の製品へのリンクを設定するために必要です。たとえば、アドレス https://stocksharp.com/store/runner/ では、**runner** という単語がこのパラメーターを通じて指定されています。

**Free** または **Paid** レベルへのアクセスは、メール [info@stocksharp.com](mailto:info@stocksharp.com) で連絡した後にのみ付与されます。既定では **Private** レベルが使用可能であり、選択したユーザー向けのプライベート形式でのみストラテジーを公開できます:

**Save** ボタンをクリックすると、ストラテジーは StockSharp サーバーに送信されます。

更新を公開する場合、すべてのパラメーターを再度入力する必要はありません。製品パラメーターを入力する代わりに、更新用のノートを入力するウィンドウが表示されます:

![Designer_publish_02](../../../images/designer_publish_02.png)

**OK** ボタンをクリックすると、更新が成功したことを示すウィンドウが表示されます:

![Designer_publish_03](../../../images/designer_publish_03.png)
