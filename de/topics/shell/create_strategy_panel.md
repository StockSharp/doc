# Strategiepanel erstellen

Jede Strategie in [Shell](../shell.md) hat zwei Panels: das Einrichtungspanel im Bereich [Emulation](user_interface/emulation.md) und das Beobachtungspanel im Bereich [Echtzeit](user_interface/real_time.md). Standardmäßig sind beide allgemein: die Parametertabelle und, für die Emulation, eine Registerkarte mit den Einstellungen der Historie. Ein eigenes Panel ist nötig, wenn die Strategie darüber hinaus etwas zu zeigen hat.

## Vorlage

Die Panels liegen in `Shell.Avalonia/Shell.Avalonia/Views/Strategies/`. Ein eigenes Panel ist ein gewöhnliches `UserControl` von Avalonia, in dem eines der beiden allgemeinen Panels verwendet wird:

```xaml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:views="using:Shell.Views.Strategies"
             xmlns:strategies="using:Shell.ViewModels.Strategies"
             x:Class="Shell.Views.Strategies.MyTestingView"
             x:DataType="strategies:StrategyDraftViewModel">
  <views:TestingView Header="Meine Strategie, Test" />
</UserControl>
```

`TestingView` ist das Einrichtungspanel, `MonitoringView` das Beobachtungspanel. Die Eigenschaft `Header` ist die Beschriftung, mit der sich das Panel in seiner Titelzeile nennt.

Der Datenkontext des Panels ist immer `StrategyDraftViewModel` — genau das Modell, das der Benutzer bearbeitet. Darüber sind der Name der Strategie, ihre Parameter, die Einstellungen des Durchlaufs und der aktuelle Zustand zugänglich, deshalb werden eigene Elemente direkt daran gebunden, ohne auf das Hauptfenster zuzugreifen.

## Panel an die Strategie binden

Welches Panel für welche Strategie angezeigt wird, entscheidet `StrategyDraftViewLocator`. Dort sind die Paare „Bereich + Art der Vorlage - Panel“ aufgeführt:

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

Die letzten beiden Zeilen sind die Standardpanels: Eine Strategie, für die nichts hinzugefügt wurde, erhält sie.

## Was nicht angezeigt werden muss

Das Trade-Chart, die Statistik, die Equity-Kurve, die Orders und die eigenen Trades müssen in Ihrem Panel nicht gezeichnet werden — das sind eigene Panels daneben, und sie werden für jede Strategie von selbst gefüllt.

## Empfohlene Inhalte

[Strategie erstellen](create_strategy.md)
