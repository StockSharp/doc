# Inicialização do adaptador: MetaApi

O código a seguir inicializa [MetaApiMessageAdapter](xref:StockSharp.MetaApi.MetaApiMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MetaApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelo token e pelo identificador da conta implantada no MetaApi.

## Veja também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
