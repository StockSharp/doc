# Configuración del conector: IIFL

Configure las siguientes propiedades antes de conectarse a IIFL. La lista se ha verificado con [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)

## Configuración avanzada

Estas propiedades controlan la autenticación y la sesión, los puntos de conexión, la transmisión y las consultas periódicas.

- `AuthorizationCode` (`string`)
- `SessionToken` (`SecureString`)
- `PortfolioName` (`string`)
- `RestEndpoint` (`string`)
- `BridgeHost` (`string`)
- `BridgePort` (`int`)
- `TokenValidationEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_iifl.md)

[Inicialización del adaptador](adapter_initialization_iifl.md)
