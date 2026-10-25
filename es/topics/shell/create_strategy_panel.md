# Crear panel de estrategia

Cada estrategia de [Shell](../shell.md) tiene dos paneles: el panel de configuración en el área [Emulación](user_interface/emulation.md) y el panel de supervisión en el área [Tiempo real](user_interface/real_time.md). De forma predeterminada, ambos son comunes: la tabla de parámetros y, para la emulación, una pestaña con la configuración del historial. Un panel propio hace falta cuando la estrategia tiene algo más que mostrar.

## Plantilla

Los paneles están en `Shell.Avalonia/Shell.Avalonia/Views/Strategies/`. Uno propio es un `UserControl` normal de Avalonia, dentro del cual se usa uno de los dos paneles comunes:

```xaml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:views="using:Shell.Views.Strategies"
             xmlns:strategies="using:Shell.ViewModels.Strategies"
             x:Class="Shell.Views.Strategies.MyTestingView"
             x:DataType="strategies:StrategyDraftViewModel">
  <views:TestingView Header="Mi estrategia, prueba" />
</UserControl>
```

`TestingView` es el panel de configuración y `MonitoringView`, el panel de supervisión. La propiedad `Header` es el texto con el que el panel se nombra a sí mismo en su barra de título.

El contexto de datos del panel es siempre `StrategyDraftViewModel`, el mismo modelo que edita el usuario. A través de él están disponibles el nombre de la estrategia, sus parámetros, la configuración de la ejecución y el estado actual, por lo que los elementos propios se enlazan con él directamente, sin recurrir a la ventana principal.

## Vinculación del panel con la estrategia

Qué panel se muestra para qué estrategia lo decide `StrategyDraftViewLocator`. Allí se enumeran los pares «área \+ tipo de plantilla \- panel»:

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

Las dos últimas líneas son precisamente los paneles predeterminados: una estrategia para la que no se ha añadido nada los recibe.

## Qué no hace falta mostrar

No hace falta dibujar en el propio panel el gráfico de operaciones, las estadísticas, la curva de rentabilidad, las órdenes ni las operaciones propias: son paneles aparte situados al lado, y se rellenan solos para cualquier estrategia.

## Contenido recomendado

[Crear estrategia](create_strategy.md)
