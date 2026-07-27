# Configuración del conector: Financial Datasets

Configure las siguientes propiedades antes de conectarse a Financial Datasets. La lista se ha verificado con [FinancialDatasetsMessageAdapter](xref:StockSharp.FinancialDatasets.FinancialDatasetsMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Configuración avanzada

Estas propiedades controlan los puntos de conexión, filtros, límites y otras opciones específicas del proveedor.

- `ActiveOnly` (`bool`)
- `FinancialPeriod` (`FinancialDatasetsPeriods`)
- `DataLimit` (`int`)
- `NewsLimit` (`int`)

## Véase también

[Configuración gráfica](graphical_configuration_financial_datasets.md)

[Inicialización del adaptador](adapter_initialization_financial_datasets.md)
