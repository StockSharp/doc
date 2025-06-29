# Создание собственной стратегии

Для создания собственной стратегии следует создать папку для своей стратегии в папке Strategies.

![Shell custom strategy 00](../../images/shell_custom_strategy_00.png)

Воспользовавшись примером стратегии SmaStrategy, необходимо создать саму стратегию.

Если для стратегии необходимо добавить собственную панель тестирования или мониторинга, то стратегия должна реализовать интерфейсы IHaveTestControl и IHaveMonitoringControl соответственно. 

```cs
public class SmaStrategy : Strategy, IHaveMonitoringControl, IHaveTestControl
	{
	...
		#region MonitoringControl
		public BaseStudioControl AddMonitorigPanel()
		{
			var usercontrol = new SmaMonitoringControl();
			usercontrol.Init(this);
			return usercontrol;
		}
		#endregion
		#region TestingControl
		public BaseStudioControl AddTestPanel()
		{
			var usercontrol = new SmaTestingControl();
			usercontrol.Init(this);
			return usercontrol;
		}
		#endregion
	...	
	}
		
```

А также необходимо создать сами панели. Как создавать собственную панель тестирования или мониторинга описано в пункте [Создание собственных панелей для стратегий](create_strategy_panel.md).

> [!TIP]
> Если для стратегии достаточно панелей тестирования или мониторинга которые используются для стратегий по умолчанию, то реализовывать интерфейсы IHaveTestControl и IHaveMonitoringControl не нужно. Shell самостоятельно запустит панели тестирования или мониторинга которые используются по умолчанию. 

![Shell custom strategy 01](../../images/shell_custom_strategy_01.png)

Чтобы созданная стратегия была доступна в окне выбора стратегий, ее необходимо добавить в словарь **DictionaryStrategies** главного окна 

```cs
	...
	//---------------------------------------------------------------------
	DictionaryStrategies = new ObservableDictionary<Guid, Strategy>
	{
		{ new SmaStrategy().GetTypeId(), new SmaStrategy() },
		{ new StairsTrendStrategy().GetTypeId(), new StairsTrendStrategy() },
		{ new StairsCountertrendStrategy().GetTypeId(), new StairsCountertrendStrategy() }
	};
	//---------------------------------------------------------------------
	...	
		
```

Для того чтобы стратегия сохранялась и после этого загружалась, в конструкторе стратегии необходимо установить параметр стратегии.

```cs
public class SmaStrategy : Strategy, IHaveMonitoringControl, IHaveTestControl
	{
	...
		public SmaStrategy()
		{
			...
			this.Param("TypeId", GetType().GUID);
			...
		}
	...	
	}
		
```

Для сохранения дополнительных полей необходимо переопределить методы **Load** и **Save**

```cs
public class SmaStrategy : Strategy, IHaveMonitoringControl, IHaveTestControl
	{
	...
		#region Load
		public override void Load(SettingsStorage storage)
		{
			base.Load(storage);
			try
			{
				_securityStr = storage.GetValue<string>(nameof(Security));
				_portfolioStr = storage.GetValue<string>(nameof(Portfolio));
				LongSmaLength = storage.GetValue<int>(nameof(LongSmaLength));
				ShortSmaLength = storage.GetValue<int>(nameof(ShortSmaLength));
				Series.CandleType = storage.GetValue(nameof(Series.CandleType), Series.CandleType);
				Series.Arg = storage.GetValue(nameof(Series.Arg), Series.Arg);
			}
			catch (Exception e)
			{
				e.LogError();
			}
		}
		#endregion
		#region Save
		public override void Save(SettingsStorage storage)
		{
			base.Save(storage);
			storage.SetValue(nameof(Security), Security?.Id);
			storage.SetValue(nameof(Portfolio), Portfolio?.Name);
			storage.SetValue(nameof(LongSmaLength), LongSmaLength);
			storage.SetValue(nameof(ShortSmaLength), ShortSmaLength);
			if (Series.CandleType != null)
				storage.SetValue(nameof(Series.CandleType), Series.CandleType.GetTypeName(false));
			if (Series.Arg != null)
				storage.SetValue(nameof(Series.Arg), Series.Arg);
		}
		#endregion
	...	
	}
		
```

## См. также

[Создание собственных панелей для стратегий](create_strategy_panel.md)
