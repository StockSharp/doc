# CHV

**volatilidade de Chaikin (CHV)** indica a diferença entre o máximo das compras e o mínimo das vendas durante um período de tempo. O indicador permite uma análise qualitativa das alterações de preço e da largura do intervalo entre o máximo e o mínimo do preço. O CHV não considera gaps de preço no seu cálculo, o que pode ser considerado uma desvantagem em certa medida.

Para usar o indicador, deve ser usada a classe [ChaikinVolatility](xref:StockSharp.Algo.Indicators.ChaikinVolatility).
##### Cálculo

O cálculo do indicador volatilidade de Chaikin começa com a determinação do spread - a diferença entre o máximo e o mínimo da barra atual. O resultado obtido é suavizado com uma média móvel exponencial ao longo do período correspondente. A volatilidade é calculada pela fórmula:

CHV = (EMA(H-L(i), n) - EMA(H-L(i-n), n)) / EMA(H-L(i-n), n) x 100.

Neste caso, H-L(i) representa a diferença de preço na barra atual, e H-L(i-n) - a diferença de preço numa barra ocorrida n períodos antes. Assim, temos a capacidade de ver visualmente como o preço mudou ao longo do tempo.

Principais parâmetros do indicador:

- **ROCPeriod** - o número do período em relação ao qual o cálculo será realizado. Inicialmente definido como 5.
- **SmoothPeriod** - o período da média móvel. Por defeito, está definido como 32.

![IndicatorChaikinVolatility](../../../../images/indicatorchaikinvolatility.png)

## Ver também

[CMO](cmo.md)
