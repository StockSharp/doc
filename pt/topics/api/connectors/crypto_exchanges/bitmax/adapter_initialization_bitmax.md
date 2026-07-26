> [!CAUTION]
> **A bolsa BitMax, posteriormente renomeada para AscendEX, encerrou as operações em 1º de julho de 2026. O conector não funciona mais; a documentação é mantida apenas para referência.**

# Inicialização do adaptador BitMax

O código abaixo demonstra como inicializar o [BitMaxMessageAdapter](xref:StockSharp.BitMax.BitMaxMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitMaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
