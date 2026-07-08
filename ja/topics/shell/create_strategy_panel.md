# ストラテジーパネルの作成

カスタムパネルは、DevExpress 要素を扱いやすくするために S# によって作成された特殊なコントロールです。

まず、ストラテジーの XAML フォルダー内に単純な UserControl を作成する必要があります。

![Shell custom strategy panel 00](../../images/shell_custom_strategy_panel_00.png)

![Shell custom strategy panel 01](../../images/shell_custom_strategy_panel_01.png)

`UserControl` を `controls:BaseStudioControl` に置き換えます。

```xaml
<controls:BaseStudioControl>
...
</controls:BaseStudioControl>
	  				
```

次に、既存のストラテジーパネルと同様に、独自のパネルロジックを実装します。

[リアルタイム](user_interface/real_time.md) パネルがパネル内のストラテジーを認識できるようにするには、ストラテジーをプロパティとして設定する必要があります。

```cs
	public partial class SmaMonitoringControl
	{
	...
		public Strategy Strategy { get; set; }
	...
	}
		
```

ストラテジー設定を保存するには、パネル内で **Load** メソッドと **Save** メソッドをオーバーライドする必要があります。

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

## 推奨コンテンツ

[ストラテジーの作成](create_strategy.md)
