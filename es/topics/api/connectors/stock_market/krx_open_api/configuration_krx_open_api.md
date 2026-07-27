# Configuración del conector: KRX Open API

Configure las siguientes propiedades antes de conectarse a KRX Open API. La lista se ha verificado con [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`KrxDataSets`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `MaxRequests` (`int`)
- `Address` (`Uri`)
- `SampleAddress` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_krx_open_api.md)

[Inicialización del adaptador](adapter_initialization_krx_open_api.md)
