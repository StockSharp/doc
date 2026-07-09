# 銘柄と接続の照合

同じ銘柄でも、取引システムが異なると別の名前で呼ばれることがあります。銘柄と、その銘柄を取引する接続を照合し、外部取引システムでどのように識別されるかを指定できます。

これにより、受信したデータを整理し、保存を簡素化できます。実際には、さまざまなソースから入ってくるすべてのデータが、ソース名ではなく銘柄名によって 1 か所に統合されます。

これは、同じ銘柄を異なる取引ボード上、または異なる接続（またはブローカー）経由で取引する場合にも役立ちます。また、ある接続からデータを取得し、別の接続経由で取引を行うこともできます。

銘柄と接続を照合するには、次の手順を実行します。

1. **銘柄** タブに移動し、**銘柄と接続** ボタンをクリックします。![Designer Security mapping 01 00](../../../images/designer_security_mapping_01_00.png)
2. 接続のリストで必要な接続を選択します。![Designer Security mapping 01](../../../images/designer_security_mapping_01.png)
3. すべての列に入力します。

   例:

   APPLE 株式銘柄。
   - 接続 - **Interactive Brokers**。![Designer Creation tool 00](../../../images/designer_creation_tool_00.png) ボタンをクリックすると、新しい行が追加されます。
   - **銘柄コード** 列と **ボードコード** 列に、銘柄コードとボードコードを指定します。**アダプター内の銘柄コード** 列と **アダプター内のボードコード** 列には、外部取引システムで指定されている銘柄コードとボードコードを指定します。**OK** をクリックします。![Designer Security mapping 01 01](../../../images/designer_security_mapping_01_01.png)
   - **Interactive Brokers** 接続と **CQG Continuum** 接続について、同じ方法で手順を繰り返します。

   | **Interactive Brokers**                                                           | **CQG Continuum**                                                                 |
   | --------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
   | ![Designer Security mapping 01 02](../../../images/designer_security_mapping_01_02.png) | ![Designer Security mapping 01 03](../../../images/designer_security_mapping_01_03.png) |
4. これで、ダウンロードされたすべてのデータ（この例では APPLE 株式のデータ）が 1 か所に保存されます。

