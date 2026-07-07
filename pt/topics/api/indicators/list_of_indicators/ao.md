# AO

﻿# AO

O **Awesome Oscillator (AO)** é um indicador técnico clássico construído subtraindo médias móveis (SMA) com períodos diferentes.

Para usar o indicador, deve ser usada a classe [AwesomeOscillator](xref:StockSharp.Algo.Indicators.AwesomeOscillator).
##### Cálculo

O histograma do Awesome Oscillator é uma média móvel simples de 34 períodos construída sobre os valores centrais das barras (H+L) / 2, subtraída de uma média móvel simples de 5 períodos sobre os pontos centrais (H+L) / 2. Assim, a linha da média móvel lenta é subtraída da rápida para obter uma ideia da força do movimento do preço e das suas intenções futuras.

MEDIAN PRICE = (HIGH + LOW) / 2  
AO = SMA (MEDIAN PRICE, 5) — SMA (MEDIAN PRICE, 34), onde  
  
MEDIAN PRICE — preço mediano  
HIGH — o preço mais alto da barra  
LOW — o preço mais baixo da barra  
SMA — média móvel simples

Os valores são apresentados para o indicador clássico e, nas definições, é sempre possível especificar os seus próprios parâmetros.

![IndicatorAwesomeOscillator](../../../../images/indicatorawesomeoscillator.png)

## Ver também

[Bollinger Bands](bollinger_bands.md)
