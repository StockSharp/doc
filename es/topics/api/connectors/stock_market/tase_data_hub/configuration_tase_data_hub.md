# Configuración del conector: TASE Data Hub

Configure las siguientes propiedades antes de conectarse a TASE Data Hub. La lista se ha verificado con [TaseDataHubMessageAdapter](xref:StockSharp.TaseDataHub.TaseDataHubMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Address` (`Uri`)
- `Scope` (`string`)
- `SecurityLookupDays` (`int`)
- `ReferenceCacheTimeout` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_tase_data_hub.md)

[Inicialización del adaptador](adapter_initialization_tase_data_hub.md)
