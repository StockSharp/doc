# Inicialización del adaptador FAST

El código siguiente muestra cómo inicializar [FastMessageAdapter](xref:StockSharp.Fix.FastMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FastMessageAdapter(Connector.TransactionIdGenerator)
{
	// choose required dialect
	Dialect = typeof(StockSharp.Fix.Dialects.Bovespa.BovespaFastDialect),
};
// load all dialect settings from an exchange config file
messageAdapter.DialectSettings.LoadSettingsFromFile(configFile);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
