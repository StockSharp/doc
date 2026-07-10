# Guardar e Carregar Definições

Em StockSharp, o mecanismo para guardar e carregar definições de estratégia é implementado através dos métodos [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) e [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)).

## Tratamento Automático de Parâmetros

Na maioria dos casos, **não é necessário** substituir os métodos `Save` e `Load`, pois a classe base [Strategy](xref:StockSharp.Algo.Strategies.Strategy) guarda e carrega automaticamente os parâmetros de estratégia criados com [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1).

A abordagem recomendada é usar o mecanismo de parâmetros de estratégia descrito em detalhe na secção [Parâmetros da Estratégia](parameters.md). Com esta abordagem, todos os parâmetros são guardados e carregados automaticamente:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetDisplay("Long SMA length", string.Empty, "Configurações básicas");
	}
}
```

## Substituição em Casos Especiais

Substituir os métodos `Save` e `Load` só é necessário em casos especiais quando precisa de guardar ou carregar dados que não fazem parte do conjunto padrão de parâmetros da estratégia. Por exemplo, para guardar estado interno, estruturas de dados não padrão ou valores em cache.

Se substituir estes métodos, **tem de chamar os métodos da classe base**:

```cs
public override void Save(SettingsStorage settings)
{
	// Primeiro chamar o método base para guardar parâmetros padrão
	base.Save(settings);
	
	// Depois adicionar a sua lógica específica de gravação
	settings.SetValue("CustomState", _customState);
}
	
public override void Load(SettingsStorage settings)
{
	// Primeiro chamar o método base para carregar parâmetros padrão
	base.Load(settings);
	
	// Depois adicionar a sua lógica específica de carregamento
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");
}
```

## Guardar e Carregar a partir de um Ficheiro

Para guardar definições num ficheiro ou carregar a partir de um ficheiro, pode usar a serialização e desserialização implementadas em StockSharp:

```cs
// Guardar definições num ficheiro
var settingsStorage = new SettingsStorage();
strategy.Save(settingsStorage);
new JsonSerializer<SettingsStorage>().Serialize(settingsStorage, "strategy.json");

// Carregar definições a partir de um ficheiro
var newStrategy = new SmaStrategy();
if (File.Exists("strategy.json"))
{
	var loadedSettings = new JsonSerializer<SettingsStorage>().Deserialize("strategy.json");
	newStrategy.Load(loadedSettings);
}
```

## Recomendações

1. Sempre que possível, use [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) para todos os parâmetros configuráveis da estratégia.
2. Substitua os métodos `Save` e `Load` apenas quando precisar de guardar/carregar dados não padrão.
3. Chame sempre os métodos base `base.Save()` e `base.Load()` ao substituir.
4. Use as ferramentas de serialização padrão do StockSharp para guardar definições num ficheiro ou carregar a partir de um ficheiro.

## Ver Também

[Parâmetros da Estratégia](parameters.md)
