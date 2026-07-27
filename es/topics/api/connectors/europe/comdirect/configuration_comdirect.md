# Configuración del conector: comdirect

Configure las siguientes propiedades antes de conectarse a comdirect. La lista se ha verificado con [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Login` (`string`)
- `Password` (`SecureString`)
- `TanType` (`ComdirectTanTypes`)
- `PollingInterval` (`TimeSpan`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `DefaultCurrency` (`string`)
- `Address` (`Uri`)

## Véase también

[Configuración gráfica](graphical_configuration_comdirect.md)

[Inicialización del adaptador](adapter_initialization_comdirect.md)
