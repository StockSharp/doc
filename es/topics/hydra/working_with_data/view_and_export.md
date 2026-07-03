# Ver y exportar

Los datos recibidos por [Hydra](../../hydra.md) se pueden ver en paneles especiales.

Para ello, en la pestaña Common, haga clic en uno de los siguientes botones: [Ticks](view_and_export/ticks.md), [Order books](view_and_export/order_books.md), [Generación de velas](candles_generation.md), [Order log](view_and_export/order_log.md), [Level 1](view_and_export/level_1_.md), [News](view_and_export/news.md), [Transactions](view_and_export/transactions.md), [Option desk](view_and_export/option_desk.md), [Indicators](view_and_export/indicators.md), [Positions](view_and_export/positions.md).

O haga clic derecho en el tipo de datos requerido, como se muestra en la figura, así como doble clic en el tipo de datos requerido.

![hydra view export](../../../images/hydra_view_export.png)

Cada panel contiene una interfaz general de configuración como la siguiente:

![hydra export 00](../../../images/hydra_export_00.png)

- La línea superior indica el almacenamiento de datos de mercado y su formato (BIN o CSV).
- La línea inferior establece el período para el que se solicitarán los datos. Al hacer clic en el botón **Select security**, aparecerá la ventana de selección de instrumentos, donde puede seleccionar uno o varios instrumentos. Si se seleccionan varios instrumentos, durante la exportación posterior a Excel o CSV el programa ordenará automáticamente los datos de distintos instrumentos en archivos diferentes.
- Si, al construir una tabla con datos, la cantidad de datos descargados supera el límite establecido, aparecerá una ventana en pantalla:![hydra tick limit](../../../images/hydra_tick_limit.png)

  debe aumentar el límite de datos descargados.
- Si los datos se recibieron de fuentes cuya zona horaria no coincide con la zona horaria actual, puede ajustar la zona horaria. Después de construir, los datos se mostrarán en la zona seleccionada por el usuario. ![hydra TZ](../../../images/hydra_tz.png)
- Como varias fuentes no ofrecen la posibilidad de descargar algunos datos, el programa proporciona el campo [Build from](any_market_data_types.md). Con este campo, el usuario puede construir datos de mercado a partir de otro tipo de datos de mercado. La misma función puede usarse para construir datos de mercado sin descarga adicional, usando como base datos ya existentes.
- Después de seleccionar los parámetros anteriores, debe hacer clic en el botón ![hydra find](../../../images/hydra_find.png).![hydra candles tf](../../../images/hydra_candles_tf.png)

Con el menú contextual, puede configurar distintos parámetros de la tabla de valores de datos de mercado: agrupación de filas, columnas disponibles, formato de visualización, etc.

![hydra export context](../../../images/hydra_export_context.png)
