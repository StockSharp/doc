# Python 策略示例

下面以 SMA 策略为例，演示如何通过源代码创建策略。该示例与[通过模块创建算法](../../using_visual_designer/first_strategy.md)中使用模块组装的 SMA 策略相似。

本节不介绍 Python 语言结构，也不详细说明作为策略基类的 [Strategy](../../../../api/strategies.md)，而是重点介绍在 **Designer** 中使用代码时需要注意的特性。

> [!TIP]
> 在 **Designer** 中创建的策略与通过 [API](../../../../api.md) 创建的策略兼容，因为二者使用相同的基类 [Strategy](../../../../api/strategies.md)。因此，与运行[策略图](../../../live_execution/running_strategies_outside_of_designer.md)相比，在 **Designer** 外部运行此类策略要简单得多。

1. 开始 Python 开发前，需要通过 clr 模块导入 .NET 依赖项。IronPython 提供该模块，用于与 .NET 交互：

```python
import clr

clr.AddReference("System.Drawing")
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo")
```

需要加载的主要库：

- clr 模块 - 使 Python 代码能够与 .NET Framework 交互。
- System.Drawing - 用于处理颜色和图形。
- StockSharp.Messages - 包含基本的 S# 消息和数据类型。
- StockSharp.Algo - 包含基本的 S# 算法组件，包括 Strategy 基类。

> [!NOTE]
> 必须在文件开头加载这些库，并且要早于对库中任何类型的使用。加载库后，可以通过常规 Python import 导入具体类型。

2. 使用专门的方式创建策略参数：

```python
def __init__(self):
	super(sma_strategy, self).__init__()
	self._isShortLessThenLong = None

	# 初始化策略参数
	self._candleTypeParam = self.Param("CandleType", DataType.TimeFrame(TimeSpan.FromMinutes(1))) \
		.SetDisplay("K线类型", "用于策略计算的K线类型。", "常规")

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

使用 [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) 类后，系统会自动处理设置的保存和恢复。

3. 创建指标并订阅市场数据时，需要将二者绑定，使订阅收到的数据能够更新指标值：

```python
# 创建指标
longSma = SMA()
longSma.Length = self.Long
shortSma = SMA()
shortSma.Length = self.Short

# 绑定K线集和指标
subscription = self.SubscribeCandles(self.CandleType)
# 将指标绑定到K线并开始处理
subscription.Bind(longSma, shortSma, self.OnProcess).Start()
```

4. 使用图表时需要注意：在 [Designer](../../../live_execution/running_strategies_outside_of_designer.md) 外部运行策略时，图表对象可能不存在。

```python
# 如果 GUI 可用，则配置图表
area = self.CreateChartArea()
if area is not None:
	self.DrawCandles(area, subscription)
	self.DrawIndicator(area, shortSma, Color.Coral)
	self.DrawIndicator(area, longSma)
	self.DrawOwnTrades(area)
```

5. 如果策略逻辑需要，请通过 [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) 启动持仓保护：

```python
self.StartProtection(self.TakeValue, self.StopValue)
```

6. 策略逻辑本身在 OnProcess 方法中实现。该方法由步骤 1 中创建的订阅调用：

```python
def OnProcess(self, candle, longValue, shortValue):
	"""
	Processes each finished candle, logs information, and executes trading logic on SMA crossing.
	
	:param candle: The processed candle message.
		:param longValue: The current value of the long SMA.
		:param shortValue: The current value of the short SMA.
	"""
	self.LogInfo("新K线 {0}: {6} {1};{2};{3};{4}; 成交量 {5}", candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId)

	# 如果 K线尚未完成，则不执行任何操作
	if candle.State != CandleStates.Finished:
		return

	# 判断短周期 SMA 是否小于长周期 SMA
	isShortLessThenLong = shortValue < longValue

	if self._isShortLessThenLong is None:
		self._isShortLessThenLong = isShortLessThenLong
	elif self._isShortLessThenLong != isShortLessThenLong:
		# 发生交叉
		direction = Sides.Sell if isShortLessThenLong else Sides.Buy

		# 计算开仓或反转持仓的数量
		volume = self.Volume if self.Position == 0 else Math.Min(Math.Abs(self.Position), self.Volume) * 2

		# 获取价格步长（未设置时默认为 1）
		priceStep = self.GetSecurity().PriceStep or 1

		# 使用偏移计算订单价格
		price = candle.ClosePrice + (priceStep if direction == Sides.Buy else -priceStep)

		if direction == Sides.Buy:
			self.BuyLimit(price, volume)
		else:
			self.SellLimit(price, volume)

		# 更新状态
		self._isShortLessThenLong = isShortLessThenLong
```

7. Python 策略必须重写虚方法 [CreateClone](xref:StockSharp.Algo.Strategies.Strategy.CreateClone)。优化策略参数或执行测试时，[Designer](../../../../designer.md) 需要通过此方法创建新的策略实例。如果未重写该方法，这些操作将无法执行。

```python
def CreateClone(self):
	"""
	!! REQUIRED!! Creates a new instance of the strategy.
	"""
	return sma_strategy()
```

> [!IMPORTANT]
> 如果未重写 CreateClone 方法，将无法使用历史数据测试策略，也无法执行参数优化。使用 Python 创建策略时，必须始终添加此方法。
