# Configuración del conector: StockData.org

Configure las siguientes propiedades antes de conectarse a StockData.org. La lista se ha verificado con [StockDataOrgMessageAdapter](xref:StockSharp.StockDataOrg.StockDataOrgMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `ExtendedHours` (`bool`)
- `AdjustedIntraday` (`bool`)
- `NewsLanguage` (`string`)
- `NewsPageSize` (`int`)
- `MaxRequests` (`int`)
- `QuoteTimeZoneId` (`string`)

## Véase también

[Configuración gráfica](graphical_configuration_stockdata_org.md)

[Inicialización del adaptador](adapter_initialization_stockdata_org.md)
