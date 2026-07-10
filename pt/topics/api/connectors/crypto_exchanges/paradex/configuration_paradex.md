# Configuração Paradex

Para trabalhar com o conector, especifique as suas credenciais de API e as definições de autenticação Starknet.

Definições principais:

- **chave** e **segredo**.
- **Conta Starknet** e **Chave Starknet**.
- **Secção**: `Spot` ou `Derivatives`.
- **Ativar spot**: ativa a secção spot quando o suporte da API estiver disponível.
- Modo de **demonstração**.
- Endpoints **Spot REST / Derivatives REST**.
- Endpoints **Spot WS / Derivatives WS**.
- **Caminho de autenticação** (predefinição: `/v1/auth`).

Documentação oficial da API:

- [API URLs](https://docs.paradex.trade/api/prod/api-urls)
- [Authentication](https://docs.paradex.trade/api/prod/authentication)
- [REST API](https://docs.paradex.trade/api/prod/rest-api)
- [Criar uma nova ordem](https://docs.paradex.trade/api/prod/orders/create-a-new-order)
- [Introdução ao WebSocket](https://docs.paradex.trade/api/prod/websocket/introduction)
- [Canais WebSocket](https://docs.paradex.trade/api/prod/websocket/channels)
- [Canal do livro de ofertas](https://docs.paradex.trade/api/prod/websocket/channels/order_book_channel)

> [!TIP]
> Os derivados da Paradex são totalmente suportados. Ative `Spot` apenas quando o ambiente da API de destino confirmar suporte para spot.

