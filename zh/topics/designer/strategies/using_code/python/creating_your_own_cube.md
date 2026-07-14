# 创建自定义模块

与[通过策略图创建模块](../../using_visual_designer/composite_elements.md)类似，也可以使用 Python 代码创建自定义模块。代码模块的功能比策略图模块更灵活。

要通过代码创建模块，请在 **自定义模块** 文件夹中创建该模块：

![Designer 源代码元素 00](../../../../../images/designer_source_code_elem_00.png)

下面的示例模块继承自 [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) 类，代码如下：

```python
import clr

# 添加对所需 StockSharp 程序集的引用
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Diagram.Core")

# 导入 .NET 和 StockSharp 中的必要类型
from System import Action
from StockSharp.Messages import Unit
from StockSharp.Messages import ICandleMessage
from StockSharp.Diagram import DiagramExternalElement

from designer_extensions import diagram_external

# 演示输入和输出插槽用法的自定义图表元素类
class empty_diagram_element(DiagramExternalElement):
	"""
	演示输入和输出插槽用法的示例图表元素。

	https://doc.stocksharp.com/topics/designer/strategies/using_code/python/creating_your_own_cube.html
	"""

	def __init__(self):
		super(empty_diagram_element, self).__init__()

		# 演示如何向图表元素添加参数的示例属性
		# 此参数名为 "MinValue"，默认值为 10
		self._minValue = self.AddParam("MinValue", 10)\
							.SetBasic(True)\
							.SetDisplay("参数", "最小值", "最小值参数说明", 10)

		# 将输出事件处理程序初始化为空列表
		# 订阅者可以为这些处理程序分配可调用方法
		self._output1_handlers = []
		self._output2_handlers = []

	# 输出插槽是带 DiagramExternal 特性的事件

	@diagram_external
	def add_Output1(self, handler: Action[Unit]):
		"""
		订阅 Output1 事件。
		
		:param handler: 触发 Output1 时要调用的可调用方法。
		"""
		self._output1_handlers.append(handler)

	def remove_Output1(self, handler):
		"""
		取消订阅 Output1 事件。
		
		:param handler: 要从 Output1 订阅者中移除的可调用方法。
		"""
		if handler in self._output1_handlers:
			self._output1_handlers.remove(handler)

	@diagram_external
	def add_Output2(self, handler: Action[Unit]):
		"""
		订阅 Output2 事件。
		
		:param handler: 触发 Output2 时要调用的可调用方法。
		"""
		self._output2_handlers.append(handler)

	def remove_Output2(self, handler):
		"""
		取消订阅 Output2 事件。
		
		:param handler: 要从 Output2 订阅者中移除的可调用方法。
		"""
		if handler in self._output2_handlers:
			self._output2_handlers.remove(handler)

	# 如果希望调用 Process 方法，请取消注释以下属性 
	# 使其在每次收到新参数时被调用
	# （无需等待接收所有输入参数）。
	#
	# @property
	# def WaitAllInput(self):
	#     return False
	
	@diagram_external
	def Process(self, candle: ICandleMessage, diff: Unit) -> None:
		"""
		输入插槽是带有 DiagramExternal 属性的方法参数。
		处理一根 K 线和一个 diff 值，然后根据逻辑调用输出事件。
		
		:param candle: 表示 K 线的 CandleMessage 输入。
		:param diff: 表示要处理的差值的 Unit。
		"""
		# 将结果计算为K线收盘价与 diff 值之和
		res = candle.ClosePrice + diff

		# 如果 diff 大于或等于 MinValue 参数，则调用 Output1，
		# 否则调用 Output2
		if diff >= self._minValue.Value:
			for handler in self._output1_handlers:
				handler(res)
		else:
			for handler in self._output2_handlers:
				handler(res)

	def Start(self):
		"""
		在图表元素启动时调用。可在此添加启动前逻辑。
		"""
		super(empty_diagram_element, self).Start()
		# 添加在元素启动前执行的自定义逻辑

	def Stop(self):
		"""
		在图表元素停止时调用。可在此添加停止后逻辑。
		"""
		super(empty_diagram_element, self).Stop()
		# 添加在元素停止后执行的自定义逻辑

	def Reset(self):
		"""
		在图表元素重置时调用。可在此添加重置逻辑。
		"""
		super(empty_diagram_element, self).Reset()
		# 添加用于重置元素内部状态的自定义逻辑
```

在这段代码中，模块包含两个输入端口和两个输出端口。对方法应用 @diagram_external 装饰器，即可定义输入端口：

```python
@diagram_external
def Process(self, candle: ICandleMessage, diff: Unit) -> None:
```

对事件的订阅操作 add_NNN 应用 @diagram_external 装饰器，即可定义输出端口。示例模块包含以下两个事件：

```python
# 输出插槽是带 DiagramExternal 特性的事件

@diagram_external
def add_Output1(self, handler: Action[Unit]):
	"""
	订阅 Output1 事件。
	
	:param handler: 触发 Output1 时要调用的可调用方法。
	"""
	self._output1_handlers.append(handler)

def remove_Output1(self, handler):
	"""
	取消订阅 Output1 事件。
	
	:param handler: 要从 Output1 订阅者中移除的可调用方法。
	"""
	if handler in self._output1_handlers:
		self._output1_handlers.remove(handler)

@diagram_external
def add_Output2(self, handler: Action[Unit]):
	"""
	订阅 Output2 事件。
	
	:param handler: 触发 Output2 时要调用的可调用方法。
	"""
	self._output2_handlers.append(handler)

def remove_Output2(self, handler):
	"""
	取消订阅 Output2 事件。
	
	:param handler: 要从 Output2 订阅者中移除的可调用方法。
	"""
	if handler in self._output2_handlers:
		self._output2_handlers.remove(handler)
```

因此，模块也会包含两个输出端口。

示例还演示了如何为模块创建属性：

```python
self._minValue = self.AddParam("MinValue", 10)\
					.SetBasic(True)\
					.SetDisplay("参数", "最小值", "最小值参数说明", 10)
```

使用 [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) 类后，系统会自动处理设置的保存和恢复。

**MinValue** 属性被标记为基本属性，因此会显示在[基本属性](../../using_visual_designer/diagram_panel.md)模式中。

已注释的 [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) 属性控制何时根据输入端口的数据调用方法：

```python
# @property
# def WaitAllInput(self):
#     return False
```

取消注释后，只要至少有一个值到达，**Process** 方法就会被调用。在本例中，该值可以是K线或数值。

要将创建的模块添加到策略图，请在组件面板的 **自定义模块** 部分选择该模块：

![Designer 源代码元素 01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING]
> 使用 Python 代码创建的模块不能用于同样使用 Python 代码创建的策略，只能用于通过[模块](../../using_visual_designer.md)创建的策略。

## 另请参阅

[创建指标](create_own_indicator.md)
