# Configuración del conector: CoinPaprika

Configure las siguientes propiedades antes de conectarse a CoinPaprika. La lista se ha verificado con [CoinPaprikaMessageAdapter](xref:StockSharp.CoinPaprika.CoinPaprikaMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `QuoteCurrency` (`string`)
- `ExchangeId` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_coinpaprika.md)

[Inicialización del adaptador](adapter_initialization_coinpaprika.md)
