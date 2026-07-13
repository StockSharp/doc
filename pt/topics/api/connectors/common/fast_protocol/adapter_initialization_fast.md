# Inicialização do adaptador FAST

O código abaixo demonstra como inicializar o [FastMessageAdapter](xref:StockSharp.Fix.FastMessageAdapter) e enviá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FastMessageAdapter(Connector.TransactionIdGenerator)
{
	// escolher o dialeto necessário
	Dialect = typeof(StockSharp.Fix.Dialects.Bovespa.BovespaFastDialect),
};
// carregar todas as configurações do dialeto a partir do arquivo de configuração da bolsa
messageAdapter.DialectSettings.LoadSettingsFromFile(configFile);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
