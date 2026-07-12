# Obter nível de preço do livro de ordens

Para obter a linha de compra necessária do livro de ordens, pode ser utilizado o seguinte esquema:

![Designer modelo de eventos 00](../../../../../images/designer_event_model_00.png)

O tipo de dados **Instrumento** é seleccionado para o cubo [Variável](../elements/data_sources/variable.md). Se o instrumento não for especificado, mas a opção **Parâmetros** do grupo de propriedades **Geral** estiver definida, então será retirado da estratégia. Para o cubo [Conversor](../elements/converters/converter.md), são seleccionados o tipo de dados e o campo correspondente da colecção de cubos para a compra Bids. O cubo indexador obtém o elemento necessário da colecção dos melhores preços de compra. Para obter um determinado valor de preço ou volume num nível, pode utilizar o cubo [Conversor](../elements/converters/converter.md).

## Conteúdo recomendado

[Galeria](../../../strategy_gallery.md)
