# Strategie erstellen

Um eine eigene Strategie zu erstellen, legen Sie im Ordner Strategies einen Ordner für Ihre Strategie an.

![Shell custom strategy 00](../../images/shell_custom_strategy_00.png)

Erstellen Sie die Strategie selbst, zum Beispiel anhand von SmaStrategy.

Wenn Sie für eine Strategie ein eigenes Testing- oder Monitoring-Panel hinzufügen müssen, muss die Strategie die Schnittstellen IHaveTestControl beziehungsweise IHaveMonitoringControl implementieren.

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

Außerdem müssen die Panels selbst erstellt werden. Wie Sie ein eigenes Testing- oder Monitoring-Panel erstellen, ist unter [Strategiepanel erstellen](create_strategy_panel.md) beschrieben.

> [!TIP]
> Wenn für die Strategie die Testing- oder Monitoring-Panels ausreichen, die für Standardstrategien verwendet werden, müssen Sie die Schnittstellen IHaveTestControl und IHaveMonitoringControl nicht implementieren. Shell startet die Standard-Testing- oder Monitoring-Panels selbst.

![Shell custom strategy 01](../../images/shell_custom_strategy_01.png)

Damit die erstellte Strategie im Strategieauswahlfenster verfügbar ist, muss sie dem Wörterbuch **DictionaryStrategies** des Hauptfensters hinzugefügt werden.

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

Damit die Strategie gespeichert und anschließend geladen werden kann, müssen Sie den Strategieparameter im Konstruktor der Strategie setzen.

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

Um zusätzliche Felder zu speichern, müssen Sie die Methoden **Load** und **Save** überschreiben.

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

## Empfohlene Inhalte

[Strategiepanel erstellen](create_strategy_panel.md)
