# Configuración del conector: Toss Securities

Configure las siguientes propiedades antes de conectarse a Toss Securities. La lista se ha verificado con [TossSecuritiesMessageAdapter](xref:StockSharp.TossSecurities.TossSecuritiesMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AccountSequence` (`long`)
- `PollingInterval` (`TimeSpan`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `PortfolioName` (`string`)
- `AccountPollingInterval` (`TimeSpan`)
- `AdjustedCandles` (`bool`)
- `RestAddress` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_toss_securities.md)

[Inicialización del adaptador](adapter_initialization_toss_securities.md)
