# Configuración del conector: XRPL DEX

Configure las siguientes propiedades antes de conectarse a XRPL DEX. La lista se ha verificado con [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `RpcEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `Account` (`string`)
- `Seed` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan la selección de mercados, la profundidad del libro, el historial, las comisiones, las consultas periódicas y la protección de transacciones.

- `Markets` (`string`)
- `DomainId` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLedgerLimit` (`int`)
- `FeeMultiplier` (`decimal`)
- `LastLedgerOffset` (`int`)
- `MarketOrderProtection` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_xrpl.md)

[Inicialización del adaptador](adapter_initialization_xrpl.md)
