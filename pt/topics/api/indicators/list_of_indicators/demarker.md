# DeMarker

O indicador **DeMarker (DeM)** avalia a pressão compradora e vendedora comparando os extremos da barra atual com os da anterior. Destaca zonas de sobrecompra e sobrevenda e ajuda a identificar potenciais pontos de viragem.

Use a classe [DeMarker](xref:StockSharp.Algo.Indicators.DeMarker) para trabalhar com este indicador.

## Cálculo

1. Para cada barra, calcular valores intermédios:
   `DeMax = max(High - PreviousHigh, 0)`
   `DeMin = max(PreviousLow - Low, 0)`
2. Suavizar `DeMax` e `DeMin` com uma média móvel de comprimento **Length**.
3. Calcular o valor final:
   `DeMarker = SMA(DeMax, Length) / (SMA(DeMax, Length) + SMA(DeMin, Length))`.

A saída é normalizada entre 0 e 1.

## Parâmetros

- **Length** - período de suavização que controla a capacidade de resposta do indicador.

## Interpretação

- **Acima de 0,7** - condições de sobrecompra, potencial correção descendente.
- **Abaixo de 0,3** - condições de sobrevenda, potencial reversão ascendente.
- **Divergência** entre o preço e o indicador avisa sobre uma alteração de tendência.

DeMarker pode ser usado para entradas contra a tendência, bem como para confirmar sinais de osciladores de momentum.

![indicator_demarker](../../../../images/indicator_demarker.png)

## Ver também

[RSI](rsi.md)
[Oscilador estocástico](stochastic_oscillator.md)
[Momentum](momentum.md)
