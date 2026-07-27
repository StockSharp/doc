# Configuración del conector: Wisdom Capital

Configure las siguientes propiedades antes de conectarse a Wisdom Capital. La lista se ha verificado con [WisdomCapitalMessageAdapter](xref:StockSharp.WisdomCapital.WisdomCapitalMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `MarketDataKey` (`SecureString`)
- `MarketDataSecret` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Token` (`SecureString`)
- `UserId` (`string`)
- `MarketDataToken` (`SecureString`)
- `MarketDataUserId` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`WisdomCapitalProducts`)
- `Source` (`string`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `EngineIoVersion` (`int`)
- `RestAddress` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_wisdom_capital.md)

[Inicialización del adaptador](adapter_initialization_wisdom_capital.md)
