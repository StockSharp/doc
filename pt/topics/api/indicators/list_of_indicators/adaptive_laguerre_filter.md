# ALF

﻿# ALF

**filtro adaptativo de Laguerre (ALF)** é um indicador desenvolvido para suavizar dados de preço com atraso mínimo, baseado nos princípios matemáticos do filtro de Laguerre.

Para usar o indicador, é necessário usar a classe [AdaptiveLaguerreFilter](xref:StockSharp.Algo.Indicators.AdaptiveLaguerreFilter).

## Descrição

O filtro adaptativo de Laguerre é uma ferramenta avançada de filtragem de ruído de mercado. Fornece uma representação mais suave do movimento do preço, mantendo uma resposta rápida a alterações reais de tendência. Este filtro é especialmente útil para reduzir o atraso frequentemente encontrado nos indicadores tradicionais de suavização.

A principal vantagem do ALF face às médias móveis clássicas está na sua capacidade de separar de forma mais eficaz o ruído de mercado dos movimentos genuínos do preço, tornando-o uma ferramenta valiosa para traders que procuram reduzir sinais falsos.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Gamma** - coeficiente de filtragem (tipicamente no intervalo de 0.1 a 0.9)

O parâmetro Gamma determina o grau de suavização: valores mais baixos criam uma linha mais suave com mais atraso, enquanto valores mais altos resultam em menos suavização mas numa resposta mais rápida às alterações de preço.

## Cálculo

O filtro adaptativo de Laguerre baseia-se em polinómios de Laguerre e representa um sistema de filtragem de resposta ao impulso finita (FIR). O cálculo usa as seguintes fórmulas:

1. São calculados os valores intermédios L0, L1, L2 e L3:
   ```
   L0(t) = (1 - γ) * price(t) + γ * L0(t-1)
   L1(t) = -γ * L0(t) + L0(t-1) + γ * L1(t-1)
   L2(t) = -γ * L1(t) + L1(t-1) + γ * L2(t-1)
   L3(t) = -γ * L2(t) + L2(t-1) + γ * L3(t-1)
   ```

2. O valor final do ALF é calculado como a média:
   ```
   ALF = (L0 + L1 + L2 + L3) / 4
   ```

Onde:
- γ (gamma) - coeficiente de filtragem
- price(t) - preço atual
- L0, L1, L2, L3 - valores intermédios do filtro

![ALF](../../../../images/indicator_adaptive_laguerre_filter.png)

## Ver também

[LaguerreRSI](laguerre_rsi.md)
[ZLEMA](zero_lag_exponential_moving_average.md)
