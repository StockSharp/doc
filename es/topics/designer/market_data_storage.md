# Almacenamiento de datos de mercado

El almacenamiento de datos históricos está diseñado para cargar datos de mercado (instrumentos, velas, operaciones tick y libros de órdenes) desde distintas fuentes y guardarlos en un almacenamiento local o remoto. [Designer](../designer.md) puede usar fuentes tanto de datos históricos como de datos en tiempo real ([Conectores](../api/connectors.md)). Después, la información almacenada queda disponible para su uso por estrategias de trading.

Para abrir la pestaña **Market data**, cambie a la pestaña **Common** y haga clic en el botón **Market data**. El área **Market data** está dividida en tres zonas. La zona izquierda contiene la lista de todos los instrumentos recibidos de todas las fuentes que se han conectado alguna vez. Las zonas centrales contienen los instrumentos activos. Con estos instrumentos puede descargar o ver el historial descargado. La zona derecha muestra los datos disponibles para el instrumento seleccionado en la zona central, y también puede descargar datos usando el instrumento seleccionado.

![Designer Repository of historical data 00](../../images/designer_repository_of_historical_data_00.png)

## Contenido recomendado

[Primeros pasos](market_data_storage/getting_started.md)
