# Creación de un indicador

La creación de un indicador propio en la [API](../../../../api.md) se describe en la sección [Indicador personalizado](../../../../api/indicators/custom_indicator.md). Estos indicadores son totalmente compatibles con **Designer**.

Para crear un indicador, en el panel **Esquema** debe seleccionar la carpeta **Indicadores**, hacer clic con el botón derecho y seleccionar **Añadir** en el menú contextual:

![Designer_Source_Code_Indicator_00](../../../../../images/designer_source_code_indicator_00.png)

El código del indicador tendrá este aspecto:

```fsharp
/// <summary>
/// Indicador de ejemplo que demuestra cómo guardar y cargar parámetros.
/// Cambia el precio de entrada en +20% o -20%.
///
/// Vea más ejemplos:
/// https://github.com/StockSharp/StockSharp/tree/master/Algo/Indicators
///
/// Documentación:
/// https://doc.stocksharp.com/topics/designer/strategies/using_code/fsharp/create_own_indicator.html
/// </summary>
type EmptyIndicator() as this =
	inherit BaseIndicator()

	// Campos internos
	let mutable changeValue = 20
	let mutable counter = 0
	let mutable isFormedValue = false

	/// <summary>
	/// Valor porcentual (+/-) usado para modificar el precio de entrada.
	/// </summary>
	member this.Change
		with get () = changeValue
		and set value =
			changeValue <- value
			this.Reset()

	/// <summary>
	/// Define si el indicador se ha formado (quedó listo para trading).
	/// </summary>
	override this.CalcIsFormed() = isFormedValue

	/// <summary>
	/// Restablece el indicador a su estado inicial.
	/// </summary>
	override this.Reset() =
		base.Reset()
		isFormedValue <- false
		counter <- 0

	/// <summary>
	/// Lógica principal para procesar valores de entrada.
	/// </summary>
	override this.OnProcess(input: IIndicatorValue) : IIndicatorValue =
		// cada décima llamada intenta devolver un valor "vacío"
		if RandomGen.GetInt(0, 10) = 0 then
			// el valor vacío aún contiene solo tiempo, sin datos reales
			DecimalIndicatorValue(this, input.Time)
		else
			// incrementar contador en cada llamada
			counter <- counter + 1

			// después de 5 entradas, el indicador se considera formado
			if counter = 5 then
				isFormedValue <- true

			let mutable value = input.ToDecimal()

			// cambio aleatorio por un factor de +/- Change%
			let randomFactor = decimal (RandomGen.GetInt(-changeValue, changeValue)) / 100m
			value <- value + (value * randomFactor)

			// devolver valor final del indicador
			let result = DecimalIndicatorValue(this, value, input.Time)
			// marcarlo aleatoriamente como final o no
			result.IsFinal <- RandomGen.GetBool()
			result

	/// <summary>
	/// Cargar configuración del indicador desde un <see cref="SettingsStorage"/> dado.
	/// </summary>
	override this.Load(storage: SettingsStorage) =
		base.Load(storage)
		this.Change <- storage.GetValue<int>(nameof(this.Change))

	/// <summary>
	/// Guardar configuración del indicador en un <see cref="SettingsStorage"/> dado.
	/// </summary>
	override this.Save(storage: SettingsStorage) =
		base.Save(storage)
		storage.SetValue(nameof(this.Change), this.Change)

	/// <summary>
	/// Representación de cadena que incluye el valor actual de <see cref="Change"/>.
	/// </summary>
	override this.ToString() =
		sprintf "Change: %d" this.Change

```

Este indicador recibe un valor entrante y realiza una desviación arbitraria según el valor establecido del parámetro **Cambio**.

La descripción de los métodos del indicador está disponible en la sección [Indicador personalizado](../../../../api/indicators/custom_indicator.md).

Para añadir el indicador creado al diagrama, debe usar el cubo [Indicador](../../using_visual_designer/elements/common/indicator.md) y especificar en él el indicador necesario:

![Designer_Source_Code_Indicator_01](../../../../../images/designer_source_code_indicator_01.png)

El parámetro **Cambio**, establecido previamente en el código del indicador, se muestra en el panel de propiedades.

> [!WARNING]
> Los indicadores creados con código F# no pueden usarse en estrategias creadas con código F#. Solo pueden usarse en estrategias creadas [a partir de cubos](../../using_visual_designer.md).
