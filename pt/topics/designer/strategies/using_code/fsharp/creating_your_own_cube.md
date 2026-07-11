# Criar o seu próprio cubo

Tal como ao criar um [cubo a partir de um diagrama](../../using_visual_designer/composite_elements.md), pode criar o seu próprio cubo com base em código F#. Esse cubo será mais funcional do que um cubo criado a partir de um diagrama.

Para criar um cubo a partir de código, este deve ser criado na pasta **Elementos próprios**:

![Designer elemento de código-fonte 00](../../../../../images/designer_source_code_elem_00.png)

No exemplo apresentado abaixo, o cubo herda da classe [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) e tem o seguinte aspeto:

```fsharp
/// <summary>
/// Elemento de diagrama de exemplo que demonstra a utilização de sockets de entrada e saída.
///
/// Ver mais detalhes:
/// https://doc.stocksharp.com/topics/designer/strategies/using_code/fsharp/creating_your_own_cube.html
/// </summary>
type EmptyDiagramElement() as this =
	inherit DiagramExternalElement()

	// Propriedade de exemplo que mostra como criar parâmetros
	let minValueParam =
		this.AddParam<int>("MinValue", 10)
			.SetBasic(true)  // tornar o parâmetro visível no modo básico
			.SetDisplay("Parâmetros", "Valor mínimo", "Descrição do parâmetro de valor mínimo", 10)

	// Sockets de saída são eventos marcados com o atributo DiagramExternal
	let output1Event = new Event<Unit>()
	let output2Event = new Event<Unit>()

	[<CLIEvent>]
	[<DiagramExternal>]
	member this.Output1 = output1Event.Publish

	[<CLIEvent>]
	[<DiagramExternal>]
	member this.Output2 = output2Event.Publish

	// Descomente a propriedade seguinte se quiser que o método Process
	// seja chamado sempre que for recebido um novo argumento
	// (não é necessário esperar que todos os argumentos de entrada sejam recebidos).
	//
	// sobrescrever this.WaitAllInput
	//     with get () = false

	// Sockets de entrada são parâmetros de métodos marcados com o atributo DiagramExternal

	[<DiagramExternal>]
	member this.Process(candle: CandleMessage, diff: Unit) =
		let res = candle.ClosePrice + diff

		if diff >= minValueParam.Value then
			// Disparar o primeiro evento de saída
			output1Event.Trigger(res)
		else
			// Disparar o segundo evento de saída
			output2Event.Trigger(res)

	override this.Start() =
		base.Start()
		// Adicionar lógica antes do arranque, se necessário

	override this.Stop() =
		base.Stop()
		// Adicionar lógica depois da paragem, se necessário

	override this.Reset() =
		base.Reset()
		// Adicionar lógica para repor o estado interno, se necessário
```

Neste código, o cubo tem dois sockets de entrada e dois sockets de saída. Os sockets de entrada são definidos aplicando o atributo [DiagramExternalAttribute](xref:StockSharp.Diagram.DiagramExternalAttribute) ao método:

```fsharp
[<DiagramExternal>]
member this.Process(candle: CandleMessage, diff: Unit) =
```

Os sockets de saída são definidos aplicando o atributo a um evento. No exemplo do cubo, existem dois eventos deste tipo:


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

Por isso, existirão também dois sockets de saída.

Além disso, é mostrado como criar uma propriedade para o cubo:

```fsharp
let minValueParam =
	this.AddParam<int>("MinValue", 10)
		.SetBasic(true)  // tornar o parâmetro visível no modo básico
		.SetDisplay("Parâmetros", "Valor mínimo", "Descrição do parâmetro de valor mínimo", 10)
```

A utilização da classe [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) aplica automaticamente a abordagem de guardar e restaurar definições.

A propriedade **Valor mínimo** está marcada como básica e ficará visível no modo [Propriedades básicas](../../using_visual_designer/diagram_panel.md).

A propriedade comentada [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) é responsável pelo momento da chamada do método com sockets de entrada:

```fsharp
// sobrescrever this.WaitAllInput
//     with get () = false
```

Se for descomentada, o método **Processo** será sempre chamado assim que pelo menos um valor chegar (no caso do exemplo, uma candle ou um valor numérico).

Para adicionar o cubo resultante ao diagrama, é necessário selecionar o cubo criado na paleta, na secção **Elementos próprios**:

![Designer elemento de código-fonte 01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING]
> Cubos em código F# não podem ser usados em estratégias criadas em código F#. Só podem ser usados em estratégias criadas [a partir de cubos](../../using_visual_designer.md).

## Ver também

[Criar um indicador](create_own_indicator.md)
