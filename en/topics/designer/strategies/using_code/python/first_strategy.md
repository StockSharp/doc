# Python Strategy Example

Creating a strategy from source code will be demonstrated using the SMA strategy example - similar to the SMA strategy example assembled from cubes in the [Creating Algorithm from Cubes](../../using_visual_designer/first_strategy.md) section.

This section will not describe Python language constructs (or [Strategy](../../../../api/strategies.md), which is used as the base for creating strategies), but will mention specific features for working with code in the **Designer**.

> [!TIP]
> Strategies created in the **Designer** are compatible with strategies created in the [API](../../../../api.md) due to using the common base class [Strategy](../../../../api/strategies.md). This makes running such strategies outside the **Designer** significantly easier than running [schemes](../../../live_execution/running_strategies_outside_of_designer.md).

1. Before starting Python development, you need to import .NET dependencies through the clr module, which is provided by IronPython for .NET interaction:

```python
import clr

clr.AddReference("System.Drawing")
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo")
```

Main libraries that need to be connected:

- clr module - imported to enable Python code interaction with .NET Framework
- System.Drawing - library needed for working with colors and graphics
- StockSharp.Messages - contains basic S# messages and data types
- StockSharp.Algo - contains basic S# algorithmic components, including the Strategy base class

> [!NOTE]
> It's important to connect libraries at the beginning of the file, before using any types from these libraries. After connecting the libraries, you can import specific types through regular Python import.

2. Strategy parameters are created using a special approach:

```python
def __init__(self):
	super(sma_strategy, self).__init__()
	self._isShortLessThenLong = None

	# Initialize strategy parameters
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

When using the [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) class, the settings save and restore approach is automatically used.

3. When creating indicators and subscribing to market data, you need to bind them so that incoming data from the subscription can update indicator values:

```python
# Create indicators
longSma = SMA()
longSma.Length = self.Long
shortSma = SMA()
shortSma.Length = self.Short

# Bind candles set and indicators
subscription = self.SubscribeCandles(self.CandleType)
# Bind indicators to the candles and start processing
subscription.Bind(longSma, shortSma, self.OnProcess).Start()
```

4. When working with charts, it's important to consider that when running the strategy [outside the Designer](../../../live_execution/running_strategies_outside_of_designer.md), the chart object might be absent.

```python
# Configure chart if GUI is available
area = self.CreateChartArea()
if area is not None:
	self.DrawCandles(area, subscription)
	self.DrawIndicator(area, shortSma, Color.Coral)
	self.DrawIndicator(area, longSma)
	self.DrawOwnTrades(area)
```

5. Start position protection through [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) if required by the strategy logic:

```python
self.StartProtection(self.TakeValue, self.StopValue)
```

6. The strategy logic itself, implemented in the OnProcess method. The method is called by the subscription created in step 1:

```python
def OnProcess(self, candle, longValue, shortValue):
	"""
	Processes each finished candle, logs information, and executes trading logic on SMA crossing.
	
	:param candle: The processed candle message.
	:param longValue: The current value of the long SMA.
	:param shortValue: The current value of the short SMA.
	"""
	self.LogInfo("New candle {0}: {6} {1};{2};{3};{4}; volume {5}", candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId)

	# If candle is not finished, do nothing
	if candle.State != CandleStates.Finished:
		return

	# Determine if short SMA is less than long SMA
	isShortLessThenLong = shortValue < longValue

	if self._isShortLessThenLong is None:
		self._isShortLessThenLong = isShortLessThenLong
	elif self._isShortLessThenLong != isShortLessThenLong:
		# Crossing happened
		direction = Sides.Sell if isShortLessThenLong else Sides.Buy

		# Calculate volume for opening position or reverting
		volume = self.Volume if self.Position == 0 else Math.Min(Math.Abs(self.Position), self.Volume) * 2

		# Get price step (default to 1 if not set)
		priceStep = self.GetSecurity().PriceStep or 1

		# Calculate order price with offset
		price = candle.ClosePrice + (priceStep if direction == Sides.Buy else -priceStep)

		if direction == Sides.Buy:
			self.BuyLimit(price, volume)
		else:
			self.SellLimit(price, volume)

		# Update state
		self._isShortLessThenLong = isShortLessThenLong
```

7. A mandatory requirement for Python strategies is to override the virtual method [CreateClone](xref:StockSharp.Algo.Strategies.Strategy.CreateClone). This method is needed to create new strategy instances required by [Designer](../../../../designer.md) when optimizing strategy parameters or during testing. Without overriding it, these operations will be impossible.

```python
def CreateClone(self):
	"""
	!! REQUIRED!! Creates a new instance of the strategy.
	"""
	return sma_strategy()
```

> [!IMPORTANT]
> Failure to override the CreateClone method will make it impossible to test the strategy on historical data or perform parameter optimization. When creating a strategy in Python, always add this method.