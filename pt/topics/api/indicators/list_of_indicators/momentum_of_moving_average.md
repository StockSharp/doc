# MOMA

**Momentum da média móvel (MOMA)** é um indicador técnico que mede a taxa de variação do preço da média móvel, combinando os conceitos de momentum e médias móveis.

Para usar o indicador, é necessário usar a classe [MomentumOfMovingAverage](xref:StockSharp.Algo.Indicators.MomentumOfMovingAverage).

## Descrição

O momentum da média móvel (MOMA) é uma combinação de dois indicadores - um indicador de momentum e uma média móvel. Primeiro, é calculada uma média móvel da série de preços e, em seguida, é medido o momentum (taxa de variação) dessa média móvel.

A ideia principal do MOMA é primeiro suavizar a série de preços usando uma média móvel, removendo assim o ruído de mercado, e depois analisar a velocidade e a direção da alteração desta curva suavizada. Isto permite obter um sinal mais limpo sobre a alteração do momentum da tendência do que calcular o momentum diretamente a partir do preço.

O MOMA ajuda a determinar a força da tendência e potenciais pontos de reversão ao focar-se nas alterações da dinâmica da média móvel, em vez do próprio preço. Valores positivos de MOMA indicam momentum ascendente da média móvel, enquanto valores negativos indicam momentum descendente.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período para o cálculo da média móvel (valor predefinido: 14)
- **MomentumPeriod** - período para o cálculo do momentum (valor predefinido: 10)

## Cálculo

O cálculo do momentum da média móvel envolve os seguintes passos:

1. Calcular a média móvel para a série de preços:
   ```
   MA = SMA(Price, Length)
   ```

2. Calcular o momentum da média móvel:
   ```
   MOMA = MA[current] - MA[current - MomentumPeriod]
   ```

Onde:
- Price - preço (normalmente o preço de fecho)
- SMA - média móvel simples
- Length - período da média móvel
- MomentumPeriod - período para o cálculo do momentum

Nota: Também podem ser usados outros tipos de médias móveis, como EMA (média móvel exponencial), WMA (média móvel ponderada), etc., em vez de SMA.

## Interpretação

O momentum da média móvel pode ser interpretado da seguinte forma:

1. **Cruzamentos da linha zero**:
   - O cruzamento do MOMA da linha zero de baixo para cima pode ser visto como um sinal altista, indicando o início ou reforço de uma tendência ascendente
   - O cruzamento do MOMA da linha zero de cima para baixo pode ser visto como um sinal baixista, indicando o início ou reforço de uma tendência descendente

2. **Valores absolutos**:
   - Valores positivos elevados de MOMA indicam forte momentum ascendente da média móvel
   - Valores negativos elevados de MOMA indicam forte momentum descendente da média móvel
   - Valores próximos de zero indicam ausência de momentum pronunciado ou uma tendência lateral

3. **Divergências**:
   - Divergência altista: o preço forma um novo mínimo, enquanto o MOMA forma um mínimo mais alto
   - Divergência baixista: o preço forma um novo máximo, enquanto o MOMA forma um máximo mais baixo

4. **Alteração de direção**:
   - Quando o MOMA altera a direção do movimento (de subida para descida ou vice-versa), isto pode sinalizar uma potencial alteração na tendência da média móvel
   - Estas reversões antecedem frequentemente alterações na direção do preço

5. **Confirmação da tendência**:
   - Valores positivos de MOMA confirmam uma tendência ascendente
   - Valores negativos de MOMA confirmam uma tendência descendente
   - Valores crescentes de MOMA indicam reforço da tendência atual
   - Valores decrescentes de MOMA indicam enfraquecimento da tendência atual

6. **Filtragem de sinais**:
   - O MOMA pode ser usado para filtrar sinais de outros indicadores
   - Por exemplo, considerar apenas sinais altista quando o MOMA é positivo, e apenas sinais baixista quando o MOMA é negativo

7. **Seleção de parâmetros**:
   - Períodos mais curtos para Length e MomentumPeriod tornam o MOMA mais sensível, mas também mais propenso a sinais falsos
   - Períodos mais longos tornam o MOMA mais suave, mas podem levar a sinais atrasados

![MOMA](../../../../images/indicator_momentum_of_moving_average.png)

## Ver também

[Impulso](momentum.md)
[SMA](sma.md)
[EMA](ema.md)
[RoC](roc.md)

