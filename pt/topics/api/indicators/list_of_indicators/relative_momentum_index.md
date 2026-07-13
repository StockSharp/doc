# RMI

**índice de momentum relativo (RMI)** é uma modificação do indicador RSI tradicional, proposta por Roger Altman. Ao contrário do RSI clássico, que calcula a razão entre aumentos e diminuições de preço ao longo de um determinado período, o RMI tem em conta a variação relativa do preço ao longo de um período de momentum selecionado.

Para utilizar o indicador, é necessário usar a classe [RelativeMomentumIndex](xref:StockSharp.Algo.Indicators.RelativeMomentumIndex).

## Descrição

O índice de momentum relativo (RMI) melhora o RSI clássico ao adicionar um parâmetro de período de momentum. Isto permite aos operadores ajustar a sensibilidade do indicador sem alterar o período principal de cálculo.

Tal como o RSI, o RMI oscila entre 0 e 100:
- Valores acima de 70 indicam normalmente um mercado em sobrecompra
- Valores abaixo de 30 indicam um mercado em sobrevenda
- A linha central em 50 serve como ponto de referência para determinar a direção principal do movimento do mercado

O RMI é particularmente útil para identificar potenciais pontos de inversão de tendência e confirmar a força da tendência atual.

## Parâmetros

- **MomentumPeriod** - período de momentum que define o desfasamento temporal para a comparação de preços.
- **Length** - período principal para calcular o indicador (semelhante ao período no RSI).

## Cálculo

O cálculo do RMI é efetuado em vários passos:

1. Calcular o momentum como a diferença entre o preço atual e o preço de n períodos atrás:
   ```
   Momentum = Price(current) - Price(current - MomentumPeriod)
   ```

2. Dividir os momentums em positivos (U) e negativos (D):
   ```
   Se Momentum > 0, então U = Momentum, D = 0
   Se Momentum < 0, então U = 0, D = |Momentum|
   ```

3. Calcular os valores médios dos momentums positivos e negativos ao longo do período especificado:
   ```
   AverageU = SMA(U, Length)
   AverageD = SMA(D, Length)
   ```

4. Calcular a força relativa:
   ```
   RS = AverageU / AverageD
   ```

5. Converter para índice de momentum relativo:
   ```
   RMI = 100 - (100 / (1 + RS))
   ```

![Gráfico do indicador RMI](../../../../images/indicator_relative_momentum_index.png)

## Ver também

[RSI](rsi.md)
[Impulso](momentum.md)
