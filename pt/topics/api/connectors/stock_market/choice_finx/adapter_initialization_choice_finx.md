# Inicialização do adaptador: Choice FinX

O código a seguir inicializa [ChoiceFinXMessageAdapter](xref:StockSharp.ChoiceFinX.ChoiceFinXMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ChoiceFinXMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_choice_finx.md).

## Veja também

[Configuração do conector](configuration_choice_finx.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
