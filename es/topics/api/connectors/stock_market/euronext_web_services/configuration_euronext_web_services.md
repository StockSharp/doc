# Configuración del conector: Euronext Web Services

Configure las siguientes propiedades antes de conectarse a Euronext Web Services. La lista se ha verificado con [EuronextWebServicesMessageAdapter](xref:StockSharp.EuronextWebServices.EuronextWebServicesMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Address` (`Uri`)
- `SessionQuality` (`EuronextSessionQualities`)
- `IntradayDepth` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_euronext_web_services.md)

[Inicialización del adaptador](adapter_initialization_euronext_web_services.md)
