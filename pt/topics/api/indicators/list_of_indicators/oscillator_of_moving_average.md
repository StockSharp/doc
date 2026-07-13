# OMA

**Oscilador da média móvel (OMA)** é um indicador técnico que mede a diferença entre duas médias móveis com períodos diferentes para determinar o momentum e potenciais pontos de reversão.

Para usar o indicador, é necessário usar a classe [OscillatorOfMovingAverage](xref:StockSharp.Algo.Indicators.OscillatorOfMovingAverage).

## Descrição

O oscilador da média móvel (OMA) representa a diferença entre uma média móvel curta e uma média móvel longa. Este indicador ajuda a determinar a força da tendência e as suas potenciais alterações ao analisar a relação entre médias móveis de diferentes períodos.

O OMA funciona segundo um princípio semelhante ao MACD (convergência/divergência de médias móveis), mas numa forma mais simples, pois não inclui uma linha de sinal. O indicador oscila em torno da linha zero, onde valores positivos indicam que a média móvel curta está acima da média móvel longa (estado altista), e valores negativos indicam que a média móvel curta está abaixo da média móvel longa (estado baixista).

A principal força do OMA está na sua capacidade de identificar alterações no momentum da tendência e gerar sinais de negociação com base em cruzamentos da linha zero e divergências de preço.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **ShortPeriod** - período da média móvel curta (valor predefinido: 12)
- **LongPeriod** - período da média móvel longa (valor predefinido: 26)

## Cálculo

O cálculo do oscilador da média móvel envolve os seguintes passos:

1. Calcular a média móvel curta:
   ```
   média móvel curta = SMA(Price, ShortPeriod)
   ```

2. Calcular a média móvel longa:
   ```
   média móvel longa = SMA(Price, LongPeriod)
   ```

3. Calcular o OMA como a diferença entre as médias móveis curta e longa:
   ```
   OMA = média móvel curta - média móvel longa
   ```

Onde:
- Price - preço (normalmente o preço de fecho)
- SMA - média móvel simples
- ShortPeriod - período da média móvel curta
- LongPeriod - período da média móvel longa

Nota: Também podem ser usados outros tipos de médias móveis, como EMA (média móvel exponencial), WMA (média móvel ponderada), etc., em vez de SMA.

## Interpretação

O oscilador da média móvel pode ser interpretado da seguinte forma:

1. **Cruzamentos da linha zero**:
   - O cruzamento do OMA da linha zero de baixo para cima (a MA curta cruza a MA longa de baixo para cima) pode ser visto como um sinal altista
   - O cruzamento do OMA da linha zero de cima para baixo (a MA curta cruza a MA longa de cima para baixo) pode ser visto como um sinal baixista

2. **Valores extremos**:
   - Valores positivos elevados do OMA indicam que o mercado pode estar em sobrecompra
   - Valores negativos elevados do OMA indicam que o mercado pode estar em sobrevenda
   - Valores extremos antecedem frequentemente correções ou reversões de tendência

3. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o OMA forma um mínimo mais alto
   - Divergência baixista: o preço forma um novo máximo, enquanto o OMA forma um máximo mais baixo
   - As divergências antecedem frequentemente reversões significativas de tendência

4. **Confirmação da tendência**:
   - Valores positivos do OMA confirmam uma tendência ascendente
   - Valores negativos do OMA confirmam uma tendência descendente
   - O aumento do valor absoluto do OMA indica reforço da tendência atual

5. **Linha central (0)**:
   - Quando o OMA oscila em torno da linha zero, pode indicar ausência de uma tendência pronunciada ou consolidação

6. **Taxa de variação**:
   - A inclinação do OMA indica a taxa de alteração da tendência
   - Uma inclinação acentuada indica alteração rápida da tendência
   - Uma inclinação suave indica alteração lenta da tendência

7. **Combinação com outros indicadores**:
   - O OMA é frequentemente usado em conjunto com outros indicadores para confirmar sinais
   - É particularmente eficaz quando combinado com indicadores de sobrecompra/sobrevenda como RSI ou Estocástico

![Gráfico do indicador OMA](../../../../images/indicator_oscillator_of_moving_average.png)

## Ver também

[MACD](macd.md)
[MovingAverageCrossover](moving_average_crossover.md)
[SMA](sma.md)
[EMA](ema.md)
