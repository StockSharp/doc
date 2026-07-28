# Dexalot

**Dexalot** conecta StockSharp con el libro central de órdenes limitadas de Dexalot que funciona en la cadena de bloques. El adaptador expone los datos de mercado y las operaciones de transacción disponibles mediante el modelo estándar de mensajes de StockSharp.

## Funciones principales

- Funciones del adaptador verificadas en el código fuente: actualizaciones de datos en tiempo real, cotizaciones de nivel 1, operaciones ejecutadas, libros de órdenes, datos de velas, solicitudes de datos históricos, operaciones de cartera y órdenes.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Utilice este conector para supervisar los mercados de Dexalot, analizar cotizaciones, operaciones, libros de órdenes y velas, solicitar datos históricos y enviar las órdenes admitidas.

Los pares disponibles, la profundidad de datos, los permisos de transacción, los límites de solicitudes y la disponibilidad dependen de Dexalot y del monedero conectado.

## Véase también

[Configuración del conector](dexalot/configuration_dexalot.md)

[Configuración gráfica](dexalot/graphical_configuration_dexalot.md)

[Inicialización del adaptador](dexalot/adapter_initialization_dexalot.md)
