# Crear un cubo e indicador

El algoritmo para crear un cubo o indicador desde un ensamblado DLL es similar al proceso desde código (vea la creación de un [cubo](../using_code/csharp/creating_your_own_cube.md) e [indicador](../using_code/csharp/create_own_indicator.md)), excepto por la etapa de selección de contenido. De forma similar, al añadir una [estrategia desde DLL](../using_dll.md), puede elegir tanto el tipo de cubo como el tipo de indicador si están creados en la DLL conectada:

![Crear un cubo e indicador 00 (1)](../../../../images/designer_import_element_00.png)

Al crear un indicador, incluya el paquete NuGet [StockSharp.Algo](https://www.nuget.org/packages/stocksharp.algo) para compilar el código. Contiene la clase base para todos los indicadores: [BaseIndicator](xref:StockSharp.Algo.Indicators.BaseIndicator).

![Crear un cubo e indicador 00 (2)](../../../../images/designer_import_indicator_00.png)

Al crear un cubo, incluya el paquete NuGet [StockSharp.Diagram.Core](https://www.nuget.org/packages/stockSharp.diagram.core) para compilar el código. Contiene la clase base para todos los cubos: [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement).

Al añadir cubos o indicadores conectados al diagrama, debe seguir los pasos descritos en las secciones de [cubo](../using_code/csharp/creating_your_own_cube.md) o [indicador](../using_code/csharp/create_own_indicator.md).

## Véase también

[Depuración de un cubo DLL con Visual Studio](debug_dll_in_visual_studio.md)
