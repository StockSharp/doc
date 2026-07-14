# Indikator erstellen

Das Erstellen eines eigenen Indikators in der [API](../../../../api.md) wird im Abschnitt [Benutzerdefinierter Indikator](../../../../api/indicators/custom_indicator.md) beschrieben. Solche Indikatoren sind vollständig mit **Designer** kompatibel.

Um einen Indikator zu erstellen, wählen Sie im Panel **Schema** den Ordner **Indikatoren** aus, klicken mit der rechten Maustaste und wählen im Kontextmenü **Hinzufügen**:

![Designer Quellcode-Indikator 00](../../../../../images/designer_source_code_indicator_00.png)

Der Indikatorcode sieht folgendermaßen aus:

```fsharp
/// <summary>
/// Beispielindikator, der das Speichern und Laden von Parametern demonstriert.
/// Ändert den Eingangspreis um +20 % oder -20 %.
///
/// Weitere Beispiele:
/// https://github.com/StockSharp/StockSharp/tree/master/Algo/Indicators
///
/// Dokumentation:
/// https://doc.stocksharp.com/topics/designer/strategies/using_code/fsharp/create_own_indicator.html
/// </summary>
type EmptyIndicator() as this =
	inherit BaseIndicator()

	// Interne Felder
	let mutable changeValue = 20
	let mutable counter = 0
	let mutable isFormedValue = false

	/// <summary>
	/// Der Prozentwert (+/-), mit dem der Eingangspreis geändert wird.
	/// </summary>
	member this.Change
		with get () = changeValue
		and set value =
			changeValue <- value
			this.Reset()

	/// <summary>
	/// Gibt an, ob der Indikator gebildet wurde (also für den Handel bereit ist).
	/// </summary>
	override this.CalcIsFormed() = isFormedValue

	/// <summary>
	/// Setzt den Indikator auf seinen Anfangszustand zurück.
	/// </summary>
	override this.Reset() =
		base.Reset()
		isFormedValue <- false
		counter <- 0

	/// <summary>
	/// Die Hauptlogik zur Verarbeitung von Eingangswerten.
	/// </summary>
	override this.OnProcess(input: IIndicatorValue) : IIndicatorValue =
		// Bei jedem 10. Aufruf versuchen, einen "leeren" Wert zurückzugeben
		if RandomGen.GetInt(0, 10) = 0 then
			// Ein leerer Wert enthält weiterhin nur die Zeit, aber keine eigentlichen Daten
			DecimalIndicatorValue(this, input.Time)
		else
			// Zähler bei jedem Aufruf erhöhen
			counter <- counter + 1

			// Nach 5 Eingaben gilt der Indikator als gebildet
			if counter = 5 then
				isFormedValue <- true

			let mutable value = input.ToDecimal()

			// Zufällige Änderung um einen Faktor von +/- Change %
			let randomFactor = decimal (RandomGen.GetInt(-changeValue, changeValue)) / 100m
			value <- value + (value * randomFactor)

			// Finalen Indikatorwert zurückgeben
			let result = DecimalIndicatorValue(this, value, input.Time)
			// Zufällig als final oder nicht final markieren
			result.IsFinal <- RandomGen.GetBool()
			result

	/// <summary>
	/// Indikatoreinstellungen aus einem angegebenen <see cref="SettingsStorage"/> laden.
	/// </summary>
	override this.Load(storage: SettingsStorage) =
		base.Load(storage)
		this.Change <- storage.GetValue<int>(nameof(this.Change))

	/// <summary>
	/// Indikatoreinstellungen in einem angegebenen <see cref="SettingsStorage"/> speichern.
	/// </summary>
	override this.Save(storage: SettingsStorage) =
		base.Save(storage)
		storage.SetValue(nameof(this.Change), this.Change)

	/// <summary>
	/// Eine Zeichenfolgendarstellung, die den aktuellen <see cref="Change"/>-Wert enthält.
	/// </summary>
	override this.ToString() =
		sprintf "Änderung: %d" this.Change

```

Dieser Indikator empfängt einen eingehenden Wert und erzeugt auf Basis des gesetzten Parameterwerts **Change** eine beliebige Abweichung.

Die Beschreibung der Indikatormethoden finden Sie im Abschnitt [Benutzerdefinierter Indikator](../../../../api/indicators/custom_indicator.md).

Um den erstellten Indikator zum Diagramm hinzuzufügen, verwenden Sie den Würfel [Indikator](../../using_visual_designer/elements/common/indicator.md) und geben darin den gewünschten Indikator an:

![Designer Quellcode-Indikator 01](../../../../../images/designer_source_code_indicator_01.png)

Der Parameter **Change**, der zuvor im Indikatorcode festgelegt wurde, wird im Eigenschaftenpanel angezeigt.

> [!WARNING]
> Indikatoren aus F#-Code können nicht in Strategien verwendet werden, die in F#-Code erstellt wurden. Sie können nur in Strategien verwendet werden, die [aus Würfeln](../../using_visual_designer.md) erstellt wurden.

