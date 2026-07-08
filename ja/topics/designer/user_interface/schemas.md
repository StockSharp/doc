# Schemes パネル

**Schemes** パネルを開くには、**Common** タブの **Schemes** ボタンをクリックする必要があります。**Schemes** パネルには、目的別にフォルダーへグループ化されたスクリプトのツリーが含まれています。ストラテジー スキームとカスタム ブロックに違いはありません。これらは共通のエディターである [ストラテジーデザイナー](../strategies/using_visual_designer/diagram_panel.md) を使用して編集されます。ただし、それらを混同しないように、2 つの独立したリストに分割され、異なるフォルダー (ストラテジーは **Backtest** フォルダー、カスタム ブロックは **Custom Blocks** フォルダー) に保存されます。編集するスキームは、リスト内の必要な項目をダブルクリックして選択します。その後、選択したスキームがデザイナーで開かれ、表示および編集できます。以下は、**Schemes** パネル内のフォルダーの説明です。

![Designer Panel Circuits 00](../../../images/designer_panel_circuits_00.png)

1. **Backtest** フォルダーには、要素のセットとそれらの間の接続からスキームとして作成された取引ストラテジー、およびコードから作成された取引ストラテジーの両方が含まれます。新しいストラテジーは、**Common** タブの **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) ボタンを押し、**Strategy** を選択して追加できます。または、**Schemes** パネルの **Backtest** フォルダーを右クリックし、ドロップダウン メニューの **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) ボタンを押して追加できます。開いたウィンドウで、ストラテジーをどのように作成するかを選択します。
   
    ![Designer Panel Circuits 04](../../../images/designer_panel_circuits_04.png)
   
    ストラテジーは、コーディングなしのビジュアル デザイナーを使用して作成することも、組み込みのソース コード エディターを使用して作成することもできます。さらに、Microsoft Visual Studio で記述されたストラテジーを含む外部 DLL ファイルを接続できます。**Strategies** の詳細情報は、[ブロックの使用](../strategies/using_visual_designer.md)セクションで説明されています。

2. **Own elements** フォルダーには、完全な機能を表し、さまざまなスキームで使用したり、1 つのスキーム内で異なるプロパティ値を使って複数回使用したりできる要素が含まれます。このような要素のセットは別個のブロックとして抽出でき、その後は任意の標準要素と同じように使用されます。**Custom block** は通常のスキームであり、任意のストラテジー スキームと同様に保存/読み込み/編集されます。新しい複合要素は、**Common** タブの **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) ボタンを押し、**Custom Blocks** を選択して追加します。または、**Schemes** パネルの **Custom Blocks** フォルダーを右クリックし、ドロップダウン メニューの **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) ボタンを押して追加します。新しいカスタム ブロックを追加すると、それらは自動的に **Element Palette** の **Custom Blocks** グループに追加され、他のストラテジー スキームやカスタム ブロックの作成に使用できます。**Custom Blocks** の詳細情報は、[複合要素の作成](../strategies/using_visual_designer/composite_elements.md)セクションで説明されています。

3. **Live** フォルダーには、取引用に追加されたストラテジーが含まれます。起動中のストラテジーはアイコン ![Designer Panel Circuits 02](../../../images/designer_panel_circuits_02.png) で示され、停止中のストラテジーはアイコン ![Designer Panel Circuits 03](../../../images/designer_panel_circuits_03.png) で示されます。**Live** フォルダーにストラテジーを追加する方法と起動する方法は、[実取引](../live_execution/getting_started.md)セクションで説明されています。

4. **Indicators** フォルダーには、自分で記述した取引ストラテジー用の独自インジケーターが含まれます。新しいインジケーターはスキームでは作成できず、コードと外部 DLL ファイルのみが使用可能です。スキームでのカスタム インジケーターの使用は、インジケーターの種類を選択するときに [インジケーター](../strategies/using_visual_designer/elements/common/indicator.md) ブロックを通じて利用できます。

5. **Remote** フォルダーには、リモート サーバー上にあるストラテジーが含まれます。

## 関連項目

[Logs パネル](logs.md)

