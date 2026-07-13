# Pesquisa de instrumentos

Ao criar seu próprio adaptador para trabalhar com uma bolsa, você precisa implementar o método de busca de instrumentos. Esse método é chamado ao enviar uma mensagem [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) e retorna informações sobre os instrumentos através de mensagens [SecurityMessage](xref:StockSharp.Messages.SecurityMessage).

## Implementando o Método SecurityLookupAsync

O método **SecurityLookupAsync** geralmente executa as seguintes ações:

1. Recupera a lista de tipos de instrumentos suportados a partir da mensagem de entrada.
2. Solicita a lista de instrumentos da bolsa via API.
3. Para cada instrumento recebido, cria uma mensagem [SecurityMessage](xref:StockSharp.Messages.SecurityMessage), preenchendo-a com os dados do instrumento.
4. Verifica se o instrumento corresponde aos critérios de busca.
5. Envia a mensagem [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) criada através do método **SendOutMessageAsync**.
6. Após processar todos os instrumentos, envia uma mensagem sobre a conclusão da busca.

Abaixo está um exemplo da implementação do método SecurityLookupAsync com base no adaptador para a bolsa Coinbase. Ao criar seu próprio adaptador, você precisa adaptar esse código à API da bolsa utilizada.

```cs
public override async ValueTask SecurityLookupAsync(SecurityLookupMessage lookupMsg, CancellationToken cancellationToken)
{
	// Obter a lista de tipos de instrumentos a procurar
	var secTypes = lookupMsg.GetSecurityTypes();

	// Determinar o número máximo de instrumentos a procurar
	var left = lookupMsg.Count ?? long.MaxValue;

	// Iterar pelos tipos de instrumentos suportados pela bolsa
	foreach (var type in new[] { "SPOT", "FUTURE" })
	{
		// Solicitar a lista de instrumentos da bolsa
		var products = await _restClient.GetProducts(type, cancellationToken);

		foreach (var product in products)
		{
			// Criar identificador do instrumento
			var secId = product.ProductId.ToStockSharp();

			// Criar mensagem com informações do instrumento
			var secMsg = new SecurityMessage
			{
				SecurityType = product.ProductType.ToSecurityType(),
				SecurityId = secId,
				Name = product.DisplayName,
				PriceStep = product.QuoteIncrement?.ToDecimal(),
				VolumeStep = product.BaseIncrement?.ToDecimal(),
				MinVolume = product.BaseMinSize?.ToDecimal(),
				MaxVolume = product.BaseMaxSize?.ToDecimal(),
				ExpiryDate = product.FutureProductDetails?.ContractExpiry,
				Multiplier = product.FutureProductDetails?.ContractSize?.ToDecimal(),

				// é necessário preencher o identificador da assinatura
				// para que o código externo entenda para qual assinatura os dados foram recebidos
				OriginalTransactionId = lookupMsg.TransactionId,
			}
			.TryFillUnderlyingId(product.BaseCurrencyId.ToUpperInvariant());

			// Verificar se o instrumento corresponde aos critérios de busca
			if (!secMsg.IsMatch(lookupMsg, secTypes))
				continue;

			// Enviar mensagem com informações do instrumento
			await SendOutMessageAsync(secMsg, cancellationToken);

			// Diminuir o contador de instrumentos restantes
			if (--left <= 0)
				break;
		}

		if (left <= 0)
			break;
	}

	// Enviar mensagem sobre a conclusão da busca
	await SendSubscriptionResultAsync(lookupMsg, cancellationToken);
}
```

Esse método permite recuperar informações sobre os instrumentos disponíveis na bolsa, incluindo suas principais características, como tipo de instrumento, volume mínimo, passo de preço, etc.
