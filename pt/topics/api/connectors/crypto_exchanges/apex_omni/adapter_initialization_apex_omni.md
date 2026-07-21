# Inicialização do adaptador ApeX Omni

O código abaixo demonstra como inicializar o [ApexOmniMessageAdapter](xref:StockSharp.ApexOmni.ApexOmniMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ApexOmniMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Seu valor>".To<SecureString>(),
	Secret = "<Seu valor>".To<SecureString>(),
	Passphrase = "<Seu valor>".To<SecureString>(),
	Seeds = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
