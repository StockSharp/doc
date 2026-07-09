# Creación de un indicador en C#

La creación de un indicador propio en la [API](../../../../api.md) se describe en la sección [Indicador personalizado](../../../../api/indicators/custom_indicator.md). Estos indicadores son totalmente compatibles con **Designer**.

Para crear un indicador, en el panel **Esquema** debe seleccionar la carpeta **Indicadores**, hacer clic con el botón derecho y seleccionar **Añadir** en el menú contextual:

![Designer_Source_Code_Indicator_00](../../../../../images/designer_source_code_indicator_00.png)

El código del indicador tendrá este aspecto:

```cs
/// <summary>
/// Indicador de ejemplo que demuestra cómo guardar y cargar parámetros.
/// 
/// Cambia el precio de entrada en +20% o -20%.
/// 
/// Vea más ejemplos https://github.com/StockSharp/StockSharp/tree/master/Algo/Indicators
/// 
/// Doc https://doc.stocksharp.com/topics/Designer_Creating_indicator_from_source_code.html
/// </summary>
public class EmptyIndicator : BaseIndicator
{
	private int _change = 20;

	public int Change
	{
		get => _change;
		set
		{
			_change = value;
			Reset();
		}
	}

	private int _counter;
	// el indicador formado recibió todas las entradas necesarias para estar disponible para trading
	private bool _isFormed;

	protected override bool CalcIsFormed() => _isFormed;

	public override void Reset()
	{
		base.Reset();

		_isFormed = default;
		_counter = default;
	}

	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		// cada décima llamada intenta devolver un valor vacío
		if (RandomGen.GetInt(0, 10) == 0)
			return new DecimalIndicatorValue(this);

		if (_counter++ == 5)
		{
			// por ejemplo, nuestro indicador necesita 5 entradas para quedar formado
			_isFormed = true;
		}

		var value = input.GetValue<decimal>();

		// cambio aleatorio de +20% o -20% del valor actual

		value += value * RandomGen.GetInt(-Change, Change) / 100.0m;

		return new DecimalIndicatorValue(this, value)
		{
			// el valor final significa que este valor para la entrada especificada
			// ya no cambia (por ejemplo, para velas que cambian con el último precio)
			IsFinal = RandomGen.GetBool()
		};
	}

	// persistir nuestras propiedades para guardarlas para futuros reinicios de la aplicación

	public override void Load(SettingsStorage storage)
	{
		base.Load(storage);
		Change = storage.GetValue<int>(nameof(Change));
	}

	public override void Save(SettingsStorage storage)
	{
		base.Save(storage);
		storage.SetValue(nameof(Change), Change);
	}

	public override string ToString() => $"Change: {Change}";
}
```

Este indicador recibe un valor entrante y realiza una desviación arbitraria basada en el valor del parámetro **Change**.

La descripción de los métodos del indicador está disponible en la sección [Indicador personalizado](../../../../api/indicators/custom_indicator.md).

Para añadir el indicador creado al diagrama, debe usar el cubo [Indicador](../../using_visual_designer/elements/common/indicator.md) y especificar en él el indicador necesario:

![Designer_Source_Code_Indicator_01](../../../../../images/designer_source_code_indicator_01.png)

El parámetro **Change**, establecido previamente en el código del indicador, se muestra en el panel de propiedades.

> [!WARNING] 
> Los indicadores creados con código C# no pueden usarse en estrategias creadas con código C#. Solo pueden usarse en estrategias creadas [a partir de cubos](../../using_visual_designer.md).
