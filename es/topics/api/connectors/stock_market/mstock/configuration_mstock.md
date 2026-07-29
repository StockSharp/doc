# Configuración del conector: m.Stock

Configure las siguientes propiedades antes de conectarse a m.Stock. La lista se ha verificado con [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `ClientCode` (`string`)

## Configuración avanzada

Estas propiedades controlan la autenticación y la sesión, los puntos de conexión, la transmisión y las consultas periódicas.

- `Password` (`SecureString`)
- `Otp` (`SecureString`)
- `UseTotp` (`bool`)
- `RefreshToken` (`SecureString`)
- `AccessToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_mstock.md)

[Inicialización del adaptador](adapter_initialization_mstock.md)
