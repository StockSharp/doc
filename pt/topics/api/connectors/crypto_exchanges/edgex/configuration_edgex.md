# Configuração edgeX

Para trabalhar com o conector, gere a **API Key** e o **Secret** na conta da bolsa e especifique-os nas definições de ligação.

Definições principais:

- **Key** e **Secret**.
- **Clearing account** e **Passphrase**.
- **Section**: `Spot` ou `Derivatives`.
- **Enable spot**: ativa a secção spot quando o suporte da API estiver disponível.
- Modo **Demo**.
- Endpoints **Spot REST / Derivatives REST**.
- Endpoints **Spot WS / Derivatives public WS / Derivatives private WS**.

Documentação oficial da API:

- [Authentication](https://edgex-1.gitbook.io/edgex-documentation/developer/api/authentication)
- [Order API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/order-api)
- [Account API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/account-api)
- [Private websocket stream](https://edgex-1.gitbook.io/edgex-documentation/developer/api/private-api/private-websocket-stream)
- [Funding API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/funding-api)
- [Meta-data API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/meta-data-api)
- [Quote API](https://edgex-1.gitbook.io/edgex-documentation/developer/api/public-api/quote-api)

> [!TIP]
> `Derivatives` está totalmente implementado. `Spot` deve ser ativado apenas quando o ambiente da API de destino o suportar.
