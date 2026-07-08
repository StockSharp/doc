# Modelo de eventos

A abordagem de criação de esquemas no [Designer](../../../designer.md) baseia-se na geração e posterior processamento de eventos. Ao construir uma estratégia, não se sabe quando ocorrerá um evento de alteração dos dados de mercado, mas é possível subscrever esse evento e processá-lo em conformidade.

Cada cubo do [Designer](../../../designer.md) que tenha um parâmetro de saída é o gerador do evento. E os cubos que têm um parâmetro de entrada podem subscrever o evento gerado pelo parâmetro de saída. Subscrever um evento não é mais do que criar uma linha de ligação entre dois cubos.

Por exemplo, o cubo [Livro de ofertas](elements/market_depths/order_book.md) gera um evento de alteração do livro de ordens. Não se sabe antecipadamente quando ocorrerá uma alteração. Ao criar uma linha de ligação entre o cubo [Livro de ofertas](elements/market_depths/order_book.md) e o cubo [Conversor](elements/converters/converter.md), é efectuada uma subscrição da alteração do livro de ordens para posterior processamento com o cubo [Conversor](elements/converters/converter.md), etc.:

![Designer Event model 00](../../../../images/designer_event_model_00.png)

## Conteúdo recomendado

[Primeira estratégia](first_strategy.md)
