# Configuração do conector Bit2Me

Os dados públicos de mercado estão disponíveis sem credenciais. Informe uma chave de API e um segredo para operações de conta e negociação.

## Parâmetros de conexão

- `Key` — chave de API do Bit2Me.
- `Secret` — segredo de API do Bit2Me.
- `RestEndpoint` — endereço da API REST. Padrão de produção: `https://gateway.bit2me.com`.
- `WebSocketEndpoint` — endereço WebSocket. Padrão de produção: `wss://ws.bit2me.com/v1/trading`.

O conector suporta ordens a mercado, limitadas e stop-limit. As assinaturas públicas WebSocket fornecem negócios e atualizações completas do livro de nível 2; as velas são baixadas via REST.

## Documentação oficial da API

- [API Bit2Me](https://api.bit2me.com/)
- [Exemplos de negociação Bit2Me](https://github.com/bit2me-devs/trading-spot-samples)
