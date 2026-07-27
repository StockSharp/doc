# Configuración del conector: Choice FinX

Configure las siguientes propiedades antes de conectarse a Choice FinX. La lista se ha verificado con [ChoiceFinXMessageAdapter](xref:StockSharp.ChoiceFinX.ChoiceFinXMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `AuthorizationHeader` (`string`)
- `AuthorizationScheme` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `VendorId` (`string`)
- `VendorKey` (`SecureString`)
- `WebSocketToken` (`SecureString`)
- `DefaultProduct` (`ChoiceFinXProducts`)
- `PortfolioName` (`string`)
- `ModeType` (`string`)
- `Mode` (`int?`)
- `DeviceId` (`string`)
- `PriceDivisor` (`decimal`)
- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_choice_finx.md)

[Inicialización del adaptador](adapter_initialization_choice_finx.md)
