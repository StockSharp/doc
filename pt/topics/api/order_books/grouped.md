# Livro de Ofertas Agrupado

Além do [livro de ofertas esparso](sparse.md), pode ser útil usar um livro de ofertas agrupado, no qual as ordens são agregadas em intervalos de preço mais amplos para simplificar a análise e identificar tendências gerais de procura e oferta.

Vantagens de um livro de ofertas agrupado:

- **Análise simplificada:** A agregação dos dados das ordens simplifica a perceção da imagem geral do mercado.
- **Identificação de tendências:** É mais fácil identificar níveis de preço importantes onde a maioria das ordens está concentrada.

## Implementação de um livro de ofertas agrupado:

Para trabalhar com um livro de ofertas agrupado, é necessário configurar primeiro a receção através de [subscrições](subscriptions.md) e, em seguida, chamar o método de extensão [Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)):

```cs
// Agrupar dados do livro de ofertas com passo de agregação de preço, por exemplo, 0,5 unidade de preço
var groupedDepth = orderBook.Group(0.5);

// groupedDepth agora contém um livro de ofertas no qual as ordens são agrupadas
// por níveis de preço com o passo de agregação especificado.
```

O método [Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)) permite agregar ordens no livro em níveis de preço maiores, simplificando a análise visual do mercado e ajudando a identificar os principais níveis de procura e oferta sem necessidade de analisar cada alteração individual de preço.
