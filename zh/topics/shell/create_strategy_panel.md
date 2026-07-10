# 创建策略面板

自定义面板是 S# 提供的专用控件，用于简化 DevExpress 元素的使用。

首先，在策略的 XAML 文件夹中创建一个普通 UserControl。

![Shell 自定义策略面板 00](../../images/shell_custom_strategy_panel_00.png)

![Shell 自定义策略面板 01](../../images/shell_custom_strategy_panel_01.png)

将 `UserControl` 替换为 `controls:BaseStudioControl`。

```xaml
<controls:BaseStudioControl>
...
</controls:BaseStudioControl>
	  				
```

然后参考现有策略面板，实现自己的面板逻辑。

为了让[实盘](user_interface/real_time.md)面板能够识别自定义面板中的策略，必须将策略定义为面板的属性：

```cs
	public partial class SmaMonitoringControl
	{
	...
		public Strategy Strategy { get; set; }
	...
	}
		
```

要保存策略设置，必须在面板中重写 **Load** 和 **Save** 方法。

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

## 推荐内容

[创建策略](create_strategy.md)
