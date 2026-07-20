# Inicialização do adaptador Morningstar Direct Web Services

O código abaixo demonstra como inicializar o [MorningstarMessageAdapter](xref:StockSharp.Morningstar.MorningstarMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MorningstarMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Seu valor>",
	Password = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
