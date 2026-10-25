# ストラテジーパネルの作成

[Shell](../shell.md) の各ストラテジーには、2 つのパネルがあります。[エミュレーション](user_interface/emulation.md) 領域の設定パネルと、[リアルタイム](user_interface/real_time.md) 領域の監視パネルです。既定では、どちらも共通のパネルで、パラメーターのテーブルと、エミュレーションの場合は履歴の設定タブがあります。独自のパネルが必要になるのは、ストラテジーにそれ以外にも表示するものがある場合です。

## ひな形

パネルは `Shell.Avalonia/Shell.Avalonia/Views/Strategies/` にあります。独自のパネルは、2 つの共通パネルのいずれかを内部で使う、通常の Avalonia の `UserControl` です。

```xaml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:views="using:Shell.Views.Strategies"
             xmlns:strategies="using:Shell.ViewModels.Strategies"
             x:Class="Shell.Views.Strategies.MyTestingView"
             x:DataType="strategies:StrategyDraftViewModel">
  <views:TestingView Header="マイストラテジー、テスト" />
</UserControl>
```

`TestingView` は設定パネル、`MonitoringView` は監視パネルです。`Header` プロパティは、パネルがタイトルバーに表示する自分の名前です。

パネルのデータコンテキストは、常に `StrategyDraftViewModel`、つまりユーザーが編集するモデルそのものです。これを通じて、ストラテジーの名前、パラメーター、実行の設定、現在の状態にアクセスできるため、独自の要素はメインウィンドウを介さずに、このモデルへ直接バインドします。

## パネルとストラテジーの関連付け

どのストラテジーにどのパネルを表示するかは、`StrategyDraftViewLocator` が決めます。そこには「領域 + テンプレートの種類 — パネル」の組み合わせが列挙されています。

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

最後の 2 行が既定のパネルです。何も追加されていないストラテジーには、これらのパネルが割り当てられます。

## 表示する必要がないもの

約定のチャート、統計、収益曲線、注文、自己約定を独自のパネルに描画する必要はありません。これらは隣にある別のパネルで、どのストラテジーについても自動的に内容が表示されます。

## 推奨コンテンツ

[ストラテジーの作成](create_strategy.md)
