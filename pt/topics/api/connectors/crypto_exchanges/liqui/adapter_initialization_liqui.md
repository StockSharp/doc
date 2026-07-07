> [!WARNING]
> Esta bolsa foi encerrada permanentemente (janeiro de 2019 — encerrada). Este conector já não está operacional. A documentação é preservada para referência histórica.

# Inicialização do adaptador Liqui

O código abaixo demonstra como inicializar o [LiquiMessageAdapter](xref:StockSharp.Liqui.LiquiMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new LiquiMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
