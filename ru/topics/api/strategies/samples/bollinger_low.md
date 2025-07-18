# Стратегия Боллинджера с фокусом на нижней полосе

## Обзор

`BollingerStrategyLowBandStrategy` - это стратегия, основанная на индикаторе [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands). Она открывает короткую позицию при достижении ценой нижней границы полос Боллинджера и закрывает ее при достижении средней линии.

## Основные компоненты

Стратегия наследуется от [Strategy](xref:StockSharp.Algo.Strategies.Strategy) и использует параметры для настройки:

```cs
public class BollingerStrategyLowBandStrategy : Strategy
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
		.BindEx(_bollingerBands, ProcessCandle)
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
private void ProcessCandle(ICandleMessage candle, IIndicatorValue bollingerValue)
{
	// Пропускаем незавершенные свечи
	if (candle.State != CandleStates.Finished)
		return;

	// Проверяем готовность стратегии к торговле
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var typed = (BollingerBandsValue)bollingerValue;

	// Торговая логика:
	// Продажа, когда цена касается нижней полосы (только при отсутствии позиции)
	if (candle.ClosePrice <= typed.LowBand && Position == 0)
	{
		SellMarket(Volume);
	}
	// Покупка для закрытия позиции, когда цена достигает средней линии (только при наличии короткой позиции)
	else if (candle.ClosePrice >= typed.MiddleBand && Position < 0)
	{
		BuyMarket(Math.Abs(Position));
	}
}
```

## Логика торговли

- **Сигнал на продажу**: цена закрытия свечи достигает или опускается ниже нижней полосы Боллинджера при отсутствии открытой позиции
- **Сигнал на покупку** (закрытие короткой позиции): цена закрытия свечи достигает или превышает среднюю линию Боллинджера при наличии короткой позиции
- Объем позиции фиксированный при открытии и равен всей текущей позиции при закрытии

## Особенности

- Стратегия автоматически определяет инструменты для работы через метод `GetWorkingSecurities()`
- Стратегия работает только с завершенными свечами
- Стратегия использует только нижнюю полосу и среднюю линию индикатора Bollinger Bands
- Открываются только короткие позиции
- Индикатор и сделки визуализируются на графике при наличии графической области
- Поддерживается оптимизация параметров для поиска оптимальных настроек стратегии