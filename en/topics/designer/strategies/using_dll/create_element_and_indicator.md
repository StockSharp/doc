# Creating a Cube and Indicator

The algorithm for creating a cube or indicator from a DLL assembly is similar to the process from code (see the creation of a [cube](../using_code/csharp/creating_your_own_cube.md) and [indicator](../using_code/csharp/create_own_indicator.md)) except for the content selection stage. Similarly, when adding a [strategy from DLL](../using_dll.md), you can choose both the type of cube and indicators if they are created in the connected DLL:

![Designer_Import_Element_00](../../../../images/designer_import_element_00.png)

When creating an indicator, to compile the code it is necessary to include the NuGet package [StockSharp.Algo](https://www.nuget.org/packages/stocksharp.algo), where the base class for all indicators [Strategy](xref:StockSharp.Algo.Indicators.BaseIndicator) is located.

![Designer_Import_Indicator_00](../../../../images/designer_import_indicator_00.png)

When creating a cube, to compile the code it is necessary to include the NuGet package [StockSharp.Diagram.Core](https://www.nuget.org/packages/stockSharp.diagram.core), where the base class for all cubes [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) is located.

When adding connected cubes or indicators to the diagram, you need to follow the steps described in the sections for a [cube](../using_code/csharp/creating_your_own_cube.md) or [indicator](../using_code/csharp/create_own_indicator.md).

## See Also

[Debugging a DLL Cube with Visual Studio](debug_dll_in_visual_studio.md)
