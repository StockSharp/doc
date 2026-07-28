# Settrade

**Settrade** conecta StockSharp con Settrade Open API v2 para las acciones de la Bolsa de Tailandia y los derivados de TFEX. El adaptador expone los datos de mercado y las operaciones de transacción disponibles mediante el modelo estándar de mensajes de StockSharp.

## Funciones principales

- Funciones del adaptador verificadas en el código fuente: actualizaciones de datos en tiempo real, cotizaciones de nivel 1, libros de órdenes, datos de velas, solicitudes de datos históricos, operaciones de cartera y órdenes para cuentas de acciones y derivados.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Utilice este conector para supervisar instrumentos de SET y TFEX, analizar cotizaciones, libros de órdenes y velas, solicitar datos históricos y ejecutar los flujos de órdenes admitidos.

Los instrumentos disponibles, la profundidad de datos, los permisos de negociación, los límites de solicitudes y la disponibilidad dependen de Settrade, del corredor y de la cuenta conectada.

## Véase también

[Configuración del conector](settrade/configuration_settrade.md)

[Configuración gráfica](settrade/graphical_configuration_settrade.md)

[Inicialización del adaptador](settrade/adapter_initialization_settrade.md)
