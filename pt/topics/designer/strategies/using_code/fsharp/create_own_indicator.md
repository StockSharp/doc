# Criar um indicador

A criação do seu próprio indicador na [API](../../../../api.md) é descrita na secção [Indicador personalizado](../../../../api/indicators/custom_indicator.md). Esses indicadores são totalmente compatíveis com o **Designer**.

Para criar um indicador, no painel **Esquema** é necessário selecionar a pasta **Indicadores**, clicar com o botão direito e, no menu de contexto, selecionar **Adicionar**:

![Designer indicador de código-fonte 00](../../../../../images/designer_source_code_indicator_00.png)

O código do indicador terá o seguinte aspeto:

```fsharp
/// <summary>
/// Indicador de exemplo que demonstra como guardar e carregar parâmetros.
/// Altera o preço de entrada em +20% ou -20%.
///
/// Ver mais exemplos:
/// https://github.com/StockSharp/StockSharp/tree/master/Algo/Indicators
///
/// Documentação:
/// https://doc.stocksharp.com/topics/designer/strategies/using_code/fsharp/create_own_indicator.html
/// </summary>
type EmptyIndicator() as this =
	inherit BaseIndicator()

	// Campos internos
	let mutable changeValue = 20
	let mutable counter = 0
	let mutable isFormedValue = false

	/// <summary>
	/// O valor percentual (+/-) usado para modificar o preço de entrada.
	/// </summary>
	member this.Change
		with get () = changeValue
		and set value =
			changeValue <- value
			this.Reset()

	/// <summary>
	/// Define se o indicador ficou formado (pronto para negociação).
	/// </summary>
	override this.CalcIsFormed() = isFormedValue

	/// <summary>
	/// Repõe o indicador para o seu estado inicial.
	/// </summary>
	override this.Reset() =
		base.Reset()
		isFormedValue <- false
		counter <- 0

	/// <summary>
	/// A lógica principal para processar valores de entrada.
	/// </summary>
	override this.OnProcess(input: IIndicatorValue) : IIndicatorValue =
		// em cada 10.ª chamada, tentar devolver um valor "vazio"
		if RandomGen.GetInt(0, 10) = 0 then
			// o valor vazio ainda contém apenas a hora, sem dados reais
			DecimalIndicatorValue(this, input.Time)
		else
			// incrementar o contador em cada chamada
			counter <- counter + 1

			// após 5 entradas, o indicador é considerado formado
			if counter = 5 then
				isFormedValue <- true

			let mutable value = input.ToDecimal()

			// alteração aleatória por um fator de +/- Change%
			let randomFactor = decimal (RandomGen.GetInt(-changeValue, changeValue)) / 100m
			value <- value + (value * randomFactor)

			// devolver o valor final do indicador
			let result = DecimalIndicatorValue(this, value, input.Time)
			// marcá-lo aleatoriamente como final ou não
			result.IsFinal <- RandomGen.GetBool()
			result

	/// <summary>
	/// Carrega as definições do indicador a partir de um <see cref="SettingsStorage"/> especificado.
	/// </summary>
	override this.Load(storage: SettingsStorage) =
		base.Load(storage)
		this.Change <- storage.GetValue<int>(nameof(this.Change))

	/// <summary>
	/// Guarda as definições do indicador num <see cref="SettingsStorage"/> especificado.
	/// </summary>
	override this.Save(storage: SettingsStorage) =
		base.Save(storage)
		storage.SetValue(nameof(this.Change), this.Change)

	/// <summary>
	/// Uma representação em string que inclui o valor atual de <see cref="Change"/>.
	/// </summary>
	override this.ToString() =
		sprintf "Alteração: %d" this.Change

```

Este indicador recebe um valor de entrada e aplica um desvio arbitrário com base no valor definido do parâmetro **Alteração**.

A descrição dos métodos do indicador está disponível na secção [Indicador personalizado](../../../../api/indicators/custom_indicator.md).

Para adicionar o indicador criado ao diagrama, é necessário usar o cubo [Indicador](../../using_visual_designer/elements/common/indicator.md) e, nele, especificar o indicador necessário:

![Designer indicador de código-fonte 01](../../../../../images/designer_source_code_indicator_01.png)

O parâmetro **Alteração**, previamente definido no código do indicador, é apresentado no painel de propriedades.

> [!WARNING]
> Indicadores em código F# não podem ser usados em estratégias criadas em código F#. Só podem ser usados em estratégias criadas [a partir de cubos](../../using_visual_designer.md).
