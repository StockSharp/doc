# Criar o seu próprio cubo

De forma semelhante à criação de um [cubo a partir de um esquema](../../using_visual_designer/composite_elements.md), pode criar o seu próprio cubo com base em código Python. Um cubo deste tipo será mais funcional do que um cubo criado a partir de um esquema.

Para criar um cubo a partir de código, tem de o criar na pasta **Blocos personalizados**:

![Designer elemento de código-fonte 00](../../../../../images/designer_source_code_elem_00.png)

No exemplo abaixo, o cubo herda da classe [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) e tem o seguinte aspeto:

```python
import clr

# Adicionar referências aos assemblies StockSharp necessários
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Diagram.Core")

# Importar os tipos necessários de .NET e StockSharp
from System import Action
from StockSharp.Messages import Unit
from StockSharp.Messages import ICandleMessage
from StockSharp.Diagram import DiagramExternalElement

from designer_extensions import diagram_external

# Classe de elemento de diagrama personalizada que demonstra a utilização de conectores de entrada e saída
class empty_diagram_element(DiagramExternalElement):
	"""
	Exemplo de elemento de diagrama que demonstra a utilização de conectores de entrada e saída.

	https://doc.stocksharp.com/topics/designer/strategies/using_code/python/creating_your_own_cube.html
	"""

	def __init__(self):
		super(empty_diagram_element, self).__init__()

		# Propriedade de exemplo para mostrar como adicionar parâmetros ao elemento de diagrama
		# Este parâmetro chama-se "MinValue" e tem o valor predefinido 10
		self._minValue = self.AddParam("MinValue", 10)\
							.SetBasic(True)\
							.SetDisplay("Parâmetros", "Valor mínimo", "Descrição do parâmetro de valor mínimo", 10)

		# Inicializar os manipuladores de eventos de saída como listas vazias
		# Os subscritores podem atribuir métodos chamáveis a estes manipuladores
		self._output1_handlers = []
		self._output2_handlers = []

	# Os conectores de saída são eventos assinalados com o atributo DiagramExternal

	@diagram_external
	def add_Output1(self, handler: Action[Unit]):
		"""
		Subscrever o evento Output1.

		:param handler: Um método chamável a invocar quando Output1 é acionado.
		"""
		self._output1_handlers.append(handler)

	def remove_Output1(self, handler):
		"""
		Anular a subscrição do evento Output1.

		:param handler: O método chamável a remover dos subscritores de Output1.
		"""
		if handler in self._output1_handlers:
			self._output1_handlers.remove(handler)

	@diagram_external
	def add_Output2(self, handler: Action[Unit]):
		"""
		Subscrever o evento Output2.

		:param handler: Um método chamável a invocar quando Output2 é acionado.
		"""
		self._output2_handlers.append(handler)

	def remove_Output2(self, handler):
		"""
		Anular a subscrição do evento Output2.

		:param handler: O método chamável a remover dos subscritores de Output2.
		"""
		if handler in self._output2_handlers:
			self._output2_handlers.remove(handler)

	# Descomente a propriedade seguinte se quiser que o método Process
	# seja chamado sempre que for recebido um novo argumento
	# (não é necessário esperar pela receção de todos os argumentos de entrada).
	#
	# @property
	# def WaitAllInput(self):
	#     return False

	@diagram_external
	def Process(self, candle: ICandleMessage, diff: Unit) -> None:
		"""
		Os conectores de entrada são parâmetros de métodos assinalados com o atributo DiagramExternal.
		Processa uma vela e um valor diff e, em seguida, invoca eventos de saída com base na lógica.

		:param candle: Entrada CandleMessage que representa uma vela.
		:param diff: Unit que representa o valor de diferença a processar.
		"""
		# Calcular o resultado como a soma do preço de fecho da vela e do valor diff
		res = candle.ClosePrice + diff

		# Invocar Output1 se diff for maior ou igual ao parâmetro MinValue,
		# caso contrário, invocar Output2
		if diff >= self._minValue.Value:
			for handler in self._output1_handlers:
				handler(res)
		else:
			for handler in self._output2_handlers:
				handler(res)

	def Start(self):
		"""
		Chamado quando o elemento de diagrama é iniciado. Adicione aqui qualquer lógica anterior ao início.
		"""
		super(empty_diagram_element, self).Start()
		# Adicionar lógica personalizada a executar antes de o elemento iniciar

	def Stop(self):
		"""
		Chamado quando o elemento de diagrama é parado. Adicione aqui qualquer lógica posterior à paragem.
		"""
		super(empty_diagram_element, self).Stop()
		# Adicionar lógica personalizada a executar depois de o elemento parar

	def Reset(self):
		"""
		Chamado quando o elemento de diagrama é reposto. Adicione aqui qualquer lógica de reposição.
		"""
		super(empty_diagram_element, self).Reset()
		# Adicionar lógica personalizada para repor o estado interno do elemento
```

Neste código, o cubo tem dois conectores de entrada e dois conectores de saída. Os conectores de entrada são definidos aplicando o decorador @diagram_external a um método:

```python
@diagram_external
def Process(self, candle: ICandleMessage, diff: Unit) -> None:
```

Os conectores de saída são definidos aplicando o decorador @diagram_external a um evento (operação de subscrição de evento add_NNN). No exemplo do cubo, existem dois eventos deste tipo:

```python
# Os conectores de saída são eventos assinalados com o atributo DiagramExternal

@diagram_external
def add_Output1(self, handler: Action[Unit]):
	"""
	Subscrever o evento Output1.

	:param handler: Um método chamável a invocar quando Output1 é acionado.
	"""
	self._output1_handlers.append(handler)

def remove_Output1(self, handler):
	"""
	Anular a subscrição do evento Output1.

	:param handler: O método chamável a remover dos subscritores de Output1.
	"""
	if handler in self._output1_handlers:
		self._output1_handlers.remove(handler)

@diagram_external
def add_Output2(self, handler: Action[Unit]):
	"""
	Subscrever o evento Output2.

	:param handler: Um método chamável a invocar quando Output2 é acionado.
	"""
	self._output2_handlers.append(handler)

def remove_Output2(self, handler):
	"""
	Anular a subscrição do evento Output2.

	:param handler: O método chamável a remover dos subscritores de Output2.
	"""
	if handler in self._output2_handlers:
		self._output2_handlers.remove(handler)
```

Assim, também existirão dois conectores de saída.

Além disso, é mostrado como criar uma propriedade para o cubo:

```python
self._minValue = self.AddParam("MinValue", 10)\
					.SetBasic(True)\
					.SetDisplay("Parâmetros", "Valor mínimo", "Descrição do parâmetro de valor mínimo", 10)
```

Ao utilizar a classe [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1), a abordagem de guardar e restaurar definições é usada automaticamente.

A propriedade **Valor mínimo** está marcada como básica e ficará visível no modo [Definições básicas](../../using_visual_designer/diagram_panel.md).

A propriedade comentada [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) determina quando é chamado o método com conectores de entrada:

```python
# @property
# def WaitAllInput(self):
#     return False
```

Se descomentar a propriedade, o método `Process` será chamado sempre que chegar pelo menos um valor (no caso do exemplo, uma vela ou um valor numérico).

Para adicionar o cubo resultante ao esquema, tem de selecionar o cubo criado na paleta, na secção **Blocos personalizados**:

![Designer elemento de código-fonte 01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING]
> Cubos criados a partir de código Python não podem ser usados em estratégias criadas em código Python. Só podem ser usados em estratégias criadas [a partir de cubos](../../using_visual_designer.md).

## Ver Também

[Criar um indicador](create_own_indicator.md)
