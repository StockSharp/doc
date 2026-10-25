# Create strategy panel

Every strategy in [Shell](../shell.md) has two panels: the setup panel in the [Emulation](user_interface/emulation.md) area and the monitoring panel in the [Real-time](user_interface/real_time.md) area. By default, both are common: the parameter table and, for emulation, a tab with the history settings. An own panel is needed when the strategy has something to show beyond that.

## Blank

The panels are located in `Shell.Avalonia/Shell.Avalonia/Views/Strategies/`. An own panel is a regular Avalonia `UserControl` that uses one of the two common panels inside:

```xaml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:views="using:Shell.Views.Strategies"
             xmlns:strategies="using:Shell.ViewModels.Strategies"
             x:Class="Shell.Views.Strategies.MyTestingView"
             x:DataType="strategies:StrategyDraftViewModel">
  <views:TestingView Header="My strategy, testing" />
</UserControl>
```

`TestingView` is the setup panel, `MonitoringView` is the monitoring panel. The `Header` property is the caption with which the panel names itself in its title bar.

The data context of the panel is always `StrategyDraftViewModel` — the very model the user edits. It gives access to the strategy name, its parameters, the run settings and the current state, so your own elements are bound to it directly, without accessing the main window.

## Binding the panel to the strategy

Which panel is shown for which strategy is decided by `StrategyDraftViewLocator`. It lists the pairs "area + template kind - panel":

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

The last two lines are the default panels: a strategy for which nothing has been added gets them.

## What you do not need to show

You do not need to draw the trades chart, statistics, equity curve, orders and own trades in your panel — these are separate panels next to it, and they are filled by themselves for any strategy.

## Recommended content

[Create strategy](create_strategy.md)
