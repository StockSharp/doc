# Inicialização do adaptador Paxos

O código abaixo demonstra como inicializar o [PaxosMessageAdapter](xref:StockSharp.Paxos.PaxosMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PaxosMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Seu valor>".To<SecureString>(),
	ClientSecret = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
