# 创建指标

有关在 [API](../../../../api.md) 中创建自定义指标的方法，请参阅[自定义指标](../../../../api/indicators/custom_indicator.md)。此类指标与 **Designer** 完全兼容。

要创建指标，请在 **Schemes** 面板中选择 **Indicators** 文件夹，右键单击该文件夹，然后在上下文菜单中选择 **Add**：

![Designer_Source_Code_Indicator_00](../../../../../images/designer_source_code_indicator_00.png)

指标代码如下：

```python
import clr
import random

clr.AddReference("StockSharp.BusinessEntities")
clr.AddReference("StockSharp.Algo")

from StockSharp.Algo.Indicators import BaseIndicator, DecimalIndicatorValue
from indicator_extensions import *

class empty_indicator(BaseIndicator):
	"""
	Sample indicator demonstrating saving and loading parameters.

	Doc https://doc.stocksharp.com/topics/designer/strategies/using_code/python/create_own_indicator.html
	
	Changes input price on +20% or -20%.
	"""
	def __init__(self):
		super(empty_indicator, self).__init__()
		self._change = 20
		self._counter = 0
		self._isFormed = False

	@property
	def Change(self) -> int:
		return self._change

	@Change.setter
	def Change(self, value):
		self._change = value
		self.Reset()

	def CalcIsFormed(self):
		"""Determines if the indicator has received sufficient inputs to be considered formed."""
		return self._isFormed

	def Reset(self):
		"""Resets the indicator's state and internal counters."""
		super(empty_indicator, self).Reset()
		self._isFormed = False
		self._counter = 0

	def OnProcess(self, input):
		"""
		Processes the incoming indicator value and applies a random change.
		
		:param input: The incoming indicator value.
		:return: A new DecimalIndicatorValue after applying changes.
		"""
		# 每第 10 次调用尝试返回空值
		if random.randint(0, 10) == 0:
			return DecimalIndicatorValue(self, input.Time)

		if self._counter == 5:
			# For example, our indicator needs 5 inputs to become formed
			self._isFormed = True
		self._counter += 1

		value = to_decimal(input)

		# 对当前值应用 +/- _change 百分比的随机变化
		value += value * random.randint(-self._change, self._change) / 100.0

		result = DecimalIndicatorValue(self, value, input.Time)
		# 根据随机决策将值标记为最终值
		result.IsFinal = bool(random.getrandbits(1))
		return result

	def Load(self, storage):
		"""
		Loads the indicator parameters from persistent storage.
		
		:param storage: The settings storage to load from.
		"""
		super(empty_indicator, self).Load(storage)
		self.Change = storage.GetValue("Change", self.Change)

	def Save(self, storage):
		"""
		Saves the indicator parameters to persistent storage.
		
		:param storage: The settings storage to save to.
		"""
		super(empty_indicator, self).Save(storage)
		storage.SetValue("Change", self.Change)

	def __str__(self):
		return f"Change: {self.Change}"

	def ToString(self):
		return str(self)
```

该指标接收输入值，并根据 **Change** 参数对该值进行随机偏移。

有关指标方法的说明，请参阅[自定义指标](../../../../api/indicators/custom_indicator.md)。

要将创建的指标添加到策略图，请使用 [Indicator](../../using_visual_designer/elements/common/indicator.md) 模块，并在其中选择所需指标：

![Designer_Source_Code_Indicator_01](../../../../../images/designer_source_code_indicator_01.png)

属性面板会显示此前在指标代码中定义的 **Change** 参数。

> [!WARNING]
> 使用 Python 代码创建的指标不能用于同样使用 Python 代码创建的策略，只能用于通过[模块](../../using_visual_designer.md)创建的策略。
