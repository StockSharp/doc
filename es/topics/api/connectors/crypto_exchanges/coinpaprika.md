# CoinPaprika

**CoinPaprika** conecta StockSharp con el servicio de datos del mercado de criptomonedas CoinPaprika. El adaptador expone los datos del proveedor mediante el modelo estándar de mensajes de StockSharp.

## Funciones principales

- Funciones del adaptador verificadas en el código fuente: actualizaciones de datos en tiempo real, cotizaciones de nivel 1, datos de velas, solicitudes de datos históricos.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.
- Este adaptador está destinado al acceso a datos y no enruta órdenes.

## Uso habitual

Utilice este conector para alimentar gráficos, almacenamiento de datos de mercado, análisis, investigación y pruebas de estrategias.

Los instrumentos, la profundidad de datos, los permisos de transacción, los límites de solicitudes y la disponibilidad dependen de CoinPaprika, del plan de API y de la cuenta conectada.

## Véase también

[Configuración del conector](coinpaprika/configuration_coinpaprika.md)

[Configuración gráfica](coinpaprika/graphical_configuration_coinpaprika.md)

[Inicialización del adaptador](coinpaprika/adapter_initialization_coinpaprika.md)
