# Configuración del conector: Tradejini

Configure las siguientes propiedades antes de conectarse a Tradejini. La lista se ha verificado con [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `ApiKey` (`SecureString`)
- `Password` (`SecureString`)
- `TwoFactorCode` (`SecureString`)
- `TwoFactorType` (`TradejiniTwoFactorTypes`)
- `Token` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `PortfolioName` (`string`)
- `DefaultProduct` (`TradejiniProducts`)
- `Address` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_tradejini.md)

[Inicialización del adaptador](adapter_initialization_tradejini.md)
