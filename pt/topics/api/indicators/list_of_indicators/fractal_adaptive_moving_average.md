# FRAMA

**Média móvel adaptativa fractal (FRAMA)** é um indicador técnico desenvolvido por John Ehlers que adapta a velocidade de reação às alterações de preço com base na dimensão fractal do mercado.

Para utilizar o indicador, é necessário usar a classe [FractalAdaptiveMovingAverage](xref:StockSharp.Algo.Indicators.FractalAdaptiveMovingAverage).

## Descrição

A média móvel adaptativa fractal (FRAMA) é um tipo avançado de média móvel exponencial (EMA) que ajusta automaticamente a sua sensibilidade às alterações de preço com base na dimensão fractal do mercado. O indicador foi desenvolvido por John Ehlers e apresentado na revista Technical Analysis of Stocks & Commodities em outubro de 2000.

A FRAMA usa o conceito de geometria fractal para analisar a estrutura do mercado. Determina quão "fractal" ou caótico está o mercado atual e, com base nisso, ajusta a velocidade de resposta do indicador:

- Em condições de mercado com tendência (menos fractais), a FRAMA responde rapidamente às alterações de preço, semelhante a uma EMA curta
- Em condições de mercado lateral (mais fractais), a FRAMA responde mais lentamente, semelhante a uma EMA longa

Isto permite que a FRAMA responda mais depressa a movimentos significativos do preço e ignore o ruído do mercado, tornando-a mais eficaz em comparação com médias móveis tradicionais.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (valor predefinido: 10-20)

## Cálculo

O cálculo da FRAMA envolve vários passos:

1. Calcular a dimensão fractal (D) com base no rácio logarítmico do comprimento high-low do preço em relação ao número de períodos:
   ```
   N1 = High(1...Length/2) - Low(1...Length/2)
   N2 = High(Length/2+1...Length) - Low(Length/2+1...Length)
   N3 = High(1...Length) - Low(1...Length)
   
   D = (log(N1 + N2) - log(N3)) / log(2)
   ```

2. Converter a dimensão fractal no fator alpha para suavização exponencial:
   ```
   Smoothing Factor = exp(-4.6 * (D - 1))
   Alpha = Smoothing Factor * Smoothing Factor
   ```

3. Aplicar o fator alpha ao preço atual e ao valor anterior da FRAMA:
   ```
   FRAMA = Alpha * Price + (1 - Alpha) * FRAMA[previous]
   ```

Onde:
- High - preço máximo do período
- Low - preço mínimo do período
- log - logaritmo natural

## Interpretação

A FRAMA pode ser interpretada de forma semelhante a outras médias móveis, mas considerando a sua natureza adaptativa:

1. **Direção da FRAMA**:
   - Uma FRAMA ascendente indica uma tendência de alta
   - Uma FRAMA descendente indica uma tendência de baixa

2. **Cruzamentos com o Preço**:
   - Quando o preço cruza a FRAMA de baixo para cima, pode ser visto como um sinal de alta
   - Quando o preço cruza a FRAMA de cima para baixo, pode ser visto como um sinal de baixa

3. **Cruzamentos de Múltiplas FRAMA**:
   - O cruzamento de uma FRAMA curta com uma FRAMA longa de baixo para cima pode indicar o início de uma tendência de alta
   - O cruzamento de uma FRAMA curta com uma FRAMA longa de cima para baixo pode indicar o início de uma tendência de baixa

4. **Ângulo de Inclinação da FRAMA**:
   - Uma inclinação acentuada indica uma tendência forte
   - Uma inclinação ligeira indica uma tendência fraca
   - Movimento horizontal indica uma tendência lateral

5. **Filtragem de Sinais**:
   - Devido à sua natureza adaptativa, a FRAMA cria menos sinais falsos do que as médias móveis tradicionais
   - Quanto mais curto for o período da FRAMA, mais sensível será o indicador às alterações de preço

6. **Níveis de Suporte e Resistência**:
   - A FRAMA pode servir como nível de suporte dinâmico numa tendência de alta
   - A FRAMA pode servir como nível de resistência dinâmico numa tendência de baixa

![indicator_fractal_adaptive_moving_average](../../../../images/indicator_fractal_adaptive_moving_average.png)

## Ver Também

[EMA](ema.md)
[KAMA](kama.md)
[VIDYA](vidya.md)
