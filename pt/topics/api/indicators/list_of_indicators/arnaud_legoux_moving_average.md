# ALMA

﻿# ALMA

**Média móvel de Arnaud Legoux (ALMA)** é um indicador desenvolvido por Arnaud Legoux e otimizado para eliminar ruído de mercado e reduzir o atraso do sinal.

Para usar o indicador, é necessário usar a classe [ArnaudLegouxMovingAverage](xref:StockSharp.Algo.Indicators.ArnaudLegouxMovingAverage).

## Descrição

A ALMA combina as vantagens de duas abordagens de suavização de dados:
1. Eliminação de ruído de mercado (como a maioria das médias móveis)
2. Minimização do atraso (típica de muitos indicadores de suavização)

O indicador ALMA usa uma distribuição normal (Gaussiana) como função de ponderação, que pode ser ajustada finamente usando os parâmetros offset e sigma. Isto torna-o uma ferramenta muito flexível e eficaz para análise técnica.

A ALMA é usada para:
- Determinar a tendência atual
- Identificar pontos de reversão
- Criar sistemas de negociação baseados em cruzamentos

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (número de velas a analisar)
- **Sigma** - sigma, um parâmetro que controla a forma da curva Gaussiana (valor recomendado: 6)
- **Offset** - offset, um parâmetro que controla a suavização e a velocidade de resposta (valor recomendado: 0.85)

## Cálculo

O cálculo da ALMA ocorre em várias etapas:

1. Determinar os pesos para cada ponto de dados na janela com base na distribuição normal (Gaussiana):
   ```
   m = floor(Offset * (Length - 1))
   s = Length / Sigma
   
   Para cada i de 0 a Length-1:
   w(i) = exp(-((i - m)^2) / (2 * s^2))
   ```

2. Normalizar os pesos:
   ```
   Sum_of_weights = soma de todos os w(i)
   
   Para cada i de 0 a Length-1:
   w_norm(i) = w(i) / Sum_of_weights
   ```

3. Calcular a ALMA como uma soma ponderada:
   ```
   ALMA = sum(Price(t-i) * w_norm(i)) para todo i de 0 a Length-1
   ```

Onde:
- Length - período da ALMA
- Offset - parâmetro de deslocamento (de 0 a 1)
- Sigma - parâmetro sigma (normalmente de 2 a 8)

![Gráfico do indicador ALMA](../../../../images/indicator_arnaud_legoux_moving_average.png)

## Ver também

[SMA](sma.md)
[EMA](ema.md)
[T3MA](t3_moving_average.md)
[ZLEMA](zero_lag_exponential_moving_average.md)
