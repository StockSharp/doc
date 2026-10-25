# Criar estratégia

As estratégias incluídas no [Shell](../shell.md) estão em `Shell.Avalonia/Shell.Core/Strategies/`. Uma própria adiciona-se no mesmo sítio e passa por três passos: a classe da estratégia, a entrada no catálogo e, se for preciso, um painel próprio.

## Classe da estratégia

Uma estratégia é um descendente normal de `Strategy` da StockSharp. Não tem nada específico do Shell:

```cs
public class MyStrategy : Strategy
{
	private readonly StrategyParam<int> _length;

	public MyStrategy()
	{
		Name = "A minha estratégia";

		_length = Param(nameof(Length), 20)
			.SetGreaterThanZero()
			// O grupo é obtido dos recursos para que o parâmetro fique no mesmo grupo que os padrão.
			.SetDisplay("Comprimento", "Comprimento da média móvel", LocalizedStrings.Parameters)
			.SetOptimize(10, 50, 5);
	}

	public int Length
	{
		get => _length.Value;
		set => _length.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// subscrições de dados e lógica de negociação
	}
}
```

O ponto de entrada é `OnStarted2(DateTime)`, e não `OnStarted`: quando é chamado, a estratégia já recebeu o instrumento, a carteira e a ligação, pelo que pode subscrever os dados aqui mesmo.

Um parâmetro declarado com `Param(...)` e com `SetOptimize(...)` definido aparece automaticamente no separador [Otimização](user_interface/optimization.md): a lista de parâmetros iterados e os limites da iteração são lidos da própria estratégia, e não definidos à parte.

## Catálogo de estratégias

Para que a estratégia apareça na janela de seleção, é preciso adicioná-la em dois sítios.

O primeiro é a fábrica de `BuiltInStrategies`, que cria uma instância pelo identificador de tipo:

```cs
public static Strategy TryCreate(string templateTypeId)
{
	...
	if (templateTypeId.Equals(MyTypeId, StringComparison.OrdinalIgnoreCase))
		return new MyStrategy();
	...
}
```

O segundo é a descrição do modelo em `LegacyStrategyTemplates`, de onde vêm o nome da estratégia na lista e o conjunto de parâmetros com os seus tipos e valores predefinidos:

```cs
new StrategyTemplateDefinition(MyTypeId, "A minha estratégia", StrategyTemplateKind.Generic, true,
[
	.. CommonParameters(),
	new("Length", "Comprimento", StrategyParameterKind.Integer, "20", true, 1),
])
```

`CommonParameters()` devolve o instrumento, a carteira e o volume, que todas as estratégias têm; o resto são os seus próprios parâmetros. Os campos da descrição vêm por esta ordem: chave, etiqueta, tipo, valor predefinido, obrigatoriedade, mínimo e máximo.

`StrategyTemplateKind` só distingue os tipos para os quais o Shell tem um painel próprio (`Sma`, `SmaIndex`, `StairsTrend`, `StairsCountertrend`, `Designer`); uma estratégia sem painel próprio é `Generic`.

Os valores que o utilizador define no painel **Estratégia** são aplicados à instância em `StrategyMaterializer`: é aí também que se acrescenta a análise dos novos parâmetros.

O Shell guarda e restaura as definições da estratégia: não é preciso escrever código de gravação à parte.

## Painel próprio

Se o painel padrão de parâmetros for suficiente, não é preciso fazer nada: o Shell mostra-o por si só. Um painel próprio é necessário quando a estratégia tem algo a mostrar além dos parâmetros: como o configurar e como observar a estratégia está descrito na secção [Criar painel de estratégia](create_strategy_panel.md).

## Conteúdo recomendado

[Criar painel de estratégia](create_strategy_panel.md)

[Executar estratégias a partir do Designer](run_strategies_from_designer.md)
