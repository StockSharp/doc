# Settings saving and loading

To save and load strategy settings the overrides of [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage))**(**[Ecng.Serialization.SettingsStorage](xref:Ecng.Serialization.SettingsStorage) storage **)** and [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage))**(**[Ecng.Serialization.SettingsStorage](xref:Ecng.Serialization.SettingsStorage) storage **)** methods used respectively. 

```cs
public override void Load(SettingsStorage settings)
{
	if (settings.Contains("UsedVolume"))
	    Id = settings.GetValue<Guid>("UsedVolume");
	
    if (settings.Contains("Ticks"))
        Name = settings.GetValue<string>("Ticks");
	
    if (settings.Contains("SpreadVolume"))
        Volume = settings.GetValue<decimal>("SpreadVolume");
	        
	base.Load(settings);
}
	
public override void Save(SettingsStorage settings)
{
    settings.SetValue("UsedVolume", UsedVolume);
    settings.SetValue("Ticks", Ticks);
    settings.SetValue("Volume", Volume);
    settings.SetValue("SpreadVolume", SpreadVolume);
	    
	base.Save(settings);
}
```

To save and load settings from an external file, you can use serialization and deserialization, respectively, implemented in [S\#](../../api.md). 

```cs
var newStrategy = new MarketProfileStrategy();
if (File.Exists("marketProfile.json"))
{
    var settingsStorage = new JsonSerializer<SettingsStorage>().Deserialize("marketProfile.json");
    newStrategy.Load(settingsStorage);
}
```

To save the settings to an external file, you must change the [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage))**(**[Ecng.Serialization.SettingsStorage](xref:Ecng.Serialization.SettingsStorage) storage **)**, method, described earlier. 

```cs
public override void Save(SettingsStorage settings)
{
    settings.SetValue("UsedVolume", UsedVolume);
    settings.SetValue("Ticks", Ticks);
    settings.SetValue("Volume", Volume);
    settings.SetValue("SpreadVolume", SpreadVolume);
	    
	base.Save(settings);
	
	new JsonSerializer<PlazaTable>().Serialize(settings, "marketProfile.json");
}
```

## Next Steps

[Orders and trades loading](orders_and_trades_loading.md)
