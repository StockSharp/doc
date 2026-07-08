# F# の使用

コードからストラテジーを作成する方法は、F# コードでの作業を好むユーザー向けです。このようなストラテジーは、ダイアグラムとは異なり機能に制限がなく、任意のアルゴリズムを記述できます。

ストラテジーの作成プロセスは、[Designer](../../../designer.md) 内で直接、または **F#** 開発環境 (最も一般的なのは **Visual Studio** と **JetBrains Rider**) で、**F#** による取引ロボットの専門的な開発用ライブラリと [API](../../../api.md) を使用して行います。

新しいストラテジーは、**Common** タブの **Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) ボタンを押して **Strategy** を選択することで追加できます。または、**Scheme** パネルの **Strategies** フォルダーを右クリックし、ドロップダウンメニューの **Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) ボタンを押します。

![Designer The creation of a strategy 00](../../../../images/designer_creation_of_strategy_00.png)

**Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) ボタンを押すと、ストラテジーを作成するコンテンツタイプを選択するウィンドウが表示されます。

![Designer_Creation_of_element_containing_source_code_00](../../../../images/designer_fsharp_create_strategy_00.png)

F# コードからストラテジーを作成するには、2 番目のタブを選択する必要があります。初期コードとして使用するテンプレートを選択することもできます。

**OK** を押すと、[ダイアグラム](../using_visual_designer.md)からストラテジーを作成した場合と同様に、新しいストラテジーが **Scheme** パネルの **Strategies** フォルダーに表示されます。ストラテジーの削除や名前変更も同様に行います。

ただし、ダイアグラムの代わりに F# コードエディターが表示されます。

![Designer_Creation_of_element_containing_source_code_01](../../../../images/designer_fsharp_create_strategy_01.png)

コードエディターのタブは、**Source Code** パネルと **Error List** パネルで構成されています。**Source Code** パネルには F# コードエディター本体があります。上部にはツールバーがあり、**Current Line**、**Line Number** などの強調表示をオンまたはオフにできます。フォントサイズを大きくするには、CTRL+MouseWheel の組み合わせを使用できます。

**Error List** パネルはコード内のエラー一覧を表示するテーブルで、行をダブルクリックすると **Source Code** パネル内のエラー位置へカーソルが自動的に移動します。

コードを編集すると、**Error List** パネルの右下隅にアイコン ![Designer The creation of the cube containing the source code 03](../../../../images/designer_creation_of_element_containing_source_code_03.png) が表示され、変更追跡が開始されたことを示します。コードの変更が停止した時点でコンパイルが行われます。

[バックテスト](../../backtesting/user_interface.md)、[ライブ](../../live_execution/getting_started.md)でのストラテジー実行、およびその他の操作は、ダイアグラムから作成したストラテジーと同様です。
