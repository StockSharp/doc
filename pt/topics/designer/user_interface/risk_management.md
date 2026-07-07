# Gestão de risco

Nos painéis [Testing Properties](components/backtesting_settings.md) e [Live Trading Properties](components/live_settings.md), pode definir as definições de controlo de risco.

Na janela Risks, é necessário selecionar uma **Risk Rule**, configurar a condição de acionamento da **Risk Rule** e a ação (Close positions, Stop trading, Cancel orders) que será executada quando ocorrer a condição da **Risk Rule**.

É possível usar várias regras de risco do mesmo tipo com ações diferentes. Por exemplo, na captura de ecrã abaixo, se o volume da ordem for 20, são executadas as ações de cancelar ordens e parar a negociação.

![Designer Risk Rule](../../../images/designer_risk_rule.png)

### Lista de regras de risco

Lista de regras de risco

- **P/L** - uma regra de risco que monitoriza o tamanho do lucro/prejuízo.
- **Position** - uma regra de risco que monitoriza o tamanho da posição.
- **Position (Time)** - uma regra de risco que monitoriza o tempo de vida de uma posição.
- **Commission** - uma regra de risco que monitoriza o tamanho da comissão.
- **Slippage** - uma regra de risco que monitoriza o valor do slippage.
- **Order Price** - uma regra de risco que monitoriza o preço de uma ordem.
- **Order Volume** - uma regra de risco que monitoriza o volume de uma ordem.
- **Order (Frequency)** - uma regra de risco que monitoriza a frequência de colocação de ordens.
- **Error in Registration/Cancellation of Order** - uma regra de risco que monitoriza o número de erros durante o registo/cancelamento de ordens.
- **Trade Price** - uma regra de risco que monitoriza o preço de um negócio.
- **Trade (Volume)** - uma regra de risco que monitoriza o volume de um negócio.
- **Trade (Frequency)** - uma regra de risco que monitoriza a frequência de realização de negócios.
- **Error** - uma regra de risco que monitoriza o número de quaisquer erros.
