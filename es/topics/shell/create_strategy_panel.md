# Crear panel de estrategia

Los paneles personalizados son controles especiales creados por S# para facilitar el trabajo con elementos DevExpress.

Primero, debe crear un UserControl simple en la carpeta XAML de su estrategia.

![Shell panel de estrategia personalizada 00](../../images/shell_custom_strategy_panel_00.png)

![Shell panel de estrategia personalizada 01](../../images/shell_custom_strategy_panel_01.png)

Sustituya `UserControl` por `controls:BaseStudioControl`.

```xaml
<controls:BaseStudioControl>
...
</controls:BaseStudioControl>
	  				
```

Después implemente la lógica de su propio panel de forma similar a los paneles de estrategia existentes.

Para que el panel [Tiempo real](user_interface/real_time.md) vea la estrategia en su panel, la estrategia debe establecerse como propiedad:

```cs
	public partial class SmaMonitoringControl
	{
	...
		public Strategy Strategy { get; set; }
	...
	}
		
```

Para guardar la configuración de la estrategia, debe sobrescribir los métodos **Load** y **Save** en el panel.

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

## Contenido recomendado

[Crear estrategia](create_strategy.md)
