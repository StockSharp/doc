# Indikator in C# erstellen

Das Erstellen eines eigenen Indikators in der [API](../../../../api.md) wird im Abschnitt [Benutzerdefinierter Indikator](../../../../api/indicators/custom_indicator.md) beschrieben. Solche Indikatoren sind vollständig mit **Designer** kompatibel.

Um einen Indikator zu erstellen, wählen Sie im Panel **Schema** den Ordner **Indikatoren** aus, klicken mit der rechten Maustaste und wählen im Kontextmenü **Hinzufügen**:

![Designer Quellcode-Indikator 00](../../../../../images/designer_source_code_indicator_00.png)

Der Indikatorcode sieht folgendermaßen aus:

```cs
/// <summary>
/// Beispielindikator, der das Speichern und Laden von Parametern demonstriert.
///
/// Ändert den Eingangspreis um +20 % oder -20 %.
///
/// Weitere Beispiele: https://github.com/StockSharp/StockSharp/tree/master/Algo/Indicators
///
/// Dokumentation: https://doc.stocksharp.com/topics/Designer_Creating_indicator_from_source_code.html
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
	// Der gebildete Indikator hat alle erforderlichen Eingaben erhalten, um für den Handel verfügbar zu sein.
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
		// Bei jedem 10. Aufruf versuchen, einen leeren Wert zurückzugeben.
		if (RandomGen.GetInt(0, 10) == 0)
			return new DecimalIndicatorValue(this);

		if (_counter++ == 5)
		{
			// Zum Beispiel benötigt unser Indikator 5 Eingaben, um gebildet zu werden.
			_isFormed = true;
		}

		var value = input.GetValue<decimal>();

		// Zufällige Änderung des aktuellen Werts um +20 % oder -20 %.

		value += value * RandomGen.GetInt(-Change, Change) / 100.0m;

		return new DecimalIndicatorValue(this, value)
		{
			// Der finale Wert bedeutet, dass sich dieser Wert für die angegebene Eingabe
			// nicht mehr ändert (zum Beispiel bei Kerzen, die sich mit dem letzten Preis ändern).
			IsFinal = RandomGen.GetBool()
		};
	}

	// Unsere Eigenschaften persistieren, um sie für weitere App-Neustarts zu speichern.

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

	public override string ToString() => $"Änderung: {Change}";
}
```

Dieser Indikator empfängt einen eingehenden Wert und erzeugt eine zufällige Abweichung auf Basis des Parameterwerts **Change**.

Die Beschreibung der Indikatormethoden finden Sie im Abschnitt [Benutzerdefinierter Indikator](../../../../api/indicators/custom_indicator.md).

Um den erstellten Indikator zum Diagramm hinzuzufügen, verwenden Sie den Würfel [Indikator](../../using_visual_designer/elements/common/indicator.md) und geben darin den erforderlichen Indikator an:

![Designer Quellcode-Indikator 01](../../../../../images/designer_source_code_indicator_01.png)

Der Parameter **Change**, der zuvor im Indikatorcode festgelegt wurde, wird im Eigenschaftenpanel angezeigt.

> [!WARNING]
> Indikatoren aus C#-Code können nicht in Strategien verwendet werden, die in C#-Code erstellt wurden. Sie können nur in Strategien verwendet werden, die [aus Würfeln](../../using_visual_designer.md) erstellt wurden.

