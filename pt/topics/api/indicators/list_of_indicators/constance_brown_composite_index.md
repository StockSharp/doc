# CBCI

**índice composto de Constance Brown (CBCI)** é um indicador desenvolvido por Constance Brown que combina elementos de vários indicadores técnicos para criar uma ferramenta abrangente de análise de mercado.

Para usar o indicador, deve ser usada a classe [ConstanceBrownCompositeIndex](xref:StockSharp.Algo.Indicators.ConstanceBrownCompositeIndex).

## Descrição

O índice composto de Constance Brown (CBCI) foi criado para reunir as vantagens de vários indicadores numa única ferramenta abrangente. Incorpora elementos do oscilador estocástico, RSI e outros osciladores para fornecer sinais mais precisos sobre potenciais reversões de mercado e movimentos de tendência.

CBCI foi concebido para:
- Identificar potenciais pontos de reversão da tendência
- Determinar níveis de sobrecompra e sobrevenda
- Detetar divergências ocultas
- Confirmar a força da tendência atual

O indicador funciona bem em vários períodos e tipos de mercado, incluindo ações, forex e mercados de matérias-primas.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período principal de cálculo do índice (valor predefinido: 14)
- **StochasticKPeriod** - período para calcular o oscilador estocástico %K (valor predefinido: 5)
- **StochasticDPeriod** - período para calcular o oscilador estocástico %D (valor predefinido: 3)

## Cálculo

O cálculo do CBCI envolve os seguintes passos:

1. Calcular RSI ao longo do período Length:
   ```
   RSI = 100 - (100 / (1 + RS))
   RS = Average Gain / Average Loss
   ```

2. Calcular o oscilador estocástico:
   ```
   %K = ((Close - Lowest Low) / (Highest High - Lowest Low)) * 100
   %D = SMA(%K, StochasticDPeriod)
   ```

3. Combinar RSI e oscilador estocástico:
   ```
   CBCI = (RSI + %K + %D) / 3
   ```

Este índice combinado pode depois ser suavizado para reduzir ruído.

## Interpretação

- **Níveis de sobrecompra e sobrevenda**:
  - Valores acima de 80 podem indicar condições de sobrecompra no mercado
  - Valores abaixo de 20 podem indicar condições de sobrevenda no mercado

- **Cruzamentos da linha central**:
  - O cruzamento de baixo para cima da linha 50 pode ser visto como um sinal altista
  - O cruzamento de cima para baixo da linha 50 pode ser visto como um sinal baixista

- **Divergências**:
  - Divergências clássicas: quando o preço e o CBCI se movem em direções opostas
  - Divergências ocultas: quando o preço e o CBCI criam diferentes tipos de máximos ou mínimos

- **Movimento de tendência**:
  - Se o CBCI se mantiver consistentemente acima de 50, pode indicar força de tendência ascendente
  - Se o CBCI se mantiver consistentemente abaixo de 50, pode indicar força de tendência descendente

![indicator_constance_brown_composite_index](../../../../images/indicator_constance_brown_composite_index.png)

## Ver também

[RSI](rsi.md)
[Oscilador estocástico](stochastic_oscillator.md)
[Oscilador estocástico %K](stochastic_oscillator_k.md)
[CCI](cci.md)
