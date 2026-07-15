# Configuração do conector TradeZero

Crie credenciais de API no portal da TradeZero e indique-as nas definições do conector.

- **Chave** - o valor `TZ-API-KEY-ID`.
- **Segredo** - o valor `TZ-API-SECRET-KEY`.
- **Rota predefinida** - uma rota de ordens preferencial opcional devolvida pelo ponto de acesso das rotas da conta.

Se a rota predefinida estiver vazia, o conector seleciona automaticamente uma rota ativa compatível. As contas de simulação e reais utilizam o mesmo anfitrião de API; as credenciais determinam o ambiente da conta.
