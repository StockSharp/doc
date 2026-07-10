# Parâmetros de Estratégia

Para configuração e otimização de estratégias, StockSharp fornece uma classe especial [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1). Os parâmetros de estratégia permitem alterar definições do algoritmo de negociação sem modificar o código, o que é especialmente conveniente ao alternar entre modos de teste e de negociação real. Além disso, estes parâmetros são usados durante a otimização para iterar automaticamente por valores e encontrar definições ótimas da estratégia.

Ao contrário das propriedades C# normais, os parâmetros criados com esta classe são apresentados automaticamente nas definições visuais (por exemplo, no Designer) e podem ser usados para otimização da estratégia.

## Criar Parâmetros de Estratégia

Os parâmetros são criados no construtor da estratégia usando o método [Strategy.Param](xref:StockSharp.Algo.Strategies.Strategy.Param``1(System.String,``0)):

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetGreaterThanZero()
							.SetDisplay("Período da SMA longa", string.Empty, "Configurações básicas");
	}
}
```

Neste exemplo, é criado um parâmetro `LongSmaLength` com valor inicial 80, é definido um validador para garantir que o valor é maior que zero e são configuradas definições de apresentação para a interface de utilizador.

## Métodos de Configuração de Parâmetros

A classe [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) fornece vários métodos para configuração de parâmetros:

### SetDisplay

O método [StrategyParam\<T\>.SetDisplay](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetDisplay(System.String,System.String,System.String)) define o nome de apresentação, a descrição e a categoria do parâmetro:

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetDisplay("Período da SMA longa", "Período da média móvel longa", "Configurações básicas");
```

### SetValidator

O método [StrategyParam\<T\>.SetValidator](xref:Ecng.ComponentModel.Extensions.SetValidator``1(``0,System.ComponentModel.DataAnnotations.ValidationAttribute)) define um validador para verificar o valor do parâmetro. StockSharp fornece um conjunto de validadores predefinidos que podem ser usados para as tarefas mais comuns:

```cs
// Verificar se o número é maior que zero
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetValidator(new IntGreaterThanZeroAttribute());

// Verificar se o número não é negativo
_volume = Param(nameof(Volume), 1)
			.SetValidator(new DecimalNotNegativeAttribute());

// Verificar o intervalo de valores
_percentage = Param(nameof(Percentage), 50)
				.SetValidator(new RangeAttribute(0, 100));

// Verificar o valor obrigatório
_security = Param<Security>(nameof(Security))
				.SetValidator(new RequiredAttribute());
```

Por conveniência, [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) tem métodos integrados para os validadores mais comuns:

```cs
// Verificar se o número é maior que zero
_longSmaLength = Param(nameof(LongSmaLength), 80).SetGreaterThanZero();

// Verificar se o número não é negativo
_volume = Param(nameof(Volume), 1).SetNotNegative();

// Verificar se o valor é NULL ou não negativo
_interval = Param<TimeSpan?>(nameof(Interval)).SetNullOrNotNegative();

// Definir o intervalo de valores
_percentage = Param(nameof(Percentage), 50).SetRange(0, 100);
```

Se os validadores integrados não forem suficientes, pode criar os seus próprios herdando de [ValidationAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationattribute):

```cs
public class EvenNumberAttribute : ValidationAttribute
{
	public EvenNumberAttribute()
		: base("Value must be an even number.")
	{
	}

	public override bool IsValid(object value)
	{
		if (value is int intValue)
			return intValue % 2 == 0;
		
		return false;
	}
}

// Usar validador personalizado
_barCount = Param(nameof(BarCount), 10)
				.SetValidator(new EvenNumberAttribute());
```

### SetHidden

O método [StrategyParam\<T\>.SetHidden](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetHidden(System.Boolean)) oculta o parâmetro no editor de propriedades:

```cs
_systemParam = Param(nameof(SystemParam), "value")
				.SetHidden(true);
```

### SetBasic

O método [StrategyParam\<T\>.SetBasic](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetBasic(System.Boolean)) marca o parâmetro como básico, o que afeta a sua apresentação na interface de utilizador. Os parâmetros básicos são apresentados no modo simplificado do editor de propriedades:

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetBasic(true);
```

![strategy parameters basic advanced](../../../images/strategy_parameters_basic_advanced.png)

### SetReadOnly

O método [StrategyParam\<T\>.SetReadOnly](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetReadOnly(System.Boolean)) torna o parâmetro só de leitura:

```cs
_calculatedParam = Param(nameof(CalculatedParam), 0)
					.SetReadOnly(true);
```

### SetCanOptimize e SetOptimize

Os métodos [StrategyParam\<T\>.SetCanOptimize](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetCanOptimize(System.Boolean)) e [StrategyParam\<T\>.SetOptimize](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetOptimize(`0,`0,`0)) especificam se o parâmetro pode ser usado para otimização e definem o intervalo de valores para otimização:

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetCanOptimize(true)
					.SetOptimize(10, 200, 10);
```

No exemplo acima, o parâmetro será otimizado no intervalo de 10 a 200 com um passo de 10.

## Usar Parâmetros na Estratégia

Os parâmetros de estratégia são usados como propriedades normais:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };
	
	// ...
}
```

## Guardar e Carregar Parâmetros

Os valores dos parâmetros são automaticamente guardados e carregados na classe base [Strategy](xref:StockSharp.Algo.Strategies.Strategy). Ao substituir os métodos [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) e [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)), deve chamar os métodos da classe base:

```cs
public override void Save(SettingsStorage settings)
{
	base.Save(settings);
	
	// Lógica adicional de salvamento...
}

public override void Load(SettingsStorage settings)
{
	base.Load(settings);
	
	// Lógica adicional de carregamento...
}
```

## Exemplo: Estratégia com Vários Parâmetros

Abaixo está um exemplo de uma estratégia com vários parâmetros:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	public DataType Série
	{
		get => _series.Value;
		set => _series.Value = value;
	}

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public int ShortSmaLength
	{
		get => _shortSmaLength.Value;
		set => _shortSmaLength.Value = value;
	}

	public SmaStrategy()
	{
		base.Name = "SMA strategy";

		Param("TypeId", GetType().GetTypeName(false)).SetHidden();
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetGreaterThanZero()
							.SetDisplay("Período da SMA longa", string.Empty, "Configurações básicas")
							.SetCanOptimize(true)
							.SetOptimize(20, 200, 10);
		
		_shortSmaLength = Param(nameof(ShortSmaLength), 30)
							.SetGreaterThanZero()
							.SetDisplay("Período da SMA curta", string.Empty, "Configurações básicas")
							.SetCanOptimize(true)
							.SetOptimize(5, 50, 5);
		
		_series = Param(nameof(Série), DataType.TimeFrame(TimeSpan.FromMinutes(15)))
					.SetDisplay("Série", string.Empty, "Configurações básicas");
	}

	// ...
}
```

Neste exemplo, criámos uma estratégia baseada no cruzamento de duas médias móveis com três parâmetros configuráveis:
- `Série` - tipo de dados e período
- `LongSmaLength` - período da média móvel longa
- `ShortSmaLength` - período da média móvel curta

Para os dois parâmetros numéricos, configurámos capacidades de otimização com intervalos especificados.

## Ver Também

[Guardar e Carregar Definições](settings_saving_and_loading.md)
