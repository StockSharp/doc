# Oscilador Estocástico %K

**Stochastic Oscillator %K** é um componente do oscilador estocástico que mostra a posição do preço de fecho atual relativamente ao intervalo de preços no período selecionado. O indicador foi desenvolvido por George Lane no final da década de 1950.

Para utilizar o indicador, use a classe [StochasticK](xref:StockSharp.Algo.Indicators.StochasticK).

## Descrição

O Stochastic Oscillator %K baseia-se na observação de que, durante tendências ascendentes, os preços de fecho tendem normalmente a concentrar-se mais perto do limite superior do intervalo de preços, enquanto durante tendências descendentes tendem a concentrar-se mais perto do limite inferior.

%K é a linha "rápida" do oscilador estocástico e é o principal componente utilizado para calcular a linha %D, que é uma média móvel de %K.

O oscilador varia de 0 a 100:

- Valores acima de 80 indicam normalmente um mercado em sobrecompra.
- Valores abaixo de 20 indicam um mercado em sobrevenda.
- Cruzamentos das linhas %K e %D podem ser utilizados como sinais de entrada ou saída.

## Parâmetros

- **Length** - período utilizado para calcular o intervalo de preços, ou seja, os máximos e mínimos. O valor predefinido comum é 14.

## Cálculo

Fórmula para calcular %K:

```
%K = 100 * ((Close - Low(Length)) / (High(Length) - Low(Length)))
```

onde:

- Close - preço de fecho atual.
- Low(Length) - preço mínimo no período Length.
- High(Length) - preço máximo no período Length.

No oscilador estocástico completo, a linha %D é calculada como uma média móvel simples de %K ao longo do período especificado, normalmente 3:

```
%D = SMA(%K, 3)
```

![IndicatorStochasticK](../../../../images/indicatorstochastick.png)

## Ver também

[Stochastic Oscillator](stochastic_oscillator.md)
