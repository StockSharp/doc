# Configuración del conector: Definedge

Configure las siguientes propiedades antes de conectarse a Definedge. La lista se ha verificado con [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `WebSocketToken` (`SecureString`)
- `UserId` (`string`)
- `AccountId` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `OneTimePassword` (`SecureString`)
- `DefaultProduct` (`DefinedgeProducts`)
- `AlgoId` (`string`)
- `Address` (`Uri`)
- `LoginAddress` (`Uri`)
- `HistoryAddress` (`Uri`)
- `InstrumentMasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_definedge.md)

[Inicialización del adaptador](adapter_initialization_definedge.md)
