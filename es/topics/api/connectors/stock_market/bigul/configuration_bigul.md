# Configuración del conector: Bigul

Configure las siguientes propiedades antes de conectarse a Bigul. La lista se ha verificado con [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `ClientCode` (`string`)
- `ApiKey` (`SecureString`)
- `ApiSecret` (`SecureString`)
- `OneTimePassword` (`SecureString`)
- `Token` (`SecureString`)
- `Source` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`BigulProducts`)
- `MarketProtection` (`decimal`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_bigul.md)

[Inicialización del adaptador](adapter_initialization_bigul.md)
