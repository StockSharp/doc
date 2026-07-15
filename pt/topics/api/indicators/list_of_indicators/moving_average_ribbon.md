# MAR

**Faixa de médias móveis (MAR)** é um indicador técnico que apresenta várias médias móveis com períodos progressivamente crescentes para visualizar a força e a direção da tendência.

Para usar o indicador, é necessário usar a classe [MovingAverageRibbon](xref:StockSharp.Algo.Indicators.MovingAverageRibbon).

## Descrição

A faixa de médias móveis (MAR) é um conjunto de várias médias móveis apresentadas num gráfico numa formação de "fita" ou "leque". Este indicador ajuda os operadores a visualizar o estado atual da tendência e a sua força de forma mais intuitiva do que usando uma ou duas médias móveis.

O MAR inclui várias médias móveis (normalmente 5 a 10) com períodos progressivamente crescentes. O intervalo entre períodos pode ser uniforme (por exemplo, 10, 20, 30, 40...) ou exponencial (por exemplo, 5, 10, 20, 40...).

A ideia principal é que o posicionamento mútuo e a forma destas médias móveis podem fornecer informação valiosa sobre o estado e a força da tendência atual, e ajudar a identificar potenciais pontos de reversão.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Período curto** - período inicial (mínimo) das médias móveis (valor predefinido: 10)
- **Período longo** - período final (máximo) das médias móveis (valor predefinido: 100)
- **RibbonCount** - número de médias móveis na fita (valor predefinido: 10)

## Cálculo

O cálculo da faixa de médias móveis envolve os seguintes passos:

1. Determinar a sequência de períodos para as médias móveis:
   ```
   Step = (LongPeriod - ShortPeriod) / (RibbonCount - 1)
   Periods = [ShortPeriod, ShortPeriod + Step, ShortPeriod + 2*Step, ..., LongPeriod]
   ```

2. Calcular a média móvel para cada período:
   ```
   MAs = [SMA(Preço, Período) para cada Período em Períodos]
   ```

Onde:
- Price - preço (normalmente o preço de fecho)
- SMA - média móvel simples
- ShortPeriod - período inicial
- LongPeriod - período final
- RibbonCount - número de médias móveis

Nota: Também podem ser usados outros tipos de médias móveis, como EMA (média móvel exponencial), WMA (média móvel ponderada), etc., em vez de SMA.

## Interpretação

A faixa de médias móveis pode ser interpretada da seguinte forma:

1. **Posicionamento mútuo das médias móveis**:
   - Quando todas as linhas estão dispostas por ordem ascendente de períodos (a mais curta no topo, a mais longa em baixo), isto indica uma forte tendência ascendente
   - Quando todas as linhas estão dispostas por ordem descendente de períodos (a mais curta em baixo, a mais longa no topo), isto indica uma forte tendência descendente
   - Quando as linhas se cruzam e não têm uma ordem clara, isto indica uma tendência lateral ou incerteza

2. **Forma da fita**:
   - Fita em expansão (aumento da distância entre linhas) indica reforço da tendência
   - Fita em contração (diminuição da distância entre linhas) indica enfraquecimento da tendência
   - Agrupamento apertado das linhas indica consolidação ou ausência de uma tendência pronunciada

3. **Cruzamentos de médias móveis**:
   - O início de interseções entre linhas pode sinalizar uma potencial alteração de tendência
   - Quando as médias móveis curtas começam a cruzar as longas, isto pode ser um sinal precoce de reversão de tendência

4. **Ângulo de inclinação da fita**:
   - Ângulo acentuado indica uma tendência forte
   - Ângulo suave indica uma tendência fraca
   - Posicionamento horizontal da fita indica uma tendência lateral

5. **Posição do preço relativamente à fita**:
   - Quando o preço está acima de toda a fita, isto confirma uma forte tendência ascendente
   - Quando o preço está abaixo de toda a fita, isto confirma uma forte tendência descendente
   - Quando o preço se move dentro da fita, isto pode indicar um estado de transição ou consolidação

6. **Estratégias de negociação**:
   - Entrar numa posição quando o preço ressalta a partir da extremidade da fita na direção da tendência
   - Sair de uma posição quando as médias móveis começam a cruzar na direção oposta
   - Usar a largura da fita para definir stop-losses e take-profits

![Gráfico do indicador MAR](../../../../images/indicator_moving_average_ribbon.png)

## Ver também

[SMA](sma.md)
[EMA](ema.md)
[MovingAverageCrossover](moving_average_crossover.md)
[GuppyMultipleMovingAverage](guppy_multiple_moving_average.md)
