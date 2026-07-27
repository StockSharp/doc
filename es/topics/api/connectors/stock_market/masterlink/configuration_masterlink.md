# Configuración del conector: MasterLink

Configure las siguientes propiedades antes de conectarse a MasterLink. La lista se ha verificado con [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Login` (`string`)
- `Password` (`SecureString`)
- `CertificatePath` (`string`)
- `CertificatePassword` (`SecureString`)
- `NodePath` (`string`)
- `GatewayDirectory` (`string`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Account` (`string`)
- `RegisterApiAuth` (`bool`)
- `MarketDataMode` (`MasterLinkMarketDataModes`)
- `AdjustedCandles` (`bool`)
- `AccountPollingInterval` (`TimeSpan`)
- `MaxLookupResults` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_masterlink.md)

[Inicialización del adaptador](adapter_initialization_masterlink.md)
