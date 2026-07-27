# Inicialização do adaptador: B3 UP2DATA

O código a seguir inicializa [B3Up2DataMessageAdapter](xref:StockSharp.B3Up2Data.B3Up2DataMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new B3Up2DataMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_b3_up2data.md).

## Veja também

[Configuração do conector](configuration_b3_up2data.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
