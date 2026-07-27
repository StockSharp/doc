# Configuración del conector: Mastertrust

Configure las siguientes propiedades antes de conectarse a Mastertrust. La lista se ha verificado con [MastertrustMessageAdapter](xref:StockSharp.Mastertrust.MastertrustMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `ClientId` (`string`)
- `OAuthClientId` (`string`)
- `OAuthClientSecret` (`SecureString`)
- `AuthorizationCode` (`SecureString`)
- `RedirectUri` (`Uri`)
- `Token` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`MastertrustProducts`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_mastertrust.md)

[Inicialización del adaptador](adapter_initialization_mastertrust.md)
