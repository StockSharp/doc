# Inicialização do adaptador: Samco

O código a seguir inicializa [SamcoMessageAdapter](xref:StockSharp.Samco.SamcoMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SamcoMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	Secret = "<Seu segredo de API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os valores de acesso e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_samco.md).

## Veja também

[Configuração do conector](configuration_samco.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
