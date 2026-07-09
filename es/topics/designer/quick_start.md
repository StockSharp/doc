# Inicio rápido

En el primer inicio, [Designer](../designer.md) abre el diagrama preconfigurado de la estrategia de media móvil.

![Designer Quick start 01](../../images/designer_quick_start_01.png)

Para ejecutarla con datos históricos, debe descargar los datos en el formato correcto. Recomendamos usar [Hydra](../hydra.md), un programa diseñado para cargar automáticamente datos de mercado (instrumentos, velas, operaciones tick, libros de órdenes y otros datos) desde distintas fuentes y guardarlos en almacenamiento local. La descarga y el almacenamiento de datos históricos se describen en detalle en [Almacenamiento de datos de mercado](market_data_storage.md).

Después de descargar los datos con [Hydra](../hydra.md), especifique en [Designer](../designer.md) el directorio donde [Hydra](../hydra.md) guardó el historial. Esto se configura en la pestaña **Backtest** -> **Almacenamiento**.

![Designer Quick start 02](../../images/designer_quick_start_02.png)

Al hacer clic en ![Designer Edit Tool](../../images/designer_edit_tool_00.png), se abre la ventana **Data storage settings**, donde puede configurar almacenamiento local o remoto. También puede configurar [Hydra](../hydra.md) [en modo servidor](../hydra/server_mode/settings.md) como fuente de datos de mercado. Al hacer clic en ![[Designer_Settings_Repository_button.png]], se abre la ventana de selección de carpeta. Seleccione la carpeta donde guardó anteriormente el historial descargado por [Hydra](../hydra.md).

Ahora obtenga los instrumentos y sus datos desde el almacenamiento local configurado. Vaya a la pestaña **Común** y seleccione el componente **Datos de mercado**.

![Designer Quick start 02](../../images/designer_quick_start_03.png)

Se abre la pestaña de gestión de datos de mercado. Para obtener los instrumentos disponibles, haga clic en [Descargar instrumentos](market_data_storage/download_instruments.md). Para descargar un instrumento, introduzca su código o seleccione la bandera **Todos**, elija la fuente de datos y haga clic en **OK**. [Designer](../designer.md) solicita los instrumentos disponibles a la fuente de datos. Todos los instrumentos encontrados aparecen en el panel **Todos los instrumentos**.

Ahora [Designer](../designer.md) puede usar los instrumentos descargados y los datos históricos disponibles en el almacenamiento. Elija una de las estrategias de demostración. En el panel [Esquemas](user_interface/schemas.md), abra la carpeta **Estrategias** y haga doble clic en la estrategia de ejemplo **SMA**. La pestaña **Sma** aparece en el espacio de trabajo. Después de cambiar a la estrategia, la cinta abre automáticamente la pestaña **Backtest**, que contiene los controles principales para crear, depurar y probar estrategias ([Creación de estrategias](strategies/using_visual_designer.md), [Ejemplo de pruebas históricas](backtesting/getting_started.md)).

![Designer Quick start 03](../../images/designer_quick_start_04_1.png)

En la pestaña **Backtest**, establezca el período de prueba, seleccione el instrumento y elija el [Almacenamiento de datos de mercado](market_data_storage.md).

Al hacer clic en ![Designer Quick start 04](../../images/designer_quick_start_04.png) en el campo **Instrumento**, se abre la ventana **Seleccionar instrumento**. Seleccione el instrumento requerido en esta ventana.

![Designer Quick start 05](../../images/designer_quick_start_05.png)

Cuando selecciona cualquier bloque en el panel **Designer**, el panel **Propiedades** muestra las propiedades de ese bloque. En el panel **Propiedades** del bloque **Velas**, puede configurar el tipo de vela y Time Frame ([Velas](../api/candles.md)).

Después de hacer clic en **Iniciar**, comienza la emulación de trading. Los resultados de la prueba están disponibles en las pestañas correspondientes del diagrama: Gráfico, Órdenes, Operaciones, P/L, Posiciones (gráfico), Estadísticas y Posiciones.

![Designer Quick start 06](../../images/designer_quick_start_06.png)
