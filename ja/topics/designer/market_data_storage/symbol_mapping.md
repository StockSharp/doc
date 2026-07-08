# シンボルマッピング

異なる取引システムでは、同じ銘柄が異なる名前で呼ばれる場合があります。**Designer** では、銘柄と、その銘柄を取引する接続を対応付け、外部取引システムでその銘柄がどのように識別されるかを指定できます。これは、1 つの銘柄を異なる取引ボードや異なる接続（またはブローカー）を通じて取引する場合に便利です。また、ある接続からデータを受信し、別の接続を通じて取引を行うこともできます。

証券と接続を対応付けるには、**All securities** パネルの **Securities and connections** ボタンをクリックします。

![Designer Security mapping 00](../../../images/designer_security_mapping_00.png)

開いたウィンドウで、![Designer Creation tool 00](../../../images/designer_creation_tool_00.png) ボタンをクリックして新しい行を追加します。

**Connection** 列で、ドロップダウンリストから接続を選択します。**Security code** 列と **Board code** 列には、**Designer** で指定されている証券コードとボードコードを指定します。**Adapter code** 列と **Adapter board** 列には、外部取引システムで指定されている証券コードとボードコードを指定します。

![Designer Security mapping 01](../../../images/designer_security_mapping_01.png)

