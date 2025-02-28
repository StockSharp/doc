# Saving and Loading Settings

In StockSharp, the mechanism for saving and loading strategy settings is implemented through the [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) and [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) methods.

## Automatic Parameter Handling

For most cases, **there is no need** to override the `Save` and `Load` methods, as the base [Strategy](xref:StockSharp.Algo.Strategies.Strategy) class automatically saves and loads strategy parameters created using [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1).

The recommended approach is to use the strategy parameter mechanism described in detail in the [Strategy Parameters](parameters.md) section. With this approach, all parameters are automatically saved and loaded:

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

## Overriding for Special Cases

Overriding the `Save` and `Load` methods is only required in special cases when you need to save or load data that is not part of the standard set of strategy parameters. For example, to save internal state, non-standard data structures, or cached values.

If you override these methods, **you must call the base class methods**:

```cs
public override void Save(SettingsStorage settings)
{
    // First call the base method to save standard parameters
    base.Save(settings);
    
    // Then add your specific saving logic
    settings.SetValue("CustomState", _customState);
}
	
public override void Load(SettingsStorage settings)
{
    // First call the base method to load standard parameters
    base.Load(settings);
    
    // Then add your specific loading logic
    if (settings.Contains("CustomState"))
        _customState = settings.GetValue<string>("CustomState");
}
```

## Saving and Loading from a File

To save settings to a file or load from a file, you can use the serialization and deserialization implemented in StockSharp:

```cs
// Save settings to a file
var settingsStorage = new SettingsStorage();
strategy.Save(settingsStorage);
new JsonSerializer<SettingsStorage>().Serialize(settingsStorage, "strategy.json");

// Load settings from a file
var newStrategy = new SmaStrategy();
if (File.Exists("strategy.json"))
{
    var loadedSettings = new JsonSerializer<SettingsStorage>().Deserialize("strategy.json");
    newStrategy.Load(loadedSettings);
}
```

## Recommendations

1. When possible, use [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) for all configurable strategy parameters.
2. Override the `Save` and `Load` methods only when you need to save/load non-standard data.
3. Always call the base methods `base.Save()` and `base.Load()` when overriding.
4. Use StockSharp's standard serialization tools to save settings to a file or load from a file.

## See also

[Strategy Parameters](parameters.md)
