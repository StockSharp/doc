# Configuración del conector: Rupeezy

Configure las siguientes propiedades antes de conectarse a Rupeezy. La lista se ha verificado con [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `ApplicationId` (`string`)
- `ApiKey` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`RupeezyProducts`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_rupeezy.md)

[Inicialización del adaptador](adapter_initialization_rupeezy.md)
