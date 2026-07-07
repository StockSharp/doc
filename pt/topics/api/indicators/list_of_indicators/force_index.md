# FI

**Force Index (FI)** é um indicador técnico desenvolvido pelo Dr. Alexander Elder que mede a força de cada movimento de preço com base na sua direção, magnitude e volume de negociação.

Para utilizar o indicador, é necessário usar a classe [ForceIndex](xref:StockSharp.Algo.Indicators.ForceIndex).

## Descrição

O Force Index é um oscilador que mede a força dos "bulls" (compradores) ou "bears" (vendedores) em cada movimento de preço. Combina três elementos importantes da informação de mercado: direção do movimento do preço, magnitude do movimento e volume de negociação.

A ideia principal do indicador é que quanto maior for a alteração do preço e quanto maior for o volume de negociação, mais forte será o movimento do mercado. Valores positivos do Force Index indicam predominância dos compradores (pressão de alta), enquanto valores negativos indicam predominância dos vendedores (pressão de baixa).

O Force Index é particularmente útil para:
- Determinar a força da tendência atual
- Identificar potenciais pontos de inversão
- Confirmar rompimentos
- Detetar divergências entre preço e força do movimento

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de suavização (valor predefinido: 13)

## Cálculo

O cálculo do Force Index envolve os seguintes passos:

1. Calcular o Force Index de um único período:
   ```
   1-Period Force Index = (Close[current] - Close[previous]) * Volume[current]
   ```

2. Suavizar usando a Exponential Moving Average (EMA):
   ```
   Force Index = EMA(1-Period Force Index, Length)
   ```

Onde:
- Close - preço de fecho
- Volume - volume de negociação
- EMA - média móvel exponencial
- Length - período de suavização

## Interpretação

O Force Index pode ser interpretado de várias formas:

1. **Cruzamentos da Linha Zero**:
   - A transição de valores negativos para positivos indica aumento da pressão de alta e pode ser vista como um sinal de compra
   - A transição de valores positivos para negativos indica aumento da pressão de baixa e pode ser vista como um sinal de venda

2. **Valores Extremos**:
   - Valores positivos elevados indicam forte pressão de alta que pode levar a condições de sobrecompra no mercado
   - Valores negativos elevados indicam forte pressão de baixa que pode levar a condições de sobrevenda no mercado

3. **Divergências**:
   - Divergência de alta (o preço forma um novo mínimo, enquanto o Force Index forma um mínimo mais alto) pode sinalizar uma potencial inversão ascendente
   - Divergência de baixa (o preço forma um novo máximo, enquanto o Force Index forma um máximo mais baixo) pode sinalizar uma potencial inversão descendente

4. **Confirmação da Tendência**:
   - Valores do Force Index consistentemente positivos confirmam a força de uma tendência de alta
   - Valores do Force Index consistentemente negativos confirmam a força de uma tendência de baixa

5. **Utilização Tripla** (segundo Elder):
   - Force Index de curto prazo (2 dias): para identificar oportunidades de curto prazo
   - Force Index de médio prazo (13 dias): para determinar tendências e correções de médio prazo
   - Force Index de longo prazo (100 dias): para identificar a tendência principal

6. **Identificação de Correções**:
   - Numa tendência de alta, dias com Force Index negativo podem indicar correções temporárias
   - Numa tendência de baixa, dias com Force Index positivo podem indicar recuperações temporárias

![indicator_force_index](../../../../images/indicator_force_index.png)

## Ver Também

[EMA](ema.md)
[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)

