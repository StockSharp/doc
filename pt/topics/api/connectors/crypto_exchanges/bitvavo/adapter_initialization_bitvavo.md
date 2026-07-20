# Inicialização do adaptador Bitvavo

O código abaixo demonstra como inicializar o [BitvavoMessageAdapter](xref:StockSharp.Bitvavo.BitvavoMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BitvavoMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Seu valor>".To<SecureString>(),
	Secret = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
