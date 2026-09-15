# Temas dos componentes gráficos S#

Todos os componentes gráficos S# estão desenhados em dois temas \- o claro e o escuro. O tema define-se uma única vez, ao nível da aplicação, e todos os componentes o assumem de imediato: as cores vêm dos recursos e não estão escritas em cada componente.

Tema claro:

![temas da API GUI 01](../../../images/api_gui_thems_01.png)

Tema escuro:

![temas da API GUI 02](../../../images/api_gui_thems_02.png)

Para definir o tema basta uma linha:

```cs
...
ThemeExtensions.ApplyDefaultTheme();
...
```

**Métodos principais de [ThemeExtensions](xref:StockSharp.Xaml.ThemeExtensions)**

- [ThemeExtensions.ApplyDefaultTheme](xref:StockSharp.Xaml.ThemeExtensions.ApplyDefaultTheme(System.Boolean)) \- aplica o tema escuro ou o claro.
- [ThemeExtensions.Invert](xref:StockSharp.Xaml.ThemeExtensions.Invert) \- muda o tema para o oposto.
- [ThemeExtensions.IsCurrDark](xref:StockSharp.Xaml.ThemeExtensions.IsCurrDark) \- se o tema atual é escuro ou não.

A mudança de tema tem efeito imediato: não é preciso reiniciar a aplicação, todos os painéis e gráficos abertos são redesenhados nas novas cores.

As cores próprias dos componentes de negociação \- subida e descida, compra e venda, níveis do livro de ofertas, grelha das tabelas \- vivem num conjunto de recursos separado e mudam juntamente com o tema. Por isso um componente próprio que retire as cores desse conjunto, em vez de as definir por si, ficará igualmente bem nos dois temas.
