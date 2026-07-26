> [!CAUTION]
> **A bolsa ZB e sua API não estão mais disponíveis. O conector não funciona; a documentação é mantida apenas para referência.**

# Inicialização do adaptador ZB

O código abaixo demonstra como inicializar o [ZBMessageAdapter](xref:StockSharp.ZB.ZBMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new ZBMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
