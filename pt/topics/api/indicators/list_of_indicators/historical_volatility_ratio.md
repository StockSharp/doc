# HVR

**rácio de volatilidade histórica (HVR)** é um indicador técnico que compara a volatilidade histórica de curto prazo com a volatilidade histórica de longo prazo para avaliar alterações na atividade do mercado.

Para utilizar o indicador, é necessário usar a classe [HistoricalVolatilityRatio](xref:StockSharp.Algo.Indicators.HistoricalVolatilityRatio).

## Descrição

O rácio de volatilidade histórica (HVR) é um indicador de volatilidade relativa que compara a volatilidade de curto prazo com a volatilidade de longo prazo do mercado. O indicador ajuda a determinar se a volatilidade atual está a aumentar ou a diminuir em relação ao seu nível histórico.

O HVR é calculado como o rácio entre a volatilidade histórica de curto prazo e a volatilidade histórica de longo prazo. Valores acima de 1,0 indicam que a volatilidade atual (de curto prazo) é superior à volatilidade de longo prazo, o que pode sinalizar aumento da atividade do mercado ou uma potencial alteração de tendência.

O indicador é particularmente útil para:
- Identificar períodos de alta e baixa volatilidade
- Determinar potenciais pontos de inversão da tendência
- Adaptar estratégias de negociação às condições atuais do mercado
- Avaliar o risco de mercado e definir tamanhos de posição adequados

## Parâmetros

O indicador tem os seguintes parâmetros:
- **ShortPeriod** - período para calcular a volatilidade de curto prazo (valor predefinido: 5)
- **LongPeriod** - período para calcular a volatilidade de longo prazo (valor predefinido: 20)

## Cálculo

O cálculo do rácio de volatilidade histórica envolve os seguintes passos:

1. Calcular a volatilidade histórica de curto prazo:
   ```
   Volatilidade de curto prazo = Desvio-padrão dos retornos logarítmicos durante ShortPeriod * Sqrt(Dias de negociação por ano)
   ```

2. Calcular a volatilidade histórica de longo prazo:
   ```
   Volatilidade de longo prazo = Desvio-padrão dos retornos logarítmicos durante LongPeriod * Sqrt(Dias de negociação por ano)
   ```

3. Calcular o HVR como o rácio entre a volatilidade de curto prazo e a volatilidade de longo prazo:
   ```
   HVR = Volatilidade de curto prazo / Volatilidade de longo prazo
   ```

Onde:
- Retornos logarítmicos - retornos logarítmicos (ln(Price[i] / Price[i-1]))
- Desvio-padrão - desvio padrão
- Dias de negociação por ano - número de dias de negociação num ano (normalmente 252 para mercados acionistas)
- ShortPeriod - período curto para cálculo da volatilidade
- LongPeriod - período longo para cálculo da volatilidade

## Interpretação

O rácio de volatilidade histórica pode ser interpretado da seguinte forma:

1. **Nível 1,0**:
   - HVR = 1,0 significa que a volatilidade de curto prazo é igual à volatilidade de longo prazo
   - HVR > 1,0 indica que a volatilidade de curto prazo é superior à volatilidade de longo prazo
   - HVR < 1,0 indica que a volatilidade de curto prazo é inferior à volatilidade de longo prazo

2. **Valores Extremos**:
   - Valores muito elevados do HVR (por exemplo, > 2,0) podem indicar um aumento acentuado da volatilidade, ocorrendo frequentemente durante pânicos de mercado ou movimentos fortes
   - Valores muito baixos do HVR (por exemplo, < 0,5) podem indicar um período de compressão de volatilidade, frequentemente anterior a movimentos fortes

3. **Tendências do HVR**:
   - HVR ascendente indica aumento da volatilidade atual
   - HVR descendente indica diminuição da volatilidade atual

4. **Estratégias de Negociação**:
   - Quando o HVR é elevado, pode ser adequado usar estratégias baseadas em rompimentos
   - Quando o HVR é baixo, estratégias de reversão à média ou negociação em intervalo podem ser mais adequadas

5. **Gestão de Risco**:
   - Valores elevados do HVR podem sinalizar a necessidade de reduzir tamanhos de posição devido ao aumento da volatilidade
   - Valores baixos do HVR podem permitir aumentar tamanhos de posição devido à volatilidade reduzida

6. **Potenciais Inversões**:
   - Valores extremos do HVR precedem frequentemente movimentos significativos do preço
   - Um aumento acentuado do HVR após um período de baixa volatilidade pode sinalizar o início de uma nova tendência

![indicator_historical_volatility_ratio](../../../../images/indicator_historical_volatility_ratio.png)

## Ver Também

[ATR](atr.md)
[StandardDeviation](standard_deviation.md)
[ChoppinessIndex](choppiness_index.md)

