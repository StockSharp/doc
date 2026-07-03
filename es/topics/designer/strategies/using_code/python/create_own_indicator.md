# Creación de un indicador

La creación de un indicador personalizado en la [API](../../../../api.md) se describe en la sección [Indicador personalizado](../../../../api/indicators/custom_indicator.md). Estos indicadores son totalmente compatibles con **Designer**.

Para crear un indicador, debe seleccionar la carpeta **Indicators** en el panel **Schemes**, hacer clic con el botón derecho y seleccionar **Add** en el menú contextual:

![Designer_Source_Code_Indicator_00](../../../../../images/designer_source_code_indicator_00.png)

El código del indicador tendrá este aspecto:

```python
import clr
import random

clr.AddReference("StockSharp.BusinessEntities")
clr.AddReference("StockSharp.Algo")

from StockSharp.Algo.Indicators import BaseIndicator, DecimalIndicatorValue
from indicator_extensions import *

class empty_indicator(BaseIndicator):
	"""
	Indicador de ejemplo que demuestra cómo guardar y cargar parámetros.

	Doc https://doc.stocksharp.com/topics/designer/strategies/using_code/python/create_own_indicator.html
	
	Cambia el precio de entrada en +20% o -20%.
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
		"""Determina si el indicador ha recibido suficientes entradas para considerarse formado."""
		return self._isFormed

	def Reset(self):
		"""Restablece el estado del indicador y los contadores internos."""
		super(empty_indicator, self).Reset()
		self._isFormed = False
		self._counter = 0

	def OnProcess(self, input):
		"""
		Procesa el valor de indicador entrante y aplica un cambio aleatorio.
		
		:param input: Valor de indicador entrante.
		:return: Nuevo DecimalIndicatorValue después de aplicar cambios.
		"""
		# Cada décima llamada intenta devolver un valor vacío
		if random.randint(0, 10) == 0:
			return DecimalIndicatorValue(self, input.Time)

		if self._counter == 5:
			# Por ejemplo, nuestro indicador necesita 5 entradas para quedar formado
			self._isFormed = True
		self._counter += 1

		value = to_decimal(input)

		# Aplicar cambio aleatorio de +/- _change por ciento al valor actual
		value += value * random.randint(-self._change, self._change) / 100.0

		result = DecimalIndicatorValue(self, value, input.Time)
		# Marcar el valor como final según una decisión aleatoria
		result.IsFinal = bool(random.getrandbits(1))
		return result

	def Load(self, storage):
		"""
		Carga los parámetros del indicador desde el almacenamiento persistente.
		
		:param storage: Almacenamiento de configuración desde el que cargar.
		"""
		super(empty_indicator, self).Load(storage)
		self.Change = storage.GetValue("Change", self.Change)

	def Save(self, storage):
		"""
		Guarda los parámetros del indicador en el almacenamiento persistente.
		
		:param storage: Almacenamiento de configuración en el que guardar.
		"""
		super(empty_indicator, self).Save(storage)
		storage.SetValue("Change", self.Change)

	def __str__(self):
		return f"Change: {self.Change}"

	def ToString(self):
		return str(self)
```

Este indicador recibe un valor entrante y realiza una desviación aleatoria basada en el valor especificado del parámetro **Change**.

La descripción de los métodos del indicador está disponible en la sección [Indicador personalizado](../../../../api/indicators/custom_indicator.md).

Para añadir el indicador creado al esquema, debe usar el cubo [Indicator](../../using_visual_designer/elements/common/indicator.md) y después establecer en él el indicador deseado:

![Designer_Source_Code_Indicator_01](../../../../../images/designer_source_code_indicator_01.png)

El parámetro **Change**, establecido previamente en el código del indicador, se muestra en el panel de propiedades.

> [!WARNING] 
> Los indicadores creados con código Python no pueden usarse en estrategias creadas con código Python. Solo pueden usarse en estrategias creadas [a partir de cubos](../../using_visual_designer.md).
