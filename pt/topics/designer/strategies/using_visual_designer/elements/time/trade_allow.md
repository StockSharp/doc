# A negociação é permitida

![Captura de tela de A negociação é permitida](../../../../../../images/designer_tradealloweddiagramelement_00.png)

Este bloco é utilizado para verificar se a negociação é actualmente permitida. São verificadas as seguintes condições:

- Todas as subscrições da estratégia a dados de mercado devem estar no estado [Online](../../../../../api/market_data/subscriptions.md) (a receber dados em tempo real).
- Todos os indicadores devem estar [formados](../../../../../api/indicators.md).
- No caso de [negociação em tempo real](../../../../live_execution/getting_started.md), o valor do acionador de entrada deve ter uma marca temporal superior à hora de início da estratégia.

### Conectores de entrada


- **Acionador** - o sinal que determina o momento em que a verificação deve ser efectuada.

### Conectores de saída


- **Sinalizador** - um sinalizador que determina se a sessão de negociação está activa.

## Ver também

[Hora actual](current_time.md)
