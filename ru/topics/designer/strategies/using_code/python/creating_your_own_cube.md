# Создание собственного кубика

Аналогично созданию [кубика из схемы](../../using_visual_designer/composite_elements.md) можно создать свой кубик на основе Python кода. Такой кубик будет более функциональным, чем кубик из схемы.

Для создания кубика из кода, необходимо создать его в папке **Собственные кубики**:

![Designer_Source_Code_Elem_00](../../../../../images/designer_source_code_elem_00.png)

В ниже приведенном примере кубик наследуется от класса [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement), и выглядит следующим образом:

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

В данном коде кубик имеет два входящих сокета и два исходящих. Входящие сокеты определяются путем применения декоратора @diagram_external к методу:

```python
@diagram_external
def Process(self, candle: ICandleMessage, diff: Unit) -> None:
```

Исходящие сокеты определяются путем применения декоратора @diagram_external к событию (операции подписки на событие add_NNN). В примере кубика таких событий два:

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

Поэтому исходящих сокета будет также два.

Дополнительно показано как сделать свойство у кубика:

```python
self._minValue = self.AddParam("MinValue", 10)\
                    .SetBasic(True)\
                    .SetDisplay("Parameters", "Min value", "Min value parameter description", 10)
```

При использовании класса [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) автоматически используется подход сохранения и восстановления настроек.

Свойство **MinValue** помечено как basic, и оно будет видно в режиме [Базовые свойства](../../using_visual_designer/diagram_panel.md).

Закомментированное свойство [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) отвечает за время вызова метода со входящими сокетами:

```python
# @property
# def WaitAllInput(self):
#     return False
```

Если раскомментировать свойство, то метод **Process** будет вызыватся всегда, как только придет хотя бы одно значение (в случае примера, это или свеча или числовое значение).

Чтобы добавить получившийся кубик на схему, необходимо в палитре в разделе **Собственные кубики** выбрать созданный кубик:

![Designer_Source_Code_Elem_01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING] 
> Кубики из Python кода невозможно использовать в стратегиях, созданных на Python коде. Их возможно использовать только в стратегиях, созданных [из кубиков](../../using_visual_designer.md).

## См. также

[Создание индикатора](create_own_indicator.md)