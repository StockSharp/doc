# Pendle

**Pendle** conecta StockSharp con los mercados de rendimiento de Pendle en las redes EVM admitidas. El adaptador expone los datos de mercado y las operaciones de transacción disponibles mediante el modelo estándar de mensajes de StockSharp.

## Funciones principales

- Funciones del adaptador verificadas en el código fuente: actualizaciones de datos en tiempo real, cotizaciones de nivel 1, datos de velas, solicitudes de datos históricos, operaciones de cartera y órdenes.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Utilice este conector para supervisar los mercados de rendimiento de Pendle, analizar cotizaciones y velas, solicitar datos históricos y ejecutar los flujos de transacción implementados por el adaptador.

Las redes y los mercados disponibles, los permisos de transacción, los límites de solicitudes y la disponibilidad dependen de Pendle y del monedero conectado.

## Véase también

[Configuración del conector](pendle/configuration_pendle.md)

[Configuración gráfica](pendle/graphical_configuration_pendle.md)

[Inicialización del adaptador](pendle/adapter_initialization_pendle.md)
