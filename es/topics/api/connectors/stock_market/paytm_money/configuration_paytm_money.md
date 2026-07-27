# Configuración del conector: Paytm Money

Configure las siguientes propiedades antes de conectarse a Paytm Money. La lista se ha verificado con [PaytmMoneyMessageAdapter](xref:StockSharp.PaytmMoney.PaytmMoneyMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `ReadAccessToken` (`SecureString`)
- `PublicAccessToken` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `RequestToken` (`SecureString`)
- `DefaultProduct` (`PaytmMoneyProducts`)
- `PortfolioName` (`string`)
- `Address` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SecurityMasterFile` (`string`)
- `PollingInterval` (`TimeSpan`)

## Véase también

[Configuración gráfica](graphical_configuration_paytm_money.md)

[Inicialización del adaptador](adapter_initialization_paytm_money.md)
