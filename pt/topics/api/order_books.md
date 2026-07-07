# Livro de Ofertas

## Descrição

O livro de ofertas (também conhecido como profundidade de mercado) é informação sobre as ordens de compra e venda atuais para um determinado instrumento, organizada por níveis de preço. No StockSharp, o livro de ofertas fornece dados sobre procura e oferta, permitindo análise de mercado em tempo real.

## Estrutura

O [Livro de Ofertas](xref:StockSharp.Messages.IOrderBookMessage) contém duas listas de ordens:

- Ordens de compra, ordenadas por preço decrescente - [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids).
- Ordens de venda, ordenadas por preço crescente - [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks).

Cada ordem inclui um preço e um volume.

## Utilização

Os dados do livro de ofertas são usados para:

- Identificar níveis de preço com volumes máximos de ordens, que podem indicar potenciais níveis de suporte ou resistência.
- Avaliar a liquidez de mercado de um instrumento.
- Desenvolver estratégias de negociação baseadas na análise de alterações no livro de ofertas.

## Obtenção de Dados

No StockSharp, a subscrição de dados do livro de ofertas e a receção de atualizações são feitas através dos [métodos da API](order_books/subscriptions.md) correspondentes.
