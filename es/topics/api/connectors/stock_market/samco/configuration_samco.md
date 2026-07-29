# Configuración del conector: Samco

Configure las siguientes propiedades antes de conectarse a Samco. La lista se ha verificado con [SamcoMessageAdapter](xref:StockSharp.Samco.SamcoMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan la autenticación y la sesión, los puntos de conexión, la transmisión y las consultas periódicas.

- `Secret` (`SecureString`)
- `SessionToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `InstrumentEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_samco.md)

[Inicialización del adaptador](adapter_initialization_samco.md)
