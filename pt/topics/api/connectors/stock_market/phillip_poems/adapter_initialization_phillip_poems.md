# Inicialização do adaptador Phillip POEMS

O código abaixo demonstra como inicializar o [PhillipPoemsMessageAdapter](xref:StockSharp.PhillipPoems.PhillipPoemsMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PhillipPoemsMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Seu valor>",
	ClientSecret = "<Seu valor>".To<SecureString>(),
	ApiKey = "<Seu valor>".To<SecureString>(),
	AccessToken = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
