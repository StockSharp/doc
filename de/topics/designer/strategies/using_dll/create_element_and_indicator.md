# Würfel und Indikator erstellen

Der Algorithmus zum Erstellen eines Würfels oder Indikators aus einer DLL-Assembly ähnelt dem Prozess aus Code (siehe Erstellen eines [Würfels](../using_code/csharp/creating_your_own_cube.md) und eines [Indikators](../using_code/csharp/create_own_indicator.md)), mit Ausnahme der Phase zur Auswahl des Inhalts. Ähnlich wie beim Hinzufügen einer [Strategie aus DLL](../using_dll.md) können Sie sowohl den Würfeltyp als auch den Indikatortyp auswählen, wenn diese in der verbundenen DLL erstellt wurden:

![Designer_Import_Element_00](../../../../images/designer_import_element_00.png)

Beim Erstellen eines Indikators binden Sie das NuGet-Paket [StockSharp.Algo](https://www.nuget.org/packages/stocksharp.algo) ein, um den Code zu kompilieren. Es enthält die Basisklasse für alle Indikatoren: [BaseIndicator](xref:StockSharp.Algo.Indicators.BaseIndicator).

![Designer_Import_Indicator_00](../../../../images/designer_import_indicator_00.png)

Beim Erstellen eines Würfels binden Sie das NuGet-Paket [StockSharp.Diagram.Core](https://www.nuget.org/packages/stockSharp.diagram.core) ein, um den Code zu kompilieren. Es enthält die Basisklasse für alle Würfel: [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement).

Beim Hinzufügen verbundener Würfel oder Indikatoren zum Diagramm müssen Sie die Schritte befolgen, die in den Abschnitten zum [Würfel](../using_code/csharp/creating_your_own_cube.md) oder [Indikator](../using_code/csharp/create_own_indicator.md) beschrieben sind.

## Siehe auch

[Debugging eines DLL-Würfels mit Visual Studio](debug_dll_in_visual_studio.md)

