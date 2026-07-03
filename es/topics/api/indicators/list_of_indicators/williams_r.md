# %R

**Williams %R (%R, Williams— Percent Range)** es un indicador de impulso que fluctúa entre 0 y -100 y muestra niveles de sobrecompra y sobreventa.

Para utilizar el indicador, se debe utilizar la clase [WilliamsR](xref:StockSharp.Algo.Indicators.WilliamsR).
##### Cálculo
  
La fórmula para calcular el indicador Williams—Rango porcentual es similar a la utilizada para calcular el Stochastic Oscillator:

%R = - (MAX(ALTO(i - n)) - CERRADO(i)) / (MAX(ALTO(i - n)) - MIN(BAJO(i - n))) * 100  
  
donde:
  
CLOSE(i) - precio de cierre de hoy;  
MAX(HIGH(i - n)) - el máximo más alto de los últimos n períodos;  
MIN(LOW(i - n)): el mínimo más bajo de los últimos n períodos.  

El valor de n se establece como parámetro del indicador.

![IndicatorWilliamsR](../../../../images/indicatorwilliamsr.png)

## Véase también

[ZigZag](zigzag.md)
