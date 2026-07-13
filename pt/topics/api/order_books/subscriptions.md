# Subscrições

## Subscrever o Livro de Ofertas

Para subscrever o livro de ofertas no StockSharp, é necessário executar os seguintes passos:

1. Subscrever o evento de receção de livros de ofertas [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) e processar objetos da interface [IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage):

```cs
// manipulador de evento
private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// Aqui você pode processar os dados do livro de ofertas, por exemplo, exibí-los na tela ou usá-los na estratégia
	Console.WriteLine($"Livro de ordens recebido para {orderBook.SecurityId}. Melhor preço de compra: {orderBook.GetBestBid()?.Price}, melhor preço de venda: {orderBook.GetBestAsk()?.Price}");
}

// assinatura do evento
connector.OrderBookReceived += OnOrderBookReceived;
```

É importante subscrever o evento [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) **antes** de enviar um pedido de subscrição para o livro de ofertas. Isto garante que não perde dados se os livros de ofertas começarem a chegar muito rapidamente após o envio do pedido de subscrição.

2. Envie um pedido de subscrição usando o método [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)):

```cs
var security = GetSecurity(); // Obter o objeto Security que pretende subscrever

// assinar o livro de ofertas
var subscription = new Subscription(DataType.MarketDepth, security);
connector.Subscribe(subscription);
```

## Cancelar a Subscrição do Livro de Ofertas

Para cancelar a subscrição do livro de ofertas, chame o método [Connector.UnSubscribe](xref:StockSharp.Algo.Connector.UnSubscribe(StockSharp.BusinessEntities.Subscription)):

```cs
connector.UnSubscribe(subscription);
```

## Esclarecimento Sobre a Receção de Livros de Ofertas

Ao trabalhar com o evento [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived), é importante compreender que os livros de ofertas que chegam através deste evento já estão compilados e prontos a usar. Isto significa que, independentemente do método de transmissão de dados pela fonte - sejam dados diferenciais (apenas alterações no livro de ofertas) ou instantâneos completos do livro de ofertas - a plataforma StockSharp processa estes dados de modo que o operador recebe um livro de ofertas completo e atualizado.

A plataforma integra automaticamente as alterações no livro de ofertas, atualizando o seu conteúdo para o estado atual antes de chamar o evento [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived). Isto simplifica o trabalho com dados, pois os operadores não precisam de processar dados diferenciais de forma independente nem compilar o livro de ofertas a partir de instantâneos consecutivos. Assim, pode ter a certeza de que os dados recebidos no manipulador de eventos refletem o estado mais recente do livro de ofertas no momento do evento.

Isto simplifica significativamente o desenvolvimento de estratégias de negociação e a análise de mercado, pois os operadores podem concentrar-se diretamente na lógica das suas estratégias, sem gastar tempo nos aspetos técnicos de compilação e processamento dos dados do livro de ofertas.

## Exemplo de Utilização

Exemplos de utilização do livro de ofertas estão disponíveis no projeto *Samples\/01\_Basic\/02\_MarketDepths* no [GitHub](https://github.com/StockSharp/StockSharp/) ou no arquivo da API StockSharp, que pode ser obtido através do [Instalador](../../installer.md). Estes exemplos fornecem ilustrações práticas de ligação a um sistema de negociação, subscrição de um livro de ofertas filtrado e processamento dos dados recebidos, podendo servir como um bom ponto de partida para desenvolver as suas próprias estratégias de negociação.

## Ver Também

[Subscrições](../market_data/subscriptions.md)
