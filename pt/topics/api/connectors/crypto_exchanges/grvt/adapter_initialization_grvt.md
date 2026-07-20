# Inicialização do adaptador GRVT

O código abaixo demonstra como inicializar o [GrvtMessageAdapter](xref:StockSharp.Grvt.GrvtMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GrvtMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Seu valor>".To<SecureString>(),
	Secret = "<Seu valor>".To<SecureString>(),
	SubAccountId = "<Seu valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
