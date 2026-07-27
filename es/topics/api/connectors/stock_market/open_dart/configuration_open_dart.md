# Configuración del conector: Open DART

Configure las siguientes propiedades antes de conectarse a Open DART. La lista se ha verificado con [OpenDartMessageAdapter](xref:StockSharp.OpenDart.OpenDartMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `Address` (`Uri`)
- `DisclosureAddress` (`Uri`)
- `DisclosureType` (`OpenDartDisclosureTypes`)
- `CorporationClass` (`OpenDartCorporationClasses`)
- `FinalReportsOnly` (`bool`)
- `BusinessYear` (`int?`)
- `ReportType` (`OpenDartReportTypes`)
- `FinancialSearchYears` (`int`)
- `MaxPages` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_open_dart.md)

[Inicialización del adaptador](adapter_initialization_open_dart.md)
