# Indicadores em Estratégias

Em StockSharp, a classe [Strategy](xref:StockSharp.Algo.Strategies.Strategy) fornece um mecanismo especial para trabalhar com indicadores, que permite controlar o seu estado de formação e determinar quando a estratégia está pronta para operar.

## Propriedade Indicators

A propriedade [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) é uma coleção de indicadores usados na estratégia. Esta coleção foi concebida para acompanhar automaticamente o estado de formação dos indicadores (warm-up).

```cs
// Acesso à coleção de indicadores
INotifyList<IIndicator> indicators = strategy.Indicators;
```

## Propriedade IsFormed

Por defeito, a implementação da propriedade [Strategy.IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) verifica se todos os indicadores na coleção [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) estão formados:

```cs
// Implementação padrão na classe Strategy
public virtual bool IsFormed => _indicators.AllFormed;
```

Uma estratégia é considerada "aquecida" e pronta para operar quando todos os indicadores na coleção estão formados (a sua propriedade [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) devolve `true`).

## Adicionar Indicadores à Coleção

Para determinar corretamente quando a estratégia está pronta, é necessário adicionar os indicadores que utiliza à coleção [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators):

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Criar indicadores
	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };
	
	// Adicionando indicadores à coleção
	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);
	
	// ...
}
```

## Que Indicadores Adicionar

Deve adicionar apenas **indicadores independentes** à coleção [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators). Esta é uma regra importante que ajuda a evitar esperas desnecessárias e a determinar corretamente quando a estratégia está pronta.

### Regras para Adicionar Indicadores:

1. **Indicadores Independentes** - adicione indicadores que processam diretamente dados de mercado (velas, ticks, etc.):

   ```cs
   // Indicadores independentes
   var sma = new SimpleMovingAverage { Length = 20 };
   var rsi = new RelativeStrengthIndex { Length = 14 };
   
   Indicators.Add(sma);
   Indicators.Add(rsi);
   ```

2. **Cadeias de Indicadores** - ao usar uma cadeia de indicadores (em que a saída de um é a entrada de outro), adicione à coleção apenas o **primeiro indicador da cadeia**:

   ```cs
   // Cadeia de indicadores
   var sma = new SimpleMovingAverage { Length = 20 };
   var stdev = new StandardDeviation { Length = 20 };
   var bollingerBands = new BollingerBands 
   { 
       SmaIndicator = sma,
       DeviationIndicator = stdev
   };
   
   // Adicione apenas o primeiro indicador da cadeia
   Indicators.Add(sma);
   // NÃO adicione indicadores dependentes de outros indicadores
   // Indicators.Add(stdev); - incorreto
   // Indicators.Add(bollingerBands); - incorreto
   ```

3. **Indicadores Combinados** - para indicadores que usam vários indicadores independentes (por exemplo, MACD), adicione todos eles:

   ```cs
   var fastEma = new ExponentialMovingAverage { Length = 12 };
   var slowEma = new ExponentialMovingAverage { Length = 26 };
   var signalEma = new ExponentialMovingAverage { Length = 9 };
   var macd = new MovingAverageConvergenceDivergence
   {
       FastEma = fastEma,
       SlowEma = slowEma,
       SignalEma = signalEma
   };
   
   // Adicione indicadores base
   Indicators.Add(fastEma);
   Indicators.Add(slowEma);
   ```

## Exemplos de Utilização

### Exemplo Básico com Duas Médias Móveis

```cs
public class SmaStrategy : Strategy
{
	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;
	
	// ...
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);
		
		_longSma = new SimpleMovingAverage { Length = LongSmaLength };
		_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
		
		// Adicione indicadores à coleção para acompanhar seu estado
		Indicators.Add(_longSma);
		Indicators.Add(_shortSma);
		
		// ...
	}
	
	private void ProcessCandle(ICandleMessage candle)
	{
		// Processar indicadores
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);
		
		// Verificar se a estratégia está pronta antes de executar a lógica de negociação
		if (!IsFormed)
			return;
			
		// Lógica de negociação
		// ...
	}
}
```

### Exemplo Usando IsFormedAndOnline

Para verificar se a estratégia está pronta para negociação, é frequentemente usado o método [IsFormedAndOnlineAndAllowTrading](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)), que combina a verificação da formação dos indicadores, do estado online e da permissão para negociar:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Processar indicadores
	var longValue = _longSma.Process(candle);
	var shortValue = _shortSma.Process(candle);
	
	// Verificação completa da prontidão da estratégia
	if (!IsFormedAndOnlineAndAllowTrading())
		return;
		
	// Lógica de negociação
	// ...
}
```

## Otimizar a Utilização de Indicadores

Em estratégias mais complexas, é importante organizar corretamente o trabalho com indicadores:

```cs
public class ComplexStrategy : Strategy
{
	private SimpleMovingAverage _sma;
	private RelativeStrengthIndex _rsi;
	private BollingerBands _bollinger;
	private StandardDeviation _stdev;
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);
		
		// Criar indicadores
		_sma = new SimpleMovingAverage { Length = 20 };
		_rsi = new RelativeStrengthIndex { Length = 14 };
		
		_stdev = new StandardDeviation { Length = 20 };
		_bollinger = new BollingerBands 
		{ 
			SmaIndicator = _sma,
			DeviationIndicator = _stdev 
		};
		
		// Adicione apenas indicadores independentes
		Indicators.Add(_sma);
		Indicators.Add(_rsi);
		// Não adicione _stdev e _bollinger, pois dependem de _sma
		
		// ...
	}
	
	// ...
}
```

## Funcionalidades Avançadas

Pode substituir a propriedade [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) na sua estratégia se o comportamento padrão não for suficiente:

```cs
public override bool IsFormed
{
	get
	{
		// Verificação padrão do indicador
		if (!base.IsFormed)
			return false;
			
		// Condições adicionais de prontidão da estratégia
		return _customCondition && _additionalCheck;
	}
}
```

## Ver Também

- [Lista de indicadores](../indicators/list_of_indicators.md)
- [Indicador personalizado](../indicators/custom_indicator.md)
- [Compatibilidade da Estratégia com Plataformas](compatibility.md)
