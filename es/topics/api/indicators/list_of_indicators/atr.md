# ATR

**Rango verdadero promedio (ATR)** es un indicador que muestra el nivel de volatilidad actual.

Para utilizar el indicador, se debe utilizar la clase [AverageTrueRange](xref:StockSharp.Algo.Indicators.AverageTrueRange).
##### Cálculo del indicador
  
El cálculo del indicador comienza con la determinación del rango verdadero (TR), que se calcula como el máximo de los siguientes tres valores:
- la diferencia entre el máximo y el mínimo actuales;  
- la diferencia entre el máximo actual y el precio de cierre anterior (valor absoluto);  
- la diferencia entre el mínimo actual y el precio de cierre anterior (valor absoluto).  

TRt = máx(High(t)-Low(t); High(t) - Close(t-1); Close(t-1)-Low(t))  

El valor absoluto se utiliza para garantizar valores positivos, ya que nos interesa la distancia entre dos puntos, no la dirección del movimiento del precio.  
  
Sobre la base de este indicador, se calcula ATR. Tiene un solo parámetro: el período N. Por defecto, se utiliza un indicador de 14 períodos, pero se puede ajustar para que se ajuste a su propia estrategia. Así es como se ve la fórmula (esta es una de las formas de la media móvil exponencial)  
  
ATR(t) = ((ATR(t-1) x (N-1)) + TR(t)) / N

![IndicatorAverageTrueRange](../../../../images/indicatoraveragetruerange.png)

## Véase también

[AO](ao.md)
