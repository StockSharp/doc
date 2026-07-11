# A/D

﻿# A/D

**Acceleration/Deceleration (A/D)** é um oscilador criado por Bill Williams. Mede a aceleração e a desaceleração do momentum da tendência.

Para usar o indicador, deve ser usada a classe [Acceleration](xref:StockSharp.Algo.Indicators.Acceleration).
##### Cálculo

O histograma A/D é a diferença entre o valor do histograma 5/34 da força motriz e a média móvel simples de 5 períodos calculada a partir desse histograma. Os valores são apresentados para o oscilador clássico e, nas definições, é sempre possível especificar os seus próprios parâmetros.

PREÇO MEDIANO = (HIGH + LOW) / 2
AO = SMA (PREÇO MEDIANO, 5) - SMA (PREÇO MEDIANO, 34)
A/D = AO - SMA (AO, 5)

onde:

PREÇO MEDIANO — preço mediano;
HIGH — o preço mais alto da barra;
LOW — o preço mais baixo da barra;
SMA — média móvel simples;
AO — indicador [oscilador incrível](ao.md).

Os parâmetros são definidos como valores dos períodos da SMA.

![IndicatorAcceleration](../../../../images/indicatoracceleration.png)

## Ver também

[Alligator](alligator.md)
