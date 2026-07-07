# Livro de Ofertas Agrupado

Além do [livro de ofertas esparso](sparse.md), pode ser útil usar um livro de ofertas agrupado, no qual as ordens são agregadas em intervalos de preço mais amplos para simplificar a análise e identificar tendências gerais de procura e oferta.

Vantagens de um livro de ofertas agrupado:

- **Análise simplificada:** A agregação dos dados das ordens simplifica a perceção da imagem geral do mercado.
- **Identificação de tendências:** É mais fácil identificar níveis de preço importantes onde a maioria das ordens está concentrada.

## Implementação de um livro de ofertas agrupado:

Para trabalhar com um livro de ofertas agrupado, é necessário configurar primeiro a receção através de [subscrições](subscriptions.md) e, em seguida, chamar o método de extensão [Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)):

```cs
// Grouping order book data with a price aggregation step, for example, 0.5 units of price
var groupedDepth = orderBook.Group(0.5);

// groupedDepth now contains an order book in which orders are grouped
// by price levels with the specified aggregation step.
```

O método [Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)) permite agregar ordens no livro em níveis de preço maiores, simplificando a análise visual do mercado e ajudando a identificar os principais níveis de procura e oferta sem necessidade de analisar cada alteração individual de preço.
