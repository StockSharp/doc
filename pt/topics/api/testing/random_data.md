# Dados aleatórios

O teste com dados aleatórios é um tipo especial de teste. Não se destina a procurar os parâmetros ótimos. Em vez disso, este tipo de teste permite detetar erros no código ao expor o algoritmo de negociação a vários cenários de bolsa. 

Regra geral, apenas um determinado conjunto de cenários é usado no desenvolvimento do algoritmo. Por isso, quando ocorre uma situação especial, o algoritmo pode não reagir corretamente ou lançar exceções. Por exemplo, podem ocorrer as seguintes situações: 

- A estratégia trabalha com velas [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) e espera, em cada iteração, que exista sempre uma vela para o período de tempo solicitado. Um período começou quando não houve uma única transação, e a vela não foi criada. Como resultado, na ausência de tratamento adequado, será lançada a [NullReferenceException](xref:System.NullReferenceException) e a estratégia irá parar.
- A estratégia trabalha com um instrumento pouco líquido e usa [IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage). A estratégia espera que o livro de ordens esteja sempre preenchido. Em determinado momento, o livro de ordens está preenchido apenas de um lado (por exemplo, há bids, mas não há offers). Se a estratégia não estiver preparada para esta situação, irá registar uma ordem incorretamente ou lançar uma exceção e parar. 
- A estratégia calcula os níveis de preço. O código está escrito de tal forma que a estratégia aguarda que os níveis definidos antecipadamente sejam rompidos. Se os níveis forem calculados e definidos incorretamente, nunca serão rompidos, ou apenas um deles será sempre rompido. Como resultado, a estratégia não executará quaisquer transações ou estas transações perderão dinheiro. 

Para estes e muitos outros cenários de funcionamento da bolsa, impossíveis de prever antecipadamente, o [S#](../../api.md) fornece testes com dados aleatórios, capazes de gerar o número máximo de condições num intervalo curto devido à sua diversidade uniforme. 

## Teste com dados aleatórios da estratégia de médias móveis

1. O exemplo SampleRandomEmulation (*..Samples\/Testing\/SampleRandomEmulation*) é quase idêntico ao exemplo SampleHistoryTesting (a sua descrição encontra-se na secção [teste com dados históricos](historical_data.md)) devido ao uso da classe unificada [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector). Mas, ao contrário do [teste com dados históricos](historical_data.md), no teste com dados aleatórios os dados de mercado não são carregados e são gerados dinamicamente. Por isso, são adicionados ao exemplo dois geradores de dados aleatórios: um para o livro de ordens e outro para as tick trades. Em SampleHistoryTesting é usado apenas um gerador - para o livro de ordens, uma vez que não há histórico armazenado.

   ```cs
   _connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
   {
       IsSubscribe = true,
       Generator = new RandomWalkTradeGenerator(new SecurityId { SecurityCode = security.Code })
       {
           Interval = TimeSpan.FromSeconds(1),
           MaxVolume = maxVolume,
           MaxPriceStepCount = 3,	
           GenerateOriginSide = true,
           MinVolume = minVolume,
           RandomArrayLength = 99,
       }
   });
   _connector.SubscribeMarketDepth(new TrendMarketDepthGenerator(_connector.GetSecurityId(security)) { GenerateDepthOnEachTrade = false });
   ```
2. O resultado do funcionamento do exemplo é o seguinte: ![Exemplo de teste de emulação](../../../images/sample_emulation_test.png)
