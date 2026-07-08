# Panel Schemes

Para abrir el panel **Schemes**, debe hacer clic en el botón **Schemes** de la pestaña **Common**. El panel **Schemes** contiene un árbol de scripts, agrupados en carpetas por propósito. Los esquemas de estrategia y los bloques personalizados no difieren. Se editan usando un editor común, [Diseñador de estrategias](../strategies/using_visual_designer/diagram_panel.md). Sin embargo, para evitar confusión entre ellos, se dividen en dos listas independientes y se almacenan en carpetas diferentes (estrategias en la carpeta **Backtest**, bloques personalizados en la carpeta **Custom Blocks**). La selección de un esquema para editar se realiza haciendo doble clic en el elemento requerido de la lista. El esquema seleccionado se abrirá entonces en el diseñador para verlo y editarlo. A continuación se describen las carpetas del panel **Schemes**:

![Designer Panel Circuits 00](../../../images/designer_panel_circuits_00.png)

1. La carpeta **Backtest** contiene estrategias de trading creadas tanto como esquemas de un conjunto de elementos y conexiones entre ellos, como desde código. Puede añadir una nueva estrategia pulsando el botón **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) en la pestaña **Common** y seleccionando **Strategy**. O haciendo clic con el botón derecho en la carpeta **Backtest** del panel **Schemes** y pulsando el botón **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) en el menú desplegable. En la ventana que se abre, seleccione exactamente cómo desea crear una estrategia.
   
    ![Designer Panel Circuits 04](../../../images/designer_panel_circuits_04.png)
   
    Las estrategias pueden crearse usando un diseñador visual sin escribir código o usando el editor de código fuente integrado. Además, pueden conectarse archivos DLL externos con estrategias escritas en Microsoft Visual Studio. La información detallada sobre **Strategies** se describe en la sección [Uso de bloques](../strategies/using_visual_designer.md).

2. La carpeta **Own elements** contiene elementos que representan una funcionalidad completa y pueden usarse en varios esquemas o varias veces dentro de un esquema con distintos valores de propiedades. Estos conjuntos de elementos pueden extraerse a un bloque separado, que luego se usará como cualquier elemento estándar. **Custom block** es un esquema normal que se guarda/carga/edita igual que cualquier esquema de estrategia. Añada un nuevo elemento compuesto pulsando el botón **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) en la pestaña **Common** y seleccionando **Custom Blocks**. O haciendo clic con el botón derecho en la carpeta **Custom Blocks** del panel **Schemes** y pulsando el botón **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) en el menú desplegable. Al añadir nuevos bloques personalizados, se añaden automáticamente a la **Element Palette**, en el grupo **Custom Blocks**, y pueden usarse al crear otros esquemas de estrategia y bloques personalizados. La información detallada sobre **Custom Blocks** se describe en la sección [Creación de elementos compuestos](../strategies/using_visual_designer/composite_elements.md).

3. La carpeta **Live** contiene estrategias añadidas para trading. Las estrategias iniciadas se marcan con el icono ![Designer Panel Circuits 02](../../../images/designer_panel_circuits_02.png), y las detenidas se marcan con el icono ![Designer Panel Circuits 03](../../../images/designer_panel_circuits_03.png). Cómo añadir estrategias a la carpeta **Live** y cómo iniciarlas se describe en la sección [Trading en vivo](../live_execution/getting_started.md).

4. La carpeta **Indicators** contiene sus propios indicadores para estrategias de trading, escritos por usted. No se pueden crear nuevos indicadores con esquemas; solo están disponibles código y archivos DLL externos. El uso de indicadores personalizados en esquemas está disponible mediante el bloque [Indicador](../strategies/using_visual_designer/elements/common/indicator.md) al seleccionar el tipo de indicador.

5. La carpeta **Remote** contiene estrategias ubicadas en un servidor remoto.

## Véase también

[Panel de logs](logs.md)
