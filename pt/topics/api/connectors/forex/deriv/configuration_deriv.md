# Configuração do conector: Deriv

Crie um token da Deriv e informe os parâmetros de conexão.

- `Token` - token de acesso pessoal ou OAuth. Obrigatório para operações protegidas.
- `AppId` - identificador da aplicação enviado com as requisições REST autenticadas.
- `AccountId` - identificador da conta de opções. Opcional quando exatamente uma conta ativa corresponde ao modo selecionado.
- `IsDemo` - seleciona a conta de demonstração. O padrão é `true`.
- `RestAddress` - ponto de conexão REST. O padrão é `https://api.derivws.com`.
- `PublicWebSocketAddress` - ponto de conexão WebSocket público de opções. O padrão é `wss://api.derivws.com/trading/v1/options/ws/public`.

As sessões públicas de dados de mercado funcionam sem token, enquanto contratos, saldos e transações exigem o token e o identificador da aplicação. As assinaturas são restauradas automaticamente em um novo endereço WebSocket de uso único.

Os parâmetros do contrato são informados por meio de [DerivOrderCondition](xref:StockSharp.Deriv.DerivOrderCondition): tipo do contrato, se o valor é uma aposta ou um pagamento, moeda do contrato, duração, barreiras e os preços de proteção de stop-loss e take-profit.

## Veja também

[Documentação oficial da API Deriv](https://developers.deriv.com/docs/)
