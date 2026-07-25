# Configuração do conector: StocksTrader

Gere um token no terminal web do StocksTrader e informe os parâmetros de conexão.

- `Token` - token bearer emitido pelo terminal web.
- `AccountId` - identificador da conta. Opcional quando exatamente uma conta corresponde ao modo selecionado.
- `IsDemo` - seleciona a conta de demonstração. O padrão é `true`.
- `Address` - ponto de conexão REST. O padrão é `https://api.stockstrader.com/`.
- `PollingInterval` - frequência com que ordens, negócios e o estado da conta são solicitados. O padrão é 5 segundos, e valores menores são elevados para 2 segundos.

Como o provedor não oferece fluxo de dados em tempo real, `PollingInterval` define a rapidez com que as alterações de ordens e posições chegam à estratégia. Reduza-o para negociação ativa e aumente-o para respeitar os limites de requisições do provedor.

Os preços de proteção são informados por meio de [StocksTraderOrderCondition](xref:StockSharp.StocksTrader.StocksTraderOrderCondition): os preços de stop-loss e take-profit da ordem ou da posição aberta.

## Veja também

[Documentação oficial da API StocksTrader](https://api-doc.stockstrader.com/)
