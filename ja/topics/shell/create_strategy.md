# ストラテジーの作成

独自のストラテジーを作成するには、Strategies フォルダー内にストラテジー用のフォルダーを作成します。

![Shell カスタム戦略 00](../../images/shell_custom_strategy_00.png)

SmaStrategy を例として、ストラテジー本体を作成します。

ストラテジーに独自のテスト用パネルまたは監視用パネルを追加する必要がある場合、ストラテジーはそれぞれ IHaveTestControl インターフェイスおよび IHaveMonitoringControl インターフェイスを実装する必要があります。

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

また、パネル自体も作成する必要があります。独自のテスト用パネルまたは監視用パネルの作成方法は、[ストラテジーパネルの作成](create_strategy_panel.md)で説明しています。

> [!TIP]
> 既定のストラテジーで使用されるテスト用パネルまたは監視用パネルが、そのストラテジーに対して十分である場合は、IHaveTestControl インターフェイスおよび IHaveMonitoringControl インターフェイスを実装する必要はありません。Shell は既定のテスト用パネルまたは監視用パネルを自動的に実行します。 

![Shell カスタム戦略 01](../../images/shell_custom_strategy_01.png)

作成したストラテジーをストラテジー選択ウィンドウで利用できるようにするには、メインウィンドウの **DictionaryStrategies** ディクショナリに追加する必要があります。 

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

ストラテジーを保存し、その後読み込めるようにするには、ストラテジーデザイナーでストラテジーパラメーターを設定する必要があります。

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

追加フィールドを保存するには、**Load** メソッドと **Save** メソッドをオーバーライドする必要があります。

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

## 推奨コンテンツ

[ストラテジーパネルの作成](create_strategy_panel.md)
