# キューブとインジケーターの作成

DLL アセンブリからキューブまたはインジケーターを作成するアルゴリズムは、コンテンツ選択段階を除き、コードから作成するプロセス（[キューブ](../using_code/csharp/creating_your_own_cube.md)と[インジケーター](../using_code/csharp/create_own_indicator.md)の作成を参照）と同様です。同様に、[DLL からストラテジー](../using_dll.md)を追加する場合、接続された DLL 内に作成されていれば、キューブタイプとインジケータータイプの両方を選択できます。

![キューブとインジケーターの作成 00](../../../../images/designer_import_element_00.png)

インジケーターを作成する場合は、コードをコンパイルするために NuGet パッケージ [StockSharp.Algo](https://www.nuget.org/packages/stocksharp.algo) を含めます。これには、すべてのインジケーターの基底クラス [BaseIndicator](xref:StockSharp.Algo.Indicators.BaseIndicator) が含まれています。

![キューブとインジケーターの作成 00](../../../../images/designer_import_indicator_00.png)

キューブを作成する場合は、コードをコンパイルするために NuGet パッケージ [StockSharp.Diagram.Core](https://www.nuget.org/packages/stockSharp.diagram.core) を含めます。これには、すべてのキューブの基底クラス [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) が含まれています。

接続されたキューブまたはインジケーターをダイアグラムに追加する場合は、[キューブ](../using_code/csharp/creating_your_own_cube.md)または[インジケーター](../using_code/csharp/create_own_indicator.md)のセクションで説明されている手順に従う必要があります。

## 関連項目

[Visual Studio で DLL キューブをデバッグ](debug_dll_in_visual_studio.md)
