# Создание кубика и индикатора

Алгоритм создания кубика или индикатор из DLL сборки аналогичен процессу из кода (см создание [кубика](../using_code/csharp/creating_your_own_cube.md) и [индикатора](../using_code/csharp/create_own_indicator.md)) за исключением этапа выбора контента. Аналогично при добалении [стратегии из DLL](../using_dll.md) можно выбрать и тип кубика и тип индикаторы, если они созданы в подключаемой DLL:

![Designer_Import_Element_00](../../../../images/designer_import_element_00.png)

В случае создания индикатора, для компилирования кода необходимо подключить NuGet пакет [StockSharp.Algo](https://www.nuget.org/packages/stocksharp.algo), где находится базовый класс для всех индикаторов [Strategy](xref:StockSharp.Algo.Indicators.BaseIndicator).

![Designer_Import_Indicator_00](../../../../images/designer_import_indicator_00.png)

В случае создания кубика, для компилирования кода необходимо подключить NuGet пакет [StockSharp.Diagram.Core](https://www.nuget.org/packages/stockSharp.diagram.core), где находится базовый класс для всех кубиков [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement).

При добавлении подключенных кубиков или индикаторов на схему необходимо выполнить шаги, описанные в разделах [кубика](../using_code/csharp/creating_your_own_cube.md) или [индикатора](../using_code/csharp/create_own_indicator.md).

## См. также

[Отладка DLL кубика с помощью Visual Studio](debug_dll_in_visual_studio.md)
