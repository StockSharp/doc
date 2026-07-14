# Compressão de dados de tick e spreads em velas

## Introdução

A API fornece ferramentas poderosas para comprimir dados de tick e spreads (melhores preços de compra/venda) em velas. Essa funcionalidade é especialmente útil para analisar dados históricos ou construir indicadores personalizados.

Os principais métodos de extensão para compressão de dados estão localizados na classe `CandleHelper`. O código-fonte completo dessa classe está [disponível no GitHub](https://github.com/StockSharp/StockSharp/blob/master/Algo/Candles/CandleHelper.cs).

Recomenda-se revisar este arquivo para uma compreensão completa de todos os métodos disponíveis e seus parâmetros.

## Métodos de Compressão

### Compressão de dados de tick em velas

```cs
// Exemplo de uso de ToCandles para ticks
var tickStorage = storageRegistry.GetTickMessageStorage(securityId, Drive, StorageFormat);
var trades = tickStorage.LoadAsync(from, to);
var candles = trades.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

// Este código carrega dados de ticks do armazenamento e os converte em velas.
// mdMsg - mensagem com os parâmetros das velas criadas (tipo, período, etc.).
// candleBuilderProvider — provedor que fornece uma implementação específica do construtor de velas.
```

### Compressão de dados de spread em velas

```cs
// Exemplo de uso de ToCandles para dados de spread
var depthStorage = storageRegistry.GetQuoteMessageStorage(securityId, Drive, StorageFormat);
var depths = depthStorage.LoadAsync(from, to);
var candles = depths.ToCandles(mdMsg, Level1Fields.SpreadMiddle, candleBuilderProvider: candleBuilderProvider);

// Aqui carregamos dados de spread e os convertemos em velas.
// Level1Fields.SpreadMiddle indica o uso do meio do spread para construir velas.
// Também é possível usar Level1Fields.BestBid ou Level1Fields.BestAsk para os melhores preços bid ou ask, respectivamente.
```

## Parâmetros de Compressão

Ao comprimir dados, os seguintes parâmetros podem ser especificados:

- `series`: A série de velas que define o tipo e os parâmetros das velas criadas.
- `type`: O tipo de dado para formar as velas (por exemplo, melhor compra, melhor venda ou meio do spread).
- `candleBuilderProvider`: O provedor para o construtor de velas (parâmetro opcional).

## Exemplo de Uso

```cs
private IEnumerable<CandleMessage> InternalGetCandles(SecurityId securityId, DateTime? from, DateTime? to)
{
	// ... (código de inicialização)

	switch (type)
	{
		case BuildTypes.Ticks:
			return StorageRegistry
					.GetTickMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

		case BuildTypes.OrderLog:
			// ... (código para construir velas a partir do registo de ordens)

		case BuildTypes.Depths:
			return StorageRegistry
					.GetQuoteMessageStorage(securityId, Drive, StorageFormat)
					.LoadAsync(from, to)
					.ToCandles(mdMsg, Convert(extraType), candleBuilderProvider: candleBuilderProvider);

		// ... (outros casos)
	}
}

// Este método demonstra várias formas de construir velas dependendo do tipo dos dados de origem.
// Suporta construção a partir de ticks, registo de ordens, spreads e outras fontes.
```

## Recursos Adicionais

### Construção de velas a partir de diversas fontes

A API permite construir velas não apenas a partir de ticks e spreads, mas também de outras fontes de dados:

```cs
// Exemplo de construção de velas a partir de várias fontes
switch (type)
{
	case BuildTypes.Ticks:
		// ... (código para ticks)

	case BuildTypes.OrderLog:
		// ... (código para registo de ordens)

	case BuildTypes.Depths:
		// ... (código para spreads)

	case BuildTypes.Level1:
		// ... (código para Level1)

	case BuildTypes.SmallerTimeFrame:
		return candleBuilderProvider
				.GetCandleMessageBuildableStorage(StorageRegistry, securityId, mdMsg.GetTimeFrame(), Drive, StorageFormat)
				.LoadAsync(from, to);

	// ... (outros casos)
}

// Este código mostra como construir velas a partir de diferentes fontes de dados: ticks, registo de ordens, spreads, dados Level1 e até velas de períodos menores.
```

## Conclusão

Os métodos de compressão de dados na API fornecem ferramentas flexíveis para trabalhar com dados de mercado. Eles permitem a conversão eficiente de dados de tick e dados de spread em velas de vários tipos e intervalos de tempo, o que é particularmente útil para análise de mercado e desenvolvimento de estratégias de negociação.
