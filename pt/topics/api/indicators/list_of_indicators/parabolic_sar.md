# SAR parabólico

**SAR parabólico (SAR)** - um indicador de tendência que indica pontos de paragem e reversão do preço, bem como a direção da tendência.

Para usar o indicador, deve ser usada a classe [ParabolicSar](xref:StockSharp.Algo.Indicators.ParabolicSar).
##### Cálculo do indicador

O preço do ponto do indicador (SAR) para o período seguinte (candle) é calculado usando as seguintes fórmulas:

SAR(n+1) = SAR(n) + a * (high - SAR(n)), para uma tendência ascendente;
SAR(n+1) = SAR(n) + a * (low - SAR(n)), para uma tendência descendente, onde:

SAR(n+1) - preço para o período n+1;
SAR(n) - preço para o período n;
high e low - novo máximo e mínimo, respetivamente (extremos). São considerados para o intervalo de tempo entre a ativação do sinal anterior do indicador e o momento atual;

a - fator de aceleração.

O fator de aceleração é um coeficiente flutuante, caracterizado por valores mínimo e máximo, e por um passo de alteração.

O fator assume um valor mínimo igual a um passo no ponto de reversão e, assim que o preço alcança um novo valor extremo de acordo com a tendência (high ou low), o fator é aumentado por um passo. Quando o fator atinge o seu valor máximo, o seu crescimento é interrompido.

![Gráfico do indicador SAR parabólico](../../../../images/indicatorparabolicsar.png)

## Ver também

[Pico](peak.md)
