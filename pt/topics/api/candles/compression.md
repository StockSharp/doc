# Compressão de Dados de Tick e Spreads em Candles

## Introdução

A API fornece ferramentas poderosas para comprimir dados de tick e spreads (melhores preços de compra/venda) em candles. Essa funcionalidade é especialmente útil para analisar dados históricos ou construir indicadores personalizados.

Os principais métodos de extensão para compressão de dados estão localizados na classe `CandleHelper`. O código-fonte completo dessa classe está [disponível no GitHub](https://github.com/StockSharp/StockSharp/blob/master/Algo/Candles/CandleHelper.cs).

Recomenda-se revisar este arquivo para uma compreensão completa de todos os métodos disponíveis e seus parâmetros.

## Métodos de Compressão

### Compressão de Dados de Tick em Candles

```cs
// Example usage of ToCandles for ticks
var tickStorage = storageRegistry.GetTickMessageStorage(securityId, Drive, StorageFormat);
var trades = tickStorage.LoadAsync(from, to);
var candles = trades.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

// This code loads tick data from storage and converts it into candles.
// mdMsg - the message with parameters of the created candles (type, time frame, etc.).
// candleBuilderProvider - the provider that supplies a specific candle builder implementation.
```

### Compressão de Dados de Spread em Candles

```cs
// Example usage of ToCandles for spread data
var depthStorage = storageRegistry.GetQuoteMessageStorage(securityId, Drive, StorageFormat);
var depths = depthStorage.LoadAsync(from, to);
var candles = depths.ToCandles(mdMsg, Level1Fields.SpreadMiddle, candleBuilderProvider: candleBuilderProvider);

// Here we load spread data and convert it into candles.
// Level1Fields.SpreadMiddle indicates using the spread middle price for building candles.
// You can also use Level1Fields.BestBid or Level1Fields.BestAsk for the best bid or ask prices, respectively.
```

## Parâmetros de Compressão

Ao comprimir dados, os seguintes parâmetros podem ser especificados:

- `series`: A série de candles que define o tipo e os parâmetros dos candles criados.
- `type`: O tipo de dado para formar os candles (por exemplo, melhor compra, melhor venda ou meio do spread).
- `candleBuilderProvider`: O provedor para o construtor de candles (parâmetro opcional).

## Exemplo de Uso

```cs
private IEnumerable<CandleMessage> InternalGetCandles(SecurityId securityId, DateTime? from, DateTime? to)
{
	// ... (initialization code)

	switch (type)
	{
		case BuildTypes.Ticks:
			return StorageRegistry
					.GetTickMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

		case BuildTypes.OrderLog:
			// ... (code for building candles from order log)

		case BuildTypes.Depths:
			return StorageRegistry
					.GetQuoteMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, Convert(extraType), candleBuilderProvider: candleBuilderProvider);

		// ... (other cases)
	}
}

// This method demonstrates various ways to build candles depending on the type of source data.
// It supports building from ticks, order log, spreads, and other sources.
```

## Recursos Adicionais

### Construção de Candles a Partir de Diversas Fontes

A API permite construir candles não apenas a partir de ticks e spreads, mas também de outras fontes de dados:

```cs
// Example of building candles from various sources
switch (type)
{
	case BuildTypes.Ticks:
		// ... (code for ticks)

	case BuildTypes.OrderLog:
		// ... (code for order log)

	case BuildTypes.Depths:
		// ... (code for spreads)

	case BuildTypes.Level1:
		// ... (code for Level1)

	case BuildTypes.SmallerTimeFrame:
		return candleBuilderProvider
				.GetCandleMessageBuildableStorage(StorageRegistry, securityId, mdMsg.GetTimeFrame(), Drive, StorageFormat)
				.LoadAsync(from, to);

	// ... (other cases)
}

// This code shows how to build candles from different data sources: ticks, order log, spreads, Level1 data, and even from smaller time frame candles.
```

## Conclusão

Os métodos de compressão de dados na API fornecem ferramentas flexíveis para trabalhar com dados de mercado. Eles permitem a conversão eficiente de dados de tick e dados de spread em candles de vários tipos e intervalos de tempo, o que é particularmente útil para análise de mercado e desenvolvimento de estratégias de negociação.
