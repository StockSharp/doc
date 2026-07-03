# Alligator

El indicador **Alligator** consta de un grupo de tres medias móviles. Estos promedios móviles tienen diferentes períodos y también se desplazan hacia adelante en el gráfico.

Para utilizar el indicador, se debe utilizar la clase [Alligator](xref:StockSharp.Algo.Indicators.Alligator).
##### Características del indicador "Alligator"
  
El indicador consta de tres líneas de diferentes colores:
  
- Azul, llamado mandíbula, con un período de cálculo de 13 y un desplazamiento de 8 compases. Cuando se ubica debajo de la curva de precios, indica un posible movimiento ascendente de precios. Si la "mandíbula" se eleva por encima de él, se espera una disminución.

- Rojo, llamado dientes de caimán, con un período de 8 y un desplazamiento de 5 compases. Pertenece a los promedios rápidos y muestra el comportamiento de los precios en el rango horario.
  
- Verde, llamado los labios, con un período de 5 y un desplazamiento de 3 compases. Analiza las tendencias del mercado en doce minutos.

Los valores se toman para el indicador clásico y en la configuración siempre es posible especificar sus propios parámetros.

![IndicatorAlligator](../../../../images/indicatoralligator.png)

## Véase también

[ADX](adx.md)
