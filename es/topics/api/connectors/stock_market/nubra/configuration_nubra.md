# Configuración del conector: Nubra

Configure las siguientes propiedades antes de conectarse a Nubra. La lista se ha verificado con [NubraMessageAdapter](xref:StockSharp.Nubra.NubraMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `DeviceId` (`string`)
- `IsDemo` (`bool`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Phone` (`string`)
- `Mpin` (`SecureString`)
- `TotpSecret` (`SecureString`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`NubraProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `UatRestAddress` (`Uri`)
- `MarketDataAddress` (`Uri`)
- `UatMarketDataAddress` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_nubra.md)

[Inicialización del adaptador](adapter_initialization_nubra.md)
