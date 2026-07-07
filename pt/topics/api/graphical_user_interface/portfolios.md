# Portfólios

Para trabalhar com portfólios, o [S#](../../api.md) disponibiliza os seguintes componentes gráficos:

- A caixa de combinação para selecionar portfólios - [PortfolioComboBox](xref:StockSharp.Xaml.PortfolioComboBox).
- A caixa de combinação com um botão que abre uma janela com uma lista de portfólios - [PortfolioEditor](xref:StockSharp.Xaml.PropertyGrid.PortfolioEditor).
- Janela da lista de portfólios - [PortfolioPickerWindow](xref:StockSharp.Xaml.PortfolioPickerWindow).
- Uma tabela que apresenta informações sobre portfólios e posições - [PortfolioGrid](xref:StockSharp.Xaml.PortfolioGrid).

> [!TIP]
> Tenha em atenção que, em todos os componentes para a lista de portfólios (a propriedade Portfolios), é usada a classe **ThreadSafeObservableCollection\<TItem\>** (localizada em Ecng.Xaml), que garante segurança de threads.

## Conteúdo recomendado

[Lista pendente](portfolios/drop_down_list.md)

[Seletor](portfolios/picker.md)

[Janela de seleção de portfólio](portfolios/portfolio_picker_window.md)

[Tabela](portfolios/table.md)
