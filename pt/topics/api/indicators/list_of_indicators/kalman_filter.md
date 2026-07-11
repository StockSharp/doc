# KalmanFilter

**filtro de Kalman** é um algoritmo recursivo que estima o estado subjacente de um sistema a partir de observações com ruído.

Para utilizar o indicador, é necessário usar a classe [KalmanFilter](xref:StockSharp.Algo.Indicators.KalmanFilter).

## Descrição

O filtro de Kalman aplica um ciclo de previsão-correção para suavizar dados de preço e reduzir o ruído do mercado. Adapta-se dinamicamente à medida que nova informação fica disponível, tornando-o útil para acompanhar tendências em mercados voláteis.

## Parâmetros

- **ProcessNoise** - variância esperada no processo subjacente.
- **ObservationNoise** - variância esperada nos dados observados.

## Cálculo

Em cada passo, o filtro executa:
1. **Previsão** do próximo estado com base na estimativa anterior.
2. **Atualização** desta previsão usando a observação de preço mais recente e as estimativas de ruído.

Isto produz uma estimativa otimizada que reage rapidamente às alterações de preço enquanto filtra flutuações de curto prazo.

![Gráfico do indicador KalmanFilter](../../../../images/indicator_kalman_filter.png)

## Ver Também

[Média móvel adaptativa de Kaufman](kama.md)
[filtro adaptativo de Laguerre](adaptive_laguerre_filter.md)
