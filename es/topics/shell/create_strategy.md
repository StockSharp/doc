# Crear estrategia

Para crear su propia estrategia, cree una carpeta para ella en la carpeta Strategies.

![Shell custom strategy 00](../../images/shell_custom_strategy_00.png)

Usando SmaStrategy como ejemplo, cree la estrategia en sí.

Si necesita añadir su propio panel de prueba o monitorización para una estrategia, la estrategia debe implementar las interfaces IHaveTestControl e IHaveMonitoringControl, respectivamente.

```cs
public class SmaStrategy : Strategy, IHaveMonitoringControl, IHaveTestControl
	{
	...
		#region MonitoringControl
				public BaseStudioControl AddMonitoringPanel()
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

También debe crear los paneles en sí. Cómo construir su propio panel de prueba o monitorización se describe en [Crear panel de estrategia](create_strategy_panel.md).

> [!TIP]
> Si son suficientes los paneles de prueba o monitorización para la estrategia que se usan en estrategias predeterminadas, no necesita implementar las interfaces IHaveTestControl e IHaveMonitoringControl. Shell ejecutará por sí mismo los paneles predeterminados de prueba o monitorización.

![Shell custom strategy 01](../../images/shell_custom_strategy_01.png)

Para que la estrategia creada esté disponible en la ventana de selección de estrategias, debe añadirse al diccionario **DictionaryStrategies** de la ventana principal.

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

Para que la estrategia se guarde y luego se cargue, debe establecer el parámetro de estrategia en el diseñador de estrategias.

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

Para guardar campos adicionales, debe sobrescribir los métodos **Load** y **Save**.

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

## Contenido recomendado

[Crear panel de estrategia](create_strategy_panel.md)
