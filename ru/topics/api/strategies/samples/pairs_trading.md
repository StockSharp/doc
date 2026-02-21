# Стратегия парного трейдинга

## Обзор

`PairsTradingStrategy` - это стратегия парного трейдинга, основанная на статистическом арбитраже между двумя связанными инструментами. Она отслеживает спред между ценами двух активов и открывает позиции при значительном отклонении спреда от среднего значения, ожидая возврата к среднему.

## Основные компоненты

Стратегия наследуется от [Strategy](xref:StockSharp.Algo.Strategies.Strategy) и использует параметры для настройки:

```cs
public class PairsTradingStrategy : Strategy
{
	private readonly StrategyParam<int> _spreadLength;
	private readonly StrategyParam<decimal> _entryThreshold;
	private readonly StrategyParam<decimal> _exitThreshold;
	private readonly StrategyParam<DataType> _candleType;

	// Последние цены по каждому инструменту
	private decimal? _lastPrice1;
	private decimal? _lastPrice2;
}
```

## Параметры стратегии

Стратегия позволяет настраивать следующие параметры:

- **SpreadLength** - период расчета среднего и стандартного отклонения спреда (по умолчанию 20)
- **EntryThreshold** - порог Z-Score для входа в позицию (по умолчанию 2.0)
- **ExitThreshold** - порог Z-Score для выхода из позиции (по умолчанию 0.5)
- **CandleType** - тип свечей для работы (по умолчанию 5-минутные)

Параметры доступны для оптимизации с указанными диапазонами значений.

## Инициализация стратегии

В методе [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) создаются индикаторы, настраиваются подписки на свечи для двух инструментов:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Получение двух инструментов для парного трейдинга
	var securities = GetWorkingSecurities().ToArray();
	if (securities.Length < 2)
		throw new InvalidOperationException("Необходимо указать 2 инструмента.");

	var sec1 = securities[0].sec;
	var sec2 = securities[1].sec;

	// Индикаторы для расчета среднего и стандартного отклонения спреда
	var sma = new SimpleMovingAverage { Length = SpreadLength };
	var stdDev = new StandardDeviation { Length = SpreadLength };

	_lastPrice1 = null;
	_lastPrice2 = null;

	// Подписка на свечи первого инструмента
	SubscribeCandles(CandleType, security: sec1)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice1 = c.ClosePrice;
		})
		.Start();

	// Подписка на свечи второго инструмента с обработкой спреда
	SubscribeCandles(CandleType, security: sec2)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice2 = c.ClosePrice;

			if (_lastPrice1 == null)
				return;

			ProcessSpread(_lastPrice1.Value, _lastPrice2.Value, sma, stdDev);
		})
		.Start();
}
```

## Обработка спреда

Метод `ProcessSpread` рассчитывает Z-Score спреда и генерирует торговые сигналы:

```cs
private void ProcessSpread(decimal price1, decimal price2,
	SimpleMovingAverage sma, StandardDeviation stdDev)
{
	// Расчет спреда как разницы цен
	var spread = price1 - price2;

	// Обработка индикаторов
	var smaValue = sma.Process(new DecimalIndicatorValue(sma, spread));
	var devValue = stdDev.Process(new DecimalIndicatorValue(stdDev, spread));

	if (!sma.IsFormed || !stdDev.IsFormed)
		return;

	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var mean = smaValue.ToDecimal();
	var dev = devValue.ToDecimal();

	if (dev == 0)
		return;

	// Расчет Z-Score: отклонение спреда от среднего в единицах стандартного отклонения
	var zScore = (spread - mean) / dev;

	// Спред слишком высокий: продаем первый инструмент, покупаем второй
	if (zScore > EntryThreshold && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Спред слишком низкий: покупаем первый инструмент, продаем второй
	else if (zScore < -EntryThreshold && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Возврат к среднему: закрываем позицию
	else if (Math.Abs(zScore) < ExitThreshold && Position != 0)
	{
		ClosePosition();
	}
}
```

## Логика торговли

- **Сигнал на продажу**: Z-Score спреда превышает порог входа (по умолчанию 2.0), при отсутствии короткой позиции
- **Сигнал на покупку**: Z-Score спреда ниже отрицательного порога входа (по умолчанию -2.0), при отсутствии длинной позиции
- **Закрытие позиции**: абсолютное значение Z-Score опускается ниже порога выхода (по умолчанию 0.5), что сигнализирует о возврате спреда к среднему

## Особенности

- Стратегия работает с двумя инструментами, получаемыми через метод `GetWorkingSecurities()`
- Спред рассчитывается как разница цен закрытия свечей двух инструментов
- Используется Z-Score для нормализации отклонения спреда от среднего значения
- Стратегия реализует классическую идею возврата к среднему (mean reversion)
- Стратегия работает только с завершенными свечами
- Поддерживается оптимизация параметров для поиска оптимальных настроек стратегии
