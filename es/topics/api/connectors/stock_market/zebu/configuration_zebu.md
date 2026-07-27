# Configuración del conector: Zebu

Configure las siguientes propiedades antes de conectarse a Zebu. La lista se ha verificado con [ZebuMessageAdapter](xref:StockSharp.Zebu.ZebuMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AuthorizationCode` (`SecureString`)
- `UserId` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `RefreshToken` (`SecureString`)
- `Token` (`SecureString`)
- `AccountId` (`string`)
- `TokenExpiresAt` (`DateTime?`)
- `DefaultProduct` (`ShoonyaProducts`)
- `ReconnectAttempts` (`int`)
- `AuthorizationAddress` (`Uri`)
- `RestEndpoint` (`string`)
- `InstrumentEndpointTemplate` (`string`)
- `WebSocketEndpoint` (`string`)

## Véase también

[Configuración gráfica](graphical_configuration_zebu.md)

[Inicialización del adaptador](adapter_initialization_zebu.md)
