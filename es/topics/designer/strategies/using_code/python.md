# Uso de Python

La creación de estrategias desde código está pensada para usuarios que prefieren trabajar con código Python. Estas estrategias, a diferencia de los esquemas, no están limitadas en capacidades y puede implementarse cualquier algoritmo.

El proceso de creación de una estrategia se realiza directamente en [Designer](../../../designer.md) o en un entorno de desarrollo **Python** (los entornos de desarrollo más populares son **Visual Studio** y **JetBrains Rider**), usando la biblioteca para el desarrollo profesional de robots de trading en **Python** y la [API](../../../api.md).

Puede añadir una nueva estrategia haciendo clic en el botón **Añadir** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) en la pestaña **Común** y seleccionando **Estrategia**. O haciendo clic con el botón derecho en la carpeta **Estrategias** del panel **Esquemas** y pulsando el botón **Añadir** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) en el menú desplegable:

![Designer The creation of a strategy 00](../../../../images/designer_creation_of_strategy_00.png)

Después de hacer clic en el botón **Añadir** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png), aparecerá una ventana con la elección del tipo de contenido sobre el que crear la estrategia:

![Designer_Creation_of_element_containing_source_code_00](../../../../images/designer_python_create_strategy_00.png)

Para crear una estrategia desde código Python, seleccione la segunda pestaña. También puede elegir una plantilla que se usará como código inicial.

Después de hacer clic en **OK**, aparecerá una nueva estrategia en la carpeta **Estrategias** del panel **Esquemas**, de forma similar a la creación de una estrategia desde un [esquema](../using_visual_designer.md). Las acciones para eliminar o renombrar la estrategia también son similares.

Pero en lugar de un esquema, se mostrará un editor de código Python:

![Designer_Creation_of_element_containing_source_code_01](../../../../images/designer_python_create_strategy_01.png)

La pestaña del editor de código consta de los paneles **Source Code** y **Error List**. El panel **Source Code** contiene el propio editor de código Python. En la parte superior hay una barra de herramientas donde puede activar o desactivar el resaltado de elementos como **Current Line**, **Line Number**, etc. Para aumentar el tamaño de fuente, puede usar la combinación CTRL+MouseWheel.

El panel **Error List** es una tabla con la lista de errores de código; al hacer doble clic en una línea, el cursor se moverá automáticamente en el panel **Source Code** a la ubicación del error.

Al editar código, aparecerá un icono ![Designer The creation of the cube containing the source code 03](../../../../images/designer_creation_of_element_containing_source_code_03.png) en la esquina inferior derecha del panel **Error List**, indicando que ha comenzado el seguimiento de cambios. La compilación del código ocurre cuando el código deja de cambiar.

La ejecución de la estrategia en [backtest](../../backtesting/user_interface.md), en [live](../../live_execution/getting_started.md) y otras operaciones funcionan de forma similar a las estrategias creadas desde esquemas.

## Limitaciones

> [!WARNING]
> [Designer](../../../designer.md) usa IronPython, que tiene las siguientes limitaciones:
> - Compatibilidad con Python versión 3.4
> - Soporte parcial de numpy mediante una implementación especial de .NET (puede encontrar un ejemplo de uso [aquí](https://github.com/StockSharp/StockSharp/blob/master/Algo.Analytics.Python/pearson_correlation_script.py))
> - Falta de soporte para otras bibliotecas populares escritas en C (pandas, scipy, etc.)
> - Soporte limitado para programación asíncrona
> - Imposibilidad de usar ciertos módulos integrados de Python debido a la dependencia de implementaciones específicas de CPython
> - El rendimiento puede ser inferior al de CPython en algunas operaciones
>
> Se recomienda tener en cuenta estas limitaciones al desarrollar estrategias de trading en Python dentro de [Designer](../../../designer.md).
