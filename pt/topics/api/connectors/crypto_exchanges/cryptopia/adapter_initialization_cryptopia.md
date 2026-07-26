> [!CAUTION]
> **A bolsa Cryptopia encerrou as operações. O conector não funciona mais; a documentação é mantida apenas para referência.**

# Inicialização do adaptador Cryptopia

O código abaixo demonstra como inicializar o [CryptopiaMessageAdapter](xref:StockSharp.Cryptopia.CryptopiaMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new CryptopiaMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

