# Configuración del conector: SEC API

Configure las siguientes propiedades antes de conectarse a SEC API. La lista se ha verificado con [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `ActiveOnly` (`bool`)
- `DefaultExchange` (`string`)
- `FormTypes` (`string`)
- `ResultLimit` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_sec_api.md)

[Inicialización del adaptador](adapter_initialization_sec_api.md)
