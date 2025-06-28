# Классическая стратегия Боллинджера

## Обзор

`BollingerStrategyClassicStrategy` - это стратегия, основанная на индикаторе [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands). Она открывает позиции при достижении ценой верхней или нижней границы полос Боллинджера.

## Основные компоненты

Стратегия наследуется от [Strategy](xref:StockSharp.Algo.Strategies.Strategy) и использует параметры для настройки:

```cs
public class BollingerStrategyClassicStrategy : Strategy
{
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	private readonly StrategyParam<DataType> _candleType;

	private BollingerBands _bollingerBands;
}
```

## Параметры стратегии

Стратегия позволяет настраивать следующие параметры:

- **BollingerLength** - период индикатора Bollinger Bands (по умолчанию 20)
- **BollingerDeviation** - множитель стандартного отклонения (по умолчанию 2.0)
- **CandleType** - тип свечей для работы (по умолчанию 5-минутные)

Все параметры доступны для оптимизации с указанными диапазонами значений.

## Инициализация стратегии

В методе [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) создается индикатор Bollinger Bands, настраивается подписка на свечи и готовится визуализация на графике:

```cs
protected override void OnStarted(DateTimeOffset time)
{
	base.OnStarted(time);

	// Создание индикатора
	_bollingerBands = new BollingerBands
	{
		Length = BollingerLength,
		Width = BollingerDeviation
	};

	// Создание подписки и привязка индикатора
	var subscription = SubscribeCandles(CandleType);
	subscription
		.Bind(_bollingerBands, ProcessCandle)
		.Start();

	// Настройка визуализации на графике
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, _bollingerBands, System.Drawing.Color.Purple);
		DrawOwnTrades(area);
	}
}
```

## Обработка свечей

Метод `ProcessCandle` вызывается для каждой завершенной свечи и реализует торговую логику:

```cs
private void ProcessCandle(ICandleMessage candle, decimal middleBand, decimal upperBand, decimal lowerBand)
{
	// Пропускаем незавершенные свечи
	if (candle.State != CandleStates.Finished)
		return;

	// Проверяем готовность стратегии к торговле
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Торговая логика:
	// Продажа, когда цена достигает или превышает верхнюю полосу
	if (candle.ClosePrice >= upperBand && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Покупка, когда цена достигает или опускается ниже нижней полосы
	else if (candle.ClosePrice <= lowerBand && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## Логика торговли

- **Сигнал на продажу**: цена закрытия свечи достигает или превышает верхнюю полосу Боллинджера при отсутствии короткой позиции
- **Сигнал на покупку**: цена закрытия свечи достигает или опускается ниже нижней полосы Боллинджера при отсутствии длинной позиции
- Объем позиции увеличивается на величину текущей позиции при каждой новой сделке

## Особенности

- Стратегия автоматически определяет инструменты для работы через метод `GetWorkingSecurities()`
- Стратегия работает только с завершенными свечами
- Индикатор и сделки визуализируются на графике при наличии графической области
- Поддерживается оптимизация параметров для поиска оптимальных настроек стратегии