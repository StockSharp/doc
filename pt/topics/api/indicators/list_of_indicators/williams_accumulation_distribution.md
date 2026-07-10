# WAD

**Williams Accumulation/Distribution (WAD)** é um indicador de volume desenvolvido por Larry Williams. Ao contrário da linha Accumulation/Distribution tradicional, o indicador WAD foca-se na relação entre o preço de fecho do período atual e o preço de fecho do período anterior para determinar a pressão compradora ou vendedora.

Para usar o indicador, é necessário usar a classe [WilliamsAccumulationDistribution](xref:StockSharp.Algo.Indicators.WilliamsAccumulationDistribution).

## Descrição

O indicador Williams Accumulation/Distribution foi concebido para identificar discrepâncias entre preço e volume que possam sinalizar potenciais inversões de tendência. O WAD é particularmente útil para revelar fraqueza no movimento atual do preço.

Características principais do WAD:
- Valores positivos indicam acumulação (pressão compradora)
- Valores negativos indicam distribuição (pressão vendedora)
- Divergências entre WAD e preço podem preceder inversões de preço

Principais aplicações do indicador:
- Confirmar a tendência atual
- Identificar potenciais inversões de preço
- Determinar a pressão compradora ou vendedora

## Cálculo

O indicador Williams Accumulation/Distribution é calculado usando a seguinte lógica:

1. Determinar a proteção do intervalo verdadeiro (TRP) para o período atual:
   ```
   TRP = Max(High - Low, |High - Close_prev|, |Low - Close_prev|)
   ```

2. Calcular o valor Accumulation/Distribution (AD) para o período atual:
   - Se Close > Close_prev (mercado em alta):
      ```
      AD = Close - Min(Low, Close_prev) 
      ```
   - Se Close < Close_prev (mercado em baixa):
      ```
      AD = Close - Max(High, Close_prev)
      ```
   - Se Close = Close_prev:
      ```
      AD = 0
      ```

3. Calcular o valor WAD acumulando os valores AD:
   ```
   WAD = Previous WAD value + AD
   ```

O indicador acumula valores positivos e negativos, formando uma linha cumulativa que pode ser usada para comparação com o movimento do preço.

![IndicatorWilliamsAccumulationDistribution](../../../../images/indicator_williams_accumulation_distribution.png)

## Ver também

[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
