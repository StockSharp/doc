# Inicialização do adaptador Talos

O código abaixo demonstra como inicializar o [TalosMessageAdapter](xref:StockSharp.Talos.TalosMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TalosMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<Seu valor>".To<EndPoint>(),
	SenderCompId = "<Seu valor>",
	TargetCompId = "<Seu valor>",
	Login = "<Seu valor>",
	Password = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
