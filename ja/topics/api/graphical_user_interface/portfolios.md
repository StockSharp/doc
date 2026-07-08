# ポートフォリオ

ポートフォリオを扱うために、[S#](../../api.md) は次のグラフィカルコンポーネントを提供します。

- ポートフォリオ選択用のコンボボックス - [PortfolioComboBox](xref:StockSharp.Xaml.PortfolioComboBox)。
- ポートフォリオ一覧のウィンドウを開くボタン付きコンボボックス - [PortfolioEditor](xref:StockSharp.Xaml.PropertyGrid.PortfolioEditor)。
- ポートフォリオ一覧ウィンドウ - [PortfolioPickerWindow](xref:StockSharp.Xaml.PortfolioPickerWindow)。
- ポートフォリオとポジションに関する情報を表示するテーブル - [PortfolioGrid](xref:StockSharp.Xaml.PortfolioGrid)。

> [!TIP]
> ポートフォリオ一覧（Portfolios プロパティ）用のすべてのコンポーネントでは、スレッドセーフ性を提供する **ThreadSafeObservableCollection\<TItem\>** クラス（Ecng.Xaml にあります）が使用されることに注意してください。 

## 推奨コンテンツ

[ドロップダウンリスト](portfolios/drop_down_list.md)

[ピッカー](portfolios/picker.md)

[ポートフォリオピッカーウィンドウ](portfolios/portfolio_picker_window.md)

[テーブル](portfolios/table.md)
