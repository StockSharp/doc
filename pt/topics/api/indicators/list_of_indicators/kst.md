# KST

**indicador KST (KST)** é um indicador técnico desenvolvido por Martin Pring que representa a soma de quatro taxas de variação (ROC) suavizadas com períodos diferentes para identificar ciclos de mercado de longo prazo.

Para utilizar o indicador, é necessário usar a classe [KnowSureThing](xref:StockSharp.Algo.Indicators.KnowSureThing).

## Descrição

O indicador indicador KST (KST) é um oscilador desenvolvido por Martin Pring para identificar tendências medindo o momentum do preço em vários horizontes temporais. O indicador combina quatro medições de taxa de variação (ROC) com períodos diferentes, dando maior importância aos períodos mais longos.

O KST baseia-se na teoria de que ciclos de mercado com durações diferentes influenciam colectivamente o movimento do preço. Ao combinar ROC de períodos diferentes, o KST procura identificar tendências cíclicas de longo prazo e determinar potenciais pontos de inversão.

O indicador é normalmente acompanhado por uma linha de sinal (média móvel do KST), e os seus cruzamentos podem ser usados para gerar sinais de negociação.

## Cálculo

O cálculo do indicador KST envolve os seguintes passos:

1. Calcular quatro medições de taxa de variação (ROC) com períodos diferentes:
   ```
   ROC1 = ((Close / Close[n1 periods ago]) - 1) * 100
   ROC2 = ((Close / Close[n2 periods ago]) - 1) * 100
   ROC3 = ((Close / Close[n3 periods ago]) - 1) * 100
   ROC4 = ((Close / Close[n4 periods ago]) - 1) * 100
   ```

2. Suavizar cada ROC usando uma média móvel simples (SMA):
   ```
   RCMA1 = SMA(ROC1, m1)
   RCMA2 = SMA(ROC2, m2)
   RCMA3 = SMA(ROC3, m3)
   RCMA4 = SMA(ROC4, m4)
   ```

3. Soma ponderada para obter o KST:
   ```
   KST = (RCMA1 * 1) + (RCMA2 * 2) + (RCMA3 * 3) + (RCMA4 * 4)
   ```

4. Calcular a linha de sinal:
   ```
   Linha de sinal = SMA(KST, período de sinal)
   ```

Onde:
- Close - preço de fecho
- n1, n2, n3, n4 - períodos para o cálculo do ROC (valores predefinidos: 10, 15, 20, 30)
- m1, m2, m3, m4 - períodos para suavização do ROC (valores predefinidos: 10, 10, 10, 15)
- período de sinal - período da linha de sinal (valor predefinido: 9)

## Interpretação

O indicador KST pode ser interpretado da seguinte forma:

1. **Cruzamentos da Linha Zero**:
   - Quando o KST cruza a linha zero de baixo para cima, pode ser visto como um sinal altista
   - Quando o KST cruza a linha zero de cima para baixo, pode ser visto como um sinal baixista

2. **Cruzamentos da Linha de Sinal**:
   - Quando o KST cruza a linha de sinal de baixo para cima, pode ser visto como um sinal altista (mais sensível do que o cruzamento da linha zero)
   - Quando o KST cruza a linha de sinal de cima para baixo, pode ser visto como um sinal baixista (mais sensível do que o cruzamento da linha zero)

3. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o KST forma um mínimo mais alto
   - Divergência baixista: o preço forma um novo máximo, enquanto o KST forma um máximo mais baixo

4. **Valores Extremos**:
   - Valores positivos elevados do KST podem indicar condições de sobrecompra do mercado
   - Valores negativos elevados do KST podem indicar condições de sobrevenda do mercado

5. **Direcção do Movimento**:
   - Tendência ascendente do KST indica um sentimento geral altista do mercado
   - Tendência descendente do KST indica um sentimento geral baixista do mercado

6. **Confirmação da Tendência**:
   - O KST pode ser usado para confirmar sinais de outros indicadores
   - A consistência entre a direcção do KST e o preço confirma a força da tendência actual

7. **Análise do Sentimento do Mercado**:
   - Valores positivos do KST indicam predominância de sentimento altista
   - Valores negativos do KST indicam predominância de sentimento baixista

![Gráfico do indicador KST](../../../../images/indicator_kst.png)

## Ver Também

[RoC](roc.md)
[MACD](macd.md)
[Impulso](momentum.md)
[RSI](rsi.md)

