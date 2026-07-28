# Configuración del conector: Pendle

Configure las siguientes propiedades antes de conectarse a Pendle. La lista se ha verificado con [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Chain` (`PendleChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Configuración avanzada

Estas propiedades controlan la selección de mercados, los límites, las consultas periódicas y el comportamiento de las transacciones.

- `MarketAddresses` (`string`)
- `MaxMarkets` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `HistoryLimit` (`int`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## Véase también

[Configuración gráfica](graphical_configuration_pendle.md)

[Inicialización del adaptador](adapter_initialization_pendle.md)
