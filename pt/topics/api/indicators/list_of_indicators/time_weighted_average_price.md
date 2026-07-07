# TWAP

**Time-Weighted Average Price (TWAP)** é um indicador que calcula o preço médio de um instrumento financeiro ponderado pelo tempo ao longo de um período específico. O TWAP é amplamente utilizado por investidores institucionais para executar grandes ordens com impacto mínimo no mercado.

Para utilizar o indicador, é necessário usar a classe [TimeWeightedAveragePrice](xref:StockSharp.Algo.Indicators.TimeWeightedAveragePrice).

## Descrição

O TWAP é um dos algoritmos de execução de ordens mais comuns, dividindo uma ordem grande numa série de ordens mais pequenas distribuídas uniformemente ao longo do tempo. O objetivo do TWAP é obter um preço médio durante um intervalo de tempo específico, minimizando o impacto no mercado.

Principais aplicações do TWAP:
- Preço de referência para avaliar a qualidade da execução de ordens
- Algoritmo de execução para minimizar o impacto no mercado
- Ferramenta para análise de mercado e tomada de decisões de negociação

Ao contrário do VWAP (Volume Weighted Average Price), o TWAP não considera os volumes negociados, concentrando-se exclusivamente no aspeto temporal.

## Cálculo

O cálculo do TWAP é efetuado somando os preços em intervalos de tempo iguais e dividindo essa soma pelo número de intervalos de tempo:

```
TWAP = (P₁ + P₂ + P₃ + ... + Pₙ) / n
```

onde:
- P₁, P₂, ..., Pₙ - preços em momentos de tempo sucessivos
- n - número de intervalos de tempo

Na implementação prática, são usados com maior frequência os preços típicos de cada período (vela):

```
Typical Price = (High + Low + Close) / 3
TWAP = Sum(Typical Price) / Number of Periods
```

Também pode ser utilizada uma fórmula recursiva para determinar o valor atual do TWAP em tempo real:

```
TWAP(current) = (TWAP(previous) * (n-1) + P(current)) / n
```

onde n é o número de observações na janela TWAP.

![IndicatorTimeWeightedAveragePrice](../../../../images/indicator_time_weighted_average_price.png)
