# Ejemplo de estrategia Python

La creación de una estrategia desde código fuente se demostrará con el ejemplo de la estrategia SMA, similar al ejemplo de estrategia SMA ensamblada desde cubos en la sección [Creación de un algoritmo a partir de cubos](../../using_visual_designer/first_strategy.md).

Esta sección no describirá las construcciones del lenguaje Python (ni [Strategy](../../../../api/strategies.md), que se usa como base para crear estrategias), pero mencionará características específicas para trabajar con código en **Designer**.

> [!TIP]
> Las estrategias creadas en **Designer** son compatibles con las estrategias creadas en la [API](../../../../api.md) gracias al uso de la clase base común [Strategy](../../../../api/strategies.md). Esto facilita mucho más la ejecución de dichas estrategias fuera de **Designer** que la ejecución de [esquemas](../../../live_execution/running_strategies_outside_of_designer.md).

1. Antes de iniciar el desarrollo en Python, debe importar dependencias .NET mediante el módulo clr, que proporciona IronPython para la interacción con .NET:

```python
import clr

clr.AddReference("System.Drawing")
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo")
```

Bibliotecas principales que deben conectarse:

- módulo clr - se importa para habilitar la interacción del código Python con .NET Framework
- System.Drawing - biblioteca necesaria para trabajar con colores y gráficos
- StockSharp.Messages - contiene mensajes y tipos de datos básicos de S#
- StockSharp.Algo - contiene componentes algorítmicos básicos de S#, incluida la clase base Strategy

> [!NOTE]
> Es importante conectar las bibliotecas al comienzo del archivo, antes de usar cualquier tipo de estas bibliotecas. Después de conectar las bibliotecas, puede importar tipos específicos mediante import normal de Python.

2. Los parámetros de la estrategia se crean usando un enfoque especial:

```python
def __init__(self):
	super(sma_strategy, self).__init__()
	self._isShortLessThenLong = None

	# Inicializar parámetros de estrategia
	self._candleTypeParam = self.Param("CandleType", DataType.TimeFrame(TimeSpan.FromMinutes(1))) \
		.SetDisplay("Tipo de vela", "Tipo de vela para el cálculo de la estrategia.", "Configuración general")

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

Al usar la clase [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1), se aplica automáticamente el enfoque de guardar y restaurar configuración.

3. Al crear indicadores y suscribirse a datos de mercado, debe vincularlos para que los datos entrantes de la suscripción puedan actualizar los valores de los indicadores:

```python
# Crear indicadores
longSma = SMA()
longSma.Length = self.Long
shortSma = SMA()
shortSma.Length = self.Short

# Vincular conjunto de velas e indicadores
subscription = self.SubscribeCandles(self.CandleType)
# Vincular indicadores a las velas e iniciar procesamiento
subscription.Bind(longSma, shortSma, self.OnProcess).Start()
```

4. Al trabajar con gráficos, es importante tener en cuenta que al ejecutar la estrategia [fuera de Designer](../../../live_execution/running_strategies_outside_of_designer.md), el objeto de gráfico puede estar ausente.

```python
# Configurar gráfico si hay GUI disponible
area = self.CreateChartArea()
if area is not None:
	self.DrawCandles(area, subscription)
	self.DrawIndicator(area, shortSma, Color.Coral)
	self.DrawIndicator(area, longSma)
	self.DrawOwnTrades(area)
```

5. Inicie la protección de posición mediante [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) si la lógica de la estrategia lo requiere:

```python
self.StartProtection(self.TakeValue, self.StopValue)
```

6. La propia lógica de la estrategia, implementada en el método OnProcess. El método es llamado por la suscripción creada en el paso 1:

```python
def OnProcess(self, candle, longValue, shortValue):
	"""
	Procesa cada vela finalizada, registra información y ejecuta la lógica de negociación en el cruce de SMA.

	:param candle: Mensaje de vela procesado.
		:param longValue: Valor actual de la SMA larga.
		:param shortValue: Valor actual de la SMA corta.
	"""
	self.LogInfo("Nueva vela {0}: {6} {1};{2};{3};{4}; volumen {5}", candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId)

	# Si la vela no está finalizada, no hacer nada
	if candle.State != CandleStates.Finished:
		return

	# Determinar si la SMA corta es menor que la SMA larga
	isShortLessThenLong = shortValue < longValue

	if self._isShortLessThenLong is None:
		self._isShortLessThenLong = isShortLessThenLong
	elif self._isShortLessThenLong != isShortLessThenLong:
		# Se produjo un cruce
		direction = Sides.Sell if isShortLessThenLong else Sides.Buy

		# Calcular volumen para abrir posición o revertir
		volume = self.Volume if self.Position == 0 else Math.Min(Math.Abs(self.Position), self.Volume) * 2

		# Obtener paso de precio (predeterminado 1 si no está definido)
		priceStep = self.GetSecurity().PriceStep or 1

		# Calcular precio de orden con desplazamiento
		price = candle.ClosePrice + (priceStep if direction == Sides.Buy else -priceStep)

		if direction == Sides.Buy:
			self.BuyLimit(price, volume)
		else:
			self.SellLimit(price, volume)

		# Actualizar estado
		self._isShortLessThenLong = isShortLessThenLong
```

7. Un requisito obligatorio para las estrategias Python es sobrescribir el método virtual [CreateClone](xref:StockSharp.Algo.Strategies.Strategy.CreateClone). Este método es necesario para crear nuevas instancias de estrategia requeridas por [Designer](../../../../designer.md) al optimizar parámetros de estrategia o durante las pruebas. Sin sobrescribirlo, estas operaciones serán imposibles.

```python
def CreateClone(self):
	"""
	!! OBLIGATORIO !! Crea una nueva instancia de la estrategia.
	"""
	return sma_strategy()
```

> [!IMPORTANT]
> No sobrescribir el método CreateClone hará imposible probar la estrategia con datos históricos o realizar optimización de parámetros. Al crear una estrategia en Python, añada siempre este método.
