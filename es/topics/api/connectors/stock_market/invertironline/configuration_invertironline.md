# Configuración del conector: InvertirOnline

Configure las siguientes propiedades antes de conectarse a InvertirOnline. La lista se ha verificado con [InvertirOnlineMessageAdapter](xref:StockSharp.InvertirOnline.InvertirOnlineMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `PortfolioName` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Token` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `DefaultCountry` (`InvertirOnlineCountries`)
- `DefaultMarket` (`string`)
- `DefaultInstrumentType` (`string`)
- `DefaultSettlement` (`InvertirOnlineSettlements`)
- `AdjustedHistory` (`bool`)
- `MarketDataPollingInterval` (`TimeSpan`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_invertironline.md)

[Inicialización del adaptador](adapter_initialization_invertironline.md)
