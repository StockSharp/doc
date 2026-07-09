# Gestão de risco

Nos painéis [Definições de backtesting](components/backtesting_settings.md) e [Definições de negociação em tempo real](components/live_settings.md), pode definir as definições de controlo de risco.

Na janela Riscos, é necessário selecionar uma **Regra de risco**, configurar a condição de acionamento da **Regra de risco** e a ação (Fechar posições, Parar negociação, Cancelar ordens) que será executada quando ocorrer a condição da **Regra de risco**.

É possível usar várias regras de risco do mesmo tipo com ações diferentes. Por exemplo, na captura de ecrã abaixo, se o volume da ordem for 20, são executadas as ações de cancelar ordens e parar a negociação.

![Designer Risk Rule](../../../images/designer_risk_rule.png)

### Lista de regras de risco

Lista de regras de risco

- **P/L** - uma regra de risco que monitoriza o tamanho do lucro/prejuízo.
- **Posição** - uma regra de risco que monitoriza o tamanho da posição.
- **Posição (tempo)** - uma regra de risco que monitoriza o tempo de vida de uma posição.
- **Comissão** - uma regra de risco que monitoriza o tamanho da comissão.
- **Deslizamento** - uma regra de risco que monitoriza o valor do slippage.
- **Preço da ordem** - uma regra de risco que monitoriza o preço de uma ordem.
- **Volume da ordem** - uma regra de risco que monitoriza o volume de uma ordem.
- **Ordem (frequência)** - uma regra de risco que monitoriza a frequência de colocação de ordens.
- **Erro no registo/cancelamento da ordem** - uma regra de risco que monitoriza o número de erros durante o registo/cancelamento de ordens.
- **Preço do negócio** - uma regra de risco que monitoriza o preço de um negócio.
- **Negócio (volume)** - uma regra de risco que monitoriza o volume de um negócio.
- **Negócio (frequência)** - uma regra de risco que monitoriza a frequência de realização de negócios.
- **Erro** - uma regra de risco que monitoriza o número de quaisquer erros.
