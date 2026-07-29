# Parámetros de estrategia

Para la configuración y optimización de estrategias, StockSharp proporciona una clase especial [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1). Los parámetros de estrategia permiten modificar los ajustes del algoritmo de negociación sin cambiar el código, lo que resulta especialmente cómodo al cambiar entre modos de prueba y negociación real. Además, estos parámetros se usan durante la optimización para recorrer valores automáticamente y encontrar los ajustes óptimos de la estrategia.

A diferencia de las propiedades C# habituales, los parámetros creados con esta clase se muestran automáticamente en los ajustes visuales (por ejemplo, en Designer) y se pueden usar para la optimización de estrategias.

## Crear parámetros de estrategia

Los parámetros se crean en el constructor de la estrategia mediante el método [Strategy.Param](xref:StockSharp.Algo.Strategies.Strategy.Param``1(System.String,``0)):

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
							.SetDisplay("Longitud de la SMA larga", string.Empty, "Configuración básica");
	}
}
```

En este ejemplo, se crea un parámetro `LongSmaLength` con un valor inicial de 80, se establece un validador para garantizar que el valor sea mayor que cero y se configuran ajustes de visualización para la interfaz de usuario.

## Métodos de configuración de parámetros

La clase [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) proporciona varios métodos para configurar parámetros:

### SetDisplay

El método [StrategyParam\<T\>.SetDisplay](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetDisplay(System.String,System.String,System.String)) establece el nombre de visualización, la descripción y la categoría del parámetro:

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetDisplay("Longitud de la SMA larga", "Período de la media móvil larga", "Configuración básica");
```

### SetValidator

El método [StrategyParam\<T\>.SetValidator](xref:Ecng.ComponentModel.Extensions.SetValidator``1(``0,System.ComponentModel.DataAnnotations.ValidationAttribute)) establece un validador para comprobar el valor del parámetro. StockSharp proporciona una serie de validadores predefinidos que se pueden usar para las tareas más comunes:

```cs
// Comprobar que el número sea mayor que cero
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetValidator(new IntGreaterThanZeroAttribute());

// Comprobar que el número no sea negativo
_volume = Param(nameof(Volume), 1)
			.SetValidator(new DecimalNotNegativeAttribute());

// Comprobar rango de valores
_percentage = Param(nameof(Percentage), 50)
				.SetValidator(new RangeAttribute(0, 100));

// Comprobar valor obligatorio
_security = Param<Security>(nameof(Security))
				.SetValidator(new RequiredAttribute());
```

Por comodidad, [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) tiene métodos integrados para los validadores más comunes:

```cs
// Comprobar que el número sea mayor que cero
_longSmaLength = Param(nameof(LongSmaLength), 80).SetGreaterThanZero();

// Comprobar que el número no sea negativo
_volume = Param(nameof(Volume), 1).SetNotNegative();

// Comprobar que el valor sea NULL o no negativo
_interval = Param<TimeSpan?>(nameof(Interval)).SetNullOrNotNegative();

// Establecer rango de valores
_percentage = Param(nameof(Percentage), 50).SetRange(0, 100);
```

Si los validadores integrados no son suficientes, puede crear uno propio heredando de [ValidationAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationattribute):

```cs
public class EvenNumberAttribute : ValidationAttribute
{
	public EvenNumberAttribute()
		: base("El valor debe ser un número par.")
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

El método [StrategyParam\<T\>.SetHidden](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetHidden(System.Boolean)) oculta el parámetro en el editor de propiedades:

```cs
_systemParam = Param(nameof(SystemParam), "value")
				.SetHidden(true);
```

### SetBasic

El método [StrategyParam\<T\>.SetBasic](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetBasic(System.Boolean)) marca el parámetro como básico, lo que afecta a su visualización en la interfaz de usuario. Los parámetros básicos se muestran en el modo simplificado del editor de propiedades:

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetBasic(true);
```

![estrategia parámetros básicos y avanzados](../../../images/strategy_parameters_basic_advanced.png)

### SetReadOnly

El método [StrategyParam\<T\>.SetReadOnly](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetReadOnly(System.Boolean)) hace que el parámetro sea de solo lectura:

```cs
_calculatedParam = Param(nameof(CalculatedParam), 0)
					.SetReadOnly(true);
```

### SetCanOptimize y SetOptimize

Los métodos [StrategyParam\<T\>.SetCanOptimize](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetCanOptimize(System.Boolean)) y [StrategyParam\<T\>.SetOptimize](xref:StockSharp.Algo.Strategies.StrategyParam`1.SetOptimize(`0,`0,`0)) especifican si el parámetro se puede usar para optimización y establecen el rango de valores para la optimización:

```cs
_longSmaLength = Param(nameof(LongSmaLength), 80)
					.SetCanOptimize(true)
					.SetOptimize(10, 200, 10);
```

En el ejemplo anterior, el parámetro se optimizará en el rango de 10 a 200 con un paso de 10.

## Uso de parámetros en una estrategia

Los parámetros de estrategia se usan como propiedades habituales:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };

	// ...
}
```

## Guardado y carga de parámetros

Los valores de los parámetros se guardan y cargan automáticamente en la clase base [Strategy](xref:StockSharp.Algo.Strategies.Strategy). Al sobrescribir los métodos [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) y [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)), debe llamar a los métodos de la clase base:

```cs
public override void Save(SettingsStorage settings)
{
	base.Save(settings);

	// Lógica adicional de guardado...
}

public override void Load(SettingsStorage settings)
{
	base.Load(settings);

	// Lógica adicional de carga...
}
```

## Ejemplo: estrategia con múltiples parámetros

A continuación se muestra un ejemplo de una estrategia con múltiples parámetros:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	public DataType Series
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
		base.Name = "Estrategia SMA";

		Param("TypeId", GetType().GetTypeName(false)).SetHidden();
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetGreaterThanZero()
							.SetDisplay("Longitud de la SMA larga", string.Empty, "Configuración básica")
							.SetCanOptimize(true)
							.SetOptimize(20, 200, 10);

		_shortSmaLength = Param(nameof(ShortSmaLength), 30)
							.SetGreaterThanZero()
							.SetDisplay("Longitud de la SMA corta", string.Empty, "Configuración básica")
							.SetCanOptimize(true)
							.SetOptimize(5, 50, 5);

		_series = Param(nameof(Series), DataType.TimeFrame(TimeSpan.FromMinutes(15)))
					.SetDisplay("Serie", string.Empty, "Configuración básica");
	}

	// ...
}
```

En este ejemplo, creamos una estrategia basada en el cruce de dos medias móviles con tres parámetros configurables:
- `Series` - tipo de datos y marco temporal
- `LongSmaLength` - periodo de la media móvil larga
- `ShortSmaLength` - periodo de la media móvil corta

Para los dos parámetros numéricos, configuramos capacidades de optimización con rangos especificados.

## Ver también

[Guardado y carga de ajustes](settings_saving_and_loading.md)
