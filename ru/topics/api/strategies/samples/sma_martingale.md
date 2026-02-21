# Скользящие средние с мартингейлом

## Обзор

`SmaStrategyMartingaleStrategy` - это торговая стратегия, основанная на пересечении двух простых скользящих средних ([SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage)) с элементами мартингейла. Стратегия использует длинную и короткую SMA для определения сигналов входа в рынок и выхода из него, увеличивая объем позиции при каждой новой сделке.

## Основные компоненты

```cs
public class SmaStrategyMartingaleStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;
	private readonly StrategyParam<DataType> _candleType;

	// Переменные для хранения предыдущих значений индикаторов
	private decimal _prevLongValue;
	private decimal _prevShortValue;
	private bool _isFirstValue = true;
}
```

## Параметры стратегии

Стратегия позволяет настраивать следующие параметры:

- **LongSmaLength** - период длинной скользящей средней (по умолчанию 80)
- **ShortSmaLength** - период короткой скользящей средней (по умолчанию 30)
- **CandleType** - тип свечей для работы (по умолчанию 5-минутные)

Все параметры доступны для оптимизации с указанными диапазонами значений.

## Инициализация стратегии

В методе [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) создаются индикаторы SMA, настраивается подписка на свечи и готовится визуализация на графике:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Создание индикаторов
	var longSma = new SimpleMovingAverage { Length = LongSmaLength };
	var shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

	// Добавление индикаторов в коллекцию стратегии для автоматического отслеживания IsFormed
	Indicators.Add(longSma);
	Indicators.Add(shortSma);

	// Создание подписки и привязка индикаторов
	var subscription = SubscribeCandles(CandleType);
	subscription
		.Bind(longSma, shortSma, ProcessCandle)
		.Start();

	// Настройка визуализации на графике
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, longSma, System.Drawing.Color.Blue);
		DrawIndicator(area, shortSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## Обработка свечей

Метод `ProcessCandle` вызывается для каждой завершенной свечи и реализует торговую логику:

```cs
private void ProcessCandle(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// Пропускаем незавершенные свечи
	if (candle.State != CandleStates.Finished)
		return;

	// Проверяем готовность стратегии к торговле
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Для первого значения только сохраняем данные без генерации сигналов
	if (_isFirstValue)
	{
		_prevLongValue = longValue;
		_prevShortValue = shortValue;
		_isFirstValue = false;
		return;
	}

	// Получаем текущее и предыдущее сравнение значений индикаторов
	var isShortLessThenLongCurrent = shortValue < longValue;
	var isShortLessThenLongPrevious = _prevShortValue < _prevLongValue;

	// Сохраняем текущие значения как предыдущие для следующей свечи
	_prevLongValue = longValue;
	_prevShortValue = shortValue;

	// Проверяем пересечение (сигнал)
	if (isShortLessThenLongPrevious == isShortLessThenLongCurrent)
		return;

	// Отменяем активные ордера перед выставлением новых
	CancelActiveOrders();

	// Определяем направление сделки
	var direction = isShortLessThenLongCurrent ? Sides.Sell : Sides.Buy;

	// Рассчитываем размер позиции (увеличиваем позицию с каждой сделкой - подход мартингейла)
	var volume = Volume + Math.Abs(Position);

	// Создаем и регистрируем ордер с соответствующей ценой
	var price = Security.ShrinkPrice(shortValue);
	RegisterOrder(CreateOrder(direction, price, volume));
}
```

## Логика торговли

- **Сигнал на покупку**: короткая SMA пересекает длинную SMA снизу вверх
- **Сигнал на продажу**: короткая SMA пересекает длинную SMA сверху вниз
- При каждой новой сделке объем увеличивается на величину текущей позиции (элемент мартингейла)
- Цена ордера устанавливается по текущему значению короткой SMA, округленному до тика инструмента

## Особенности

- Стратегия автоматически определяет инструменты для работы через метод `GetWorkingSecurities()`
- Стратегия работает только с завершенными свечами
- Стратегия отслеживает пересечения индикаторов путем сравнения текущего и предыдущего соотношения SMA
- Перед выставлением новых ордеров отменяются все активные ордера
- Реализован принцип мартингейла - увеличение объема позиции при каждой новой сделке
- Индикаторы и сделки визуализируются на графике при наличии графической области
- Поддерживается оптимизация параметров для поиска оптимальных настроек стратегии