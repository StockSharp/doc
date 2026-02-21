# Индикаторы в стратегии

В StockSharp класс [Strategy](xref:StockSharp.Algo.Strategies.Strategy) предоставляет специальный механизм для работы с индикаторами, который позволяет контролировать состояние их формирования и определить момент, когда стратегия готова к работе.

## Свойство Indicators

Свойство [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) представляет собой коллекцию индикаторов, используемых в стратегии. Эта коллекция предназначена для автоматического отслеживания состояния формирования (прогрева) индикаторов.

```cs
// Получение доступа к коллекции индикаторов
INotifyList<IIndicator> indicators = strategy.Indicators;
```

## Свойство IsFormed

По умолчанию реализация свойства [Strategy.IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) проверяет, сформированы ли все индикаторы в коллекции [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators):

```cs
// Стандартная реализация в классе Strategy
public virtual bool IsFormed => _indicators.AllFormed;
```

Стратегия считается "прогретой" и готовой к работе, когда все индикаторы в коллекции сформированы (их свойство [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) возвращает `true`).

## Добавление индикаторов в коллекцию

Для правильного определения момента готовности стратегии необходимо добавлять используемые индикаторы в коллекцию [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators):

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Создание индикаторов
	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };

	// Добавление индикаторов в коллекцию
	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);

	// ...
}
```

## Какие индикаторы нужно добавлять

В коллекцию [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) следует добавлять только **независимые индикаторы**. Это важное правило, которое поможет избежать лишнего ожидания и корректно определить момент готовности стратегии.

### Правила добавления индикаторов:

1. **Независимые индикаторы** - добавляйте индикаторы, которые напрямую обрабатывают рыночные данные (свечи, тики и т.д.):

   ```cs
   // Независимые индикаторы
   var sma = new SimpleMovingAverage { Length = 20 };
   var rsi = new RelativeStrengthIndex { Length = 14 };
   
   Indicators.Add(sma);
   Indicators.Add(rsi);
   ```

2. **Цепочки индикаторов** - при использовании цепочки индикаторов (когда выход одного является входом для другого), добавляйте в коллекцию только **первый индикатор в цепочке**:

   ```cs
   // Цепочка индикаторов
   var sma = new SimpleMovingAverage { Length = 20 };
   var stdev = new StandardDeviation { Length = 20 };
   var bollingerBands = new BollingerBands 
   { 
       SmaIndicator = sma,
       DeviationIndicator = stdev
   };
   
   // Добавляем только первый индикатор в цепочке
   Indicators.Add(sma);
   // НЕ добавляем индикаторы, зависящие от других индикаторов
   // Indicators.Add(stdev); - неправильно
   // Indicators.Add(bollingerBands); - неправильно
   ```

3. **Комбинированные индикаторы** - для индикаторов, которые используют несколько независимых индикаторов (например, MACD), добавляйте их все:

   ```cs
   var fastEma = new ExponentialMovingAverage { Length = 12 };
   var slowEma = new ExponentialMovingAverage { Length = 26 };
   var signalEma = new ExponentialMovingAverage { Length = 9 };
   var macd = new MovingAverageConvergenceDivergence
   {
       FastEma = fastEma,
       SlowEma = slowEma,
       SignalEma = signalEma
   };
   
   // Добавляем базовые индикаторы
   Indicators.Add(fastEma);
   Indicators.Add(slowEma);
   ```

## Примеры использования

### Базовый пример с двумя скользящими средними

```cs
public class SmaStrategy : Strategy
{
	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;
	
	// ...
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		_longSma = new SimpleMovingAverage { Length = LongSmaLength };
		_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

		// Добавляем индикаторы в коллекцию для отслеживания их состояния
		Indicators.Add(_longSma);
		Indicators.Add(_shortSma);

		// ...
	}
	
	private void ProcessCandle(ICandleMessage candle)
	{
		// Обрабатываем индикаторы
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);
		
		// Проверяем готовность стратегии перед выполнением торговой логики
		if (!IsFormed)
			return;
			
		// Торговая логика
		// ...
	}
}
```

### Пример с использованием IsFormedAndOnline

Для проверки готовности стратегии к торговле часто используется метод [IsFormedAndOnlineAndAllowTrading](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)), который объединяет проверку формирования индикаторов, онлайн-статуса и разрешения на торговлю:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Обработка индикаторов
	var longValue = _longSma.Process(candle);
	var shortValue = _shortSma.Process(candle);
	
	// Комплексная проверка готовности стратегии
	if (!IsFormedAndOnlineAndAllowTrading())
		return;
		
	// Торговая логика
	// ...
}
```

## Оптимизация использования индикаторов

В более сложных стратегиях важно правильно организовать работу с индикаторами:

```cs
public class ComplexStrategy : Strategy
{
	private SimpleMovingAverage _sma;
	private RelativeStrengthIndex _rsi;
	private BollingerBands _bollinger;
	private StandardDeviation _stdev;
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Создаем индикаторы
		_sma = new SimpleMovingAverage { Length = 20 };
		_rsi = new RelativeStrengthIndex { Length = 14 };
		
		_stdev = new StandardDeviation { Length = 20 };
		_bollinger = new BollingerBands 
		{ 
			SmaIndicator = _sma,
			DeviationIndicator = _stdev 
		};
		
		// Добавляем только независимые индикаторы
		Indicators.Add(_sma);
		Indicators.Add(_rsi);
		// Не добавляем _stdev и _bollinger, так как они зависят от _sma
		
		// ...
	}
	
	// ...
}
```

## Расширенные возможности

Вы можете переопределить свойство [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) в вашей стратегии, если стандартного поведения недостаточно:

```cs
public override bool IsFormed
{
	get
	{
		// Стандартная проверка индикаторов
		if (!base.IsFormed)
			return false;
			
		// Дополнительные условия готовности стратегии
		return _customCondition && _additionalCheck;
	}
}
```

## См. также

- [Список индикаторов](../indicators/list_of_indicators.md)
- [Собственный индикатор](../indicators/custom_indicator.md)
- [Совместимость стратегий с платформами](compatibility.md)
