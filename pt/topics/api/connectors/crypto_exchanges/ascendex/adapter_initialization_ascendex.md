# Inicialização do adaptador: AscendEX

O código a seguir inicializa [AscendExMessageAdapter](xref:StockSharp.AscendEx.AscendExMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new AscendExMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	Secret = "<Seu segredo de API>".To<SecureString>(),
	AccountGroup = 0,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_ascendex.md).

## Veja também

[Configuração do conector](configuration_ascendex.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
