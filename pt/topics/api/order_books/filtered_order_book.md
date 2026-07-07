# Livro de Ofertas Filtrado

Um livro de ofertas filtrado é uma ferramenta especializada no StockSharp que permite aos traders e às estratégias automatizadas operar no mercado excluindo as suas próprias ordens da análise. Isto é criticamente importante ao utilizar várias estratégias em paralelo, para evitar situações em que uma estratégia começa a "negociar" com outra, sem perceber que o volume no livro de ofertas vem de outro participante de mercado ou é resultado de ações de outra estratégia em execução em paralelo.

## Vantagens do Livro de Ofertas Filtrado

- **Evitar auto-negociação:** As estratégias não executarão ordens contra si próprias nem entre si quando forem executadas em paralelo.
- **Pureza da análise:** Permite que as estratégias analisem as condições de mercado apenas com base em ordens externas, sem distorções causadas pelas suas próprias ordens.
- **Eficiência de execução:** Ajuda a melhorar a qualidade da execução de ordens, minimizando o impacto das próprias ordens no preço de mercado.

## Exemplo de Subscrição

A abordagem para trabalhar com o livro de ofertas filtrado usa o mesmo método que a [subscrição de um livro de ofertas normal](subscriptions.md), mas com um valor [DataType](xref:StockSharp.Messages.DataType) diferente. Abaixo encontra-se um exemplo que ilustra a subscrição de um livro de ofertas filtrado para um instrumento específico:

1. **Subscrever o evento de atualização do livro de ofertas:** [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) para receber atualizações do livro de ofertas. Este evento é usado tanto para o livro de ofertas normal como para o filtrado.

    Ao processar o evento, verifique [Subscription.DataType](xref:StockSharp.Messages.SubscriptionBase`1.DataType) no objeto `subscription` associado ao evento. Se [Subscription.DataType](xref:StockSharp.Messages.SubscriptionBase`1.DataType) for [DataType](xref:StockSharp.Messages.DataType.FilteredMarketDepth), isso indica que o livro de ofertas recebido está filtrado:

    ```cs
    connector.OrderBookReceived += (sender, subscription, orderBook) =>
    {
        if (subscription.DataType == DataType.FilteredMarketDepth)
        {
            // Handling logic for the filtered order book
            Console.WriteLine($"Received filtered order book for {orderBook.SecurityId}.");
        }
    };
    ```

2. **Enviar a subscrição:** Forme um objeto [Subscription](xref:StockSharp.BusinessEntities.Subscription) e envie-o para o connector:

    ```cs
    var subscription = new Subscription(DataType.FilteredMarketDepth, security);
    connector.Subscribe(subscription);
    
    // or like this
    //var subscription = connector.SubscribeFilteredMarketDepth(security);
    ```

## Conclusão

Usar o livro de ofertas filtrado no StockSharp fornece aos traders e programadores de estratégias uma ferramenta flexível para análise de mercado, permitindo evitar auto-interações indesejadas entre estratégias executadas em simultâneo e simplificando a tomada de decisões com base nos dados de ordens de mercado.
