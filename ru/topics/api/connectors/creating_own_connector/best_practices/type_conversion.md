# Конвертация типов

Компонент конвертации типов играет важную роль в обеспечении совместимости между типами данных, используемыми в StockSharp, и форматами, специфичными для конкретной биржи.

## Основные функции

1. Преобразование типов StockSharp (например, [Sides](xref:StockSharp.Messages.Sides), [OrderTypes](xref:StockSharp.Messages.OrderTypes), [TimeInForce](xref:StockSharp.Messages.TimeInForce)) в строковые представления, используемые биржей.
2. Обратное преобразование данных, полученных от биржи, в типы StockSharp.
3. Конвертация идентификаторов инструментов между форматами StockSharp и биржи.
4. Преобразование временных форматов и таймфреймов.

## Пример реализации

Ниже приведен пример класса с методами расширения для конвертации типов:

```cs
static class Extensions
{
	// Конвертация направления заявки StockSharp в строковое представление биржи
	public static string ToNative(this Sides side)
	{
		return side switch
		{
			Sides.Buy => "buy",
			Sides.Sell => "sell",
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};
	}

	// Конвертация строкового представления направления заявки биржи в тип StockSharp
	public static Sides ToSide(this string side)
		=> side?.ToLowerInvariant() switch
		{
			"buy" or "bid" => Sides.Buy,
			"sell" or "ask" or "offer" => Sides.Sell,
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};

	// Конвертация типа заявки StockSharp в строковое представление биржи
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

	// Конвертация строкового представления типа заявки биржи в тип StockSharp
	public static OrderTypes ToOrderType(this string type)
		=> type?.ToLowerInvariant() switch
		{
			"limit" => OrderTypes.Limit,
			"market" => OrderTypes.Market,
			"stop" or "stop limit" => OrderTypes.Conditional,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, LocalizedStrings.InvalidValue),
		};

	// Другие методы конвертации...

	// Словарь для сопоставления таймфреймов StockSharp и строковых представлений биржи
	public static readonly PairSet<TimeSpan, string> TimeFrames = new()
	{
		{ TimeSpan.FromMinutes(1), "ONE_MINUTE" },
		{ TimeSpan.FromMinutes(5), "FIVE_MINUTE" },
		// Другие таймфреймы...
	};

	// Конвертация таймфрейма StockSharp в строковое представление биржи
	public static string ToNative(this TimeSpan timeFrame)
		=> TimeFrames.TryGetValue(timeFrame) ?? throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, LocalizedStrings.InvalidValue);

	// Конвертация строкового представления таймфрейма биржи в TimeSpan
	public static TimeSpan ToTimeFrame(this string name)
		=> TimeFrames.TryGetKey2(name) ?? throw new ArgumentOutOfRangeException(nameof(name), name, LocalizedStrings.InvalidValue);
}
```

## Рекомендации

- Используйте методы расширения для удобства использования функций конвертации.
- Обрабатывайте все возможные значения перечислений, включая `null` и неизвестные значения.
- Используйте `switch` выражения (C# 8.0+) для более чистого и читаемого кода.
- Добавляйте проверки на недопустимые значения и выбрасывайте исключения с понятными сообщениями об ошибках.
- Рассмотрите возможность использования словарей для сопоставления значений, особенно для сложных или часто изменяющихся соответствий (например, для таймфреймов).

Правильная реализация конвертации типов значительно упрощает работу с данными в других частях коннектора и уменьшает вероятность ошибок, связанных с несоответствием форматов.