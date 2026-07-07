# ATR

﻿# ATR

**Average True Range (ATR)** é um indicador que mostra o nível de volatilidade atual.

Para usar o indicador, deve ser usada a classe [AverageTrueRange](xref:StockSharp.Algo.Indicators.AverageTrueRange).
##### Cálculo do indicador
  
O cálculo do indicador começa com a determinação do True Range (TR), que é calculado como o máximo dos três valores seguintes:  
- a diferença entre o máximo e o mínimo atuais;  
- a diferença entre o máximo atual e o preço de fecho anterior (valor absoluto);  
- a diferença entre o mínimo atual e o preço de fecho anterior (valor absoluto).  

TRt = max(High(t)-Low(t); High(t) - Close(t-1); Close(t-1)-Low(t))  

O valor absoluto é usado para garantir valores positivos, porque estamos interessados na distância entre dois pontos, não na direção do movimento do preço.  
  
Com base neste indicador, é calculado o ATR. Tem apenas um parâmetro - o período N. Por defeito, é usado um indicador de 14 períodos, mas pode ser ajustado à sua própria estratégia. A fórmula tem o seguinte aspeto (esta é uma das formas da média móvel exponencial)  
  
ATR(t) = ((ATR(t-1) x (N-1)) + TR(t)) / N  

![IndicatorAverageTrueRange](../../../../images/indicatoraveragetruerange.png)

## Ver também

[AO](ao.md)
