# Envelopes

**Envelopes** é um indicador que forma um canal criado ao deslocar uma média móvel por um valor específico. O método de construção do indicador replica exatamente a construção das bandas de Bollinger, exceto no cálculo da distância das linhas exteriores em relação à média. Se as bandas de Bollinger usam o desvio padrão para este cálculo, nos **Envelopes** esta distância é definida manualmente nas definições.
Os parâmetros definidos são o período da média móvel e o tamanho do desvio.

Para utilizar o indicador, deve ser usada a classe [Envelope](xref:StockSharp.Algo.Indicators.Envelope).

![Gráfico do indicador Envelopes](../../../../images/indicatorenvelope.png)

## Ver Também

[EMA](ema.md)
