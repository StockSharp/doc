# Criar estratégia

Para criar a sua própria estratégia, crie uma pasta para a estratégia na pasta Strategies.

![Shell custom strategy 00](../../images/shell_custom_strategy_00.png)

Usando a SmaStrategy como exemplo, crie a estratégia propriamente dita.

Se precisar de adicionar o seu próprio painel de teste ou monitorização para uma estratégia, então a estratégia deve implementar as interfaces IHaveTestControl e IHaveMonitoringControl, respetivamente.

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

Também é necessário criar os próprios painéis. A forma de construir o seu próprio painel de teste ou monitorização é descrita em [Criar painel de estratégia](create_strategy_panel.md).

> [!TIP]
> Se os painéis de teste ou monitorização usados para as estratégias predefinidas forem suficientes para a estratégia, então não precisa de implementar as interfaces IHaveTestControl e IHaveMonitoringControl. O Shell executará por si só os painéis de teste ou monitorização predefinidos. 

![Shell custom strategy 01](../../images/shell_custom_strategy_01.png)

Para que a estratégia criada fique disponível na janela de seleção de estratégias, deve ser adicionada ao dicionário **DictionaryStrategies** da janela principal. 

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

Para que a estratégia seja guardada e posteriormente carregada, é necessário definir o parâmetro da estratégia no construtor da estratégia.

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

Para guardar campos adicionais, deve substituir os métodos **Load** e **Save**.

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

## Conteúdo recomendado

[Criar painel de estratégia](create_strategy_panel.md)
