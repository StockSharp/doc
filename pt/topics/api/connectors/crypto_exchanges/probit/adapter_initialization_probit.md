# Inicialização do adaptador ProBit Global

O código abaixo demonstra como inicializar o [ProBitMessageAdapter](xref:StockSharp.ProBit.ProBitMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ProBitMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Seu identificador de cliente OAuth>".To<SecureString>(),
	Secret = "<Seu segredo de cliente OAuth>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Omita `Key` e `Secret` quando precisar apenas de dados públicos de mercado.

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
