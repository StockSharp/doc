# Пример стратегии на Python

Создание стратегии из исходного кода будет рассмотрено на примере стратегии SMA — аналогичный пример стратегии SMA, собранной из кубиков в пункте [Создание алгоритма из кубиков](../../using_visual_designer/first_strategy.md).

Данный раздел не будет описывать конструкции Python языка (как и [Strategy](../../../../api/strategies.md), на базе чего создаются стратегии), но будут упомянуты особенности для работы кода в **Дизайнере**.

> [!TIP]
> Стратегии, создающиеся в **Дизайнере**, совместимы со стратегиями, создающимися в [API](../../../../api.md) за счет использования общего базового класса [Strategy](../../../../api/strategies.md). Это делает запуск таких стратегий вне **Дизайнера** значительно проще, чем запуск схем из [визуального редактора](../../../live_execution/running_strategies_outside_of_designer.md).

1. Перед началом разработки на Python необходимо импортировать зависимости .NET через модуль clr, который предоставляется IronPython для взаимодействия с .NET:

```python
import clr

clr.AddReference("System.Drawing")
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo")
```

Основные библиотеки, которые требуется подключить:

- Модуль clr - импортируется для обеспечения взаимодействия Python кода с .NET Framework
- System.Drawing - библиотека, необходимая для работы с цветами и графикой
- StockSharp.Messages - содержит основные сообщения и типы данных S#
- StockSharp.Algo - содержит основные алгоритмические компоненты S#, включая базовый класс Strategy

> [!NOTE]
> Важно подключать библиотеки в начале файла, до использования любых типов из этих библиотек. После подключения библиотек можно импортировать конкретные типы через обычный Python import.

2. Параметры стратегии создаются через специальный подход:

```python
def __init__(self):
	super(sma_strategy, self).__init__()
	self._isShortLessThenLong = None

	# Инициализировать параметры стратегии
	self._candleTypeParam = self.Param("CandleType", DataType.TimeFrame(TimeSpan.FromMinutes(1))) \
		.SetDisplay("Candle type", "Candle type for strategy calculation.", "General")

	self._long = self.Param("Long", 80)
	self._short = self.Param("Short", 30)

	self._takeValue = self.Param("TakeValue", Unit(0, UnitTypes.Absolute))
	self._stopValue = self.Param("StopValue", Unit(2, UnitTypes.Percent))

@property
def CandleType(self):
	return self._candleTypeParam.Value

@CandleType.setter
def CandleType(self, value):
	self._candleTypeParam.Value = value

@property
def Long(self):
	return self._long.Value

@Long.setter
def Long(self, value):
	self._long.Value = value

@property
def Short(self):
	return self._short.Value

@Short.setter
def Short(self, value):
	self._short.Value = value

@property
def TakeValue(self):
	return self._takeValue.Value

@TakeValue.setter
def TakeValue(self, value):
	self._takeValue.Value = value

@property
def StopValue(self):
	return self._stopValue.Value

@StopValue.setter
def StopValue(self, value):
	self._stopValue.Value = value
```

При использовании класса [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) автоматически используется подход сохранения и восстановления настроек.

3. При создании индикаторов и подписки на маркет-данные необходимо связать их, чтобы поступающие данные из подписки могли обновлять значения индикаторов:

```python
# Создать индикаторы
longSma = SMA()
longSma.Length = self.Long
shortSma = SMA()
shortSma.Length = self.Short

# Связать набор свечей и индикаторы
subscription = self.SubscribeCandles(self.CandleType)
# Связать индикаторы со свечами и запустить обработку
subscription.Bind(longSma, shortSma, self.OnProcess).Start()
```

4. При работе с графиком необходимо учитывать, что в случае запуска стратегии [вне Дизайнера](../../../live_execution/running_strategies_outside_of_designer.md) объект графика может быть отсутствовать.

```python
# Настроить график, если доступен GUI
area = self.CreateChartArea()
if area is not None:
	self.DrawCandles(area, subscription)
	self.DrawIndicator(area, shortSma, Color.Coral)
	self.DrawIndicator(area, longSma)
	self.DrawOwnTrades(area)
```

5. Запустить защиту позиций через [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)), если такое требует логика стратегии:

```python
self.StartProtection(self.TakeValue, self.StopValue)
```

6. Сама логика стратегии, реализованная в методе OnProcess. Метод вызывается подпиской, созданной на этапе 1:

```python
def OnProcess(self, candle, longValue, shortValue):
	"""
	Processes each finished candle, logs information, and executes trading logic on SMA crossing.
	
	:param candle: The processed candle message.
	:param longValue: The current value of the long SMA.
	:param shortValue: The current value of the short SMA.
	"""
	self.LogInfo("New candle {0}: {6} {1};{2};{3};{4}; volume {5}", candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId)

		# Если свеча не завершена, ничего не делаем
	if candle.State != CandleStates.Finished:
		return

	# Определить, меньше ли короткая SMA длинной SMA
	isShortLessThenLong = shortValue < longValue

	if self._isShortLessThenLong is None:
		self._isShortLessThenLong = isShortLessThenLong
	elif self._isShortLessThenLong != isShortLessThenLong:
		# Произошло пересечение
		direction = Sides.Sell if isShortLessThenLong else Sides.Buy

		# Рассчитать объём для открытия позиции или разворота
		volume = self.Volume if self.Position == 0 else Math.Min(Math.Abs(self.Position), self.Volume) * 2

		# Получить шаг цены (по умолчанию 1, если не задан)
		priceStep = self.GetSecurity().PriceStep or 1

		# Рассчитать цену заявки со смещением
		price = candle.ClosePrice + (priceStep if direction == Sides.Buy else -priceStep)

		if direction == Sides.Buy:
			self.BuyLimit(price, volume)
		else:
			self.SellLimit(price, volume)

		# Обновить состояние
		self._isShortLessThenLong = isShortLessThenLong
```

7. Обязательным требованием для стратегий на Python является переопределение виртуального метода [CreateClone](xref:StockSharp.Algo.Strategies.Strategy.CreateClone). Этот метод нужен для создания новых экземпляров стратегии, которые требуются для [Designer](../../../../designer.md) при оптимизации параметров стратегии или при тестировании. Без его переопределения эти операции будут невозможны.

```python
def CreateClone(self):
	"""
	!! REQUIRED!! Creates a new instance of the strategy.
	"""
	return sma_strategy()
```

> [!IMPORTANT]
> Отсутствие переопределения метода CreateClone приведет к невозможности тестирования стратегии на исторических данных или проведения оптимизации параметров. При создании стратегии на Python всегда добавляйте этот метод.
