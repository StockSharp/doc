# Создание собственных панелей для стратегий

У каждой стратегии в [Shell](../shell.md) есть две панели: панель настройки в области [Эмуляция](user_interface/emulation.md) и панель наблюдения в области [Реал\-тайм](user_interface/real_time.md). По умолчанию обе \- общие: таблица параметров и, для эмуляции, вкладка с настройками истории. Собственная панель нужна, когда стратегии есть что показать сверх этого.

## Заготовка

Панели лежат в `Shell.Avalonia/Shell.Avalonia/Views/Strategies/`. Собственная \- это обычный `UserControl` Avalonia, внутри которого используется одна из двух общих панелей:

```xaml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:views="using:Shell.Views.Strategies"
             xmlns:strategies="using:Shell.ViewModels.Strategies"
             x:Class="Shell.Views.Strategies.MyTestingView"
             x:DataType="strategies:StrategyDraftViewModel">
  <views:TestingView Header="Моя стратегия, тестирование" />
</UserControl>
```

`TestingView` \- панель настройки, `MonitoringView` \- панель наблюдения. Свойство `Header` \- это подпись, которой панель называет себя в своей строке заголовка.

Контекстом данных панели всегда является `StrategyDraftViewModel` \- та самая модель, которую редактирует пользователь. Через неё доступны название стратегии, её параметры, настройки прогона и текущее состояние, поэтому собственные элементы привязываются к ней напрямую, без обращения к главному окну.

## Привязка панели к стратегии

Какая панель показывается для какой стратегии, решает `StrategyDraftViewLocator`. Там перечислены пары «область \+ вид шаблона \- панель»:

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

Две последние строки \- это и есть панели по умолчанию: стратегия, для которой ничего не добавлено, получает их.

## Что показывать не нужно

График сделок, статистику, кривую доходности, заявки и собственные сделки рисовать в своей панели не нужно \- это отдельные панели рядом, и они наполняются сами для любой стратегии.

## См. также

[Создание собственной стратегии](create_strategy.md)
