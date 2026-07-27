# Inicialização do adaptador Bit2Me

O código abaixo demonstra como inicializar [Bit2MeMessageAdapter](xref:StockSharp.Bit2Me.Bit2MeMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new Bit2MeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	Secret = "<Seu segredo de API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Omita `Key` e `Secret` quando precisar apenas de dados públicos. Os endereços REST e WebSocket podem ser alterados por `RestEndpoint` e `WebSocketEndpoint`.

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
