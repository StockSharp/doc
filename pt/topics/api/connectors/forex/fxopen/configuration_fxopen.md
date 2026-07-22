# Configuração do conector: FXOpen TickTrader

Crie um token Web API da FXOpen e informe os parâmetros de conexão.

- `WebApiId` - identificador do token Web API.
- `Key` - chave da Web API.
- `Secret` - segredo da Web API.
- `OneTimePassword` - senha de uso único opcional quando a autenticação de dois fatores for exigida.
- `IsDemo` - seleciona o ambiente de demonstração. O padrão é `false`.
- `Address` - ponto de conexão REST. Padrão para a conta real: `https://ttlivewebapi.fxopen.net`.
- `FeedAddress` - WebSocket de fluxo de dados. Padrão para a conta real: `wss://marginalttlivewebapi.fxopen.net/feed`.
- `TradeAddress` - WebSocket de operações. Padrão para a conta real: `wss://marginalttlivewebapi.fxopen.net/trade`.

Ao ativar `IsDemo`, os pontos de conexão oficiais de demonstração do TickTrader são selecionados, exceto quando um endereço foi personalizado. Identificador, chave e segredo são obrigatórios para assinaturas WebSocket e operações protegidas.

## Veja também

[Documentação oficial da API FXOpen](https://ticktrader.fxopen.com/api)

[API Web REST do TickTrader](https://ttlivewebapi.fxopen.net/api/doc/index?apiaddress=ttlivewebapi.fxopen.net&apiport=443)
