# Obter o melhor preço para o instrumento

Para registar uma ordem de compra ao melhor preço actual do instrumento, pode ser utilizado o seguinte esquema:

![Designer Get the best rates for the tool 00](../../../../../images/designer_get_best_quote_for_instrument_00.png)

Para o cubo [Variable](../elements/data_sources/variable.md), é seleccionado o tipo de dados **Instrument**. Se o instrumento não for especificado, mas a flag **Parameters** do grupo de propriedades **Common** estiver definida, então será retirado da estratégia e passado para o cubo [Order book](../elements/market_depths/order_book.md). O cubo [Order book](../elements/market_depths/order_book.md), depois de receber o instrumento actual a partir da variável, passa as alterações do livro de ordens do instrumento seleccionado através do parâmetro de saída. Ao receber alterações do livro de ordens, o cubo [Converter](../elements/converters/converter.md) escolhe delas o valor actual do melhor preço de compra.

## Conteúdo recomendado

[Obter posição actual](get_current_position.md)
