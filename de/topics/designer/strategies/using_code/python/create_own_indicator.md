# Indikator erstellen

Das Erstellen eines eigenen Indikators in der [API](../../../../api.md) wird im Abschnitt [Benutzerdefinierter Indikator](../../../../api/indicators/custom_indicator.md) beschrieben. Solche Indikatoren sind vollständig mit **Designer** kompatibel.

Um einen Indikator zu erstellen, wählen Sie im Panel **Schemata** den Ordner **Indikatoren** aus, klicken mit der rechten Maustaste und wählen im Kontextmenü **Hinzufügen**:

![Designer_Source_Code_Indicator_00](../../../../../images/designer_source_code_indicator_00.png)

Der Indikatorcode sieht folgendermaßen aus:

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
		# Bei jedem 10. Aufruf versuchen, einen leeren Wert zurückzugeben
		if random.randint(0, 10) == 0:
			return DecimalIndicatorValue(self, input.Time)

		if self._counter == 5:
			# Zum Beispiel benötigt unser Indikator 5 Eingaben, um gebildet zu werden
			self._isFormed = True
		self._counter += 1

		value = to_decimal(input)

		# Zufällige Änderung um +/- _change Prozent auf den aktuellen Wert anwenden
		value += value * random.randint(-self._change, self._change) / 100.0

		result = DecimalIndicatorValue(self, value, input.Time)
		# Wert auf Basis einer zufälligen Entscheidung als final markieren
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

Dieser Indikator empfängt einen eingehenden Wert und erzeugt auf Basis des angegebenen Parameterwerts **Änderung** eine zufällige Abweichung.

Die Beschreibung der Indikatormethoden finden Sie im Abschnitt [Benutzerdefinierter Indikator](../../../../api/indicators/custom_indicator.md).

Um den erstellten Indikator zum Schema hinzuzufügen, verwenden Sie den Würfel [Indikator](../../using_visual_designer/elements/common/indicator.md) und legen darin den gewünschten Indikator fest:

![Designer_Source_Code_Indicator_01](../../../../../images/designer_source_code_indicator_01.png)

Der Parameter **Änderung**, der zuvor im Indikatorcode festgelegt wurde, wird im Eigenschaftenpanel angezeigt.

> [!WARNING]
> Indikatoren aus Python-Code können nicht in Strategien verwendet werden, die in Python-Code erstellt wurden. Sie können nur in Strategien verwendet werden, die [aus Würfeln](../../using_visual_designer.md) erstellt wurden.

