# Configurações do conector ProBit Global

Informe os parâmetros de conexão do ProBit Global nas configurações do conector.

## Parâmetros de conexão

- `Key` - identificador de cliente OAuth emitido nas credenciais da API ProBit.
- `Secret` - segredo de cliente OAuth.
- `RestEndpoint` - endereço da API REST.
- `AuthEndpoint` - endereço do ponto de acesso de token OAuth.
- `WebSocketEndpoint` - endereço do servidor WebSocket.

Os dados públicos de mercado funcionam sem credenciais. Negociação, saldos, histórico de ordens e canais WebSocket privados exigem `Key` e `Secret`.

Para uma compra a mercado, defina o valor na moeda de cotação em `ProBitOrderCondition.QuoteAmount`.

## Documentação oficial da API

- [Documentação da API ProBit Global](https://docs-en.probit.com/)
- [Credenciais da API ProBit Global](https://www.probit.com/en-us/my-page/api-management/api-credential)
