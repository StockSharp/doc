# Conversão de Tipos

O componente de conversão de tipos desempenha um papel importante para garantir a compatibilidade entre os tipos de dados usados no StockSharp e os formatos específicos de uma determinada exchange.

## Funções Principais

1. Conversão dos tipos do StockSharp (por exemplo, [Sides](xref:StockSharp.Messages.Sides), [OrderTypes](xref:StockSharp.Messages.OrderTypes), [TimeInForce](xref:StockSharp.Messages.TimeInForce)) para representações de string usadas pela exchange.
2. Conversão reversa dos dados recebidos da exchange para os tipos do StockSharp.
3. Conversão de identificadores de instrumentos entre os formatos do StockSharp e da exchange.
4. Conversão de formatos de tempo e períodos.

## Exemplo de Implementação

Abaixo está um exemplo de uma classe com métodos de extensão para conversão de tipos:

```cs
static class Extensions
{
	// Converting StockSharp order side to exchange string representation
	public static string ToNative(this Sides side)
	{
		return side switch
		{
			Sides.Buy => "buy",
			Sides.Sell => "sell",
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};
	}

	// Converting exchange order side string representation to StockSharp type
	public static Sides ToSide(this string side)
		=> side?.ToLowerInvariant() switch
		{
			"buy" or "bid" => Sides.Buy,
			"sell" or "ask" or "offer" => Sides.Sell,
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};

	// Converting StockSharp order type to exchange string representation
	public static string ToNative(this OrderTypes? type)
	{
		return type switch
		{
			null => null,
			OrderTypes.Limit => "limit",
			OrderTypes.Market => "market",
			OrderTypes.Conditional => "stop",
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, LocalizedStrings.InvalidValue),
		};
	}

	// Converting exchange order type string representation to StockSharp type
	public static OrderTypes ToOrderType(this string type)
		=> type?.ToLowerInvariant() switch
		{
			"limit" => OrderTypes.Limit,
			"market" => OrderTypes.Market,
			"stop" or "stop limit" => OrderTypes.Conditional,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, LocalizedStrings.InvalidValue),
		};

	// Outros métodos de conversão...

	// Dicionário para mapear períodos do StockSharp para representações de string da bolsa
	public static readonly PairSet<TimeSpan, string> TimeFrames = new()
	{
		{ TimeSpan.FromMinutes(1), "ONE_MINUTE" },
		{ TimeSpan.FromMinutes(5), "FIVE_MINUTE" },
		// Outros períodos...
	};

	// Converter o período do StockSharp para a representação de string da bolsa
	public static string ToNative(this TimeSpan timeFrame)
		=> TimeFrames.TryGetValue(timeFrame) ?? throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, LocalizedStrings.InvalidValue);

	// Converter a representação em string do período da bolsa para TimeSpan
	public static TimeSpan ToTimeFrame(this string name)
		=> TimeFrames.TryGetKey2(name) ?? throw new ArgumentOutOfRangeException(nameof(name), name, LocalizedStrings.InvalidValue);
}
```

## Recomendações

- Use métodos de extensão para um uso conveniente das funções de conversão.
- Trate todos os valores de enumeração possíveis, incluindo `null` e valores desconhecidos.
- Use expressões `switch` (C# 8.0+) para um código mais limpo e legível.
- Adicione verificações para valores inválidos e lance exceções com mensagens de erro claras.
- Considere usar dicionários para mapear valores, especialmente para mapeamentos complexos ou que mudam com frequência (por exemplo, para períodos).

A implementação adequada da conversão de tipos simplifica significativamente o trabalho com dados em outras partes do conector e reduz a probabilidade de erros relacionados a incompatibilidades de formato.
