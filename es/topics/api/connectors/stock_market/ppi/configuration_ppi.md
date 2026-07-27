# Configuración del conector: PPI

Configure las siguientes propiedades antes de conectarse a PPI. La lista se ha verificado con [PpiMessageAdapter](xref:StockSharp.Ppi.PpiMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AuthorizedClient` (`string`)
- `ClientKey` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Token` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `DefaultMarket` (`string`)
- `DefaultInstrumentType` (`string`)
- `DefaultSettlement` (`string`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `RealtimeAddress` (`Uri`)
- `SandboxRealtimeAddress` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_ppi.md)

[Inicialización del adaptador](adapter_initialization_ppi.md)
