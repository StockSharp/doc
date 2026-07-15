# STC

**Ciclo de tendência de Schaff (STC)** é um indicador de momentum desenvolvido por Doug Schaff. O STC baseia-se no pressuposto de que os ciclos de mercado se movem mais frequentemente entre condições de sobrecompra e sobrevenda do que numa tendência verdadeira.

Para utilizar o indicador, é necessário usar a classe [SchaffTrendCycle](xref:StockSharp.Algo.Indicators.SchaffTrendCycle).

## Descrição

O Ciclo de tendência de Schaff combina as vantagens do Oscilador estocástico, do MACD e da análise cíclica. Este indicador pode reagir a alterações de tendência mais rapidamente do que indicadores tradicionais como o MACD ou o Estocástico.

O STC oscila entre 0 e 100:
- Valores acima de 75 indicam normalmente condições de sobrecompra
- Valores abaixo de 25 indicam condições de sobrevenda
- Cruzar o nível 50 pode sinalizar uma alteração de tendência

Principais sinais do indicador:
- Comprar quando o STC cruza o nível 25 de baixo para cima (saída da zona de sobrevenda)
- Vender quando o STC cruza o nível 75 de cima para baixo (saída da zona de sobrecompra)

## Parâmetros

- **Período** - período principal para calcular o indicador.

## Cálculo

O cálculo do STC é efetuado em vários passos:

1. Calcular o MACD:
   ```
   MACD = EMA(Close, Fast) - EMA(Close, Slow)
   Signal = EMA(MACD, Signal)
   ```
   onde Fast, Slow e Signal são normalmente 23, 50 e 10, respetivamente.

2. Calcular o Oscilador estocástico com base no MACD:
   ```
   Stoch_K = 100 * ((MACD - Lowest(MACD, Length)) / (Highest(MACD, Length) - Lowest(MACD, Length)))
   Stoch_D = EMA(Stoch_K, 3)
   ```

3. Repetir o cálculo estocástico para obter o STC:
   ```
   STC = 100 * ((Stoch_D - Lowest(Stoch_D, Length)) / (Highest(Stoch_D, Length) - Lowest(Stoch_D, Length)))
   ```

O resultado é um oscilador mais suave do que o Estocástico clássico e que reage mais rapidamente às alterações de tendência do que o MACD.

![Gráfico do indicador STC](../../../../images/indicator_schaff_trend_cycle.png)

## Ver também

[MACD](macd.md)
[Estocástico](stochastic_oscillator.md)
