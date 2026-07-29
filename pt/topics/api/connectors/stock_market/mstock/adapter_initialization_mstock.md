# Inicialização do adaptador: m.Stock

O código a seguir inicializa [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new MStockMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	ClientCode = "<Seu código de cliente>",
	Password = "<Sua senha>".To<SecureString>(),
	Otp = "<OTP atual>".To<SecureString>(),
	UseTotp = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os valores de acesso e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_mstock.md).

## Veja também

[Configuração do conector](configuration_mstock.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
