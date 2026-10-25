# 创建策略面板

[Shell](../shell.md) 中的每个策略都有两个面板：[模拟](user_interface/emulation.md)区域中的设置面板和[实盘](user_interface/real_time.md)区域中的监控面板。默认情况下，这两个面板都是通用面板：参数表，以及模拟时的历史数据设置选项卡。当策略除此之外还有其他内容需要展示时，才需要自定义面板。

## 模板

面板位于 `Shell.Avalonia/Shell.Avalonia/Views/Strategies/`。自定义面板是一个普通的 Avalonia `UserControl`，其中使用两个通用面板之一：

```xaml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:views="using:Shell.Views.Strategies"
             xmlns:strategies="using:Shell.ViewModels.Strategies"
             x:Class="Shell.Views.Strategies.MyTestingView"
             x:DataType="strategies:StrategyDraftViewModel">
  <views:TestingView Header="我的策略，测试" />
</UserControl>
```

`TestingView` 是设置面板，`MonitoringView` 是监控面板。`Header` 属性是面板在其标题栏中显示的名称。

面板的数据上下文始终是 `StrategyDraftViewModel`，也就是用户正在编辑的模型。通过它可以访问策略名称、参数、运行设置和当前状态，因此自定义元素可以直接绑定到该模型，而无需访问主窗口。

## 将面板绑定到策略

哪个策略显示哪个面板由 `StrategyDraftViewLocator` 决定。其中列出了“区域 + 模板类型 → 面板”的对应关系：

```cs
public Control Build(object data)
	=> data switch
	{
		StrategyDraftViewModel { WorkspaceKind: StrategyWorkspaceKind.Backtest, TemplateKind: StrategyTemplateKind.Sma } => new SmaTestingView(),
		StrategyDraftViewModel { WorkspaceKind: StrategyWorkspaceKind.Realtime, TemplateKind: StrategyTemplateKind.Sma } => new SmaMonitoringView(),
		...
		StrategyDraftViewModel { WorkspaceKind: StrategyWorkspaceKind.Backtest } => new TestingView(),
		StrategyDraftViewModel { WorkspaceKind: StrategyWorkspaceKind.Realtime } => new MonitoringView(),
		_ => null,
	};
```

最后两行就是默认面板：没有添加任何对应关系的策略会使用这两个面板。

## 无需显示的内容

无需在自己的面板中绘制成交图表、统计信息、收益曲线、订单和自有成交：它们是旁边的独立面板，对任何策略都会自动填充内容。

## 推荐内容

[创建策略](create_strategy.md)
