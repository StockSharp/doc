# Configuración del conector: Primary

Configure las siguientes propiedades antes de conectarse a Primary. La lista se ha verificado con [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Token` (`SecureString`)
- `Proprietary` (`string`)
- `DefaultMarket` (`string`)
- `MarketDataLevel` (`int`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SandboxWebSocketAddress` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_primary.md)

[Inicialización del adaptador](adapter_initialization_primary.md)
