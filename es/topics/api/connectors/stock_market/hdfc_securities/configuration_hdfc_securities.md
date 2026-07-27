# Configuración del conector: HDFC Securities

Configure las siguientes propiedades antes de conectarse a HDFC Securities. La lista se ha verificado con [HdfcMessageAdapter](xref:StockSharp.HdfcSecurities.HdfcMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `RequestToken` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Token` (`SecureString`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`HdfcProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_hdfc_securities.md)

[Inicialización del adaptador](adapter_initialization_hdfc_securities.md)
