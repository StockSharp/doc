# Criar um indicador em C#

A criação do seu próprio indicador na [API](../../../../api.md) é descrita na secção [Indicador personalizado](../../../../api/indicators/custom_indicator.md). Esses indicadores são totalmente compatíveis com o **Designer**.

Para criar um indicador, no painel **Scheme** é necessário selecionar a pasta **Indicators**, clicar com o botão direito e, no menu de contexto, selecionar **Add**:

![Designer_Source_Code_Indicator_00](../../../../../images/designer_source_code_indicator_00.png)

O código do indicador terá o seguinte aspeto:

```cs
/// <summary>
/// Indicador de exemplo que demonstra como guardar e carregar parâmetros.
/// 
/// Altera o preço de entrada em +20% ou -20%.
/// 
/// Ver mais exemplos em https://github.com/StockSharp/StockSharp/tree/master/Algo/Indicators
/// 
/// Documentação https://doc.stocksharp.com/topics/Designer_Creating_indicator_from_source_code.html
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
	// o indicador formado recebeu todas as entradas necessárias para ficar disponível para negociação
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
		// em cada 10.ª chamada, tentar devolver um valor vazio
		if (RandomGen.GetInt(0, 10) == 0)
			return new DecimalIndicatorValue(this);

		if (_counter++ == 5)
		{
			// por exemplo, o nosso indicador precisa de 5 entradas para ficar formado
			_isFormed = true;
		}

		var value = input.GetValue<decimal>();

		// alteração aleatória de +20% ou -20% do valor atual

		value += value * RandomGen.GetInt(-Change, Change) / 100.0m;

		return new DecimalIndicatorValue(this, value)
		{
			// valor final significa que este valor para a entrada especificada
			// já não é alterado (por exemplo, para candles que mudam com o último preço)
			IsFinal = RandomGen.GetBool()
		};
	}

	// persistir as nossas propriedades para as guardar para reinícios futuros da aplicação

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

Este indicador recebe um valor de entrada e aplica um desvio arbitrário com base no valor do parâmetro **Change**.

A descrição dos métodos do indicador está disponível na secção [Indicador personalizado](../../../../api/indicators/custom_indicator.md).

Para adicionar o indicador criado ao diagrama, é necessário usar o cubo [Indicator](../../using_visual_designer/elements/common/indicator.md) e, nele, especificar o indicador necessário:

![Designer_Source_Code_Indicator_01](../../../../../images/designer_source_code_indicator_01.png)

O parâmetro **Change**, previamente definido no código do indicador, é apresentado no painel de propriedades.

> [!WARNING] 
> Indicadores em código C# não podem ser usados em estratégias criadas em código C#. Só podem ser usados em estratégias criadas [a partir de cubos](../../using_visual_designer.md).
