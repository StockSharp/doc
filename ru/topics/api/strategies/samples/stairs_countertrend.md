# Контртрендовая стратегия лестницей

## Обзор

`StairsCountertrendStrategy` - это контртрендовая торговая стратегия, которая открывает позиции против установившегося тренда определенной длины.

## Основные компоненты

```cs
public class StairsCountertrendStrategy : Strategy
{
	private readonly StrategyParam<int> _length;
	private readonly StrategyParam<DataType> _candleType;
	
	private int _bullLength;
	private int _bearLength;
}
```

## Параметры стратегии

Стратегия позволяет настраивать следующие параметры:

- **Length** - количество последовательных свечей одного направления для идентификации тренда (по умолчанию 3)
- **CandleType** - тип свечей для работы (по умолчанию 5-минутные)

Параметр Length доступен для оптимизации в диапазоне от 2 до 10 с шагом 1.

## Инициализация стратегии

В методе [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) обнуляются счетчики, создается подписка на свечи и готовится визуализация:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Сброс счетчиков
	_bullLength = 0;
	_bearLength = 0;

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

	// Обновляем счетчики на основе направления свечи
	if (candle.OpenPrice < candle.ClosePrice)
	{
		// Бычья свеча
		_bullLength++;
		_bearLength = 0;
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		// Медвежья свеча
		_bullLength = 0;
		_bearLength++;
	}

	// Контртрендовая стратегия: 
	// Продажа после Length последовательных бычьих свечей
	if (_bullLength >= Length && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Покупка после Length последовательных медвежьих свечей
	else if (_bearLength >= Length && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## Логика торговли

- **Сигнал на продажу**: `Length` последовательных бычьих свечей (цена закрытия выше цены открытия) при отсутствии короткой позиции
- **Сигнал на покупку**: `Length` последовательных медвежьих свечей (цена закрытия ниже цены открытия) при отсутствии длинной позиции
- Объем позиции увеличивается на величину текущей позиции при каждой новой сделке

## Особенности

- Стратегия автоматически определяет инструменты для работы через метод `GetWorkingSecurities()`
- Стратегия работает только с завершенными свечами
- Стратегия использует рыночные ордера для входа в позицию
- Стратегия применяет контртрендовый подход, открывая позиции против установившегося тренда
- Счетчики свечей сбрасываются при появлении свечи противоположного направления
- Свечи и сделки визуализируются на графике при наличии графической области
- Поддерживается оптимизация длины последовательности для поиска оптимальных настроек стратегии