# Livro de ordens esparso

Um livro de ofertas esparso é uma apresentação do livro de ofertas que mostra todos os níveis de preço possíveis, incluindo aqueles que não têm ordens ativas no momento. Esta abordagem permite aos traders avaliar visualmente os "intervalos" entre ordens, ou seja, níveis de preço onde não existem ordens de compra ou venda, oferecendo uma perspetiva sobre potenciais níveis de resistência ou suporte.

## Porquê Usar um Livro de Ofertas Esparso

Usar um livro de ofertas esparso tem várias vantagens:

1. **Visualização de intervalos:** É mais fácil identificar níveis de preço sem ordens, o que pode indicar potenciais pontos de entrada ou saída.
2. **Análise de liquidez:** Uma representação clara da distribuição das ordens ajuda a avaliar a liquidez do instrumento em diferentes níveis de preço.
3. **Planeamento estratégico:** Compreender a estrutura do livro de ofertas permite planear operações de negociação com maior precisão, considerando potenciais "vazios" de liquidez.

## Criar um Livro de Ofertas Esparso

Para trabalhar com um livro de ofertas agrupado, primeiro tem de configurar a receção através de [subscrições](subscriptions.md) e, em seguida, chamar o método de extensão [Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal},System.Int32)). O método recebe os seguintes parâmetros:

- `priceRange` - a diferença de preço até à qual os níveis devem ser expandidos.
- `priceStep` - o passo de preço do instrumento de negociação. É usado caso `priceRange` tenha menor precisão nos níveis de preço do que `priceStep`, sendo necessário arredondar os preços obtidos para o passo de preço do instrumento.

```cs
// It is assumed that orderBook is an IOrderBookMessage object obtained from StockSharp
var sparseDepth = orderBook.Sparse(priceRange, priceStep);

// Now, sparseDepth contains a representation of the original order book,
// where all possible price levels are considered, including those without any orders.
```

Neste exemplo, [Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal},System.Int32)) é usado para criar um livro de ofertas esparso, que permite mostrar todos os níveis de preço, mesmo aqueles sem ordens ativas. Isto pode ser útil para analisar potenciais níveis "vazios" que podem servir como níveis de suporte ou resistência.
