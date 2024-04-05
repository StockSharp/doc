# Сохранение и загрузка настроек

Для сохранения и загрузки настроек стратегии используются переопределения методов [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage))**(**[Ecng.Serialization.SettingsStorage](xref:Ecng.Serialization.SettingsStorage) storage**)** и [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage))**(**[Ecng.Serialization.SettingsStorage](xref:Ecng.Serialization.SettingsStorage) storage**)** соответственно. 

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

Для сохранения и загрузки настроек из внешнего файла можно воспользоваться соответственно сериализацией и десериализацией, реализованной в [S\#](StockSharpAbout.md). 

```cs
var newStrategy = new MarketProfileStrategy();
if (File.Exists("marketProfile.json"))
{
    //Загрузка настроек стратегии из существующего конфигурационного файла
    var settingsStorage = new JsonSerializer<SettingsStorage>().Deserialize("marketProfile.json");
    newStrategy.Load(settingsStorage);
}
```

Для сохранения настроек во внешний файл необходимо внести изменения в метод [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage))**(**[Ecng.Serialization.SettingsStorage](xref:Ecng.Serialization.SettingsStorage) storage**)**, описанный ранее. 

```cs
public override void Save(SettingsStorage settings)
{
    settings.SetValue("UsedVolume", UsedVolume);
    settings.SetValue("Ticks", Ticks);
    settings.SetValue("Volume", Volume);
    settings.SetValue("SpreadVolume", SpreadVolume);
	    
	base.Save(settings);
	
	//Сохраняем настройки в файл
	new JsonSerializer<SettingsStorage>().Serialize(settings, "marketProfile.json");
}
```

## Следующие шаги

[Загрузка заявок и сделок](StrategyOrdersLoad.md)
