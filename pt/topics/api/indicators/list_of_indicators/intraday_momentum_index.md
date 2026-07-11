# IMI

**índice de momentum intradiário (IMI)** é um indicador técnico desenvolvido por Tushar Chande que combina princípios de preço intradiário e o conceito de RSI para medir o momentum intradiário.

Para utilizar o indicador, é necessário usar a classe [IntradayMomentumIndex](xref:StockSharp.Algo.Indicators.IntradayMomentumIndex).

## Descrição

O índice de momentum intradiário (IMI) foi criado como uma modificação do clássico índice de força relativa (RSI), especificamente adaptada para analisar a dinâmica intradiária do mercado. Em vez de usar preços de fecho sequenciais, como no RSI tradicional, o IMI compara o preço de fecho com o preço de abertura de cada período.

O IMI avalia com que frequência e com que intensidade o preço de fecho excede o preço de abertura (momentum positivo) ou fica abaixo do preço de abertura (momentum negativo) ao longo de um determinado período. Isto permite identificar a direção predominante e a força do movimento intradiário.

O indicador é particularmente útil para:
- Determinar a direção intradiária do mercado
- Identificar potenciais pontos de inversão
- Determinar níveis de sobrecompra e sobrevenda
- Detetar divergências entre preço e momentum

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 14)

## Cálculo

O cálculo do índice de momentum intradiário envolve os seguintes passos:

1. Determinar o movimento intradiário do preço:
   ```
   Gain = Close - Open, if Close > Open
   Loss = Open - Close, if Close < Open
   ```

2. Calcular a soma dos movimentos positivos e negativos ao longo do período Length:
   ```
   Sum Gains = soma de todos os Gains durante o período Length
   Sum Losses = soma de todas as Losses durante o período Length
   ```

3. Calcular o IMI usando uma fórmula semelhante ao RSI:
   ```
   IMI = 100 * (Sum Gains / (Sum Gains + Sum Losses))
   ```

Nota: Se (Sum Gains + Sum Losses) for igual a zero, o IMI é definido como 50 para evitar divisão por zero.

## Interpretação

O índice de momentum intradiário é interpretado de forma semelhante ao RSI:

1. **Intervalo de Valores**:
   - O IMI oscila entre 0 e 100
   - Valores acima de 50 indicam predominância de momentum intradiário positivo
   - Valores abaixo de 50 indicam predominância de momentum intradiário negativo

2. **Níveis de Sobrecompra e Sobrevenda**:
   - Valores acima de 70 são normalmente vistos como indicação de condições de sobrecompra no mercado
   - Valores abaixo de 30 são normalmente vistos como indicação de condições de sobrevenda no mercado

3. **Cruzamentos da Linha Central**:
   - Cruzar a linha 50 de baixo para cima pode ser visto como um sinal de alta
   - Cruzar a linha 50 de cima para baixo pode ser visto como um sinal de baixa

4. **Divergências**:
   - Divergência de alta: o preço forma um novo mínimo, enquanto o IMI forma um mínimo mais alto
   - Divergência de baixa: o preço forma um novo máximo, enquanto o IMI forma um máximo mais baixo

5. **Failed Swings**:
   - Se o IMI não conseguir atingir o nível de sobrecompra durante uma tendência de alta, isto pode indicar fraqueza da tendência
   - Se o IMI não conseguir atingir o nível de sobrevenda durante uma tendência de baixa, isto pode indicar fraqueza da tendência

6. **Confirmação da Tendência**:
   - Valores sustentados do IMI acima de 50 confirmam uma tendência de alta
   - Valores sustentados do IMI abaixo de 50 confirmam uma tendência de baixa

![indicator_intraday_momentum_index](../../../../images/indicator_intraday_momentum_index.png)

## Ver Também

[RSI](rsi.md)
[IntradayIntensityIndex](intraday_intensity_index.md)
[Momentum](momentum.md)
[RelativeMomentumIndex](relative_momentum_index.md)

