# Сохранение и загрузка настроек

В StockSharp механизм сохранения и загрузки настроек стратегий реализован через методы [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) и [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)).

## Автоматическая работа с параметрами

Для большинства случаев **нет необходимости** в переопределении методов `Save` и `Load`, так как базовый класс [Strategy](xref:StockSharp.Algo.Strategies.Strategy) автоматически сохраняет и загружает параметры стратегии, созданные с использованием [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1).

Рекомендуемый подход - использовать механизм параметров стратегии, подробно описанный в разделе [Параметры стратегии](parameters.md). При таком подходе все параметры автоматически сохраняются и загружаются:

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
							.SetDisplay("Long SMA length", string.Empty, "Base settings");
	}
}
```

## Переопределение для особых случаев

Переопределение методов `Save` и `Load` требуется только в особых случаях, когда необходимо сохранить или загрузить данные, которые не входят в стандартный набор параметров стратегии. Например, для сохранения внутреннего состояния, нестандартных структур данных или кэшированных значений.

Если вы переопределяете эти методы, **обязательно нужно вызывать методы базового класса**:

```cs
public override void Save(SettingsStorage settings)
{
	// Сначала вызываем базовый метод для сохранения стандартных параметров
	base.Save(settings);
	
	// Затем добавляем свою специфичную логику сохранения
	settings.SetValue("CustomState", _customState);
}
	
public override void Load(SettingsStorage settings)
{
	// Сначала вызываем базовый метод для загрузки стандартных параметров
	base.Load(settings);
	
	// Затем добавляем свою специфичную логику загрузки
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");
}
```

## Сохранение и загрузка из файла

Для сохранения настроек в файл или загрузки из файла можно использовать сериализацию и десериализацию, реализованную в StockSharp:

```cs
// Сохранение настроек в файл
var settingsStorage = new SettingsStorage();
strategy.Save(settingsStorage);
new JsonSerializer<SettingsStorage>().Serialize(settingsStorage, "strategy.json");

// Загрузка настроек из файла
var newStrategy = new SmaStrategy();
if (File.Exists("strategy.json"))
{
	var loadedSettings = new JsonSerializer<SettingsStorage>().Deserialize("strategy.json");
	newStrategy.Load(loadedSettings);
}
```

## Рекомендации

1. По возможности, используйте [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) для всех настраиваемых параметров стратегии.
2. Переопределяйте методы `Save` и `Load` только при необходимости сохранить/загрузить нестандартные данные.
3. Всегда вызывайте базовые методы `base.Save()` и `base.Load()` при переопределении.
4. Для сохранения настроек в файл или загрузки из файла используйте стандартные средства сериализации StockSharp.

## См. также

[Параметры стратегии](parameters.md)
