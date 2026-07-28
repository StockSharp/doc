# Chainflip

**Chainflip** conecta StockSharp con la red de liquidez entre cadenas Chainflip. El adaptador expone los datos de mercado y las operaciones de transacción disponibles mediante el modelo estándar de mensajes de StockSharp.

## Funciones principales

- Funciones del adaptador verificadas en el código fuente: actualizaciones de datos en tiempo real, cotizaciones de nivel 1, operaciones ejecutadas, libros de órdenes, operaciones de cartera y órdenes.
- El transporte, las sesiones y los formatos del proveedor quedan ocultos tras la API estándar de StockSharp.

## Uso habitual

Utilice este conector para supervisar los fondos de liquidez de Chainflip, analizar cotizaciones ejecutables y libros de órdenes, y realizar los intercambios entre cadenas implementados por el adaptador.

Los activos y las cadenas de destino disponibles, los permisos de transacción, los límites de solicitudes y la disponibilidad dependen de Chainflip y del monedero conectado.

## Véase también

[Configuración del conector](chainflip/configuration_chainflip.md)

[Configuración gráfica](chainflip/graphical_configuration_chainflip.md)

[Inicialización del adaptador](chainflip/adapter_initialization_chainflip.md)
