# Компрессия тиковых данных и спредов в свечи

## Введение

API предоставляет мощные инструменты для компрессии тиковых данных и спредов (лучших цен из стаканов) в свечи. Эта функциональность особенно полезна при анализе исторических данных или при построении собственных индикаторов.

Основные методы-расширения для работы с компрессией данных находятся в классе `CandleHelper`. Полный исходный код этого класса [доступен на GitHub](https://github.com/StockSharp/StockSharp/blob/master/Algo/Candles/CandleHelper.cs).

Рекомендуется ознакомиться с этим файлом для полного понимания всех доступных методов и их параметров.

## Методы компрессии

### Компрессия тиков в свечи

Для преобразования тиковых данных в свечи используется метод-расширение `ToCandles`:

```cs
// Пример использования ToCandles для тиков
var tickStorage = storageRegistry.GetTickMessageStorage(securityId, Drive, StorageFormat);
var trades = tickStorage.Load(from, to);
var candles = trades.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

// Этот код загружает тиковые данные из хранилища и преобразует их в свечи.
// mdMsg - это сообщение с параметрами создаваемых свечей (тип, таймфрейм и т.д.).
// candleBuilderProvider - это провайдер, который предоставляет конкретную реализацию построителя свечей.
```

### Компрессия стаканов в свечи

Для преобразования данных стакана (лучших цен) в свечи используется тот же метод `ToCandles`, но с дополнительным параметром:

```cs
// Пример использования ToCandles для стаканов
var depthStorage = storageRegistry.GetQuoteMessageStorage(securityId, Drive, StorageFormat);
var depths = depthStorage.Load(from, to);
var candles = depths.ToCandles(mdMsg, Level1Fields.SpreadMiddle, candleBuilderProvider: candleBuilderProvider);

// Здесь мы загружаем данные стакана и преобразуем их в свечи.
// Level1Fields.SpreadMiddle указывает, что мы используем среднюю цену спреда для построения свечей.
// Можно также использовать Level1Fields.BestBid или Level1Fields.BestAsk для использования лучших цен покупки или продажи соответственно.
```

## Параметры компрессии

При компрессии можно указать следующие параметры:

- `series`: Серия свечей, определяющая тип и параметры создаваемых свечей.
- `type`: Тип данных для формирования свечей (например, лучшая цена покупки, продажи или середина спреда).
- `candleBuilderProvider`: Провайдер построителя свечей (необязательный параметр).

## Пример использования

```cs
private IEnumerable<CandleMessage> InternalGetCandles(SecurityId securityId, DateTime? from, DateTime? to)
{
	// ... (код инициализации)

	switch (type)
	{
		case BuildTypes.Ticks:
			return StorageRegistry
					.GetTickMessageStorage(securityId, Drive, StorageFormat)
					.Load(from, to)
					.ToCandles(mdMsg, candleBuilderProvider: candleBuilderProvider);

		case BuildTypes.OrderLog:
			// ... (код для построения свечей из лога заявок)

		case BuildTypes.Depths:
			return StorageRegistry
					.GetQuoteMessageStorage(securityId, Drive, StorageFormat)
					.Load(from, to)
					.ToCandles(mdMsg, Convert(extraType), candleBuilderProvider: candleBuilderProvider);

		// ... (другие случаи)
	}
}

// Этот метод демонстрирует различные способы построения свечей в зависимости от типа исходных данных.
// Он поддерживает построение из тиков, лога заявок, стаканов и других источников.
```

## Дополнительные возможности

### Построение свечей из различных источников

API позволяет строить свечи не только из тиков и стаканов, но и из других источников данных:

```cs
// Пример построения свечей из различных источников
switch (type)
{
	case BuildTypes.Ticks:
		// ... (код для тиков)

	case BuildTypes.OrderLog:
		// ... (код для лога заявок)

	case BuildTypes.Depths:
		// ... (код для стаканов)

	case BuildTypes.Level1:
		// ... (код для Level1)

	case BuildTypes.SmallerTimeFrame:
		return candleBuilderProvider
				.GetCandleMessageBuildableStorage(StorageRegistry, securityId, mdMsg.GetTimeFrame(), Drive, StorageFormat)
				.Load(from, to);

	// ... (другие случаи)
}

// Этот код показывает, как можно строить свечи из различных источников данных:
// тиков, лога заявок, стаканов, данных уровня 1 и даже из свечей меньшего таймфрейма.
```

## Заключение

Методы компрессии данных в API предоставляют гибкие инструменты для работы с рыночными данными. Они позволяют эффективно преобразовывать тиковые данные и данные стакана в свечи различных типов и временных интервалов, что особенно полезно при анализе рынка и разработке торговых стратегий.