> [!CAUTION]
> **A bolsa PrizmBit e sua API não estão mais disponíveis. O conector não funciona; a documentação é mantida apenas para referência.**

# Inicialização do adaptador PrizmBit

O código abaixo demonstra como inicializar o [PrizmBitMessageAdapter](xref:StockSharp.PrizmBit.PrizmBitMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new PrizmBitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

