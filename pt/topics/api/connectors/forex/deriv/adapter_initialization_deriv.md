# Inicialização do adaptador: Deriv

O código a seguir inicializa [DerivMessageAdapter](xref:StockSharp.Deriv.DerivMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DerivMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AppId = "<app-id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelo token e pelo identificador da aplicação emitidos para a conta demo ou real escolhida.

## Veja também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
