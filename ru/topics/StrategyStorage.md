# Сохранение и загрузка настроек

Для сохранения и загрузки настроек стратегии используются переопределения методов [Save](xref:StockSharp.Algo.Strategies.Strategy.Save) и [Load](xref:StockSharp.Algo.Strategies.Strategy.Load) соответственно. 

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
if (File.Exists("marketProfile.xml"))
{
    //Загрузка настроек стратегии из существующего конфигурационного файла
    var settingsStorage = new XmlSerializer<SettingsStorage>().Deserialize("marketProfile.xml");
    newStrategy.Load(settingsStorage);
}
```

Для сохранения настроек во внешний файл необходимо внести изменения в метод [Save](xref:StockSharp.Algo.Strategies.Strategy.Save), описанный ранее. 

```cs
public override void Save(SettingsStorage settings)
{
    settings.SetValue("UsedVolume", UsedVolume);
    settings.SetValue("Ticks", Ticks);
    settings.SetValue("Volume", Volume);
    settings.SetValue("SpreadVolume", SpreadVolume);
	    
	base.Save(settings);
	
	//Сохраняем настройки в файл
	new XmlSerializer<SettingsStorage>().Serialize(settings, "marketProfile.xml");
}
```

### Следующие шаги

[Загрузка заявок и сделок](StrategyOrdersLoad.md)

## См. также
