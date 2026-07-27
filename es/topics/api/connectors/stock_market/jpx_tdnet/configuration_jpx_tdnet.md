# Configuración del conector: JPX TDnet

Configure las siguientes propiedades antes de conectarse a JPX TDnet. La lista se ha verificado con [JpxTdnetMessageAdapter](xref:StockSharp.JpxTdnet.JpxTdnetMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Address` (`Uri`)
- `ViewerAddress` (`Uri`)
- `IndexMode` (`JpxTdnetIndexModes`)
- `DefaultLookupDays` (`int`)
- `MaxDays` (`int`)
- `SecurityLookupDays` (`int`)
- `RequestInterval` (`TimeSpan`)
- `MaxDocumentSizeMb` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_jpx_tdnet.md)

[Inicialización del adaptador](adapter_initialization_jpx_tdnet.md)
