# Exemplo de estratégia em Python

A criação de uma estratégia a partir de código-fonte será demonstrada usando o exemplo da estratégia SMA, de forma semelhante ao exemplo da estratégia SMA montada a partir de cubos na secção [Creating Algorithm from Cubes](../../using_visual_designer/first_strategy.md).

Esta secção não irá descrever construções da linguagem Python (nem a [Strategy](../../../../api/strategies.md), que é usada como base para criar estratégias), mas irá mencionar funcionalidades específicas para trabalhar com código no **Designer**.

> [!TIP]
> As estratégias criadas no **Designer** são compatíveis com estratégias criadas na [API](../../../../api.md), graças à utilização da classe base comum [Strategy](../../../../api/strategies.md). Isto torna a execução dessas estratégias fora do **Designer** significativamente mais simples do que executar [esquemas](../../../live_execution/running_strategies_outside_of_designer.md).

1. Antes de iniciar o desenvolvimento em Python, tem de importar as dependências .NET através do módulo clr, fornecido pelo IronPython para interação com .NET:

```python
import clr

clr.AddReference("System.Drawing")
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo")
```

Principais bibliotecas que têm de ser ligadas:

- módulo clr - importado para permitir a interação do código Python com .NET Framework
- System.Drawing - biblioteca necessária para trabalhar com cores e gráficos
- StockSharp.Messages - contém mensagens e tipos de dados básicos do S#
- StockSharp.Algo - contém componentes algorítmicos básicos do S#, incluindo a classe base Strategy

> [!NOTE]
> É importante ligar as bibliotecas no início do ficheiro, antes de usar quaisquer tipos dessas bibliotecas. Depois de ligar as bibliotecas, pode importar tipos específicos através de import Python normal.

2. Os parâmetros da estratégia são criados através de uma abordagem especial:

```python
def __init__(self):
	super(sma_strategy, self).__init__()
	self._isShortLessThenLong = None

	# Inicializar parâmetros da estratégia
	self._candleTypeParam = self.Param("CandleType", DataType.TimeFrame(TimeSpan.FromMinutes(1))) \
		.SetDisplay("Candle type", "Candle type for strategy calculation.", "General")

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

Ao utilizar a classe [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1), a abordagem de guardar e restaurar definições é usada automaticamente.

3. Ao criar indicadores e subscrever dados de mercado, tem de os associar para que os dados recebidos da subscrição possam atualizar os valores dos indicadores:

```python
# Criar indicadores
longSma = SMA()
longSma.Length = self.Long
shortSma = SMA()
shortSma.Length = self.Short

# Associar o conjunto de velas e os indicadores
subscription = self.SubscribeCandles(self.CandleType)
# Associar os indicadores às velas e iniciar o processamento
subscription.Bind(longSma, shortSma, self.OnProcess).Start()
```

4. Ao trabalhar com gráficos, é importante ter em conta que, ao executar a estratégia [fora do Designer](../../../live_execution/running_strategies_outside_of_designer.md), o objeto do gráfico pode estar ausente.

```python
# Configurar o gráfico se a GUI estiver disponível
area = self.CreateChartArea()
if area is not None:
	self.DrawCandles(area, subscription)
	self.DrawIndicator(area, shortSma, Color.Coral)
	self.DrawIndicator(area, longSma)
	self.DrawOwnTrades(area)
```

5. Iniciar a proteção da posição através de [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)), se tal for exigido pela lógica da estratégia:

```python
self.StartProtection(self.TakeValue, self.StopValue)
```

6. A lógica da estratégia propriamente dita, implementada no método OnProcess. O método é chamado pela subscrição criada no passo 1:

```python
def OnProcess(self, candle, longValue, shortValue):
	"""
	Processa cada vela concluída, regista informações e executa a lógica de negociação no cruzamento das SMA.
	
	:param candle: A mensagem de vela processada.
		:param longValue: O valor atual da SMA longa.
		:param shortValue: O valor atual da SMA curta.
	"""
	self.LogInfo("Nova vela {0}: {6} {1};{2};{3};{4}; volume {5}", candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId)

	# Se a vela não estiver concluída, não fazer nada
	if candle.State != CandleStates.Finished:
		return

	# Determinar se a SMA curta é menor do que a SMA longa
	isShortLessThenLong = shortValue < longValue

	if self._isShortLessThenLong is None:
		self._isShortLessThenLong = isShortLessThenLong
	elif self._isShortLessThenLong != isShortLessThenLong:
		# Ocorreu um cruzamento
		direction = Sides.Sell if isShortLessThenLong else Sides.Buy

		# Calcular o volume para abrir posição ou inverter
		volume = self.Volume if self.Position == 0 else Math.Min(Math.Abs(self.Position), self.Volume) * 2

		# Obter o passo de preço (predefinição para 1 se não estiver definido)
		priceStep = self.GetSecurity().PriceStep or 1

		# Calcular o preço da ordem com deslocamento
		price = candle.ClosePrice + (priceStep if direction == Sides.Buy else -priceStep)

		if direction == Sides.Buy:
			self.BuyLimit(price, volume)
		else:
			self.SellLimit(price, volume)

		# Atualizar o estado
		self._isShortLessThenLong = isShortLessThenLong
```

7. Um requisito obrigatório para estratégias Python é substituir o método virtual [CreateClone](xref:StockSharp.Algo.Strategies.Strategy.CreateClone). Este método é necessário para criar novas instâncias da estratégia exigidas pelo [Designer](../../../../designer.md) ao otimizar parâmetros da estratégia ou durante testes. Sem o substituir, estas operações serão impossíveis.

```python
def CreateClone(self):
	"""
	!! OBRIGATÓRIO!! Cria uma nova instância da estratégia.
	"""
	return sma_strategy()
```

> [!IMPORTANT]
> Não substituir o método CreateClone tornará impossível testar a estratégia em dados históricos ou realizar otimização de parâmetros. Ao criar uma estratégia em Python, adicione sempre este método.
