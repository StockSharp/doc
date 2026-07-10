# Indicador personalizado

Para criar o seu próprio indicador, é necessário implementar a interface [IIndicator](xref:StockSharp.Algo.Indicators.IIndicator). Como exemplo, pode consultar o código-fonte de outros indicadores localizados no repositório [GitHub/StockSharp](https://github.com/StockSharp/StockSharp). Eis como é a implementação da média móvel simples [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage):

```cs
/// <summary>
/// Média móvel simples.
/// </summary>
[DisplayName("SMA")]
[Description("Média móvel simples.")]
public class SimpleMovingAverage : LengthIndicator<decimal>
{
	/// <summary>
	/// Cria <see cref="SimpleMovingAverage"/>.
	/// </summary>
	public SimpleMovingAverage()
	{
		Length = 32;
	}

	/// <summary>
	/// Processar valor de entrada.
	/// </summary>
	/// <param name="input">Valor de entrada.</param>
	/// <returns>Valor resultante.</returns>
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		var newValue = input.GetValue<decimal>();
		if (input.IsFinal)
		{
			Buffer.Add(newValue);
			if (Buffer.Count > Length)
				Buffer.RemoveAt(0);
		}
		
		if (input.IsFinal)
			return new DecimalIndicatorValue(this, Buffer.Sum() / Length);
		
		return new DecimalIndicatorValue(this, (Buffer.Skip(1).Sum() + newValue) / Length);
	}
}
```


[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) herda de [LengthIndicator\<TResult\>](xref:StockSharp.Algo.Indicators.LengthIndicator`1), da qual devem herdar todos os indicadores com um parâmetro de comprimento de período.

## Propriedades e métodos importantes dos indicadores

Ao criar um indicador personalizado, deve prestar especial atenção às seguintes propriedades e métodos:

### NumValuesToInitialize

A propriedade [NumValuesToInitialize](xref:StockSharp.Algo.Indicators.IIndicator.NumValuesToInitialize) indica quantos valores o indicador requer para inicialização (formação ou "aquecimento"). Este valor é usado para determinar quando o indicador pode ser considerado formado e pronto para utilização:

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => Length;
```

Para indicadores mais complexos compostos por vários componentes, este valor é normalmente determinado como o máximo de todas as partes constituintes:

```cs
/// <inheritdoc />
public override int NumValuesToInitialize => _shortEma.NumValuesToInitialize.Max(_longEma.NumValuesToInitialize);
```

### Measure

A propriedade [Measure](xref:StockSharp.Algo.Indicators.IIndicator.Measure) define o tipo de medição e a dimensionalidade fornecidos pelo indicador:

```cs
/// <inheritdoc />
public override IndicatorMeasures Measure => IndicatorMeasures.Percent;
```

Tipos de medição disponíveis:
- `IndicatorMeasures.Price` - o indicador mede preço (por exemplo, médias móveis)
- `IndicatorMeasures.Percent` - o indicador usa uma escala percentual de 0 a 100 (por exemplo, RSI)
- `IndicatorMeasures.MinusOnePlusOne` - o indicador usa uma escala de -1 a +1
- `IndicatorMeasures.Volume` - o indicador mede volume (por exemplo, OBV)

Esta propriedade é criticamente importante para apresentar corretamente indicadores num gráfico. Quando vários indicadores com dimensões diferentes são sobrepostos no mesmo painel, são criados eixos Y separados para indicadores com tipos `Measure` diferentes. Isto permite apresentar visualmente todos os indicadores na sua escala natural, mesmo que um tenha valores na ordem dos milhares (por exemplo, preço), enquanto outro é medido em frações de uma unidade (por exemplo, oscilador).

### Save e Load

Os métodos [Save](xref:StockSharp.Algo.Indicators.BaseIndicator.Save(Ecng.Serialization.SettingsStorage)) e [Load](xref:StockSharp.Algo.Indicators.BaseIndicator.Load(Ecng.Serialization.SettingsStorage)) são necessários para guardar e carregar definições de indicadores:

```cs
/// <inheritdoc />
public override void Save(SettingsStorage storage)
{
	base.Save(storage);

	storage.SetValue(nameof(ShortPeríodo), ShortPeríodo);
	storage.SetValue(nameof(LongPeríodo), LongPeríodo);
}

/// <inheritdoc />
public override void Load(SettingsStorage storage)
{
	base.Load(storage);

	ShortPeríodo = storage.GetValue<int>(nameof(ShortPeríodo));
	LongPeríodo = storage.GetValue<int>(nameof(LongPeríodo));
}
```

## Indicadores compostos

Alguns indicadores são compostos e usam outros indicadores nos seus cálculos. Por isso, os indicadores podem ser reutilizados entre si, como demonstrado no exemplo de implementação do indicador Chaikin Volatilidade [ChaikinVolatilidade](xref:StockSharp.Algo.Indicators.ChaikinVolatility):

```cs
/// <summary>
/// Volatilidade de Chaikin.
/// </summary>
[DisplayName("Volatilidade")]
[Description("Volatilidade de Chaikin.")]
public class ChaikinVolatilidade : BaseIndicator<IIndicatorValue>
{
	/// <summary>
	/// Cria <see cref="ChaikinVolatilidade"/>.
	/// </summary>
	public ChaikinVolatilidade()
	{
		Ema = new ExponentialMovingAverage();
		Roc = new RateOfChange();
	}

	/// <summary>
	/// Média móvel.
	/// </summary>
	[ExpandableObject]
	[DisplayName("MA")]
	[Description("Média móvel.")]
	[Category("Principal")]
	public ExponentialMovingAverage Ema { get; private set; }

	/// <summary>
	/// Taxa de mudança.
	/// </summary>
	[ExpandableObject]
	[DisplayName("ROC")]
	[Description("Taxa de variação.")]
	[Category("Principal")]
	public RateOfChange Roc { get; private set; }

	/// <summary>
	/// Se o indicador está formado.
	/// </summary>
	public override bool IsFormed
	{
		get { return Roc.IsFormed; }
	}

	/// <summary>
	/// Processar valor de entrada.
	/// </summary>
	/// <param name="input">Valor de entrada.</param>
	/// <returns>Valor resultante.</returns>
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		var candle = input.GetValue<Candle>();
		var emaValue = Ema.Process(input.SetValue(this, candle.HighPrice - candle.LowPrice));
		
		if (Ema.IsFormed)
		{
			return Roc.Process(emaValue);
		}
		
		return input;
	}
}
```

## Indicadores com várias linhas

O último tipo de indicadores são os que não só são compostos por outros indicadores, como também são apresentados graficamente com vários estados em simultâneo (várias linhas). Por exemplo, [AverageDirectionalIndex](xref:StockSharp.Algo.Indicators.AverageDirectionalIndex):

```cs
/// <summary>
/// Índice direcional médio de Welles Wilder.
/// </summary>
[DisplayName("ADX")]
[Description("Índice direcional médio de Welles Wilder.")]
public class AverageDirectionalIndex : BaseComplexIndicator
{
	/// <summary>
	/// Cria <see cref="AverageDirectionalIndex"/>.
	/// </summary>
	public AverageDirectionalIndex()
		: this(new DirectionalIndex { Length = 14 }, new WilderMovingAverage { Length = 14 })
	{
	}

	/// <summary>
	/// Cria <see cref="AverageDirectionalIndex"/>.
	/// </summary>
	/// <param name="dx">Índice de Movimento Direcional de Welles Wilder.</param>
	/// <param name="movingAverage">Média móvel.</param>
	public AverageDirectionalIndex(DirectionalIndex dx, LengthIndicator<decimal> movingAverage)
	{
		if (dx == null)
			throw new ArgumentNullException(nameof(dx));
		if (movingAverage == null)
			throw new ArgumentNullException(nameof(movingAverage));
		
		InnerIndicators.Add(Dx = dx);
		InnerIndicators.Add(MovingAverage = movingAverage);
		Mode = ComplexIndicatorModes.Sequence;
	}

	/// <summary>
	/// Índice de movimento direcional de Welles Wilder.
	/// </summary>
	[Browsable(false)]
	public DirectionalIndex Dx { get; private set; }

	/// <summary>
	/// Média móvel.
	/// </summary>
	[Browsable(false)]
	public LengthIndicator<decimal> MovingAverage { get; private set; }

	/// <summary>
	/// Comprimento do período.
	/// </summary>
	[DisplayName("Período")]
	[Description("Período do indicador.")]
	[Category("Principal")]
	public virtual int Length
	{
		get { return MovingAverage.Length; }
		set
		{
			MovingAverage.Length = Dx.Length = value;
			Reset();
		}
	}
}
```

Esses indicadores devem herdar da classe [BaseComplexIndicator](xref:StockSharp.Algo.Indicators.BaseComplexIndicator) e passar os componentes do indicador para [BaseComplexIndicator.InnerIndicators](xref:StockSharp.Algo.Indicators.BaseComplexIndicator.InnerIndicators). Além disso, cada indicador complexo deve declarar o seu próprio tipo de valor derivado de `ComplexIndicatorValue`.

## Exemplo de indicador complexo com implementação de SaveLoad

Abaixo está um exemplo de implementação do Percentage Volume Oscillator (PVO), que demonstra a implementação de `NumValuesToInitialize`, `Measure` e dos métodos `Save` e `Load`:

```cs
/// <summary>
/// Percentage Volume Oscillator (PVO).
/// </summary>
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.PVOKey,
	Description = LocalizedStrings.PercentageVolumeOscillatorKey)]
[IndicatorIn(typeof(CandleIndicatorValue))]
[Doc("topics/api/indicators/list_of_indicators/percentage_volume_oscillator.html")]
[IndicatorOut(typeof(PercentageVolumeOscillatorValue))]
public class PercentageVolumeOscillator : BaseComplexIndicator<PercentageVolumeOscillatorValue>
{
	private readonly ExponentialMovingAverage _shortEma;
	private readonly ExponentialMovingAverage _longEma;

	/// <summary>
	/// Inicializa uma nova instância de <see cref="PercentageVolumeOscillator"/>.
	/// </summary>
	public PercentageVolumeOscillator()
		: this(new(), new())
	{
		ShortPeríodo = 12;
		LongPeríodo = 26;
	}

	/// <summary>
	/// Inicializa uma nova instância de <see cref="PercentageVolumeOscillator"/>.
	/// </summary>
	/// <param name="shortEma">MME de curto prazo.</param>
	/// <param name="longEma">MME de longo prazo.</param>
	public PercentageVolumeOscillator(ExponentialMovingAverage shortEma, ExponentialMovingAverage longEma)
		: base(shortEma, longEma)
	{
		_shortEma = shortEma;
		_longEma = longEma;
	}

	/// <summary>
	/// Período curto.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.ShortPeríodoKey,
		Description = LocalizedStrings.ShortMaDescKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public int ShortPeríodo
	{
		get => _shortEma.Length;
		set => _shortEma.Length = value;
	}

	/// <summary>
	/// Período longo.
	/// </summary>
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.LongPeríodoKey,
		Description = LocalizedStrings.LongMaDescKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public int LongPeríodo
	{
		get => _longEma.Length;
		set => _longEma.Length = value;
	}

	/// <inheritdoc />
	public override IndicatorMeasures Measure => IndicatorMeasures.Volume;

	/// <inheritdoc />
	public override int NumValuesToInitialize => _shortEma.NumValuesToInitialize.Max(_longEma.NumValuesToInitialize);

	/// <inheritdoc />
	protected override bool CalcIsFormed() => _shortEma.IsFormed && _longEma.IsFormed;

	/// <inheritdoc />
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		var volume = input.ToCandle().TotalVolume;

		var result = new PercentageVolumeOscillatorValue(this, input.Time);

		var shortValue = _shortEma.Process(input, volume);
		var longValue = _longEma.Process(input, volume);

		result.Add(_shortEma, shortValue);
		result.Add(_longEma, longValue);

		if (_longEma.IsFormed)
		{
			var den = longValue.ToDecimal();
			var pvo = den == 0 ? 0 : ((shortValue.ToDecimal() - den) / den) * 100;
			result.Add(this, new DecimalIndicatorValue(this, pvo, input.Time));
		}

		return result;
	}

	/// <inheritdoc />
	public override void Save(SettingsStorage storage)
	{
		base.Save(storage);

		storage.SetValue(nameof(ShortPeríodo), ShortPeríodo);
		storage.SetValue(nameof(LongPeríodo), LongPeríodo);
	}

	/// <inheritdoc />
	public override void Load(SettingsStorage storage)
	{
		base.Load(storage);

		ShortPeríodo = storage.GetValue<int>(nameof(ShortPeríodo));
		LongPeríodo = storage.GetValue<int>(nameof(LongPeríodo));
	}

	/// <inheritdoc />
	public override string ToString() => base.ToString() + $" S={ShortPeríodo},L={LongPeríodo}";

	/// <inheritdoc />
	protected override PercentageVolumeOscillatorValue CreateValue(DateTimeOffset time)
		=> new(this, time);
}
```

```cs
/// <summary>
/// Valor do indicador <see cref="PercentageVolumeOscillator"/>.
/// </summary>
public class PercentageVolumeOscillatorValue : ComplexIndicatorValue<PercentageVolumeOscillator>
{
	/// <summary>
	/// Inicializa uma nova instância da classe <see cref="PercentageVolumeOscillatorValue"/>.
	/// </summary>
	/// <param name="indicator">Indicator.</param>
	/// <param name="time">Value time.</param>
	public PercentageVolumeOscillatorValue(PercentageVolumeOscillator indicator, DateTimeOffset time)
			: base(indicator, time)
	{
	}
}
```

Este exemplo demonstra:
1. Implementação de `NumValuesToInitialize` para um indicador complexo
2. Especificação do tipo de medição através da propriedade `Measure`
3. Implementação de um tipo de valor dedicado para o indicador complexo
4. Implementações corretas dos métodos `Save` e `Load` para guardar e carregar parâmetros
5. Substituição de `ToString()` para uma apresentação conveniente da configuração do indicador
