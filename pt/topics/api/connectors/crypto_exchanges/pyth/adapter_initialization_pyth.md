# Inicialização do adaptador Pyth

O código abaixo demonstra como inicializar o [PythMessageAdapter](xref:StockSharp.Pyth.PythMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PythMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
