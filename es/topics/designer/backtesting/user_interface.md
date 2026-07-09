# Interfaz de usuario

Para ejecutar la prueba sobre historial, debe seleccionar una estrategia cuyo esquema se probará sobre datos históricos. La estrategia se selecciona en el panel [Esquemas](../user_interface/schemas.md), dentro de la carpeta de estrategias, haciendo doble clic en la estrategia de interés. Al seleccionar una estrategia para el espacio de trabajo, aparece una nueva pestaña con la estrategia; al cambiar a esta pestaña, la pestaña **Emulación** se abrirá automáticamente en la cinta.

![Designer Interface Backtesting 00](../../../images/designer_interface_backtesting_00.png)

En la pestaña **Emulación**, puede cambiar el nombre de la estrategia y darle una breve descripción.

Para ejecutar la prueba sobre historial, en la **pestaña Emulation** especifique la ruta a los datos históricos en el campo Market Data y establezca el período de prueba. La estrategia para pruebas se inicia haciendo clic en el **botón Start** ![Designer Interface Backtesting 01](../../../images/designer_interface_backtesting_01.png). Después de iniciar la estrategia para pruebas, se activan el botón **Pausa** ![Designer Interface Backtesting 02](../../../images/designer_interface_backtesting_02.png), que suspende la prueba, y el botón **Detener** ![Designer Interface Backtesting 03](../../../images/designer_interface_backtesting_03.png), que detiene completamente la prueba. Al editar la estrategia, son útiles los botones **Undo(Ctrl+Z)** ![Designer Interface Backtesting 04](../../../images/designer_interface_backtesting_04.png), que cancela la última acción, **Redo(Ctrl+Y)** ![Designer Interface Backtesting 05](../../../images/designer_interface_backtesting_05.png), que rehace la acción deshecha, y **Refresh(Ctrl+R)** ![Designer Interface Backtesting 06](../../../images/designer_interface_backtesting_06.png), que actualiza completamente el esquema. Además, desde la **pestaña Emulation** puede usar el **Depurador** ([Depuración](debugging.md)) o ejecutar la **Optimización** de la estrategia.

La pestaña de la estrategia seleccionada contiene de forma predeterminada los siguientes paneles:

- El panel **Esquema**, en el que se realiza el trabajo principal de diseño de la estrategia y sus componentes, combinando cubos y líneas de conexión. El esquema se describe en detalle en la sección [Panel de diagrama](../strategies/using_visual_designer/diagram_panel.md).
- Panel de elementos de información, que contiene **Gráfico**, **Órdenes**, **Operaciones**, **Estadísticas** y otros componentes. Puede añadir el componente requerido seleccionándolo en la pestaña **Emulación**, en el grupo **Componentes**.
- El panel **Propiedades** está contraído de forma predeterminada en el lado derecho de la pestaña de estrategia. En el panel **Propiedades** puede organizar los ajustes generales de **Emulación**. Por ejemplo, el **Formato de almacenamiento de datos de mercado** puede establecerse en **BIN** o **CSV**, según el formato de archivo del almacenamiento seleccionado. El tipo de datos puede ser Ticks o Candles. Si se selecciona Ticks, las velas se formarán a partir de los ticks especificados en la [Configuración de backtesting](../user_interface/components/backtesting_settings.md).

## Contenido recomendado

[Configuración de backtesting](../user_interface/components/backtesting_settings.md)
