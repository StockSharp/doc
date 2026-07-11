# MAC

**Cruzamento de médias móveis (MAC)** é um indicador técnico que acompanha cruzamentos entre médias móveis curta e longa para identificar potenciais pontos de entrada e saída do mercado.

Para usar o indicador, é necessário usar a classe [MovingAverageCrossover](xref:StockSharp.Algo.Indicators.MovingAverageCrossover).

## Descrição

O indicador de cruzamento de médias móveis (MAC) é um dos indicadores mais usados e fáceis de compreender na análise técnica. Baseia-se no conceito de que, quando uma média móvel de curto prazo cruza uma média móvel de longo prazo, isso pode sinalizar uma alteração de tendência ou um movimento significativo do preço.

O MAC usa duas médias móveis com períodos diferentes:
1. Média móvel curta (FastMA) - reflete o movimento recente do preço
2. Média móvel longa (SlowMA) - reflete o movimento do preço a prazo mais longo

O indicador é normalmente representado como a diferença entre as médias móveis curta e longa, o que permite identificar facilmente o momento do cruzamento (quando o valor do indicador cruza a linha zero).

O MAC é amplamente usado tanto em estratégias de trading autónomas como como parte de sistemas mais complexos, como o MACD (convergência/divergência de médias móveis).

## Parâmetros

O indicador tem os seguintes parâmetros:
- **ShortPeriod** - período da média móvel curta (valor predefinido: 9)
- **LongPeriod** - período da média móvel longa (valor predefinido: 26)

## Cálculo

O cálculo do indicador de cruzamento de médias móveis envolve os seguintes passos:

1. Calcular a média móvel curta:
   ```
   FastMA = SMA(Price, ShortPeriod)
   ```

2. Calcular a média móvel longa:
   ```
   SlowMA = SMA(Price, LongPeriod)
   ```

3. Calcular o valor do MAC como a diferença entre as médias móveis curta e longa:
   ```
   MAC = FastMA - SlowMA
   ```

Onde:
- Price - preço (normalmente o preço de fecho)
- SMA - média móvel simples
- ShortPeriod - período da média móvel curta
- LongPeriod - período da média móvel longa

Nota: Também podem ser usados outros tipos de médias móveis, como EMA (média móvel exponencial), WMA (média móvel ponderada), etc., em vez de SMA.

## Interpretação

O indicador de cruzamento de médias móveis pode ser interpretado da seguinte forma:

1. **Cruzamentos da linha zero**:
   - O cruzamento da linha zero do MAC de baixo para cima (FastMA cruza a SlowMA de baixo para cima) gera um sinal altista, indicando um potencial início de tendência ascendente
   - O cruzamento da linha zero do MAC de cima para baixo (FastMA cruza a SlowMA de cima para baixo) gera um sinal baixista, indicando um potencial início de tendência descendente

2. **Valor do indicador**:
   - Um valor positivo do MAC indica que a média móvel curta está acima da média móvel longa, frequentemente interpretado como um estado de mercado altista
   - Um valor negativo do MAC indica que a média móvel curta está abaixo da média móvel longa, frequentemente interpretado como um estado de mercado baixista

3. **Distância entre médias móveis**:
   - O aumento da distância entre médias móveis (aumento do valor absoluto do MAC) indica reforço da tendência
   - A diminuição da distância entre médias móveis (diminuição do valor absoluto do MAC) pode indicar enfraquecimento da tendência e potencial reversão

4. **Sinais falsos**:
   - Durante períodos de consolidação lateral, o MAC pode gerar vários sinais falsos devido a cruzamentos frequentes das médias móveis
   - Indicadores ou regras adicionais são frequentemente usados para filtrar sinais falsos (por exemplo, exigir que o preço esteja acima/abaixo de ambas as médias móveis)

5. **Combinação com outros indicadores**:
   - O MAC é frequentemente usado em conjunto com indicadores de momentum (RSI, Estocástico) para confirmar sinais
   - Também pode ser combinado com indicadores de tendência e volatilidade para criar sistemas de trading mais abrangentes

6. **Seleção de parâmetros**:
   - Períodos mais curtos (por exemplo, 5 e 20) são mais sensíveis e adequados para trading de curto prazo
   - Períodos mais longos (por exemplo, 50 e 200) são menos sensíveis e adequados para trading de longo prazo

![indicator_moving_average_crossover](../../../../images/indicator_moving_average_crossover.png)

## Ver também

[SMA](sma.md)
[EMA](ema.md)
[MACD](macd.md)
[MovingAverageRibbon](moving_average_ribbon.md)
