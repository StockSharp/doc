# FI

**Índice de força (FI)** é um indicador técnico desenvolvido pelo Dr. Alexander Elder que mede a força de cada movimento de preço com base na sua direção, magnitude e volume de negociação.

Para utilizar o indicador, é necessário usar a classe [ForceIndex](xref:StockSharp.Algo.Indicators.ForceIndex).

## Descrição

O Índice de força é um oscilador que mede a força dos compradores ou vendedores em cada movimento de preço. Combina três elementos importantes da informação de mercado: direção do movimento do preço, magnitude do movimento e volume de negociação.

A ideia principal do indicador é que quanto maior for a alteração do preço e quanto maior for o volume de negociação, mais forte será o movimento do mercado. Valores positivos do Índice de força indicam predominância dos compradores (pressão de alta), enquanto valores negativos indicam predominância dos vendedores (pressão de baixa).

O Índice de força é particularmente útil para:
- Determinar a força da tendência atual
- Identificar potenciais pontos de inversão
- Confirmar rompimentos
- Detetar divergências entre preço e força do movimento

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Período** - período de suavização (valor predefinido: 13)

## Cálculo

O cálculo do Índice de força envolve os seguintes passos:

1. Calcular o Índice de força de um único período:
   ```
   Índice de força de 1 período = (Close[current] - Close[previous]) * Volume[current]
   ```

2. Suavizar usando a média móvel exponencial (EMA):
   ```
   Índice de força = EMA(Índice de força de 1 período, Length)
   ```

Onde:
- Close - preço de fecho
- Volume - volume de negociação
- EMA - média móvel exponencial
- Length - período de suavização

## Interpretação

O Índice de força pode ser interpretado de várias formas:

1. **Cruzamentos da Linha Zero**:
   - A transição de valores negativos para positivos indica aumento da pressão de alta e pode ser vista como um sinal de compra
   - A transição de valores positivos para negativos indica aumento da pressão de baixa e pode ser vista como um sinal de venda

2. **Valores Extremos**:
   - Valores positivos elevados indicam forte pressão de alta que pode levar a condições de sobrecompra no mercado
   - Valores negativos elevados indicam forte pressão de baixa que pode levar a condições de sobrevenda no mercado

3. **Divergências**:
   - Divergência de alta (o preço forma um novo mínimo, enquanto o Índice de força forma um mínimo mais alto) pode sinalizar uma potencial inversão ascendente
   - Divergência de baixa (o preço forma um novo máximo, enquanto o Índice de força forma um máximo mais baixo) pode sinalizar uma potencial inversão descendente

4. **Confirmação da Tendência**:
   - Valores do Índice de força consistentemente positivos confirmam a força de uma tendência de alta
   - Valores do Índice de força consistentemente negativos confirmam a força de uma tendência de baixa

5. **Utilização Tripla** (segundo Elder):
   - Índice de força de curto prazo (2 dias): para identificar oportunidades de curto prazo
   - Índice de força de médio prazo (13 dias): para determinar tendências e correções de médio prazo
   - Índice de força de longo prazo (100 dias): para identificar a tendência principal

6. **Identificação de Correções**:
   - Numa tendência de alta, dias com Índice de força negativo podem indicar correções temporárias
   - Numa tendência de baixa, dias com Índice de força positivo podem indicar recuperações temporárias

![Gráfico do indicador FI](../../../../images/indicator_force_index.png)

## Ver Também

[EMA](ema.md)
[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
