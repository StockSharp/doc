# Adapterinitialisierung FAST

Der folgende Code zeigt, wie der [FastMessageAdapter](xref:StockSharp.Fix.FastMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FastMessageAdapter(Connector.TransactionIdGenerator)
{
	// erforderlichen Dialekt auswählen
	Dialect = typeof(StockSharp.Fix.Dialects.Bovespa.BovespaFastDialect),
};
// alle Dialekteinstellungen aus der Börsenkonfigurationsdatei laden
messageAdapter.DialectSettings.LoadSettingsFromFile(configFile);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
