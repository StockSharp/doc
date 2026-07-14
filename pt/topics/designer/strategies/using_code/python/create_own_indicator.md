# Criar um indicador

A criação de um indicador personalizado na [API](../../../../api.md) é descrita na secção [Indicador personalizado](../../../../api/indicators/custom_indicator.md). Esses indicadores são totalmente compatíveis com o **Designer**.

Para criar um indicador, é necessário selecionar a pasta **Indicadores** no painel **Esquemas**, clicar com o botão direito e selecionar **Adicionar** no menu de contexto:

![Designer indicador de código-fonte 00](../../../../../images/designer_source_code_indicator_00.png)

O código do indicador terá o seguinte aspeto:

```python
import clr
import random

clr.AddReference("StockSharp.BusinessEntities")
clr.AddReference("StockSharp.Algo")

from StockSharp.Algo.Indicators import BaseIndicator, DecimalIndicatorValue
from indicator_extensions import *

class empty_indicator(BaseIndicator):
	"""
	Indicador de exemplo que demonstra como guardar e carregar parâmetros.

	Documentação https://doc.stocksharp.com/topics/designer/strategies/using_code/python/create_own_indicator.html

	Altera o preço de entrada em +20% ou -20%.
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
		"""Determina se o indicador recebeu entradas suficientes para ser considerado formado."""
		return self._isFormed

	def Reset(self):
		"""Repõe o estado do indicador e os contadores internos."""
		super(empty_indicator, self).Reset()
		self._isFormed = False
		self._counter = 0

	def OnProcess(self, input):
		"""
		Processa o valor de indicador recebido e aplica uma alteração aleatória.

		:param input: O valor de indicador recebido.
		:return: Um novo DecimalIndicatorValue depois de aplicar as alterações.
		"""
		# Em cada 10.ª chamada, tentar devolver um valor vazio
		if random.randint(0, 10) == 0:
			return DecimalIndicatorValue(self, input.Time)

		if self._counter == 5:
			# Por exemplo, o nosso indicador precisa de 5 entradas para ficar formado
			self._isFormed = True
		self._counter += 1

		value = to_decimal(input)

		# Aplicar uma alteração aleatória de +/- _change por cento ao valor atual
		value += value * random.randint(-self._change, self._change) / 100.0

		result = DecimalIndicatorValue(self, value, input.Time)
		# Marcar o valor como final com base numa decisão aleatória
		result.IsFinal = bool(random.getrandbits(1))
		return result

	def Load(self, storage):
		"""
		Carrega os parâmetros do indicador a partir do armazenamento persistente.

		:param storage: O armazenamento de definições a partir do qual carregar.
		"""
		super(empty_indicator, self).Load(storage)
		self.Change = storage.GetValue("Change", self.Change)

	def Save(self, storage):
		"""
		Guarda os parâmetros do indicador no armazenamento persistente.

		:param storage: O armazenamento de definições onde guardar.
		"""
		super(empty_indicator, self).Save(storage)
		storage.SetValue("Change", self.Change)

	def __str__(self):
		return f"Alteração: {self.Change}"

	def ToString(self):
		return str(self)
```

Este indicador recebe um valor de entrada e aplica um desvio aleatório com base no valor especificado do parâmetro **Change**.

A descrição dos métodos do indicador está disponível na secção [Indicador personalizado](../../../../api/indicators/custom_indicator.md).

Para adicionar o indicador criado ao esquema, é necessário usar o cubo [Indicador](../../using_visual_designer/elements/common/indicator.md) e, em seguida, definir nele o indicador pretendido:

![Designer indicador de código-fonte 01](../../../../../images/designer_source_code_indicator_01.png)

O parâmetro **Change**, previamente definido no código do indicador, é apresentado no painel de propriedades.

> [!WARNING]
> Indicadores em código Python não podem ser usados em estratégias criadas em código Python. Só podem ser usados em estratégias criadas [a partir de cubos](../../using_visual_designer.md).
