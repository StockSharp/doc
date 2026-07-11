# III

**índice de intensidade intradiária (III)** é um indicador técnico desenvolvido por David Bostian que avalia a relação entre o preço de fecho, o intervalo de preços e o volume de negociação dentro de um dia de negociação.

Para utilizar o indicador, é necessário usar a classe [IntradayIntensityIndex](xref:StockSharp.Algo.Indicators.IntradayIntensityIndex).

## Descrição

O índice de intensidade intradiária (III) combina informação sobre o movimento do preço e o volume de negociação para avaliar a intensidade da pressão compradora ou vendedora dentro de um dia de negociação. O indicador baseia-se no pressuposto de que a posição do preço de fecho em relação ao intervalo de preços do dia, combinada com o volume, pode indicar a direção e força do movimento do mercado.

O III é particularmente útil para identificar alterações intradiárias no sentimento do mercado e determinar potenciais pontos de inversão. Valores positivos do indicador indicam pressão compradora (preço de fecho mais próximo do máximo do dia), enquanto valores negativos indicam pressão vendedora (preço de fecho mais próximo do mínimo do dia).

O índice de intensidade intradiária é especialmente eficaz para:
- Identificar alterações intradiárias no sentimento do mercado
- Determinar potenciais pontos de inversão
- Confirmar sinais de outros indicadores
- Avaliar a força da tendência atual

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de suavização (valor predefinido: 14)

## Cálculo

O cálculo do índice de intensidade intradiária envolve os seguintes passos:

1. Calcular o valor III individual para cada período:
   ```
   III bruto = ((2 * Close - High - Low) / ((High - Low) * Volume)) * Volume
   ```

2. Suavizar usando uma média móvel simples:
   ```
   III = SMA(III bruto, Length)
   ```

Onde:
- Close - preço de fecho
- High - preço máximo do período
- Low - preço mínimo do período
- Volume - volume de negociação
- SMA - média móvel simples
- Length - período de suavização

## Interpretação

O índice de intensidade intradiária pode ser interpretado da seguinte forma:

1. **Cruzamentos da Linha Zero**:
   - A transição de valores negativos para positivos pode ser vista como um sinal de alta, indicando aumento da pressão compradora
   - A transição de valores positivos para negativos pode ser vista como um sinal de baixa, indicando aumento da pressão vendedora

2. **Valores Extremos**:
   - Valores positivos elevados indicam forte pressão compradora
   - Valores negativos elevados indicam forte pressão vendedora
   - Valores extremos podem indicar condições de sobrecompra ou sobrevenda no mercado

3. **Divergências**:
   - Divergência de alta: o preço forma um novo mínimo, enquanto o III forma um mínimo mais alto
   - Divergência de baixa: o preço forma um novo máximo, enquanto o III forma um máximo mais baixo

4. **Tendências do III**:
   - Valores do III consistentemente positivos confirmam uma tendência de alta
   - Valores do III consistentemente negativos confirmam uma tendência de baixa
   - Oscilações em torno da linha zero podem indicar uma tendência lateral ou incerteza

5. **Combinação com Outros Indicadores**:
   - O III é frequentemente usado em combinação com outros indicadores técnicos para confirmar sinais
   - É particularmente eficaz quando combinado com indicadores de tendência e volume

6. **Alterações nos Valores**:
   - Uma alteração rápida de valores negativos para positivos pode indicar uma mudança acentuada no sentimento do mercado
   - A convergência gradual para a linha zero pode indicar enfraquecimento do momentum atual

![indicator_intraday_intensity_index](../../../../images/indicator_intraday_intensity_index.png)

## Ver Também

[IntradayMomentumIndex](intraday_momentum_index.md)
[BalanceOfPower](balance_of_power.md)
[ForceIndex](force_index.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
