# Трендовая стратегия

## Обзор

`OneCandleTrendStrategy` - это простая трендовая стратегия, которая принимает решения на основе анализа одной свечи.

## Основные компоненты

```cs
public class OneCandleTrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleType;
}
```

## Параметры стратегии

Стратегия позволяет настраивать следующие параметры:

- **CandleType** - тип свечей для работы (по умолчанию 5-минутные)

## Инициализация стратегии

В методе [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) создается подписка на свечи и готовится визуализация:

```cs
protected override void OnStarted(DateTimeOffset time)
{
	base.OnStarted(time);

	// Создание подписки
	var subscription = SubscribeCandles(CandleType);
	
	subscription
		.Bind(ProcessCandle)
		.Start();

	// Настройка визуализации на графике
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawOwnTrades(area);
	}
}
```

## Обработка свечей

Метод `ProcessCandle` вызывается для каждой завершенной свечи и реализует торговую логику:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Проверяем, завершена ли свеча
	if (candle.State != CandleStates.Finished)
		return;

	// Проверяем готовность стратегии к торговле
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Трендовая стратегия: покупка на бычьей свече, продажа на медвежьей свече
	if (candle.OpenPrice < candle.ClosePrice && Position <= 0)
	{
		// Бычья свеча - покупка
		BuyMarket(Volume + Math.Abs(Position));
	}
	else if (candle.OpenPrice > candle.ClosePrice && Position >= 0)
	{
		// Медвежья свеча - продажа
		SellMarket(Volume + Math.Abs(Position));
	}
}
```

## Логика торговли

- **Сигнал на покупку**: бычья свеча (цена закрытия выше цены открытия) при отсутствии длинной позиции
- **Сигнал на продажу**: медвежья свеча (цена закрытия ниже цены открытия) при отсутствии короткой позиции
- Объем позиции увеличивается на величину текущей позиции при каждой новой сделке

## Особенности

- Стратегия автоматически определяет инструменты для работы через метод `GetWorkingSecurities()`
- Стратегия работает только с завершенными свечами
- Стратегия использует рыночные ордера для входа в позицию
- Стратегия применяет простую логику определения тренда на основе одной свечи
- Свечи и сделки визуализируются на графике при наличии графической области