# Inicialização do adaptador Questrade

O código seguinte demonstra como inicializar o [QuestradeMessageAdapter](xref:StockSharp.Questrade.QuestradeMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QuestradeMessageAdapter(Connector.TransactionIdGenerator)
{
	RefreshToken = "<Token de atualização>".ToSecureString(),
	Account = "<Conta>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
