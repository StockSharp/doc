# Create strategy

To create your own strategy, create a folder for your strategy in the Strategies folder.

![Shell custom strategy 00](~/images/Shell_custom_strategy_00.png)

Using the example of SmaStrategy, you need to create the strategy itself.

If you need to add your own testing or monitoring panel for a strategy, then the strategy must implement the IHaveTestControl and IHaveMonitoringControl interfaces, respectively.

```cs
public class SmaStrategy : Strategy, IHaveMonitoringControl, IHaveTestControl
	{
	...
		\#region MonitoringControl
		public BaseStudioControl AddMonitorigPanel()
		{
			var usercontrol \= new SmaMonitoringControl();
			usercontrol.Init(this);
			return usercontrol;
		}
		\#endregion
		\#region TestingControl
		public BaseStudioControl AddTestPanel()
		{
			var usercontrol \= new SmaTestingControl();
			usercontrol.Init(this);
			return usercontrol;
		}
		\#endregion
	...	
	}
		
```

And you also need to create the panels themselves. How to build your own testing or monitoring panel is described in [Create strategy panel](Shell_custom_strategy_panel.md).

> [!TIP]
> If there are enough testing or monitoring panels for the strategy, which are used for default strategies, then you do not need to implement the IHaveTestControl and IHaveMonitoringControl interfaces. Shell will run the default testing or monitoring panels by itself. 

![Shell custom strategy 01](~/images/Shell_custom_strategy_01.png)

In order for the created strategy to be available in the strategy selection window, it must be added to the **DictionaryStrategies** dictionary of the main window. 

```cs
	...
	\/\/\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-
	DictionaryStrategies \= new ObservableDictionary\<Guid, Strategy\>
	{
		{ new SmaStrategy().GetTypeId(), new SmaStrategy() },
		{ new StairsTrendStrategy().GetTypeId(), new StairsTrendStrategy() },
		{ new StairsCountertrendStrategy().GetTypeId(), new StairsCountertrendStrategy() }
	};
	\/\/\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-
	...	
		
```

In order for the strategy to be saved and then loaded, you need to set the strategy parameter in the strategy designer.

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

To save additional fields, you must override the **Load** and **Save** methods

```cs
public class SmaStrategy : Strategy, IHaveMonitoringControl, IHaveTestControl
	{
	...
		\#region Load
		public override void Load(SettingsStorage storage)
		{
			base.Load(storage);
			try
			{
				\_securityStr \= storage.GetValue\<string\>(nameof(Security));
				\_portfolioStr \= storage.GetValue\<string\>(nameof(Portfolio));
				LongSmaLength \= storage.GetValue\<int\>(nameof(LongSmaLength));
				ShortSmaLength \= storage.GetValue\<int\>(nameof(ShortSmaLength));
				Series.CandleType \= storage.GetValue(nameof(Series.CandleType), Series.CandleType);
				Series.Arg \= storage.GetValue(nameof(Series.Arg), Series.Arg);
			}
			catch (Exception e)
			{
				e.LogError();
			}
		}
		\#endregion
		\#region Save
		public override void Save(SettingsStorage storage)
		{
			base.Save(storage);
			storage.SetValue(nameof(Security), Security?.Id);
			storage.SetValue(nameof(Portfolio), Portfolio?.Name);
			storage.SetValue(nameof(LongSmaLength), LongSmaLength);
			storage.SetValue(nameof(ShortSmaLength), ShortSmaLength);
			if (Series.CandleType \!\= null)
				storage.SetValue(nameof(Series.CandleType), Series.CandleType.GetTypeName(false));
			if (Series.Arg \!\= null)
				storage.SetValue(nameof(Series.Arg), Series.Arg);
		}
		\#endregion
	...	
	}
		
```

## Recommended content

[Create strategy panel](Shell_custom_strategy_panel.md)
