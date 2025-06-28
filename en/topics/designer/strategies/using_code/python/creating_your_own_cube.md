# Creating Your Own Cube

Similar to creating a [cube from a scheme](../../using_visual_designer/composite_elements.md), you can create your own cube based on Python code. Such a cube will be more functional than a cube created from a scheme.

To create a cube from code, you need to create it in the **Custom Cubes** folder:

![Designer_Source_Code_Elem_00](../../../../../images/designer_source_code_elem_00.png)

In the example below, the cube inherits from the [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) class and looks as follows:

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

# Custom diagram element class that demonstrates input and output sockets usage
class empty_diagram_element(DiagramExternalElement):
	"""
	Sample diagram element demonstrating input and output sockets usage.

	https://doc.stocksharp.com/topics/designer/strategies/using_code/python/creating_your_own_cube.html
	"""

	def __init__(self):
		super(empty_diagram_element, self).__init__()

		# Example property to show how to add parameters to the diagram element
		# This parameter is named "MinValue" with a default value of 10
		self._minValue = self.AddParam("MinValue", 10)\
							.SetBasic(True)\
							.SetDisplay("Parameters", "Min value", "Min value parameter description", 10)

		# Initialize output event handlers as empty lists
		# Subscribers can assign callable methods to these handlers
		self._output1_handlers = []
		self._output2_handlers = []

	# Output sockets are events marked with the DiagramExternal attribute

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

	# Uncomment the following property if you want the Process method 
	# to be called every time when a new argument is received
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
		# Calculate the result as the sum of the candle's close price and the diff value
		res = candle.ClosePrice + diff

		# Invoke Output1 if diff is greater than or equal to the MinValue parameter,
		# otherwise invoke Output2
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
		# Add custom logic to be executed before the element starts

	def Stop(self):
		"""
		Called when the diagram element stops. Add any post-stop logic here.
		"""
		super(empty_diagram_element, self).Stop()
		# Add custom logic to be executed after the element stops

	def Reset(self):
		"""
		Called when the diagram element resets. Add any reset logic here.
		"""
		super(empty_diagram_element, self).Reset()
		# Add custom logic to reset the internal state of the element
```

In this code, the cube has two incoming sockets and two outgoing sockets. Incoming sockets are defined by applying the @diagram_external decorator to a method:

```python
@diagram_external
def Process(self, candle: ICandleMessage, diff: Unit) -> None:
```

Outgoing sockets are defined by applying the @diagram_external decorator to an event (event subscription operation add_NNN). In the cube example, there are two such events:

```python
# Output sockets are events marked with the DiagramExternal attribute

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

Therefore, there will also be two outgoing sockets.

Additionally, it shows how to create a property for the cube:

```python
self._minValue = self.AddParam("MinValue", 10)\
					.SetBasic(True)\
					.SetDisplay("Parameters", "Min value", "Min value parameter description", 10)
```

When using the [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) class, the settings save and restore approach is automatically used.

The **MinValue** property is marked as basic, and it will be visible in [Basic Properties](../../using_visual_designer/diagram_panel.md) mode.

The commented [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) property determines when the method with incoming sockets is called:

```python
# @property
# def WaitAllInput(self):
#     return False
```

If you uncomment the property, the **Process** method will be called whenever at least one value arrives (in the example case, either a candle or a numeric value).

To add the resulting cube to the scheme, you need to select the created cube in the palette under the **Custom Cubes** section:

![Designer_Source_Code_Elem_01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING] 
> Cubes from Python code cannot be used in strategies created in Python code. They can only be used in strategies created [from cubes](../../using_visual_designer.md).

## See Also

[Creating an Indicator](create_own_indicator.md)