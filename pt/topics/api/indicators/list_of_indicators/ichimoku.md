# Ichimoku

**Ichimoku** é um indicador representado por uma combinação de cinco linhas, três das quais são médias móveis, e duas são derivadas destas. O Ichimoku identifica a presença de uma tendência, bem como indica zonas de suporte/resistência e retrações da tendência.

Para utilizar o indicador, deve ser usada a classe [Ichimoku](xref:StockSharp.Algo.Indicators.Ichimoku).
##### Descrição do Indicador Ichimoku
  
Graficamente, o indicador consiste em cinco linhas coloridas semelhantes a médias móveis simples:  
  
- Tenkan (linha de conversão) — a linha mais rápida, reage primeiro às alterações de preço. O seu objetivo principal é determinar a direção da tendência de curto prazo. Na versão clássica, usa um segmento de 9 barras para trás. É construída como metade da soma dos preços máximo e mínimo.  
  
- Kijun (linha base) — indica a tendência de médio prazo, com um período de 26.  
  
- Senkou A e Senkou B — projetadas e apresentadas 26 períodos no futuro; em conjunto formam o que é chamado de nuvem (Kumo), que mostra áreas de suporte e resistência e é um componente essencial do indicador.  
  
- Chikou (linha atrasada) — representa o último preço de fecho, deslocado 26 períodos para trás. Ajuda a confirmar sinais: se cruza o gráfico de baixo para cima, é um sinal de compra, e de cima para baixo — um sinal de venda. Essencialmente, Chikou atua como um filtro de tendência.  

![IndicatorIchimoku](../../../../images/indicatorichimoku.png)

## Ver Também

[JMA](jma.md)

