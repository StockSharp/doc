# Beispiel einer Python-Strategie

Das Erstellen einer Strategie aus Quellcode wird anhand des Beispiels einer SMA-Strategie gezeigt, ähnlich wie im Beispiel der aus Würfeln zusammengesetzten SMA-Strategie im Abschnitt [Creating Algorithm from Cubes](../../using_visual_designer/first_strategy.md).

Dieser Abschnitt beschreibt keine Python-Sprachkonstrukte (oder [Strategy](../../../../api/strategies.md), die als Basis für die Erstellung von Strategien verwendet wird), sondern erwähnt spezielle Aspekte der Arbeit mit Code im **Designer**.

> [!TIP]
> Strategien, die im **Designer** erstellt wurden, sind durch die gemeinsame Basisklasse [Strategy](../../../../api/strategies.md) mit Strategien aus der [API](../../../../api.md) kompatibel. Dadurch ist das Ausführen solcher Strategien außerhalb des **Designer** deutlich einfacher als das Ausführen von [Schemata](../../../live_execution/running_strategies_outside_of_designer.md).

1. Vor dem Beginn der Python-Entwicklung müssen Sie .NET-Abhängigkeiten über das Modul clr importieren, das von IronPython für die Interaktion mit .NET bereitgestellt wird:

```python
import clr

clr.AddReference("System.Drawing")
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo")
```

Wichtige Bibliotheken, die eingebunden werden müssen:

- clr-Modul - wird importiert, um die Interaktion von Python-Code mit .NET Framework zu ermöglichen
- System.Drawing - Bibliothek, die für die Arbeit mit Farben und Grafiken benötigt wird
- StockSharp.Messages - enthält grundlegende S#-Nachrichten und Datentypen
- StockSharp.Algo - enthält grundlegende algorithmische S#-Komponenten einschließlich der Basisklasse Strategy

> [!NOTE]
> Es ist wichtig, Bibliotheken am Anfang der Datei einzubinden, bevor Typen aus diesen Bibliotheken verwendet werden. Nach dem Einbinden der Bibliotheken können Sie bestimmte Typen über den regulären Python-Import importieren.

2. Strategieparameter werden mit einem speziellen Ansatz erstellt:

```python
def __init__(self):
	super(sma_strategy, self).__init__()
	self._isShortLessThenLong = None

	# Strategieparameter initialisieren
	self._candleTypeParam = self.Param("CandleType", DataType.TimeFrame(TimeSpan.FromMinutes(1))) \
		.SetDisplay("Kerzentyp", "Kerzentyp für die Strategieberechnung.", "Allgemein")

	self._long = self.Param("Long", 80)
	self._short = self.Param("Short", 30)

	self._takeValue = self.Param("TakeValue", Unit(0, UnitTypes.Absolute))
	self._stopValue = self.Param("StopValue", Unit(2, UnitTypes.Percent))

@property
def CandleType(self):
	return self._candleTypeParam.Value

@CandleType.setter
def CandleType(self, value):
	self._candleTypeParam.Value = value

@property
def Long(self):
	return self._long.Value

@Long.setter
def Long(self, value):
	self._long.Value = value

@property
def Short(self):
	return self._short.Value

@Short.setter
def Short(self, value):
	self._short.Value = value

@property
def TakeValue(self):
	return self._takeValue.Value

@TakeValue.setter
def TakeValue(self, value):
	self._takeValue.Value = value

@property
def StopValue(self):
	return self._stopValue.Value

@StopValue.setter
def StopValue(self, value):
	self._stopValue.Value = value
```

Bei Verwendung der Klasse [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) wird automatisch der Ansatz zum Speichern und Wiederherstellen von Einstellungen verwendet.

3. Beim Erstellen von Indikatoren und Abonnieren von Marktdaten müssen Sie diese binden, damit eingehende Daten aus dem Abonnement die Indikatorwerte aktualisieren können:

```python
# Indikatoren erstellen
longSma = SMA()
longSma.Length = self.Long
shortSma = SMA()
shortSma.Length = self.Short

# Kerzensatz und Indikatoren binden
subscription = self.SubscribeCandles(self.CandleType)
# Indikatoren an die Kerzen binden und Verarbeitung starten
subscription.Bind(longSma, shortSma, self.OnProcess).Start()
```

4. Bei der Arbeit mit Charts ist es wichtig zu berücksichtigen, dass das Chart-Objekt fehlen kann, wenn die Strategie [außerhalb des Designer](../../../live_execution/running_strategies_outside_of_designer.md) ausgeführt wird.

```python
# Chart konfigurieren, wenn eine GUI verfügbar ist
area = self.CreateChartArea()
if area is not None:
	self.DrawCandles(area, subscription)
	self.DrawIndicator(area, shortSma, Color.Coral)
	self.DrawIndicator(area, longSma)
	self.DrawOwnTrades(area)
```

5. Starten Sie bei Bedarf den Positionsschutz über [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)), wenn die Strategielogik dies erfordert:

```python
self.StartProtection(self.TakeValue, self.StopValue)
```

6. Die Strategielogik selbst ist in der Methode OnProcess implementiert. Die Methode wird durch das in Schritt 1 erstellte Abonnement aufgerufen:

```python
def OnProcess(self, candle, longValue, shortValue):
	"""
	Processes each finished candle, logs information, and executes trading logic on SMA crossing.

	:param candle: The processed candle message.
		:param longValue: The current value of the long SMA.
		:param shortValue: The current value of the short SMA.
	"""
	self.LogInfo("Neue Kerze {0}: {6} {1};{2};{3};{4}; Volumen {5}", candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId)

	# Wenn die Kerze nicht abgeschlossen ist, nichts tun
	if candle.State != CandleStates.Finished:
		return

	# Bestimmen, ob short SMA kleiner als long SMA ist
	isShortLessThenLong = shortValue < longValue

	if self._isShortLessThenLong is None:
		self._isShortLessThenLong = isShortLessThenLong
	elif self._isShortLessThenLong != isShortLessThenLong:
		# Kreuzung ist aufgetreten
		direction = Sides.Sell if isShortLessThenLong else Sides.Buy

		# Volumen zum Öffnen oder Umkehren einer Position berechnen
		volume = self.Volume if self.Position == 0 else Math.Min(Math.Abs(self.Position), self.Volume) * 2

		# Preisschritt abrufen (Standardwert 1, falls nicht gesetzt)
		priceStep = self.GetSecurity().PriceStep or 1

		# Orderpreis mit Offset berechnen
		price = candle.ClosePrice + (priceStep if direction == Sides.Buy else -priceStep)

		if direction == Sides.Buy:
			self.BuyLimit(price, volume)
		else:
			self.SellLimit(price, volume)

		# Zustand aktualisieren
		self._isShortLessThenLong = isShortLessThenLong
```

7. Eine zwingende Voraussetzung für Python-Strategien ist das Überschreiben der virtuellen Methode [CreateClone](xref:StockSharp.Algo.Strategies.Strategy.CreateClone). Diese Methode wird benötigt, um neue Strategieinstanzen zu erstellen, die von [Designer](../../../../designer.md) bei der Optimierung von Strategieparametern oder während des Testings benötigt werden. Ohne Überschreiben dieser Methode sind diese Operationen nicht möglich.

```python
def CreateClone(self):
	"""
	!! REQUIRED!! Creates a new instance of the strategy.
	"""
	return sma_strategy()
```

> [!IMPORTANT]
> Wenn die Methode CreateClone nicht überschrieben wird, ist es unmöglich, die Strategie auf historischen Daten zu testen oder eine Parameteroptimierung durchzuführen. Fügen Sie diese Methode beim Erstellen einer Strategie in Python immer hinzu.
