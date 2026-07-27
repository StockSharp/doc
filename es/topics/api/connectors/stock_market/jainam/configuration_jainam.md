# Configuración del conector: Jainam

Configure las siguientes propiedades antes de conectarse a Jainam. La lista se ha verificado con [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `UserId` (`string`)
- `AppCode` (`string`)
- `ApiSecret` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`JainamProducts`)
- `ReconnectAttempts` (`int`)
- `PollingInterval` (`TimeSpan`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`string`)
- `WebSocketAddress` (`string`)

## Véase también

[Configuración gráfica](graphical_configuration_jainam.md)

[Inicialización del adaptador](adapter_initialization_jainam.md)
