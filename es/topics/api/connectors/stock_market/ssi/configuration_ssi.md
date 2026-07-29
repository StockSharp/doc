# Configuración del conector: SSI

Configure las siguientes propiedades antes de conectarse a SSI. La lista se ha verificado con [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)
- `Account` (`string`)

## Configuración avanzada

Estas propiedades controlan la autenticación y la sesión, los puntos de conexión, la transmisión y las consultas periódicas.

- `PrivateKey` (`SecureString`)
- `Otp` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_ssi.md)

[Inicialización del adaptador](adapter_initialization_ssi.md)
