# Eigenen Würfel erstellen

Ähnlich wie beim Erstellen eines [Würfels aus einem Schema](../../using_visual_designer/composite_elements.md) können Sie einen eigenen Würfel auf Basis von Python-Code erstellen. Ein solcher Würfel ist funktionaler als ein aus einem Schema erstellter Würfel.

Um einen Würfel aus Code zu erstellen, müssen Sie ihn im Ordner **Benutzerdefinierte Blöcke** erstellen:

![Designer_Source_Code_Elem_00](../../../../../images/designer_source_code_elem_00.png)

Im folgenden Beispiel erbt der Würfel von der Klasse [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) und sieht so aus:

```python
import clr

# Verweise auf die erforderlichen StockSharp-Assemblys hinzufügen
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Diagram.Core")

# Erforderliche Typen aus .NET und StockSharp importieren
from System import Action
from StockSharp.Messages import Unit
from StockSharp.Messages import ICandleMessage
from StockSharp.Diagram import DiagramExternalElement

from designer_extensions import diagram_external

# Benutzerdefinierte Diagrammelementklasse, die die Verwendung von Eingabe- und Ausgabesockets demonstriert
class empty_diagram_element(DiagramExternalElement):
	"""
	Beispiel-Diagrammelement zur Demonstration der Verwendung von Eingabe- und Ausgabesockets.

	https://doc.stocksharp.com/topics/designer/strategies/using_code/python/creating_your_own_cube.html
	"""

	def __init__(self):
		super(empty_diagram_element, self).__init__()

		# Beispieleigenschaft, die zeigt, wie Parameter zum Diagrammelement hinzugefügt werden
		# Dieser Parameter heißt "MinValue" und hat den Standardwert 10
		self._minValue = self.AddParam("MinValue", 10)\
							.SetBasic(True)\
							.SetDisplay("Parameter", "Mindestwert", "Beschreibung des Mindestwert-Parameter", 10)

		# Ausgabeereignishandler als leere Listen initialisieren
		# Abonnenten können diesen Handlern aufrufbare Methoden zuweisen
		self._output1_handlers = []
		self._output2_handlers = []

	# Ausgabesockets sind Ereignisse, die mit dem Attribut DiagramExternal markiert sind

	@diagram_external
	def add_Output1(self, handler: Action[Unit]):
		"""
		Abonniert das Output1-Ereignis.

		:param handler: Eine aufrufbare Methode, die beim Auslösen von Output1 aufgerufen wird.
		"""
		self._output1_handlers.append(handler)

	def remove_Output1(self, handler):
		"""
		Meldet das Output1-Ereignis ab.

		:param handler: Die aufrufbare Methode, die aus den Output1-Abonnenten entfernt wird.
		"""
		if handler in self._output1_handlers:
			self._output1_handlers.remove(handler)

	@diagram_external
	def add_Output2(self, handler: Action[Unit]):
		"""
		Abonniert das Output2-Ereignis.

		:param handler: Eine aufrufbare Methode, die beim Auslösen von Output2 aufgerufen wird.
		"""
		self._output2_handlers.append(handler)

	def remove_Output2(self, handler):
		"""
		Meldet das Output2-Ereignis ab.

		:param handler: Die aufrufbare Methode, die aus den Output2-Abonnenten entfernt wird.
		"""
		if handler in self._output2_handlers:
			self._output2_handlers.remove(handler)

	# Entfernen Sie die Auskommentierung der folgenden Eigenschaft, wenn die Process-Methode
	# jedes Mal aufgerufen werden soll, wenn ein neues Argument empfangen wird
	# (es muss nicht gewartet werden, bis alle Eingabeargumente empfangen wurden).
	#
	# @property
	# def WaitAllInput(self):
	#     return False

	@diagram_external
	def Process(self, candle: ICandleMessage, diff: Unit) -> None:
		"""
		Eingabesockets sind Methodenparameter, die mit dem Attribut DiagramExternal markiert sind.
		Verarbeitet eine Kerze und einen diff-Wert und ruft anschließend gemäß der Logik Ausgabeereignisse auf.

		:param candle: CandleMessage-Eingabe, die eine Kerze darstellt.
		:param diff: Unit-Wert, der den zu verarbeitenden Differenzwert darstellt.
		"""
		# Ergebnis als Summe aus Schlusskurs der Kerze und diff-Wert berechnen
		res = candle.ClosePrice + diff

		# Output1 aufrufen, wenn diff größer oder gleich dem MinValue-Parameter ist,
		# andernfalls Output2 aufrufen
		if diff >= self._minValue.Value:
			for handler in self._output1_handlers:
				handler(res)
		else:
			for handler in self._output2_handlers:
				handler(res)

	def Start(self):
		"""
		Wird aufgerufen, wenn das Diagrammelement startet. Fügen Sie hier Logik vor dem Start hinzu.
		"""
		super(empty_diagram_element, self).Start()
		# Benutzerdefinierte Logik hinzufügen, die vor dem Start des Elements ausgeführt wird

	def Stop(self):
		"""
		Wird aufgerufen, wenn das Diagrammelement stoppt. Fügen Sie hier Logik nach dem Stop hinzu.
		"""
		super(empty_diagram_element, self).Stop()
		# Benutzerdefinierte Logik hinzufügen, die nach dem Stop des Elements ausgeführt wird

	def Reset(self):
		"""
		Wird aufgerufen, wenn das Diagrammelement zurückgesetzt wird. Fügen Sie hier Reset-Logik hinzu.
		"""
		super(empty_diagram_element, self).Reset()
		# Benutzerdefinierte Logik zum Zurücksetzen des internen Zustands des Elements hinzufügen
```

In diesem Code hat der Würfel zwei eingehende und zwei ausgehende Sockets. Eingehende Sockets werden definiert, indem der Decorator @diagram_external auf eine Methode angewendet wird:

```python
@diagram_external
def Process(self, candle: ICandleMessage, diff: Unit) -> None:
```

Ausgehende Sockets werden definiert, indem der Decorator @diagram_external auf ein Ereignis angewendet wird (Ereignisabonnement-Operation add_NNN). Im Beispiel des Würfels gibt es zwei solche Ereignisse:

```python
# Ausgabesockets sind Ereignisse, die mit dem Attribut DiagramExternal markiert sind

@diagram_external
def add_Output1(self, handler: Action[Unit]):
	"""
	Abonniert das Output1-Ereignis.

	:param handler: Eine aufrufbare Methode, die beim Auslösen von Output1 aufgerufen wird.
	"""
	self._output1_handlers.append(handler)

def remove_Output1(self, handler):
	"""
	Meldet das Output1-Ereignis ab.

	:param handler: Die aufrufbare Methode, die aus den Output1-Abonnenten entfernt wird.
	"""
	if handler in self._output1_handlers:
		self._output1_handlers.remove(handler)

@diagram_external
def add_Output2(self, handler: Action[Unit]):
	"""
	Abonniert das Output2-Ereignis.

	:param handler: Eine aufrufbare Methode, die beim Auslösen von Output2 aufgerufen wird.
	"""
	self._output2_handlers.append(handler)

def remove_Output2(self, handler):
	"""
	Meldet das Output2-Ereignis ab.

	:param handler: Die aufrufbare Methode, die aus den Output2-Abonnenten entfernt wird.
	"""
	if handler in self._output2_handlers:
		self._output2_handlers.remove(handler)
```

Daher gibt es auch zwei ausgehende Sockets.

Zusätzlich wird gezeigt, wie eine Eigenschaft für den Würfel erstellt wird:

```python
self._minValue = self.AddParam("MinValue", 10)\
					.SetBasic(True)\
					.SetDisplay("Parameter", "Mindestwert", "Beschreibung des Mindestwert-Parameter", 10)
```

Bei Verwendung der Klasse [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) wird automatisch der Ansatz zum Speichern und Wiederherstellen von Einstellungen verwendet.

Die Eigenschaft **Mindestwert** ist als Basiseigenschaft markiert und wird im Modus [Basiseinstellungen](../../using_visual_designer/diagram_panel.md) sichtbar sein.

Die auskommentierte Eigenschaft [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) bestimmt, wann die Methode mit eingehenden Sockets aufgerufen wird:

```python
# @property
# def WaitAllInput(self):
#     return False
```

Wenn Sie die Auskommentierung der Eigenschaft entfernen, wird die Methode **Prozess** immer aufgerufen, sobald mindestens ein Wert eintrifft (im Beispiel entweder eine Kerze oder ein numerischer Wert).

Um den resultierenden Würfel zum Schema hinzuzufügen, wählen Sie den erstellten Würfel in der Palette im Abschnitt **Benutzerdefinierte Blöcke** aus:

![Designer_Source_Code_Elem_01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING]
> Würfel aus Python-Code können nicht in Strategien verwendet werden, die in Python-Code erstellt wurden. Sie können nur in Strategien verwendet werden, die [aus Würfeln](../../using_visual_designer.md) erstellt wurden.

## Siehe auch

[Indikator erstellen](create_own_indicator.md)
