# Мульти-таймфрейм стратегия

## Обзор

`MultiTimeframeStrategy` - это стратегия, использующая два временных интервала для принятия торговых решений. Часовые свечи определяют направление тренда через пересечение скользящих средних, а 5-минутные свечи с индикатором [RelativeStrengthIndex](xref:StockSharp.Algo.Indicators.RelativeStrengthIndex) используются для точного входа в позицию по направлению тренда.

## Основные компоненты

Стратегия наследуется от [Strategy](xref:StockSharp.Algo.Strategies.Strategy) и использует параметры для настройки:

```cs
public class MultiTimeframeStrategy : Strategy
{
	private readonly StrategyParam<int> _fastSmaLength;
	private readonly StrategyParam<int> _slowSmaLength;
	private readonly StrategyParam<int> _rsiLength;
	private readonly StrategyParam<decimal> _takeProfit;
	private readonly StrategyParam<decimal> _stopLoss;

	// Направление тренда на старшем таймфрейме
	private Sides? _hourlyTrend;
}
```

## Параметры стратегии

Стратегия позволяет настраивать следующие параметры:

- **FastSmaLength** - период быстрой скользящей средней для часового графика (по умолчанию 10)
- **SlowSmaLength** - период медленной скользящей средней для часового графика (по умолчанию 30)
- **RsiLength** - период RSI для 5-минутного графика (по умолчанию 14)
- **TakeProfit** - размер тейк-профита в процентах (по умолчанию 2)
- **StopLoss** - размер стоп-лосса в процентах (по умолчанию 1)

Параметры доступны для оптимизации с указанными диапазонами значений.

## Инициализация стратегии

В методе [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) создаются индикаторы и настраиваются подписки на свечи двух таймфреймов:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	var fastSma = new SimpleMovingAverage { Length = FastSmaLength };
	var slowSma = new SimpleMovingAverage { Length = SlowSmaLength };
	var rsi = new RelativeStrengthIndex { Length = RsiLength };

	_hourlyTrend = null;

	// Часовые свечи для определения тренда (пересечение SMA)
	SubscribeCandles(TimeSpan.FromHours(1))
		.Bind(fastSma, slowSma, ProcessHourlyCandle)
		.Start();

	// 5-минутные свечи для точного входа (RSI)
	SubscribeCandles(TimeSpan.FromMinutes(5))
		.Bind(rsi, ProcessEntryCandle)
		.Start();

	// Настройка защиты позиции (тейк-профит и стоп-лосс)
	StartProtection(
		new Unit(TakeProfit, UnitTypes.Percent),
		new Unit(StopLoss, UnitTypes.Percent)
	);

	// Настройка визуализации на графике
	var area = CreateChartArea();
	if (area != null)
	{
		DrawIndicator(area, fastSma, System.Drawing.Color.Blue);
		DrawIndicator(area, slowSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## Обработка часовых свечей

Метод `ProcessHourlyCandle` определяет направление тренда на старшем таймфрейме:

```cs
private void ProcessHourlyCandle(ICandleMessage candle, decimal fastValue, decimal slowValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	// Определение тренда по пересечению скользящих средних
	_hourlyTrend = fastValue > slowValue ? Sides.Buy : Sides.Sell;
}
```

## Обработка 5-минутных свечей

Метод `ProcessEntryCandle` реализует вход в позицию по сигналу RSI в направлении тренда:

```cs
private void ProcessEntryCandle(ICandleMessage candle, decimal rsiValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	if (_hourlyTrend == null || !IsFormedAndOnlineAndAllowTrading())
		return;

	// Покупка: тренд вверх и RSI в зоне перепроданности
	if (_hourlyTrend == Sides.Buy && rsiValue < 30 && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Продажа: тренд вниз и RSI в зоне перекупленности
	else if (_hourlyTrend == Sides.Sell && rsiValue > 70 && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
}
```

## Логика торговли

- **Определение тренда**: быстрая SMA выше медленной SMA на часовом графике означает восходящий тренд, ниже - нисходящий
- **Сигнал на покупку**: восходящий тренд на часовом графике и RSI < 30 на 5-минутном графике при отсутствии длинной позиции
- **Сигнал на продажу**: нисходящий тренд на часовом графике и RSI > 70 на 5-минутном графике при отсутствии короткой позиции
- **Защита позиции**: автоматическая установка тейк-профита и стоп-лосса через `StartProtection`

## Особенности

- Стратегия использует два таймфрейма: часовой для тренда и 5-минутный для входа
- Вход в позицию осуществляется только в направлении тренда старшего таймфрейма
- RSI используется как фильтр для поиска оптимальных точек входа (перепроданность/перекупленность)
- Позиция автоматически защищается стоп-лоссом и тейк-профитом
- Стратегия работает только с завершенными свечами
- Индикаторы и сделки визуализируются на графике при наличии графической области
- Поддерживается оптимизация параметров для поиска оптимальных настроек стратегии
