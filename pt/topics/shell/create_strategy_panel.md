# Criar painel de estratégia

Cada estratégia do [Shell](../shell.md) tem dois painéis: o painel de configuração na área [Emulação](user_interface/emulation.md) e o painel de acompanhamento na área [Tempo real](user_interface/real_time.md). Por predefinição, ambos são comuns: a tabela de parâmetros e, para a emulação, um separador com as definições do histórico. Um painel próprio é necessário quando a estratégia tem algo mais a mostrar.

## Modelo

Os painéis estão em `Shell.Avalonia/Shell.Avalonia/Views/Strategies/`. Um próprio é um `UserControl` normal do Avalonia, dentro do qual se usa um dos dois painéis comuns:

```xaml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:views="using:Shell.Views.Strategies"
             xmlns:strategies="using:Shell.ViewModels.Strategies"
             x:Class="Shell.Views.Strategies.MyTestingView"
             x:DataType="strategies:StrategyDraftViewModel">
  <views:TestingView Header="A minha estratégia, teste" />
</UserControl>
```

`TestingView` é o painel de configuração e `MonitoringView`, o painel de acompanhamento. A propriedade `Header` é o texto com que o painel se designa a si próprio na sua barra de título.

O contexto de dados do painel é sempre `StrategyDraftViewModel`, o mesmo modelo que o utilizador edita. Através dele estão disponíveis o nome da estratégia, os seus parâmetros, as definições da execução e o estado atual, pelo que os elementos próprios se ligam a ele diretamente, sem recorrer à janela principal.

## Associação do painel à estratégia

Que painel é mostrado para que estratégia decide-o `StrategyDraftViewLocator`. Aí são enumerados os pares «área \+ tipo de modelo \- painel»:

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

As duas últimas linhas são precisamente os painéis predefinidos: uma estratégia para a qual não foi adicionado nada recebe-os.

## O que não é preciso mostrar

Não é preciso desenhar no próprio painel o gráfico de negócios, as estatísticas, a curva de rendibilidade, as ordens nem os negócios próprios: são painéis à parte ao lado, e enchem-se sozinhos para qualquer estratégia.

## Conteúdo recomendado

[Criar estratégia](create_strategy.md)
