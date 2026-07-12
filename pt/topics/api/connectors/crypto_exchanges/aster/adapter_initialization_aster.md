# Inicialização do adaptador Aster

O código abaixo demonstra como inicializar o [AsterMessageAdapter](xref:StockSharp.Aster.AsterMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AsterMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<A sua chave de API>".To<SecureString>(),
	Secret = "<O seu segredo de API>".To<SecureString>(),
	Section = AsterSections.Derivatives,
	DerivativesProtocolMode = AsterDerivativesProtocolModes.Legacy,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
