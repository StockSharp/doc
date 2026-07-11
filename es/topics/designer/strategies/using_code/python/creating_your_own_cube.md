# Crear su propio cubo

De forma similar a la creación de un [cubo a partir de un esquema](../../using_visual_designer/composite_elements.md), puede crear su propio cubo basado en código Python. Este cubo será más funcional que un cubo creado a partir de un esquema.

Para crear un cubo desde código, debe crearlo en la carpeta **Bloques personalizados**:

![Designer elemento de código fuente 00](../../../../../images/designer_source_code_elem_00.png)

En el ejemplo siguiente, el cubo hereda de la clase [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) y tiene este aspecto:

```python
import clr

# Añadir referencias a los ensamblados StockSharp requeridos
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Diagram.Core")

# Importar tipos necesarios de .NET y StockSharp
from System import Action
from StockSharp.Messages import Unit
from StockSharp.Messages import ICandleMessage
from StockSharp.Diagram import DiagramExternalElement

from designer_extensions import diagram_external

# Clase de elemento de diagrama personalizado que demuestra el uso de sockets de entrada y salida
class empty_diagram_element(DiagramExternalElement):
	"""
	Elemento de diagrama de ejemplo que demuestra el uso de sockets de entrada y salida.

	https://doc.stocksharp.com/topics/designer/strategies/using_code/python/creating_your_own_cube.html
	"""

	def __init__(self):
		super(empty_diagram_element, self).__init__()

		# Propiedad de ejemplo para mostrar cómo añadir parámetros al elemento de diagrama
		# Este parámetro se llama "MinValue" y tiene un valor predeterminado de 10
		self._minValue = self.AddParam("MinValue", 10)\
							.SetBasic(True)\
							.SetDisplay("Parámetros", "Valor mínimo", "Descripción del parámetro de valor mínimo", 10)

		# Inicializar manejadores de eventos de salida como listas vacías
		# Los suscriptores pueden asignar métodos invocables a estos manejadores
		self._output1_handlers = []
		self._output2_handlers = []

	# Los sockets de salida son eventos marcados con el atributo DiagramExternal

	@diagram_external
	def add_Output1(self, handler: Action[Unit]):
		"""
		Suscribirse al evento Output1.

		:param handler: Método invocable que se llamará cuando Output1 se active.
		"""
		self._output1_handlers.append(handler)

	def remove_Output1(self, handler):
		"""
		Cancelar la suscripción al evento Output1.

		:param handler: Método invocable que se eliminará de los suscriptores de Output1.
		"""
		if handler in self._output1_handlers:
			self._output1_handlers.remove(handler)

	@diagram_external
	def add_Output2(self, handler: Action[Unit]):
		"""
		Suscribirse al evento Output2.

		:param handler: Método invocable que se llamará cuando Output2 se active.
		"""
		self._output2_handlers.append(handler)

	def remove_Output2(self, handler):
		"""
		Cancelar la suscripción al evento Output2.

		:param handler: Método invocable que se eliminará de los suscriptores de Output2.
		"""
		if handler in self._output2_handlers:
			self._output2_handlers.remove(handler)

	# Descomente la siguiente propiedad si desea que el método Process
	# se llame cada vez que se recibe un nuevo argumento
	# (no es necesario esperar a recibir todos los argumentos de entrada).
	#
	# @property
	# def WaitAllInput(self):
	#     return False

	@diagram_external
	def Process(self, candle: ICandleMessage, diff: Unit) -> None:
		"""
		Los sockets de entrada son parámetros de método marcados con el atributo DiagramExternal.
		Procesa una vela y un valor diff, y luego invoca eventos de salida según la lógica.

		:param candle: Entrada CandleMessage que representa una vela.
		:param diff: Unit que representa el valor de diferencia que se va a procesar.
		"""
		# Calcular el resultado como suma del precio de cierre de la vela y el valor diff
		res = candle.ClosePrice + diff

		# Invocar Output1 si diff es mayor o igual que el parámetro MinValue;
		# en caso contrario, invocar Output2
		if diff >= self._minValue.Value:
			for handler in self._output1_handlers:
				handler(res)
		else:
			for handler in self._output2_handlers:
				handler(res)

	def Start(self):
		"""
		Se llama cuando el elemento de diagrama se inicia. Añada aquí lógica previa al inicio.
		"""
		super(empty_diagram_element, self).Start()
		# Añadir lógica personalizada que se ejecutará antes de que el elemento se inicie

	def Stop(self):
		"""
		Se llama cuando el elemento de diagrama se detiene. Añada aquí lógica posterior a la detención.
		"""
		super(empty_diagram_element, self).Stop()
		# Añadir lógica personalizada que se ejecutará después de que el elemento se detenga

	def Reset(self):
		"""
		Se llama cuando el elemento de diagrama se restablece. Añada aquí cualquier lógica de restablecimiento.
		"""
		super(empty_diagram_element, self).Reset()
		# Añadir lógica personalizada para restablecer el estado interno del elemento
```

En este código, el cubo tiene dos sockets de entrada y dos sockets de salida. Los sockets de entrada se definen aplicando el decorador @diagram_external a un método:

```python
@diagram_external
def Process(self, candle: ICandleMessage, diff: Unit) -> None:
```

Los sockets de salida se definen aplicando el decorador @diagram_external a un evento (operación de suscripción a evento add_NNN). En el ejemplo del cubo hay dos eventos de este tipo:

```python
# Los sockets de salida son eventos marcados con el atributo DiagramExternal

@diagram_external
def add_Output1(self, handler: Action[Unit]):
	"""
	Suscribirse al evento Output1.

	:param handler: Método invocable que se llamará cuando Output1 se active.
	"""
	self._output1_handlers.append(handler)

def remove_Output1(self, handler):
	"""
	Cancelar la suscripción al evento Output1.

	:param handler: Método invocable que se eliminará de los suscriptores de Output1.
	"""
	if handler in self._output1_handlers:
		self._output1_handlers.remove(handler)

@diagram_external
def add_Output2(self, handler: Action[Unit]):
	"""
	Suscribirse al evento Output2.

	:param handler: Método invocable que se llamará cuando Output2 se active.
	"""
	self._output2_handlers.append(handler)

def remove_Output2(self, handler):
	"""
	Cancelar la suscripción al evento Output2.

	:param handler: Método invocable que se eliminará de los suscriptores de Output2.
	"""
	if handler in self._output2_handlers:
		self._output2_handlers.remove(handler)
```

Por lo tanto, también habrá dos sockets de salida.

Además, se muestra cómo crear una propiedad para el cubo:

```python
self._minValue = self.AddParam("MinValue", 10)\
					.SetBasic(True)\
					.SetDisplay("Parámetros", "Valor mínimo", "Descripción del parámetro de valor mínimo", 10)
```

Al usar la clase [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1), se aplica automáticamente el enfoque de guardar y restaurar configuración.

La propiedad **Valor mínimo** está marcada como básica y será visible en el modo [Propiedades básicas](../../using_visual_designer/diagram_panel.md).

La propiedad comentada [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) determina cuándo se llama el método con sockets de entrada:

```python
# @property
# def WaitAllInput(self):
#     return False
```

Si descomenta la propiedad, el método **Proceso** se llamará siempre que llegue al menos un valor (en el ejemplo, una vela o un valor numérico).

Para añadir el cubo resultante al esquema, debe seleccionar el cubo creado en la paleta, en la sección **Bloques personalizados**:

![Designer elemento de código fuente 01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING]
> Los cubos creados con código Python no pueden usarse en estrategias creadas con código Python. Solo pueden usarse en estrategias creadas [a partir de cubos](../../using_visual_designer.md).

## Véase también

[Creación de un indicador](create_own_indicator.md)
