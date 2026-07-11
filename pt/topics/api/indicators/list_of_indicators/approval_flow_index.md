# AFI

﻿# AFI

**índice de fluxo de aprovação (AFI)** é um indicador que mede a força da tendência com base na relação entre volume e movimento do preço.

Para usar o indicador, é necessário usar a classe [ApprovalFlowIndex](xref:StockSharp.Algo.Indicators.ApprovalFlowIndex).

## Descrição

O índice de fluxo de aprovação (AFI) ajuda a avaliar a intensidade do fluxo de ordens no mercado e a determinar a força da tendência atual. Este indicador analisa a relação entre o volume de negociação e o movimento do preço para identificar potenciais pontos de reversão ou confirmar a continuação da tendência.

O indicador AFI pode ser usado para:
- Determinar a força da tendência atual
- Identificar divergências entre o preço e o indicador
- Procurar potenciais pontos de reversão do mercado

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo do indicador

## Cálculo

O cálculo do índice de fluxo de aprovação baseia-se na análise da alteração de preço e do volume ao longo de um período específico:

1. Primeiro, calcule a alteração do preço no período
2. Depois relacione esta alteração com o volume de negociação
3. Os valores resultantes são somados ao longo do período selecionado (parâmetro Length)

O AFI procura determinar em que medida o volume de negociação "aprova" o movimento do preço.

Valores positivos de AFI indicam uma tendência ascendente forte, enquanto valores negativos sugerem uma tendência descendente. Valores próximos de zero podem indicar ausência de uma tendência pronunciada.

![indicator_approval_flow_index](../../../../images/indicator_approval_flow_index.png)

## Ver também

[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
