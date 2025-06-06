# Create strategy panel

Custom panels are special controls created by S\# to facilitate working with DevExpress elements.

First, you need to create a simple UserControl in the XAML folder of your strategy.

![Shell custom strategy panel 00](../../images/shell_custom_strategy_panel_00.png)

![Shell custom strategy panel 01](../../images/shell_custom_strategy_panel_01.png)

Replace `UserControl` with `controls:BaseStudioControl`.

```xaml
<controls:BaseStudioControl>
...
</controls:BaseStudioControl>
	  				
```

Then implement your own panel logic by analogy with the existing strategy panels.

In order for the [Real\-time](user_interface/real_time.md) panel to see the strategy in your panel, your strategy must be set as a property:

```cs
	public partial class SmaMonitoringControl
	{
	...
		public Strategy Strategy { get; set; }
	...
	}
		
```

To save the strategy settings, you must override the **Load** and **Save** methods in the panel.

```cs
	public partial class SmaMonitoringControl
	{
	...
		public override void Load(SettingsStorage storage)
		{
			base.Load(storage);
			try
			{
				Strategy = MainWindow.Instance.CreateStrategy(storage.GetValue<SettingsStorage>(nameof(Strategy)));
				Init(Strategy);
			}
			catch (Exception e)
			{
				e.LogError();
			}
		}
		public override void Save(SettingsStorage storage)
		{
			base.Save(storage);
			storage.SetValue(nameof(Strategy), Strategy.Save());
		}
	...
	}
		
```

## Recommended content

[Create strategy](create_strategy.md)
