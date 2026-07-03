# Crear su propio cubo

De forma similar a la creación de un [cubo a partir de un diagrama](../../using_visual_designer/composite_elements.md), puede crear su propio cubo basado en código F#. Este cubo será más funcional que un cubo creado a partir de un diagrama.

Para crear un cubo desde código, debe crearse en la carpeta **Own elements**:

![Designer_Source_Code_Elem_00](../../../../../images/designer_source_code_elem_00.png)

En el ejemplo que se muestra a continuación, el cubo hereda de la clase [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) y tiene este aspecto:

```fsharp
/// <summary>
/// Elemento de diagrama de ejemplo que demuestra el uso de sockets de entrada y salida.
///
/// Vea más detalles:
/// https://doc.stocksharp.com/topics/designer/strategies/using_code/fsharp/creating_your_own_cube.html
/// </summary>
type EmptyDiagramElement() as this =
	inherit DiagramExternalElement()

	// Propiedad de ejemplo que muestra cómo crear parámetros
	let minValueParam =
		this.AddParam<int>("MinValue", 10)
			.SetBasic(true)  // hacer visible el parámetro en modo básico
			.SetDisplay("Parameters", "Min value", "Min value parameter description", 10)

	// Los sockets de salida son eventos marcados con el atributo DiagramExternal
	let output1Event = new Event<Unit>()
	let output2Event = new Event<Unit>()

	[<CLIEvent>]
	[<DiagramExternal>]
	member this.Output1 = output1Event.Publish

	[<CLIEvent>]
	[<DiagramExternal>]
	member this.Output2 = output2Event.Publish

	// Descomente la siguiente propiedad si desea que el método Process
	// se llame cada vez que se recibe un nuevo argumento
	// (no es necesario esperar a recibir todos los argumentos de entrada).
	//
	// override this.WaitAllInput 
	//     with get () = false

	// Los sockets de entrada son parámetros de método marcados con el atributo DiagramExternal

	[<DiagramExternal>]
	member this.Process(candle: CandleMessage, diff: Unit) =
		let res = candle.ClosePrice + diff

		if diff >= minValueParam.Value then
			// Activar el primer evento de salida
			output1Event.Trigger(res)
		else
			// Activar el segundo evento de salida
			output2Event.Trigger(res)

	override this.Start() =
		base.Start()
		// Añadir lógica antes del inicio si es necesario

	override this.Stop() =
		base.Stop()
		// Añadir lógica después de detener si es necesario

	override this.Reset() =
		base.Reset()
		// Añadir lógica para restablecer el estado interno si es necesario
```

En este código, el cubo tiene dos sockets de entrada y dos sockets de salida. Los sockets de entrada se definen aplicando el atributo [DiagramExternalAttribute](xref:StockSharp.Diagram.DiagramExternalAttribute) al método:

```fsharp
[<DiagramExternal>]
member this.Process(candle: CandleMessage, diff: Unit) =
```

Los sockets de salida se definen aplicando el atributo a un evento. En el ejemplo del cubo hay dos eventos de este tipo:


```fsharp
let output1Event = new Event<Unit>()
let output2Event = new Event<Unit>()

[<CLIEvent>]
[<DiagramExternal>]
member this.Output1 = output1Event.Publish

[<CLIEvent>]
[<DiagramExternal>]
member this.Output2 = output2Event.Publish
```

Por lo tanto, también habrá dos sockets de salida.

Además, se muestra cómo crear una propiedad para el cubo:

```fsharp
let minValueParam =
	this.AddParam<int>("MinValue", 10)
		.SetBasic(true)  // hacer visible el parámetro en modo básico
		.SetDisplay("Parameters", "Min value", "Min value parameter description", 10)
```

El uso de la clase [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) aplica automáticamente el enfoque para guardar y restaurar la configuración.

La propiedad **MinValue** está marcada como básica y será visible en el modo de [propiedades básicas](../../using_visual_designer/diagram_panel.md).

La propiedad comentada [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) es responsable del momento de llamada del método con sockets de entrada:

```fsharp
// override this.WaitAllInput 
//     with get () = false
```

Si se descomenta, el método **Process** se llamará siempre en cuanto llegue al menos un valor (en el ejemplo, una vela o un valor numérico).

Para añadir el cubo resultante al diagrama, debe seleccionar el cubo creado en la paleta, en la sección **Own elements**:

![Designer_Source_Code_Elem_01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING] 
> Los cubos creados con código F# no pueden usarse en estrategias creadas con código F#. Solo pueden usarse en estrategias creadas [a partir de cubos](../../using_visual_designer.md).

## Véase también

[Creación de un indicador](create_own_indicator.md)
