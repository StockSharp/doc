# Eigenen Würfel erstellen

Ähnlich wie beim Erstellen eines [Würfels aus einem Diagramm](../../using_visual_designer/composite_elements.md) können Sie einen eigenen Würfel auf Basis von C#-Code erstellen. Ein solcher Würfel ist funktionaler als ein Würfel aus einem Diagramm.

Um einen Würfel aus Code zu erstellen, muss er im Ordner **Eigene Elemente** erstellt werden:

![Designer Quellcode-Element 00](../../../../../images/designer_source_code_elem_00.png)

Im folgenden Beispiel erbt der Würfel von der Klasse [DiagramExternalElement](xref:StockSharp.Diagram.DiagramExternalElement) und sieht so aus:

```cs
/// <summary>
/// Beispiel-Diagrammelement, das die Verwendung von Eingabe- und Ausgabeanschlüssen demonstriert.
///
/// https://doc.stocksharp.com/topics/Designer_Combine_Source_code_and_standard_elements.html
/// </summary>
public class EmptyDiagramElement : DiagramExternalElement
{
	private readonly DiagramElementParam<int> _minValue;

	public EmptyDiagramElement()
	{
		// Beispiel-Eigenschaft, um zu zeigen, wie Parameter erstellt werden.

		_minValue = AddParam("MinValue", 10)
			.SetBasic(true) // Parameter im Basismodus sichtbar machen
			.SetDisplay("Parameter", "Mindestwert", "Beschreibung des Mindestwert-Parameter", 10);
	}

	// Ausgabeanschlüsse sind Ereignisse, die mit dem Attribut DiagramExternal markiert sind.

	[DiagramExternal]
	public event Action<Unit> Output1;

	[DiagramExternal]
	public event Action<Unit> Output2;

	// Eingabeanschlüsse sind Methodenparameter, die mit dem Attribut DiagramExternal markiert sind.

	// Auskommentierung entfernen, damit die Process-Methode jedes Mal aufgerufen wird,
	// wenn ein neues Argument empfangen wurde.
	// Es muss nicht gewartet werden, bis alle Eingabeargumente empfangen wurden.
	//public override bool WaitAllInput => false;

	[DiagramExternal]
	public void Process(CandleMessage candle, Unit diff)
	{
		var res = candle.ClosePrice + diff;

		if (diff >= _minValue.Value)
			Output1?.Invoke(res);
		else
			Output2?.Invoke(res);
	}

	public override void Start()
	{
		base.Start();

		// Logik vor dem Start hinzufügen
	}

	public override void Stop()
	{
		base.Stop();

		// Logik nach dem Stop hinzufügen
	}

	public override void Reset()
	{
		base.Reset();

		// Logik zum Zurücksetzen des internen Zustands hinzufügen
	}
}
```

In diesem Code hat der Würfel zwei eingehende und zwei ausgehende Anschlüsse. Eingehende Anschlüsse werden definiert, indem das Attribut [DiagramExternalAttribute](xref:StockSharp.Diagram.DiagramExternalAttribute) auf die Methode angewendet wird:

```cs
[DiagramExternal]
public void Process(CandleMessage candle, Unit diff)
```

Ausgehende Anschlüsse werden definiert, indem das Attribut auf ein Ereignis angewendet wird. Im Beispiel des Würfels gibt es zwei solche Ereignisse:


```cs
[DiagramExternal]
public event Action<Unit> Output1;

[DiagramExternal]
public event Action<Unit> Output2;
```

Daher gibt es auch zwei ausgehende Anschlüsse.

Zusätzlich zeigt das Beispiel, wie eine Eigenschaft für den Würfel erstellt wird:

```cs
_minValue = AddParam("MinValue", 10)
	.SetBasic(true) // Parameter im Basismodus sichtbar machen
	.SetDisplay("Parameter", "Mindestwert", "Beschreibung des Mindestwert-Parameter", 10);
```

Die Verwendung der Klasse [DiagramElementParam](xref:StockSharp.Diagram.DiagramElementParam`1) nutzt automatisch den Ansatz zum Speichern und Wiederherstellen von Einstellungen.

Die Eigenschaft **MinValue** ist als basic markiert und wird im Modus [Grundeigenschaften](../../using_visual_designer/diagram_panel.md) sichtbar sein.

Die auskommentierte Eigenschaft [WaitAllInput](xref:StockSharp.Diagram.DiagramExternalElement.WaitAllInput) ist für den Zeitpunkt des Methodenaufrufs mit eingehenden Anschlüssen zuständig:

```cs
//public override bool WaitAllInput => false;
```

Wenn die Auskommentierung entfernt wird, wird die Methode **Process** immer aufgerufen, sobald mindestens ein Wert eintrifft. Im Beispiel ist dies entweder eine Kerze oder ein numerischer Wert.

Um den resultierenden Würfel zum Diagramm hinzuzufügen, wählen Sie den erstellten Würfel in der Palette im Abschnitt **Eigene Elemente** aus:

![Designer Quellcode-Element 01](../../../../../images/designer_source_code_elem_01.png)

> [!WARNING]
> Würfel aus C#-Code können nicht in Strategien verwendet werden, die in C#-Code erstellt wurden. Sie können nur in Strategien verwendet werden, die [aus Würfeln](../../using_visual_designer.md) erstellt wurden.

## Siehe auch

[Indikator erstellen](create_own_indicator.md)

