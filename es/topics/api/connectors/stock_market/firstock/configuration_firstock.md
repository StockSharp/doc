# Configuración del conector: Firstock

Configure las siguientes propiedades antes de conectarse a Firstock. La lista se ha verificado con [FirstockMessageAdapter](xref:StockSharp.Firstock.FirstockMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `UserId` (`string`)
- `Password` (`SecureString`)
- `OneTimePassword` (`SecureString`)
- `VendorCode` (`string`)
- `ApiKey` (`SecureString`)
- `Token` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`FirstockProducts`)
- `MarketProtection` (`decimal`)
- `PriceDivisor` (`decimal`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `SymbolsAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_firstock.md)

[Inicialización del adaptador](adapter_initialization_firstock.md)
