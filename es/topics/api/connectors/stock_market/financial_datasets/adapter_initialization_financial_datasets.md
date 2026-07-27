# Inicialización del adaptador: Financial Datasets

El siguiente código inicializa [FinancialDatasetsMessageAdapter](xref:StockSharp.FinancialDatasets.FinancialDatasetsMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinancialDatasetsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_financial_datasets.md).

## Véase también

[Configuración del conector](configuration_financial_datasets.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
