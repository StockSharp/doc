# bandas de Bollinger

**bandas de Bollinger** são um indicador oscilante usado para medir a volatilidade do mercado. Permite avaliar se o preço está alto ou baixo em comparação com a média móvel. A banda central corresponde à média móvel simples do preço. As bandas superior e inferior são níveis nos quais o preço pode ser considerado alto ou baixo relativamente à média móvel.

Para usar o indicador, deve ser usada a classe [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands).
##### Cálculo

Os seguintes parâmetros com as definições correspondentes são usados para calcular bandas de Bollinger:
- tipo de desvio padrão — normalmente double;
- período da média móvel — ao critério do operador.

Assim, o indicador é formado por três linhas: central, superior e inferior, cada uma com a sua fórmula:

Linha média (ML) = média móvel (SMA (Close, N))
Banda superior = ML + (D x desvio padrão)
Banda inferior = ML - (D x desvio padrão), onde

D - a largura do canal definida nas definições, Desvio padrão (StdDev) - desvio padrão, calculado pela fórmula: SQRT(Sum(Close, n))^2, n)/n), onde
Sum - a soma de n períodos, n - período de cálculo, SQRT - raiz quadrada, Close - preço de fecho.

![Gráfico do indicador bandas de Bollinger](../../../../images/indicatorbollingerbands.png)

## Ver também

[CHV](chv.md)
