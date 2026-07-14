# Eigenen Würfel erstellen

Ähnlich wie beim Erstellen eines [Würfels aus einem Diagramm](../../using_visual_designer/composite_elements.md) können Sie einen eigenen Würfel auf Basis von F#-Code erstellen. Ein solcher Würfel ist funktionaler als ein Würfel aus einem Diagramm.

Um einen Würfel aus Code zu erstellen, muss er im Ordner **Eigene Elemente** erstellt werden:

![Designer Quellcode-Element 00](../../../../../images/designer_source_code_elem_00.png)

Im folgenden Beispiel erbt der Würfel von der Klasse [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) und sieht so aus:

```fsharp
/// <summary>
/// Beispiel-Diagrammelement, das die Verwendung von Eingabe- und Ausgabeanschlüssen demonstriert.
///
/// Weitere Details:
/// https://doc.stocksharp.com/topics/designer/strategies/using_code/fsharp/creating_your_own_cube.html
/// </summary>
type EmptyDiagramElement() as this =
	inherit DiagramExternalElement()

	// Beispieleigenschaft, die zeigt, wie Parameter erstellt werden
	let minValueParam =
		this.AddParam<int>("MinValue", 10)
			.SetBasic(true)  // Parameter im Basismodus sichtbar machen
			.SetDisplay("Parameter", "Mindestwert", "Beschreibung des Mindestwert-Parameter", 10)

	// Ausgabeanschlüsse sind Ereignisse, die mit dem Attribut DiagramExternal markiert sind
	let output1Event = new Event<Unit>()
	let output2Event = new Event<Unit>()

	[<CLIEvent>]
	[<DiagramExternal>]
	member this.Output1 = output1Event.Publish

	[<CLIEvent>]
	[<DiagramExternal>]
	member this.Output2 = output2Event.Publish

	// Entfernen Sie die Auskommentierung der folgenden Eigenschaft, wenn die Process-Methode
	// jedes Mal aufgerufen werden soll, wenn ein neues Argument empfangen wird
	// (es muss nicht gewartet werden, bis alle Eingabeargumente empfangen wurden).
	//
	// this.WaitAllInput überschreiben
	//     with get () = false

	// Eingabeanschlüsse sind Methodenparameter, die mit dem Attribut DiagramExternal markiert sind

	[<DiagramExternal>]
	member this.Process(candle: CandleMessage, diff: Unit) =
		let res = candle.ClosePrice + diff

		if diff >= minValueParam.Value then
			// Erstes Ausgabeereignis auslösen
			output1Event.Trigger(res)
		else
			// Zweites Ausgabeereignis auslösen
			output2Event.Trigger(res)

	override this.Start() =
		base.Start()
		// Bei Bedarf Logik vor dem Start hinzufügen

	override this.Stop() =
		base.Stop()
		// Bei Bedarf Logik nach dem Stop hinzufügen

	override this.Reset() =
		base.Reset()
		// Bei Bedarf Logik zum Zurücksetzen des internen Zustands hinzufügen
```

In diesem Code hat der Würfel zwei eingehende und zwei ausgehende Anschlüsse. Eingehende Anschlüsse werden definiert, indem das Attribut [DiagramExternalAttribute](xref:StockSharp.Diagram.DiagramExternalAttribute) auf die Methode angewendet wird:

```fsharp
[<DiagramExternal>]
member this.Process(candle: CandleMessage, diff: Unit) =
```

Ausgehende Anschlüsse werden definiert, indem das Attribut auf ein Ereignis angewendet wird. Im Beispiel des Würfels gibt es zwei solche Ereignisse:


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

Daher gibt es auch zwei ausgehende Anschlüsse.

Zusätzlich wird gezeigt, wie eine Eigenschaft für den Würfel erstellt wird:

```fsharp
let minValueParam =
	this.AddParam<int>("MinValue", 10)
		.SetBasic(true)  // Parameter im Basismodus sichtbar machen
		.SetDisplay("Parameter", "Mindestwert", "Beschreibung des Mindestwert-Parameter", 10)
```

Bei Verwendung der Klasse [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) wird automatisch der Ansatz zum Speichern und Wiederherstellen von Einstellungen verwendet.

Die Eigenschaft **MinValue** ist als basic markiert und wird im Modus [Grundeigenschaften](../../using_visual_designer/diagram_panel.md) sichtbar sein.

Die auskommentierte Eigenschaft [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) ist für den Zeitpunkt des Methodenaufrufs mit eingehenden Anschlüssen zuständig:

```fsharp
// this.WaitAllInput überschreiben
//     with get () = false
```

Wenn die Auskommentierung entfernt wird, wird die Methode **Process** immer aufgerufen, sobald mindestens ein Wert eintrifft (im Beispiel ist dies entweder eine Kerze oder ein numerischer Wert).

Um den resultierenden Würfel zum Diagramm hinzuzufügen, wählen Sie den erstellten Würfel in der Palette im Abschnitt **Eigene Elemente** aus:

![Designer Quellcode-Element 01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING]
> Würfel aus F#-Code können nicht in Strategien verwendet werden, die in F#-Code erstellt wurden. Sie können nur in Strategien verwendet werden, die [aus Würfeln](../../using_visual_designer.md) erstellt wurden.

## Siehe auch

[Indikator erstellen](create_own_indicator.md)

