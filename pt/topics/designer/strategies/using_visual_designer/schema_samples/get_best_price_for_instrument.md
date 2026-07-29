# Obter o melhor preço para o instrumento

Para registar uma ordem de compra ao melhor preço atual do instrumento, pode ser utilizado o seguinte esquema:

![Designer Obter as melhores cotações para a ferramenta 00](../../../../../images/designer_get_best_quote_for_instrument_00.png)

Para o cubo [Variável](../elements/data_sources/variable.md), é selecionado o tipo de dados **Instrumento**. Se o instrumento não for especificado, mas a opção **Parâmetros** do grupo de propriedades **Geral** estiver definida, então será retirado da estratégia e passado para o cubo [Livro de ordens](../elements/market_depths/order_book.md). O cubo [Livro de ordens](../elements/market_depths/order_book.md), depois de receber o instrumento atual a partir da variável, passa as alterações do livro de ordens do instrumento selecionado através do parâmetro de saída. Ao receber alterações do livro de ordens, o cubo [Conversor](../elements/converters/converter.md) escolhe delas o valor atual do melhor preço de compra.

## Conteúdo recomendado

[Obter posição atual](get_current_position.md)
