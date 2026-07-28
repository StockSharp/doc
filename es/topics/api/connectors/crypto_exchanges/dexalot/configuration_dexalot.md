# Configuración del conector: Dexalot

Configure las siguientes propiedades antes de conectarse a Dexalot. La lista se ha verificado con [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `RpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan las direcciones de contratos, los filtros de pares, la profundidad del libro, las consultas periódicas y el comportamiento de las transacciones.

- `TradePairsAddress` (`string`)
- `PortfolioAddress` (`string`)
- `Pairs` (`string`)
- `OrderBookDepth` (`int`)
- `PrivatePollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `SelfTradePrevention` (`DexalotSelfTradePrevention`)

## Véase también

[Configuración gráfica](graphical_configuration_dexalot.md)

[Inicialización del adaptador](adapter_initialization_dexalot.md)
