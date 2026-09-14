# Portafolios

Para trabajar con portafolios, [S#](../../api.md) ofrece los siguientes componentes gráficos:

- Cuadro combinado para seleccionar un portafolio - [PortfolioComboBox](xref:StockSharp.Xaml.PortfolioComboBox).
- Cuadro combinado con un botón que abre una ventana con una lista de portafolios - [PortfolioEditor](xref:StockSharp.Xaml.PropertyGrid.PortfolioEditor).
- Ventana de lista de portafolios - [PortfolioPickerWindow](xref:StockSharp.Xaml.PortfolioPickerWindow).
- Tabla que muestra información sobre portafolios y posiciones - [PortfolioGrid](xref:StockSharp.Xaml.PortfolioGrid).
- Tabla para mostrar cambios de posiciones \- [PositionChangeGrid](xref:StockSharp.Xaml.PositionChangeGrid).

> [!TIP]
> Tenga en cuenta que en todos los componentes de la lista de portafolios (la propiedad Portfolios), se usa la clase **ThreadSafeObservableCollection\<TItem\>** (ubicada en Ecng.Xaml), que proporciona seguridad de subprocesos. 

## Contenido recomendado

[Lista desplegable](portfolios/drop_down_list.md)

[Selector](portfolios/picker.md)

[Ventana de selección de portafolios](portfolios/portfolio_picker_window.md)

[Tabla](portfolios/table.md)

[Cambios de posiciones](portfolios/position_changes.md)
