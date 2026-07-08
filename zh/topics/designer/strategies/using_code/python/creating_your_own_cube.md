# 创建自定义模块

与[通过策略图创建模块](../../using_visual_designer/composite_elements.md)类似，也可以使用 Python 代码创建自定义模块。代码模块的功能比策略图模块更灵活。

要通过代码创建模块，请在 **Custom Cubes** 文件夹中创建该模块：

![Designer_Source_Code_Elem_00](../../../../../images/designer_source_code_elem_00.png)

下面的示例模块继承自 [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) 类，代码如下：

```python
import clr

# Add references to the required StockSharp assemblies
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Diagram.Core")

# Import necessary types from .NET and StockSharp
from System import Action
from StockSharp.Messages import Unit
from StockSharp.Messages import ICandleMessage
from StockSharp.Diagram import DiagramExternalElement

from designer_extensions import diagram_external

# 演示输入和输出插槽用法的自定义图表元素类
class empty_diagram_element(DiagramExternalElement):
	"""
	Sample diagram element demonstrating input and output sockets usage.

	https://doc.stocksharp.com/topics/designer/strategies/using_code/python/creating_your_own_cube.html
	"""

	def __init__(self):
		super(empty_diagram_element, self).__init__()

		# 演示如何向图表元素添加参数的示例属性
		# 此参数名为 "MinValue"，默认值为 10
		self._minValue = self.AddParam("MinValue", 10)\
							.SetBasic(True)\
							.SetDisplay("Parameters", "Min value", "Min value parameter description", 10)

		# 将输出事件处理程序初始化为空列表
		# 订阅者可以为这些处理程序分配可调用方法
		self._output1_handlers = []
		self._output2_handlers = []

	# 输出插槽是带 DiagramExternal 特性的事件

	@diagram_external
	def add_Output1(self, handler: Action[Unit]):
		"""
		Subscribe to the Output1 event.
		
		:param handler: A callable method to be invoked when Output1 is triggered.
		"""
		self._output1_handlers.append(handler)

	def remove_Output1(self, handler):
		"""
		Unsubscribe from the Output1 event.
		
		:param handler: The callable method to be removed from the Output1 subscribers.
		"""
		if handler in self._output1_handlers:
			self._output1_handlers.remove(handler)

	@diagram_external
	def add_Output2(self, handler: Action[Unit]):
		"""
		Subscribe to the Output2 event.
		
		:param handler: A callable method to be invoked when Output2 is triggered.
		"""
		self._output2_handlers.append(handler)

	def remove_Output2(self, handler):
		"""
		Unsubscribe from the Output2 event.
		
		:param handler: The callable method to be removed from the Output2 subscribers.
		"""
		if handler in self._output2_handlers:
			self._output2_handlers.remove(handler)

	# 如果希望调用 Process 方法，请取消注释以下属性 
	# 使其在每次收到新参数时被调用
	# (no need to wait for all input args to be received).
	#
	# @property
	# def WaitAllInput(self):
	#     return False
	
	@diagram_external
	def Process(self, candle: ICandleMessage, diff: Unit) -> None:
		"""
		Input sockets are method parameters marked with the DiagramExternal attribute.
		Processes a candle and a diff value, then invokes output events based on the logic.
		
		:param candle: CandleMessage input representing a candlestick.
		:param diff: Unit representing the difference value to be processed.
		"""
		# 将结果计算为蜡烛收盘价与 diff 值之和
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
		Called when the diagram element starts. Add any pre-start logic here.
		"""
		super(empty_diagram_element, self).Start()
		# 添加在元素启动前执行的自定义逻辑

	def Stop(self):
		"""
		Called when the diagram element stops. Add any post-stop logic here.
		"""
		super(empty_diagram_element, self).Stop()
		# 添加在元素停止后执行的自定义逻辑

	def Reset(self):
		"""
		Called when the diagram element resets. Add any reset logic here.
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
	Subscribe to the Output1 event.
	
	:param handler: A callable method to be invoked when Output1 is triggered.
	"""
	self._output1_handlers.append(handler)

def remove_Output1(self, handler):
	"""
	Unsubscribe from the Output1 event.
	
	:param handler: The callable method to be removed from the Output1 subscribers.
	"""
	if handler in self._output1_handlers:
		self._output1_handlers.remove(handler)

@diagram_external
def add_Output2(self, handler: Action[Unit]):
	"""
	Subscribe to the Output2 event.
	
	:param handler: A callable method to be invoked when Output2 is triggered.
	"""
	self._output2_handlers.append(handler)

def remove_Output2(self, handler):
	"""
	Unsubscribe from the Output2 event.
	
	:param handler: The callable method to be removed from the Output2 subscribers.
	"""
	if handler in self._output2_handlers:
		self._output2_handlers.remove(handler)
```

因此，模块也会包含两个输出端口。

示例还演示了如何为模块创建属性：

```python
self._minValue = self.AddParam("MinValue", 10)\
					.SetBasic(True)\
					.SetDisplay("Parameters", "Min value", "Min value parameter description", 10)
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

要将创建的模块添加到策略图，请在组件面板的 **Custom Cubes** 部分选择该模块：

![Designer_Source_Code_Elem_01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING]
> 使用 Python 代码创建的模块不能用于同样使用 Python 代码创建的策略，只能用于通过[模块](../../using_visual_designer.md)创建的策略。

## 另请参阅

[创建指标](create_own_indicator.md)
