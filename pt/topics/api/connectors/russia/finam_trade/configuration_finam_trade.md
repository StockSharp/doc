# Configuração do conector: API de negociação da Finam

Configure as propriedades a seguir antes de se conectar à Finam. A lista foi verificada com [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Token` (`SecureString`) — segredo obrigatório da API de negociação da Finam. O adaptador o troca por um token de sessão de curta duração.
- `AccountId` (`string`) — identificador opcional da conta de negociação. Quando vazio, o adaptador usa a primeira conta disponível para o token.

## Configurações avançadas

- `AppId` (`string`) — identificador do aplicativo enviado durante a criação da sessão. O padrão é `StockSharp`.
- `PollingInterval` (`TimeSpan`) — intervalo para consultar instantâneos da conta e das ordens. O padrão é 30 segundos; valores inferiores a um segundo são rejeitados.
- `LookupLimit` (`int`) — quantidade máxima de instrumentos retornados por uma pesquisa sem restrições. O padrão é `10000` e o valor deve ser positivo.
- `RestAddress` (`string`) — endereço base da API REST. O padrão é `https://api.finam.ru/`.
- `WebSocketAddress` (`string`) — endereço da API WebSocket. O padrão é `wss://api.finam.ru/ws`.

Mantenha os endereços preenchidos, salvo se a Finam ou um gateway compatível tiver atribuído outros pontos de conexão.

## Veja também

[Configuração gráfica](graphical_configuration_finam_trade.md)

[Inicialização do adaptador](adapter_initialization_finam_trade.md)
